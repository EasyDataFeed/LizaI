#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using RPMoutletInventory;
using Databox.Libs.RPMoutletInventory;
using System.Security.Authentication;
using System.IO;
using System.Net;
using RPMoutletInventory.Extensions;
using RPMoutletInventory.Helpers;
using RPMoutletInventory.Models;
using System.Text.RegularExpressions;
using System.Threading;
using LumenWorks.Framework.IO.Csv;
using Ionic.Zip;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;
using RPMoutletInventory.Compares;
using RPMoutletInventory.DataItems;
using RPMoutletInventory.DataItems.Meyer;
using RPMoutletInventory.Enums;

#endregion

namespace WheelsScraper
{
    public class RPMoutletInventory : BaseScraper
    {
        List<EbayItem> EbayItems;

        public RPMoutletInventory()
        {
            Name = "RPMoutletInventory";
            Url = "https://www.RPMoutletInventory.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            SpecialSettings = new ExtSettings();
            EbayItems = new List<EbayItem>();
        }

        #region constants
        private const int SleepForNextEbayRequest = 120000;

        private const string EbayLoginPage =
           "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&ru=http%3A%2F%2Fwww.ebay.com%2F";

        private const string DownloadItemsUrl =
            "http://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeDownload&RefId=";

        private const string CreateDownloadRequestUrl =
            "http://k2b-bulk.ebay.com/ws/eBayISAPI.dll?SMDownloadRequestSuccess";

        private const string DownloadReportUrl =
            "http://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeDownload&jobId=";
        private const string MeyerApiUrl = "http://meyerapi.meyerdistributing.com/http/default/ProdAPI/v2/";
        private const string MeyerApiInventoryRequest = "http://meyerapi.meyerdistributing.com/http/default/ProdAPI/v2/ItemInformation?ItemNumber=";
        private const string RefPattern = @"Your\sref\s#\sis\s(\d+)";

        private bool _processedSuccess;

        #endregion

        #region default methods
        private ExtSettings extSett
        {
            get
            {
                return (ExtSettings)Settings.SpecialSettings;
            }
        }

        public override Type[] GetTypesForXmlSerialization()
        {
            return new Type[] { typeof(ExtSettings) };
        }

        public override System.Windows.Forms.Control SettingsTab
        {
            get
            {
                var frm = new ucExtSettings();
                frm.Sett = Settings;
                return frm;
            }
        }

        public override WareInfo WareInfoType
        {
            get
            {
                return new ExtWareInfo();
            }
        }
        #endregion

        #region login methods
        protected bool EbayLogin()
        {
            if (!String.IsNullOrEmpty(extSett.ebayLog) && !String.IsNullOrEmpty(extSett.ebayPass))
            {
                var html = PageRetriever.ReadFromServer(EbayLoginPage, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);
                var inputNodes = htmlDoc.DocumentNode.SelectNodes(".//input");
                var actionUrl = htmlDoc.DocumentNode.SelectSingleNode(".//form").AttributeOrNull("action");
                var data = "userid=" + HttpUtility.UrlEncode(extSett.ebayLog) +
                           "&pass=" + HttpUtility.UrlEncode(extSett.ebayPass);

                foreach (var inputNode in inputNodes)
                {
                    var inName = inputNode.AttributeOrNull("name");
                    var inVal = inputNode.AttributeOrNull("value");
                    data += string.Format("&{0}={1}", inName, inVal);
                }

                html = PageRetriever.WriteToServer(actionUrl, data, true);
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);
                var accInfoNode = htmlDoc.DocumentNode.SelectSingleNode(".//a[contains(text(), 'Continue')]");

                if (accInfoNode != null)
                {
                    return true;
                }
            }
            throw new AuthenticationException("Login eBay failed.");
        }
        protected bool MeyerLogin()
        {
            PageRetriever.Accept = "application/json";
            PageRetriever.ContentType = "application/json";
            PageRetriever.Headers.Clear();

            if (extSett.mayerTokenExp > DateTime.Now)
            {
                PageRetriever.Headers.Add(HttpRequestHeader.Authorization, "Espresso " + extSett.mayerToken + ": 1");
                return true;
            }
            MessagePrinter.PrintMessage("Getting new Mayers ApiKey...");
            if (String.IsNullOrEmpty(extSett.mayerLogin))
            {
                throw new NoLoginException("No Mayers login found!");
            }
            var authInfo = new { username = extSett.mayerLogin, password = extSett.mayerPass };
            var jss = new JavaScriptSerializer();
            var jsonData = jss.Serialize(authInfo);

            var response = PageRetriever.WriteToServer(MeyerApiUrl + "Authentication", jsonData);
            var authoResp = jss.Deserialize<AuthorizeResp>(response);

            if (authoResp == null)
            {
                throw new HttpParseException("API key not found or not authorized: Authentication failed");
            }
            extSett.mayerToken = authoResp.ApiKey;
            extSett.mayerTokenExp = authoResp.Expiration.AddDays(-1);
            ModuleSettings.Default.SaveConfig();
            MessagePrinter.PrintMessage("New ApiKey saved");
            PageRetriever.Headers.Clear();
            PageRetriever.Headers.Add(HttpRequestHeader.Authorization, "Espresso " + extSett.mayerToken + ": 1");
            return true;
        }
        protected bool LoginMotovicity()
        {
            try
            {
                Url = "https://www.motovicity.com/";
                MessagePrinter.PrintMessage("Starting login Motovicity...");
                if (String.IsNullOrEmpty(extSett.motoLog) || String.IsNullOrEmpty(extSett.motoPass))
                {
                    throw new Exception("No valid login found Motovicity");
                }

                string loginURL = string.Format("{0}BusinessToBusiness/Login.aspx", this.Url);
                string html = PageRetriever.ReadFromServer(loginURL, true);
                var doc = CreateDoc(html);

                string login = HttpUtility.UrlEncode(extSett.motoLog);
                string password = HttpUtility.UrlEncode(extSett.motoPass);
                string EventTarget = string.Empty;
                string EventArgument = string.Empty;
                string ViewState = string.Empty;
                string EventValidation = string.Empty;
                string ViewStateGenerator = string.Empty;

                var tagInput = doc.DocumentNode.SelectSingleNode("//input[@name='__EVENTTARGET']");
                if (tagInput != null)
                {
                    EventTarget = tagInput.Attributes["value"].Value;
                    EventTarget = HttpUtility.UrlEncode(EventTarget);
                }
                tagInput = doc.DocumentNode.SelectSingleNode("//input[@name='__EVENTARGUMENT']");
                if (tagInput != null)
                {
                    EventArgument = tagInput.Attributes["value"].Value;
                    EventArgument = HttpUtility.UrlEncode(EventArgument);
                }
                tagInput = doc.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATE']");
                if (tagInput != null)
                {
                    ViewState = tagInput.Attributes["value"].Value;
                    ViewState = HttpUtility.UrlEncode(ViewState);
                }
                tagInput = doc.DocumentNode.SelectSingleNode("//input[@name='__EVENTVALIDATION']");
                if (tagInput != null)
                {
                    EventValidation = tagInput.Attributes["value"].Value;
                    EventValidation = HttpUtility.UrlEncode(EventValidation);
                }
                tagInput = doc.DocumentNode.SelectSingleNode("//input[@name='__VIEWSTATEGENERATOR']");
                if (tagInput != null)
                {
                    ViewStateGenerator = tagInput.Attributes["value"].Value;
                    ViewStateGenerator = HttpUtility.UrlEncode(ViewStateGenerator);
                }
                //string postData = string.Format("__EVENTTARGET={0}&__EVENTARGUMENT={1}&__VIEWSTATE={2}&__EVENTVALIDATION={3}"
                //    + "&ctl00%24phmiddleCol%24motoLogon%24txtUserName={4}"
                //    + "&ctl00%24phmiddleCol%24motoLogon%24txtPassword={5}"
                //    + "&ctl00%24phmiddleCol%24motoLogon%24btnLogOn=logon", EventTarget, EventArgument, ViewState, EventValidation,
                //    login, password);

                string postData = string.Format("__LASTFOCUS=&__VIEWSTATE={2}&__VIEWSTATEGENERATOR={4}&__EVENTTARGET={0}&__EVENTARGUMENT={1}&__EVENTVALIDATION={3}&ctl00$Body$userName={5}&ctl00$Body$password={6}&ctl00$Body$loginButton=LOGIN", EventTarget, EventArgument, ViewState, EventValidation, ViewStateGenerator,
                    login, password);

                html = PageRetriever.WriteToServer(loginURL, postData, true);
                if (html.Contains("LOGOUT"))
                {
                    MessagePrinter.PrintMessage("Motovicity login done!");
                    return true;
                }
                else
                {
                    MessagePrinter.PrintMessage("Motovicity login failed!");
                    return false;
                }
            }
            catch (Exception err)
            {
                MessagePrinter.PrintMessage(err.Message, ImportanceLevel.Critical);
                return false;
            }
        }
        protected bool LoginTurn14()
        {
            //var Url = "https://www.turn14.com/";
            var Url = "https://www.turn14.com/";
            MessagePrinter.PrintMessage("Starting Turn14 login...");
            if (string.IsNullOrEmpty(extSett.turn14Log) || string.IsNullOrEmpty(extSett.turn14Pass))
            {
                string message = "No valid Turn14 login found";
                MessagePrinter.PrintMessage(message);
                throw new Exception(message);
            }

            string loginURL = string.Format("{0}user/login", Url);

            var data = HttpUtility.UrlEncode("username") + "=" + HttpUtility.UrlEncode(extSett.turn14Log) + "&" +
                    HttpUtility.UrlEncode("password") + "=" + HttpUtility.UrlEncode(extSett.turn14Pass);

            try
            {
                PageRetriever.Accept = "text/html";
                PageRetriever.ContentType = "application/x-www-form-urlencoded";
                PageRetriever.WriteToServer(loginURL, data, true, false, true);
                PageRetriever.ReadFromServer("https://www.turn14.com/index.php", true);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
            //if (html.Contains("LOGOUT"))
            //{
            //    MessagePrinter.PrintMessage("Turn14 login done!");
            //    return true;
            //}

            //return false;
        }
        #endregion

        protected override void RealStartProcess()
        {
            extSett.TransferInfoItems.Clear();

            if (string.IsNullOrEmpty(extSett.BrandsFilePath))
            {
                MessagePrinter.PrintMessage("You must choice File with brands", ImportanceLevel.Critical);
                return;
            }

            extSett.BrandsAlignmentsItems = new List<BrandsAlignment>();
            extSett.BrandsAlignmentsItems = CsvManager.ReadBrandsAlignments(extSett.BrandsFilePath);

            MessagePrinter.PrintMessage($"Download SCE export.");

            string sceExportFilePath = SceApiHelper.LoadProductsExport(Settings);
            extSett.SceItems = CsvManager.ReadSceExport(sceExportFilePath);

            extSett.SceItems = extSett.SceItems.Distinct(new BrandPartNumberEqualityComparer()).ToList();

            foreach (SceItem sceItem in extSett.SceItems)
            {
                //if (sceItem.ManufacturerPartNumber == "252-253")
                //{

                //}

                foreach (BrandsAlignment brandsAlignment in extSett.BrandsAlignmentsItems)
                {
                    if (string.Equals(sceItem.Brand, brandsAlignment.BrandInSce, StringComparison.OrdinalIgnoreCase))
                    {
                        extSett.TransferInfoItems.Add(new TransferInfoItem()
                        {
                            ProdIdSce = sceItem.ProdId,
                            ProdTypeSce = sceItem.ProductType,
                            BrandSce = sceItem.Brand,
                            PartNumberSce = sceItem.PartNumber,
                            ManufacturerPartNumberSce = sceItem.ManufacturerPartNumber,

                            BrandMeyerLong = brandsAlignment.BrandInMayerLong,
                            BrandMeyerShort = brandsAlignment.BrandInMayerShort,
                            BrandPremier = brandsAlignment.BrandInPremier,
                            BrandTurn14 = brandsAlignment.BrandInTurn14,
                            BrandInEbay = brandsAlignment.BrandInEbay,
                        });
                    }
                }
            }

            if (File.Exists(sceExportFilePath))
                File.Delete(sceExportFilePath);

            //process mayer
            EbayItems.Clear();

            if (extSett.UseEbay)
                lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = 1 });
            if (extSett.UseMeyer)
                lstProcessQueue.Add(new ProcessQueueItem { ItemType = 11 });
            if (extSett.UsePremier)
                lstProcessQueue.Add(new ProcessQueueItem { ItemType = 8 });
            if (extSett.UseTurn14)
                lstProcessQueue.Add(new ProcessQueueItem { ItemType = 7 });

            StartOrPushPropertiesThread();
        }

        #region Motovisity

        //protected void ProcessBrandsMotovicity(ProcessQueueItem pqi)
        //{
        //    if (cancel)
        //        return;

        //    MessagePrinter.PrintMessage(string.Format("Downloading products for brand {0}. Please wait...", HttpUtility.UrlDecode(pqi.Name)));
        //    string html = PageRetriever.ReadFromServer(pqi.URL, true);
        //    MessagePrinter.PrintMessage(string.Format("Products for brand {0} downloaded!", HttpUtility.UrlDecode(pqi.Name)));
        //    var doc = CreateDoc(html);

        //    int pageNum = 1;

        //    var products = doc.DocumentNode.SelectNodes(".//div[@class='partsearch-maincontainer']");

        //    foreach (var prod in products)
        //    {
        //        InvItem wi = new InvItem();
        //        var partNumber = prod.SelectSingleNode(".//div[@class='partTitle']/span[@class='matdestitle']");
        //        if (partNumber != null)
        //        {
        //            wi.PartNumber = partNumber.InnerText.RemoveDirtyData().Trim().Split('(')[1].Replace(")", "").Trim();
        //        }
        //        else
        //        {
        //            wi.PartNumber = string.Empty;
        //        }
        //        if (!String.IsNullOrEmpty(wi.PartNumber))
        //        {
        //            var inStockTd = prod.SelectSingleNode(".//span[@class='in-stock-amount']");
        //            int qty;
        //            if (inStockTd != null)
        //            {
        //                if (int.TryParse(inStockTd.InnerText.TrimEnd('+'), out qty))
        //                {
        //                    wi.Available = qty;
        //                }
        //                else
        //                {
        //                    wi.Available = 0;
        //                }
        //            }
        //            else
        //            {
        //                wi.Available = 0;
        //            }
        //            InvItems.Add(wi);
        //        }

        //    }

        //    var nextPage = doc.DocumentNode.SelectSingleNode(".//ul[@class='pagination']");
        //    if (nextPage != null)
        //    {
        //        if (nextPage.InnerHtml.Contains("NEXT"))
        //        {
        //            pageNum = int.Parse(nextPage.SelectSingleNode(".//a[@class='active']").InnerText);
        //            string nextPageURL = string.Format("{0}BusinessToBusiness/Products.aspx?br={2}&ste=300&ca=true&page={1}",
        //                this.Url, pageNum + 1, pqi.Name);
        //            lock (this)
        //            {
        //                lstProcessQueue.Add(new ProcessQueueItem { URL = nextPageURL, ItemType = 5, Name = pqi.Name });
        //            }
        //        }
        //    }

        //    pqi.Processed = true;
        //    StartOrPushPropertiesThread();
        //}

        #endregion

        #region Turn 14    

        protected void Turn14Inventory(ProcessQueueItem pqi)
        {
            try
            {
                if (cancel)
                    return;

                if (!LoginTurn14())
                {
                    lstProcessQueue.Clear();
                    pqi.Processed = true;
                    return;
                }

                MessagePrinter.PrintMessage("Downloading inventory from Turn14. Please wait...");
                bool turn14Processed = false;

                //string csvFile = @"D:\Leprizoriy\Work SCE\EDF\EDF Modules\RPMoutletInventory\EDF\turnfourteen.csv";
                int counter = 0;
                string csvFile = string.Empty;

                while (counter < 10)
                {
                    csvFile = LoadCsvFile();
                    if (!string.IsNullOrEmpty(csvFile))
                    {
                        break;
                        Thread.Sleep(10000);
                        csvFile = LoadCsvFile();
                    }
                    counter++;
                    MessagePrinter.PrintMessage("Wait 4 minutes for next try...", ImportanceLevel.Mid);
                    Thread.Sleep(1000*60*4);
                }

                if (string.IsNullOrEmpty(csvFile))
                {
                    MessagePrinter.PrintMessage(string.Format("Turn 14 Inventory not downloaded!"));
                }
                else
                {
                    MessagePrinter.PrintMessage("Turn 14 Inventory downloaded!");
                    if (File.Exists(csvFile))
                    {
                        MessagePrinter.PrintMessage(string.Format("Reading all inventory items from the file {0}", csvFile));
                        using (var sr = File.OpenText(csvFile))
                        {
                            using (var csv = new CsvReader(sr, true, ','))
                            {
                                while (csv.ReadNextRecord())
                                {
                                    try
                                    {
                                        string brand = csv["PrimaryVendor"];
                                        bool brandFound = false;

                                        var resultBrand = extSett.BrandsAlignmentsItems.Where(i => string.Equals(i.BrandInTurn14, brand, StringComparison.OrdinalIgnoreCase)).ToList();
                                        if (resultBrand.Count > 0)
                                        {
                                            brand = resultBrand[0].BrandInTurn14;
                                            brandFound = true;
                                        }

                                        if (brandFound)
                                        {
                                            string manufacturerPartNumber = csv["PartNumber"];

                                            //if (manufacturerPartNumber == "252-253")
                                            //{

                                            //}

                                            string partNumber = csv["InternalPartNumber"];
                                            string webPrice = csv["Map"];
                                            string costPrice = csv["Cost"];
                                            string msrp = csv["Retail"];
                                            string jobber = csv["Jobber"];
                                            string warehouseEastStock = csv["EastStock"];
                                            string warehouseWesStock = csv["WestStock"];
                                            string dsfee = csv["DSFee"];

                                            string stock = string.Empty;
                                            stock = csv["Stock"];
                                            string MfrStockSTR = csv["MfrStock"];

                                            if (string.IsNullOrEmpty(stock))
                                                stock = "0";

                                            if (brand == "Fluidampr")
                                                webPrice = csv["Jobber"];

                                            if (!string.IsNullOrEmpty(costPrice))
                                            {
                                                if (string.IsNullOrEmpty(webPrice))
                                                    webPrice = GetPriceRules(brand, double.Parse(costPrice)).ToString();

                                                if (string.IsNullOrEmpty(msrp))
                                                    msrp = webPrice;

                                                if (double.Parse(costPrice) > double.Parse(msrp))
                                                    msrp = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

                                                if (double.Parse(costPrice) >= double.Parse(webPrice))
                                                    webPrice = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

                                                if (double.Parse(msrp) < double.Parse(webPrice))
                                                    msrp = webPrice;

                                                jobber = webPrice;
                                            }

                                            foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
                                            {
                                                if (String.Equals(transferInfoItem.BrandTurn14, brand, StringComparison.CurrentCultureIgnoreCase) &&
                                                    String.Equals(transferInfoItem.ManufacturerPartNumberSce, manufacturerPartNumber, StringComparison.CurrentCultureIgnoreCase))
                                                {
                                                    turn14Processed = true;
                                                    if (!string.IsNullOrEmpty(dsfee))
                                                    {
                                                        if (dsfee.Contains("%"))
                                                        {
                                                            MfrStockSTR = "0";
                                                        }
                                                        else
                                                        {
                                                            string correctedDsFee = dsfee.ToLower().Replace("$", "").Replace("no", string.Empty).Replace("fee", string.Empty);
                                                            double.TryParse(correctedDsFee, out double parsedDsFee);

                                                            if (parsedDsFee >= 10)
                                                            {
                                                                MfrStockSTR = "0";
                                                            }
                                                            else if (double.Parse(costPrice) <= 100)
                                                            {
                                                                MfrStockSTR = "0";
                                                            }
                                                        }
                                                    }
                                                    if (string.IsNullOrEmpty(MfrStockSTR))
                                                    {
                                                        MfrStockSTR = "0";
                                                    }

                                                    transferInfoItem.PartNumberTurn14 = partNumber;
                                                    transferInfoItem.ManufacturerPartNumberTurn14 = manufacturerPartNumber;
                                                    transferInfoItem.Msrp = msrp.Replace(",", string.Empty);
                                                    transferInfoItem.Jobber = jobber.Replace(",", string.Empty);
                                                    transferInfoItem.WebPrice = webPrice.Replace(",", string.Empty);
                                                    transferInfoItem.CostPrice = costPrice.Replace(",", string.Empty);

                                                    transferInfoItem.Turn14WarehouseWesStock = warehouseWesStock;
                                                    transferInfoItem.Turn14WarehouseEastStock = warehouseEastStock;
                                                    transferInfoItem.QuantityTurn14 = stock;
                                                    transferInfoItem.Turn14MfrStock = MfrStockSTR;

                                                    // transferInfoItem.EbayPrice = webPrice;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                        File.Delete(csvFile);
                        MessagePrinter.PrintMessage("Turn14 Inventory processed.");
                    }
                    else
                    {
                        MessagePrinter.PrintMessage(string.Format("File {0} not found!", csvFile), ImportanceLevel.Critical);
                    }
                }

                if (extSett.Turn14InventoryFile)
                    if (extSett.TransferInfoItems.Count > 0 && turn14Processed)
                        ScraperHelper.CreateTurn14File(this, extSett);

                if (extSett.UpdatePriceInSce)
                    if (extSett.TransferInfoItems.Count > 0 && turn14Processed)
                        ScraperHelper.CreateTurn14BatchPriceFile(this, extSett);

                pqi.Processed = true;
                StartOrPushPropertiesThread();
            }
            catch (Exception ex)
            {

            }
        }

        private string LoadCsvFile()
        {
            try
            {
                var Url = "https://www.turn14.com/";
                string curUrl = string.Format("{0}export.php", Url);
                string postData = "stockExport=items";
                string localPath = Settings.Location;
                string FilePath = localPath.Substring(0, localPath.Length - (localPath.Length - localPath.LastIndexOf('\\')) + 1) + "turnfourteen.zip";
                PageRetriever.SaveFromServerWithPost(curUrl, postData, FilePath, true);
                Thread.Sleep(2000);
                if (File.Exists(FilePath))
                {
                    MessagePrinter.PrintMessage(string.Format("File {0} downloaded!", FilePath));
                    string unzippedFile = FilePath.Replace(".zip", ".csv");
                    Thread.Sleep(2000);
                    using (var zipArc = ZipFile.Read(FilePath))
                    {
                        zipArc[0].FileName = Path.GetFileName(unzippedFile);

                        if (File.Exists(unzippedFile))
                            File.Delete(unzippedFile);

                        Thread.Sleep(2000);

                        zipArc[0].Extract(Path.GetDirectoryName(unzippedFile), ExtractExistingFileAction.OverwriteSilently);
                        MessagePrinter.PrintMessage(string.Format("File {0} unzipped!", unzippedFile));
                    }
                    Thread.Sleep(2000);
                    File.Delete(FilePath);
                    MessagePrinter.PrintMessage(string.Format("File {0} deleted!", FilePath));
                    return unzippedFile;
                }
                else
                {
                    MessagePrinter.PrintMessage(string.Format("File {0} not found!", FilePath), ImportanceLevel.Critical);
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message, ImportanceLevel.Critical);
            }
            return string.Empty;
        }

        public static double GetPriceRules(string Brand, double price)
        {

            //PriceRules = new Dictionary<string, double>();
            if (Brand == "ARP")
                price = (price + 11) + ((price + 11) * 0.08);
            else if (Brand == "AutoMeter")
                price = (price + 11) + ((price + 11) * 0.13);
            else if (Brand == "Bully Dog")
                price = (price + 11) + ((price + 11) * 0.13);
            else if (Brand == "COMP Cams")
                price = (price + 17) + ((price + 17) * 0.11);
            else if (Brand == "Gates")
                price = (price + 11) + ((price + 11) * 0.1);
            else if (Brand == "Goodridge")
                price = (price + 11) + ((price + 11) * 0.1);
            else if (Brand == "Manley Performance")
                price = (price + 11) + ((price + 11) * 0.1);
            else if (Brand == "ZEX")
                price = price + (price * 0.1204);
            else if (Brand == "Stoptech")
                price = (price + 50) + ((price + 50) * 0.125);
            else if (Brand == "Walbro")
                price = (price + 11) + ((price + 11) * 0.1);
            else if (Brand == "Fluidampr")
                price = (price + 11) + ((price + 11) * 0.095);
            else if (Brand == "HKS")
                price = (price + 39) + ((price + 39) * 0.074);
            else if (Brand == "Hotchkis")
                price = (price + 35) + ((price + 35) * 0.09);
            else if (Brand == "Hypertech")
                price = (price + 11) + ((price + 11) * 0.18);
            else if (Brand == "FAST")
                price = price + (price * 0.2);
            else if (Brand == "aFe")
                price = price + (price * 0.25);

            // return("DiabloSport", (price + 11) + (price * 0.08));

            return Math.Round(price, 2);
        }

        #endregion

        #region Premier

        protected void ProcessPremierInventory(ProcessQueueItem pqi)
        {
            try
            {
                if (cancel)
                    return;

                MessagePrinter.PrintMessage("Downloading Premier inventory from FTP. Please wait...");
                bool premierProcessed = false;
                //string csvFilePath = @"D:\Altaresh\SVN\VS\EDF Development\WheelsScraper 2\WheelsScraper\bin\Debug\StandardExport.csv";
                string csvFilePath = downloadCsvFile();
                if (String.IsNullOrEmpty(csvFilePath))
                {
                    lstProcessQueue.Clear();
                    pqi.Processed = true;
                    MessagePrinter.PrintMessage("File does not exist on Premier FTP", ImportanceLevel.Critical);
                    return;
                }

                using (StreamReader rd = new StreamReader(new FileStream(csvFilePath, FileMode.Open)))
                {
                    string line;
                    StreamWriter sr = new StreamWriter(new FileStream(csvFilePath.Replace("StandardExport.csv", "") + "Output.csv", FileMode.Create));
                    while ((line = rd.ReadLine()) != null)
                    {
                        if (line.Count((char c) => c == '|') == 10)
                            sr.WriteLine(line);
                        else
                            MessagePrinter.PrintMessage("Fixing corrupted line in file...");
                    };
                    sr.Close();
                }

                File.Delete(csvFilePath);
                csvFilePath = csvFilePath.Replace("StandardExport.csv", "") + "Output.csv";
                if (!string.IsNullOrEmpty(csvFilePath))
                {
                    using (var sr = File.OpenText(csvFilePath))
                    {
                        using (var csv = new CsvReader(sr, true, '|'))
                        {
                            while (csv.ReadNextRecord())
                            {

                                string brand = csv["Manufacturer Name"];
                                bool brandFound = false;

                                var resultBrand = extSett.BrandsAlignmentsItems.Where(i => string.Equals(i.BrandInPremier, brand, StringComparison.OrdinalIgnoreCase)).ToList();
                                if (resultBrand.Count > 0)
                                {
                                    brand = resultBrand[0].BrandInPremier;
                                    brandFound = true;
                                }

                                if (brandFound)
                                {
                                    string partNumber = csv["Part Number"].Trim();
                                    string manufacturerPartNumber = csv["Manufacturer Part Number"];
                                    string warehouse = csv["Warehouse Availability"];

                                    foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
                                    {
                                        if (String.Equals(transferInfoItem.BrandPremier, brand, StringComparison.CurrentCultureIgnoreCase) &&
                                            String.Equals(transferInfoItem.ManufacturerPartNumberSce, manufacturerPartNumber, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            premierProcessed = true;

                                            transferInfoItem.PartNumberPremier = partNumber;
                                            transferInfoItem.ManufacturerPartNumberPremier = manufacturerPartNumber;

                                            string[] warehouses = warehouse.TrimEnd(';').Split(';'); // get all warehouse lines

                                            int quantity = 0;
                                            // sum values of warehouse lines to get quantity
                                            foreach (string warehouseLine in warehouses)
                                            {
                                                if (warehouseLine.Contains("Utah Warehouse"))
                                                    transferInfoItem.PremierUtahWarehouse = warehouseLine.Split(':')[1].Trim();
                                                else if (warehouseLine.Contains("Kentucky Warehouse"))
                                                    transferInfoItem.PremierKentuckyWarehouse = warehouseLine.Split(':')[1].Trim();
                                                else if (warehouseLine.Contains("Texas Warehouse"))
                                                    transferInfoItem.PremierTexasWarehouse = warehouseLine.Split(':')[1].Trim();
                                                else if (warehouseLine.Contains("California Warehouse"))
                                                    transferInfoItem.PremierCaliforniaWarehouse = warehouseLine.Split(':')[1].Trim();
                                                else if (warehouseLine.Contains("Calgary AB Warehouse"))
                                                    transferInfoItem.PremierCalgaryABWarehouse = warehouseLine.Split(':')[1].Trim();
                                                else if (warehouseLine.Contains("Washington Warehouse"))
                                                    transferInfoItem.PremierWashingtonWarehouse = warehouseLine.Split(':')[1].Trim();
                                                else if (warehouseLine.Contains("Ponoka AB Warehouse"))
                                                    transferInfoItem.PremierPonokaABWarehouse = warehouseLine.Split(':')[1].Trim();
                                                else if (warehouseLine.Contains("Colorado Warehouse"))
                                                    transferInfoItem.PremierColoradoWarehouse = warehouseLine.Split(':')[1].Trim();

                                                quantity += int.Parse(warehouseLine.Split(':')[1].Trim());
                                            }

                                            transferInfoItem.QuantityPremier = quantity.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    File.Delete(csvFilePath);
                    MessagePrinter.PrintMessage("Premier Inventory downloaded!");
                }
                else
                {
                    MessagePrinter.PrintMessage("Inventory Premier not downloaded!", ImportanceLevel.Critical);
                }

                if (extSett.PremierInventoryFile)
                    if (extSett.TransferInfoItems.Count > 0 && premierProcessed)
                        ScraperHelper.CreatePremierInventoryFile(this, extSett);

                pqi.Processed = true;
                StartOrPushPropertiesThread();
            }
            catch (Exception ex)
            {

            }

        }

        private string downloadCsvFile()
        {
            try
            {
                string zipFilePath = Regex.Replace(Settings.Location, @"[\w-%&]*.edf", "PremierItemExport.zip");
                string csvFileName = "StandardExport.csv";
                downloadFTPFile(zipFilePath, extSett.InvFtpAddress, extSett.InvFtpLogin, extSett.InvFtpPassword);
                if (File.Exists(zipFilePath))
                {
                    using (var zipArc = ZipFile.Read(zipFilePath))
                    {
                        zipArc[csvFileName].Extract(Path.GetDirectoryName(zipFilePath), ExtractExistingFileAction.OverwriteSilently);
                    }
                    
                    File.Delete(zipFilePath);
                    return string.Format("{0}\\{1}", Path.GetDirectoryName(zipFilePath), csvFileName);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private bool downloadFTPFile(string filePath, string ftpAddress, string user, string password)
        {
            try
            {
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpAddress);
                requestFileDownload.Timeout = 10000000;
                requestFileDownload.Credentials = new NetworkCredential(user, password);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                Stream responseStream = responseFileDownload.GetResponseStream();
                FileStream writeStream = new FileStream(filePath, FileMode.Create);

                int Length = 2048;
                byte[] buffer = new byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }

                responseStream.Close();
                writeStream.Close();

                requestFileDownload = null;
                responseFileDownload = null;

                return true;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message, ImportanceLevel.Critical);
            }

            return false;
        }

        #endregion

        #region Meyer

        protected void ProcessMayer(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            if (!MeyerLogin())
            {
                pqi.Processed = true;
                lstProcessQueue.Clear();
                return;
            }

            MessagePrinter.PrintMessage("Meyer processeing");
            bool meyerProcessed = false;
            foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
            {
                if (string.IsNullOrEmpty(transferInfoItem.BrandMeyerShort))
                    continue;

                string manufacturerPartNumber = $"{transferInfoItem.BrandMeyerShort.ToLower()}{transferInfoItem.ManufacturerPartNumberSce.ToLower()}";

                var jsonData = PageRetriever.ReadFromServer(MeyerApiInventoryRequest + HttpUtility.UrlEncode(manufacturerPartNumber));
                if (!jsonData.Contains("errorCode"))
                    try
                    {
                        var jsonItem = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JsonMeyerItem>>(jsonData);
                        if (jsonItem != null)
                        {
                            meyerProcessed = true;

                            transferInfoItem.PartNumberMeyer = jsonItem[0].ItemNumber;
                            transferInfoItem.ManufacturerPartNumberMeyer = transferInfoItem.ManufacturerPartNumberSce;
                            transferInfoItem.QuantityMeyer = jsonItem[0].QtyAvailable.ToString();
                        }
                    }
                    catch
                    {
                    }
            }

            MessagePrinter.PrintMessage("Meyer processed");

            if (extSett.MeyerInventoryFile)
                if (extSett.TransferInfoItems.Count > 0 && meyerProcessed)
                    ScraperHelper.CreateMeyerIntentoryFile(this, extSett);

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        #endregion

        #region Ebay

        private void ProcessCreateDownloadRequest(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                _processedSuccess = false;
                CreateDownloadRequest();
                if (_processedSuccess)
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem
                        {
                            ItemType = 2
                        });
                    }
                }
            }
            catch (Exception e)
            {
                this.PrintError(e);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        private void ProcessUploadEbayTemplate(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                _processedSuccess = false;
                UploadEbayTemplate(EbayItems);
                if (_processedSuccess)
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem
                        {
                            ItemType = 10
                        });
                    }
                }
            }
            catch (Exception e)
            {
                this.PrintError(e);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }
        private void ProcessCheckLastUploading(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                _processedSuccess = false;
                CheckLastUploading(extSett.LastUploadedJobId);
                if (!_processedSuccess)
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem
                        {
                            ItemType = 10
                        });
                    }
                    MessagePrinter.PrintMessage("Next try after 2 minutes...");
                    Thread.Sleep(SleepForNextEbayRequest);
                }
            }
            catch (Exception e)
            {
                this.PrintError(e);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }
        private void ProcessLoadEbayItems(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                _processedSuccess = false;
                string path = LoadEbayItems(extSett.LastDownloadRequestId);
                if (!_processedSuccess)
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem
                        {
                            ItemType = 2
                        });
                    }
                    MessagePrinter.PrintMessage("Next try after 2 minutes...");
                    Thread.Sleep(SleepForNextEbayRequest);
                }
                else
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem
                        {
                            ItemType = 3,
                            Item = path
                        });
                    }
                }
            }
            catch (Exception e)
            {
                this.PrintError(e);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }
        protected void GetSkuFromFile(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            MessagePrinter.PrintMessage("Start reading eBay items");
            var ebayItems = FileHelper.ReadEbayItems((string)pqi.Item);
            foreach (var ebayItem in ebayItems)
            {
                var query = GetItemEbay(extSett.apiTokenEB);
                query.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
                query.ItemID = ebayItem.EbayId;
                query.IncludeItemSpecifics = true;
                query.IncludeItemCompatibilityList = true;
                query.Execute();
                var item = query.ApiResponse.Item;
                foreach (NameValueListType spec in item.ItemSpecifics)
                {
                    if (spec.Name == "Manufacturer Part Number" && String.IsNullOrEmpty(ebayItem.PartNumber))
                    {
                        ebayItem.PartNumber = spec.Value[0].ToString().Trim();
                    }
                    else if (spec.Name == "Brand")
                        ebayItem.Brand = spec.Value[0].ToString().Trim();
                }
                EbayItems.Add(ebayItem);
            }

            MessagePrinter.PrintMessage(string.Format("{0} eBay items loaded", ebayItems.Count));

            lock (this)
            {
                lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = 9 });
            }
            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }
        private void UploadEbayTemplate(List<EbayItem> ebayItems)
        {
            if (!ebayItems.Any())
                return;

            MessagePrinter.PrintMessage(string.Format("Start update eBay Quantity for {0} items", ebayItems.Count));
            var matchedItems = 0;
            var notFound = 0;

            foreach (EbayItem ebayItem in ebayItems)
            {
                var flagFound = false;
                foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
                {
                    if (string.Equals(transferInfoItem.BrandInEbay, ebayItem.Brand, StringComparison.CurrentCultureIgnoreCase) &&
                        string.Equals(transferInfoItem.ManufacturerPartNumberSce, ebayItem.PartNumber, StringComparison.CurrentCultureIgnoreCase))
                    {
                        flagFound = true;
                        matchedItems++;
                        transferInfoItem.EbayId = ebayItem.EbayId;
                        transferInfoItem.EbayPrice = transferInfoItem.WebPrice;
                        transferInfoItem.FoundOnEbay = true;
                        int.TryParse(transferInfoItem.QuantityMeyer, out var meyerQty);
                        int.TryParse(transferInfoItem.QuantityTurn14, out var turn14Qty);
                        int.TryParse(transferInfoItem.QuantityPremier, out var premierQty);

                        transferInfoItem.EbayQuantity = meyerQty + turn14Qty + premierQty;

                        ebayItem.BrandSce = transferInfoItem.BrandSce;
                        ebayItem.ManufacturerPartSce = transferInfoItem.ManufacturerPartNumberSce;
                        ebayItem.Found = true;
                    }
                }

                if (!flagFound)
                {
                    notFound++;
                    MessagePrinter.PrintMessage(string.Format($"Ebay item with PartNumber: {ebayItem.PartNumber} not found."), ImportanceLevel.High);
                }
            }

            MessagePrinter.PrintMessage($"{matchedItems} items matched\n{notFound} items not found\n");
            
            ScraperHelper.CreateEbayIntentoryFile(this,extSett, ebayItems);

            List<TransferInfoItem> matchedInvItems = extSett.TransferInfoItems.Where(i => i.FoundOnEbay).ToList();

            foreach (TransferInfoItem ebayItem in matchedInvItems)
            {
                ExtWareInfo wi = new ExtWareInfo()
                {
                    PartNum = ebayItem.ManufacturerPartNumberSce,
                    Brand = ebayItem.BrandInEbay,
                    EbayId = ebayItem.EbayId
                };

                AddWareInfo(wi);
                OnItemLoaded(wi);
            }

            if (matchedInvItems.Any())
            {
                string fileName = string.Empty;
                if (extSett.UpdatePriceInSce)
                    fileName = FileHelper.WriteEbayPriceInvTemplate(matchedInvItems);
                else
                    fileName = FileHelper.WriteEbayTemplate(matchedInvItems);

                var html = HttpHelper.Post(extSett.EbaySecurityToken, fileName);
                var refRegex = new Regex(RefPattern);
                if (refRegex.IsMatch(html))
                {
                    ModuleSettings.Default.LastUploadedJobId = refRegex.Match(html).Groups[1].Value;
                    ModuleSettings.Default.SaveConfig();
                    foreach (TransferInfoItem transferInfoItem in matchedInvItems)
                    {
                        transferInfoItem.FoundOnEbay = false;
                    }
                    MessagePrinter.PrintMessage(string.Format("Ebay file uploaded success: # {0}",
                        extSett.LastUploadedJobId));
                    _processedSuccess = true;
                }
                else
                {
                    var docNode = html.GetDocNode();
                    string errMes = null;
                    var errorMessage = "Ebay file uploading failed";
                    var errorNode = docNode.SelectSingleNode(".//td[@class='Error']");
                    if (errorNode != null)
                    {
                        errMes = errorNode.InnerTextOrNull();
                    }
                    if (!string.IsNullOrEmpty(errMes))
                    {
                        errorMessage = string.Format("{0}\n{1}", errorMessage, errMes);
                    }
                    MessagePrinter.PrintMessage(errorMessage, ImportanceLevel.High);
                }
            }
            else
            {
                MessagePrinter.PrintMessage("No items for updating after matching");
            }
        }

        private void CreateDownloadRequest()
        {
            try
            {
                //extSett.Email = "service@rpmoutlet.com";
                MessagePrinter.PrintMessage("Creating download request for eBay items");
                var data = extSett.Email.PrepareDownloadReqFilter();
                if (!EbayLogin()) return;
                var html = PageRetriever.WriteToServer(CreateDownloadRequestUrl, data, true);
                var refRegex = new Regex(@"Your\sref\s#\sis\s(\d+)");
                if (refRegex.IsMatch(html))
                {
                    ModuleSettings.Default.LastDownloadRequestId = refRegex.Match(html).Groups[1].Value;
                    ModuleSettings.Default.SaveConfig();
                    MessagePrinter.PrintMessage(string.Format("Creating download request success: # {0}",
                        extSett.LastDownloadRequestId));
                    _processedSuccess = true;
                }
                else
                {
                    var docNode = html.GetDocNode();
                    string errMes = null;
                    var errorMessage = "Creating download request failed";
                    var errorNode = docNode.SelectSingleNode(".//td[@class='Error']");
                    if (errorNode != null)
                    {
                        errMes = errorNode.InnerTextOrNull();
                    }
                    if (!string.IsNullOrEmpty(errMes))
                    {
                        errorMessage = string.Format("{0}\n{1}", errorMessage, errMes);
                    }
                    MessagePrinter.PrintMessage(errorMessage, ImportanceLevel.High);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private string LoadEbayItems(string requestId)
        {
            string itemsFileName = null;
            if (!string.IsNullOrEmpty(requestId))
            {
                MessagePrinter.PrintMessage(string.Format("Getting eBay items file # {0}", requestId));
                if (!EbayLogin()) return null;
                var downlUrl = string.Format("{0}{1}", DownloadItemsUrl, requestId);
                var fileName = string.Format("ebay_items_FileExchange_Response_{0}.csv", requestId);
                fileName = Path.Combine(Path.GetTempPath(), fileName);
                var resp = PageRetriever.ReadFromServer(downlUrl, true, true);
                if (resp.StartsWith("Item ID"))
                {
                    PageRetriever.SaveFromServer(downlUrl, fileName, true);
                    MessagePrinter.PrintMessage(string.Format("Ebay items saved: {0}", fileName));
                    itemsFileName = fileName;
                    _processedSuccess = true;
                }
                else
                {
                    MessagePrinter.PrintMessage(
                        string.Format("The requested file # {0} is not completed.", requestId),
                        ImportanceLevel.High);
                }
            }
            else
            {
                MessagePrinter.PrintMessage("Last download request ID is empty", ImportanceLevel.High);
            }
            return itemsFileName;
        }

        private GetItemCall GetItemEbay(string token)
        {
            var apiContext = new ApiContext();
            apiContext.SoapApiServerUrl = "https://api.ebay.com/wsapi";
            ApiCredential apiCredential = new ApiCredential();
            apiCredential.eBayToken = token;
            apiContext.ApiCredential = apiCredential;
            //apiContext.Site = code;
            return new GetItemCall(apiContext);
        }
        protected void ProcessMotovicity(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            if (!LoginMotovicity())
            {
                lstProcessQueue.Clear();
                pqi.Processed = true;
                return;
            }

            string currentUrl = string.Format("{0}BusinessToBusiness/Default.aspx", this.Url);
            string html = PageRetriever.ReadFromServer(currentUrl, true);
            var doc = CreateDoc(html);

            //var brands = doc.DocumentNode.SelectNodes("//select[@id='ctl00_motoBrandCategorySearch_ddlBrands']/option");
            var brands = doc.DocumentNode.SelectNodes("//ul[@id='brands-list']/li/a");
            string brand;
            if (brands != null)
            {
                for (int i = 1; i < brands.Count; i++)
                {
                    //brand = HttpUtility.UrlEncode(HttpUtility.HtmlDecode(brands[i].NextSibling.InnerText));
                    brand = HttpUtility.UrlEncode(brands[i].InnerTextOrNull());
                    //currentUrl = string.Format("{0}BusinessToBusiness/Products.aspx?cl=0&cg=0&cp=0&ca=False&ps=&st=Dynamic&br={1}",
                    //this.Url, brand);
                    currentUrl = this.Url + "BusinessToBusiness/" + brands[i].AttributeOrNull("href");
                    //currentUrl = brands[i].AttributeOrNull("href").Split('=')[1].Split('&')[0];
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem { URL = currentUrl, ItemType = 5, Name = brand });
                    }
                }
            }
            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }
        private void CheckLastUploading(string jobId)
        {
            if (!string.IsNullOrEmpty(jobId))
            {
                MessagePrinter.PrintMessage(string.Format("Start checking uploading # {0}", jobId));

                //wait processing report
                Thread.Sleep(660000);
                if (!Login()) return;
                var reportUrl = string.Format("{0}{1}", DownloadReportUrl, jobId);
                var fileName = string.Format("FileExchange_Response_{0}.csv", jobId);
                fileName = Path.Combine(Path.GetTempPath(), fileName);
                var resp = PageRetriever.ReadFromServer(reportUrl, true, true);
                if (resp.StartsWith("Line"))
                {
                    PageRetriever.SaveFromServer(reportUrl, fileName, true);
                    MessagePrinter.PrintMessage(string.Format("Uploading report # {0} saved: {1}", jobId, fileName));
                    MessagePrinter.PrintMessage("Reading report");
                    List<ReportItem> reports = new List<ReportItem>();
                    try
                    {
                        reports = FileHelper.ReadUploadingReport(fileName);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Reading uploading report # {0} failed.\nSee report file: {1}", jobId, fileName), e);
                    }
                    var success = 0;
                    var errors = 0;
                    foreach (var reportItem in reports)
                    {
                        if (reportItem.Status == "Success")
                        {
                            ++success;
                        }
                        else
                        {
                            MessagePrinter.PrintMessage(string.Format("Report # {0}\n{1}", jobId, reportItem), ImportanceLevel.High);
                            ++errors;
                        }
                    }
                    MessagePrinter.PrintMessage(
                        string.Format("Uploading # {0} checked\n{1} Items updated\n{2} Success\n{3} Errors", jobId,
                            reports.Count, success, errors));
                    _processedSuccess = true;
                }
                else
                {
                    MessagePrinter.PrintMessage(
                        string.Format("The requested file # {0} is not completed.", jobId),
                        ImportanceLevel.High);
                }
            }
            else
            {
                MessagePrinter.PrintMessage("Last load-results report ID is empty", ImportanceLevel.High);
            }
        }

        #endregion    

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case 1:
                    act = ProcessCreateDownloadRequest;
                    break;
                case 2:
                    act = ProcessLoadEbayItems;
                    break;
                case 3:
                    act = GetSkuFromFile;
                    break;
                case 4:
                    act = ProcessMotovicity;
                    break;
                //case 5:
                //    act = ProcessBrandsMotovicity;
                //    break;
                case 7:
                    act = Turn14Inventory;
                    break;
                case 8:
                    act = ProcessPremierInventory;
                    break;
                case 9:
                    act = ProcessUploadEbayTemplate;
                    break;
                case 10:
                    act = ProcessCheckLastUploading;
                    break;
                case 11:
                    act = ProcessMayer;
                    break;
                //case 20:
                //    act = ProcessMeyerInfo;
                //    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }

        //private void ProcessMeyerInfo(ProcessQueueItem pqi)
        //{
        //    var brandAlignment = (BrandsAlignment)pqi.Item;

        //    var jsonData = PageRetriever.ReadFromServer(pqi.URL + HttpUtility.UrlEncode(pqi.Name));
        //    if (!jsonData.Contains("errorCode"))
        //        try
        //        {
        //            var jsonItem = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JsonMeyerItem>>(jsonData);
        //            if (jsonItem != null)
        //            {
        //                MeyerItem meyerItem = new MeyerItem
        //                {
        //                    Upc = jsonItem[0].UPC,
        //                    Stock = jsonItem[0].QtyAvailable,
        //                    BrandMeyerLong = brandAlignment.BrandInMayerLong,
        //                    BrandMeyerShort = brandAlignment.BrandInMayerShort,
        //                    BrandSce = brandAlignment.BrandInSce,
        //                    ManufacturerPartNumber = pqi.Name,
        //                };

        //                meyerItems.Add(meyerItem);
        //            }
        //        }
        //        catch
        //        {

        //        }

        //    pqi.Processed = true;
        //    StartOrPushPropertiesThread();
        //}
    }
}
