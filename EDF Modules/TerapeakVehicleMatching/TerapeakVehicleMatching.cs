#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using TerapeakVehicleMatching;
using Databox.Libs.TerapeakVehicleMatching;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TerapeakVehicleMatching.DataItems;
using System.Xml;
using TerapeakVehicleMatching.Extensions;
using System.Windows.Forms;
using System.Xml.Serialization;
using TerapeakVehicleMatching.Helpers;
using static TerapeakVehicleMatching.DataItems.EndFile;
using FileHelper = TerapeakVehicleMatching.Helpers.FileHelper;

#endregion

namespace WheelsScraper
{
    public class TerapeakVehicleMatching : BaseScraper
    {
        private const int PageWaitSleep = 6000;
        private const int PageScrollSleep = 1500;
        private const int PageSendKeysSleep = 1500;
        //private const string EbayApiAppId = "AdamParr-Comparis-PRD-35d705b3d-641a6ced";
        //private const string EbayApiAppId = "Shopping-Shopping-PRD-debee2917-03c4335b";
        private const string TerapeakItemFileName = "TerapeakItems.xml";
        private const int DescriptionLength = 32400;
        private readonly string _specifications = $"Specification##";
        private int counter1million = 0;
        private int fileCounter = 0;
        private static readonly object _lockObject = new object();
        private List<TerapeakVehicleMatchingInfo> TerapeakVehicleMatchingItems { get; set; }
        private List<SceExportItem> ExportItems { get; set; }
        private Dictionary<string, bool> EbayApiIdKeys = new Dictionary<string, bool>()
        {
            {"Shopping-Shopping-PRD-debee2917-03c4335b", false },
        };

        public TerapeakVehicleMatching()
        {
            Name = "TerapeakVehicleMatching";
            Url = "https://www.TerapeakVehicleMatching.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            TerapeakVehicleMatchingItems = new List<TerapeakVehicleMatchingInfo>();
            ExportItems = new List<SceExportItem>();
            SpecialSettings = new ExtSettings();
            Complete += TerapeakInfo_Complete;
        }

        private void TerapeakInfo_Complete(object sender, EventArgs e)
        {
            if (extSett.DataForFile.Count > 0)
            {
                #region Local files

                //string filePath12 = FileHelper.CreateScrapeFile(8, extSett.DataForFile);
                //MessagePrinter.PrintMessage($"Local file created {filePath12}");


                //string filePath12 = FileHelper.CreateEndFile(FileHelper.GetSettingsPath("TerapeakBeforeDistinkt.csv"), extSett.EndFiles);
                //MessagePrinter.PrintMessage($"Local file created {filePath12}");

                //extSett.EndFiles = extSett.EndFiles.Distinct(new EndFileEqualityComparer()).ToList();

                //string filePath = FileHelper.CreateEndFile(FileHelper.GetSettingsPath("TerapeakAfterDistinkt.csv"), extSett.EndFiles);
                //MessagePrinter.PrintMessage($"Local file created {filePath}");


                #endregion

                int counter = -1;
                List<IndexCollecttionItem> indexItems = new List<IndexCollecttionItem>();
                List<IndexCollecttionItem> indexItemsNotInExport = new List<IndexCollecttionItem>();
                SceExportItem.SceExportItemEqualityComparer comparer = new SceExportItem.SceExportItemEqualityComparer();
                foreach (var tItem in extSett.DataForFile)
                {
                    counter++;
                    var exportData = ExportItems.Where(i =>
                        !string.IsNullOrEmpty(i.VehicleMake) && i.VehicleMake == tItem.Make && i.VehicleModel == tItem.Model).ToList();

                    if (exportData.Any())
                    {
                        int exportCounter = -1;
                        exportData.ForEach(exportItem =>
                        {
                            exportCounter++;
                            if ((exportItem.StartYear >= tItem.StartYear &&
                                 exportItem.StartYear <= tItem.EndYear)
                                || (exportItem.EndYear >= tItem.StartYear &&
                                    exportItem.EndYear <= tItem.EndYear)
                                || (tItem.StartYear >= exportItem.StartYear &&
                                    tItem.StartYear <= exportItem.EndYear)
                                || (tItem.EndYear >= exportItem.StartYear &&
                                    tItem.EndYear <= exportItem.EndYear))
                            {
                                var exportIndex = ExportItems.FindIndex(i => comparer.Equals(i, exportItem));

                                indexItems.Add(new IndexCollecttionItem()
                                {
                                    TearapeakIndex = counter,
                                    ExportIndex = exportIndex,
                                });
                            }
                        });
                    }
                    else
                    {
                        indexItemsNotInExport.Add(new IndexCollecttionItem()
                        {
                            TearapeakIndex = counter,
                            ExportIndex = 0
                        });
                    }

                    if (counter % 1000 == 0)
                        MessagePrinter.PrintMessage($"1000 itemsProcessed {extSett.DataForFile.Count - counter} items left");
                }

                //remove duplicates from indexItems
                indexItems = indexItems.Distinct(IndexCollecttionItem.TearapeakIndexExportIndexComparer).ToList();
                indexItemsNotInExport = indexItemsNotInExport.Distinct(IndexCollecttionItem.TearapeakIndexExportIndexComparer).ToList();
                //need to create  extSett.EndFiles collections by indexses

                foreach (IndexCollecttionItem indexCollecttionItem in indexItems)
                {
                    var tItem = extSett.DataForFile[indexCollecttionItem.TearapeakIndex];
                    var exportItem = ExportItems[indexCollecttionItem.ExportIndex];

                    var endFileItem = new EndFile()
                    {
                        EbayItemId = tItem.ItemNumber,
                        ScePartNumber = exportItem.PartNumber,
                        SceWebPrice = exportItem.WebPrice,
                        Make = exportItem.VehicleMake,
                        Model = exportItem.VehicleModel,
                        StartYearEbay = tItem.StartYear,
                        EndYearEbay = tItem.EndYear,
                        StartYearSCE = exportItem.StartYear,
                        EndYearSCE = exportItem.EndYear,
                        Competitor = tItem.StoreName,
                        EbayBrand = tItem.Brand,
                        EbayCategory = tItem.Categories,
                        EbaySku = tItem.PartNumber,
                        TTotalSold = tItem.ItemSold,
                        ETotalSold = tItem.QuantitySold,
                        TTotalSales = tItem.TSales,
                        AvarageSalePrice = tItem.AvarageSalePrice,
                        EbayPrice = tItem.ConvertedCurrentPrice
                    };

                    extSett.EndFiles.Add(endFileItem);
                }

                var groupedItems = extSett.EndFiles.GroupBy(x => new
                {
                    ((EndFile)x).ScePartNumber,
                    ((EndFile)x).EbayBrand,
                    ((EndFile)x).Competitor
                });

                List<EndFile> resultItems = new List<EndFile>();

                foreach (var groupedItem in groupedItems)
                {
                    bool firstItem = true;
                    EndFile minPriceItem = new EndFile();
                    foreach (EndFile endFile in groupedItem)
                    {
                        if (firstItem)
                        {
                            minPriceItem = endFile;
                            firstItem = false;
                        }

                        if (minPriceItem.EbayPrice > endFile.EbayPrice)
                            minPriceItem = endFile;
                    }

                    resultItems.Add(minPriceItem);
                }

                //include notMatched products 
                foreach (IndexCollecttionItem indexItemsNotInExportItem in indexItemsNotInExport)
                {
                    var tItem = extSett.DataForFile[indexItemsNotInExportItem.TearapeakIndex];

                    var endFileItem = new EndFile()
                    {
                        EbayItemId = tItem.ItemNumber,
                        StartYearEbay = tItem.StartYear,
                        EndYearEbay = tItem.EndYear,
                        Competitor = tItem.StoreName,
                        EbayBrand = tItem.Brand,
                        EbayCategory = tItem.Categories,
                        EbaySku = tItem.PartNumber,
                        TTotalSold = tItem.ItemSold,
                        ETotalSold = tItem.QuantitySold,
                        TTotalSales = tItem.TSales,
                        AvarageSalePrice = tItem.AvarageSalePrice,
                        EbayPrice = tItem.ConvertedCurrentPrice,
                        Make = tItem.Make,
                        Model = tItem.Model
                    };

                    resultItems.Add(endFileItem);
                }

                string filePathRes = FileHelper.CreateEndFile(FileHelper.GetSettingsPath("TerapeakResult.csv"), resultItems);
                MessagePrinter.PrintMessage($"Local file created {filePathRes}");
            }
        }

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

        protected override bool Login()
        {
            return true;
        }

        protected override void RealStartProcess()
        {
            Wares.Clear();
            extSett.DataForFile = new List<ExtWareInfo>();
            extSett.EndFiles = new List<EndFile>();
            TerapeakVehicleMatchingItems.Clear();
            ExportItems.Clear();
            // Settings.MaxThreads = 1;
            MessagePrinter.PrintMessage($"Downloading SCE export.");

            List<string> exportFiles = new List<string>();
            exportFiles = SceApiHelper.LoadProductsExport(Settings);

            MessagePrinter.PrintMessage($"SCE export downloaded");


            foreach (string sceExportFilePath in exportFiles)
            {
                ExportItems.AddRange(FileHelper.ReadSceExport(sceExportFilePath));
            }

            ExportItems = ExportItems.Distinct(new SceExportItem.SceExportItemEqualityComparer()).ToList();

            if (ExportItems.Count == 0)
            {
                throw new Exception("SCE export empty");
            }

            ProcessScrape();

            #region For Test

            //TerapeakVehicleMatchingItems.Add(new TerapeakVehicleMatchingInfo()
            //{
            //    ItemId = "abc",
            //    TSales = "asd"
            //});

            //MessagePrinter.PrintMessage($"Terapeak items - {TerapeakVehicleMatchingItems.Count}");

            //try
            //{
            //    string filePath = FileHelper.GetSettingsPath(TerapeakItemFileName);
            //    MessagePrinter.PrintMessage($"creating file with terapeak data");
            //    WriteToXmlFile(filePath, TerapeakVehicleMatchingItems);
            //    MessagePrinter.PrintMessage($"file with terapeak data created - {filePath}");

            //    //TerapeakInfoItems = ReadFromXmlFile<List<TerapeakInfoItem>>(filePath);
            //}
            //catch (Exception e)
            //{

            //}

            #endregion

            //int counterForTest = 0;

            MessagePrinter.PrintMessage($"{TerapeakVehicleMatchingItems.Count} - products scraped from Terapeak");

            foreach (var tInfoItem in TerapeakVehicleMatchingItems)
            {
                //if (counterForTest == 200)
                //    break;

                //counterForTest++;

                lock (this)
                    lstProcessQueue.Add(new ProcessQueueItem { URL = tInfoItem.ItemId, Item = tInfoItem, ItemType = 10 });
            }

            StartOrPushPropertiesThread();
        }



        public static void CheckServerError(IWebDriver driver)
        {
            var answerHtmlDoc = GetHtmlDocument(driver);
            var servErrorNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Server Error')]");
            if (servErrorNode != null)
            {
                string error = servErrorNode.InnerText;
                if (error == "Server Error")
                {

                }
            }

            var connectionErrorNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'There is no Internet connection')]");
            if (connectionErrorNode != null)
            {
                string error = connectionErrorNode.InnerText;
                if (error == "There is no Internet connection")
                {

                }
            }
        }

        private static bool LengthChanged(long oldLength, long newLength)
        {
            if (oldLength == newLength)
                return false;

            return true;
        }

        private static void ScrollHtmlDocument(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("javascript:window.scrollBy(0,10000)");
            Thread.Sleep(PageScrollSleep);
            js.ExecuteScript("javascript:window.scrollBy(0,3000)");
            Thread.Sleep(PageScrollSleep);
            js.ExecuteScript("javascript:window.scrollBy(0,3000)");
            
            Thread.Sleep(PageScrollSleep);

            CheckCollapsed(driver);
        }

        private static HtmlAgilityPack.HtmlDocument GetHtmlDocument(IWebDriver driver)
        {
            var answerHtml = driver.PageSource;
            var answerHtmlDoc = new HtmlAgilityPack.HtmlDocument();
            answerHtml = HttpUtility.HtmlDecode(answerHtml);
            answerHtmlDoc.LoadHtml(answerHtml);

            return answerHtmlDoc;
        }

        private static void CheckCollapsed(IWebDriver driver)
        {
            HtmlAgilityPack.HtmlDocument answerHtmlDoc = GetHtmlDocument(driver);

            //try found "Answers Collapsed"
            HtmlNode collapsedAnwersNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//span[contains(text(),'Answers Collapsed')]");
            if (collapsedAnwersNode != null)
            {
                //load collapsed anwers
                string id = collapsedAnwersNode.AttributeOrNull("id");
                IWebElement collapsedAnswersElement = driver.FindElement(By.Id(id));

                if (collapsedAnswersElement.Displayed)
                    collapsedAnswersElement.Click();
                
                Thread.Sleep(PageWaitSleep);
            }
            else
            {
                HtmlNode viewMoreNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(),'View More')]");
                if (viewMoreNode != null)
                {
                    string id = viewMoreNode.AttributeOrNull("id");
                    IWebElement viewMoreElement = driver.FindElement(By.Id(id));

                    if (viewMoreElement.Displayed)
                        viewMoreElement.Click();
                    
                    Thread.Sleep(PageWaitSleep);
                }
                else
                {
                    string endPageError = "END PAGE ERROR!";
                }
            }
        }

        public bool ProcessScrape()
        {
            List<ReportItem> reportItems = new List<ReportItem>();

            var loginInfo = Settings.Logins;
            string login = "";
            string pass = "";

            foreach (var activLogin in loginInfo)
            {
                if (activLogin.IsActive == true)
                {
                    login = activLogin.Login;
                    pass = activLogin.Password;
                }
            }

            try
            {
                using (IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
                {
                    var productLink = /*"https://sell.terapeak.com/product-research?buyerCountryCodes&categoryId&endDate=1552719599999&fromPrice=100&isAnyCriteria=false&keywords=clutch&listingConditions=&listingTypes=&productId=&sellerCountryCodes&sellerName=&site=motors.ebay.com&startDate=1544947200000&toPrice&transactionSite"; */ extSett.Link;
                    Thread.Sleep(PageWaitSleep);
                    driver.Navigate().GoToUrl(productLink);
                    //driver.Navigate().GoToUrl("http://ip.xss.myip.ru");
                    Thread.Sleep(PageWaitSleep);
                    CheckServerError(driver);
                    Actions actions = new Actions(driver);

                    IWebElement quoraLogIn = driver.FindElement(By.XPath("//button[@id = 'login-using-ebay' and @data-test = 'loginButton']"));
                    quoraLogIn.Click();
                    Thread.Sleep(PageWaitSleep);

                    IWebElement quoraEmail1 = driver.FindElement(By.XPath("//input[@class = 'textbox__control textbox__control--underline' and @name = 'userid']"));
                    Thread.Sleep(PageWaitSleep);
                    actions.MoveToElement(quoraEmail1);
                    actions.Click();
                    actions.Perform();
                    quoraEmail1.SendKeys(login);
                    Thread.Sleep(PageSendKeysSleep);

                    IWebElement quoraPass = driver.FindElement(By.XPath("//input[@class = 'textbox__control textbox__control--underline' and @name = 'pass']"));
                    Thread.Sleep(PageWaitSleep);
                    actions.MoveToElement(quoraPass);
                    actions.Click();
                    actions.Perform();
                    quoraPass.SendKeys(pass);
                    Thread.Sleep(PageSendKeysSleep);

                    IWebElement quoraButtonClick = driver.FindElement(By.XPath("//button[@class = 'btn btn--primary btn--large btn--fluid' and @type = 'submit']"));
                    Thread.Sleep(PageWaitSleep);
                    quoraButtonClick.Click();
                    Thread.Sleep(PageWaitSleep);
                    Thread.Sleep(PageWaitSleep);

                    //find element 50 items on page
                    ScrollHtmlDocument(driver);
                    IWebElement _50ItemsElement = driver.FindElement(By.XPath("//button[@value = '50' and @type = 'button']"));
                    if (_50ItemsElement != null && _50ItemsElement.Displayed)
                    {
                        _50ItemsElement.Click();
                        Thread.Sleep(PageWaitSleep);
                        Thread.Sleep(PageWaitSleep);
                    }

                    try
                    {
                        long oldLength = driver.PageSource.Length;
                        bool endPage = false;
                        
                        while (!endPage)
                        {
                            var htmlDoc = GetHtmlDocument(driver);
                            ScrollHtmlDocument(driver);
                            long newLength = driver.PageSource.Length;

                            if (LengthChanged(oldLength, newLength))
                                oldLength = newLength;
                            else
                            {
                                htmlDoc = GetHtmlDocument(driver);
                                endPage = true;
                            }
                            string itemsIds = string.Empty;
                            var product = htmlDoc.DocumentNode.SelectNodes("//tr[@data-test='tbody-tr']");
                            if (product != null)
                            {
                                foreach (var item in product)
                                {
                                    TerapeakVehicleMatchingInfo tVMInfo = new TerapeakVehicleMatchingInfo();

                                    tVMInfo.ItemId = item.SelectSingleNode("./td[2]/div/span[1]/span[3]").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.ItemId))
                                    {

                                    }
                                    else
                                    {
                                        itemsIds += $"[{tVMInfo.ItemId}]";
                                    }

                                    tVMInfo.FormatStoreFixedPrice = item.SelectSingleNode("./td[3]/span").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.FormatStoreFixedPrice))
                                    {

                                    }

                                    tVMInfo.AvarageSalePrice = item.SelectSingleNode("./td[5]/span").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.AvarageSalePrice))
                                    {

                                    }

                                    tVMInfo.Bids = item.SelectSingleNode("./td[4]/span").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.Bids))
                                    {

                                    }

                                    tVMInfo.AverageShipping = item.SelectSingleNode("./td[8]/span").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.AverageShipping))
                                    {

                                    }

                                    tVMInfo.Date = item.SelectSingleNode("./td[9]/span").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.Date))
                                    {

                                    }

                                    tVMInfo.ItemSold = item.SelectSingleNode("./td[6]/span").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.ItemSold))
                                    {

                                    }


                                    tVMInfo.TSales = item.SelectSingleNode("./td[7]/span").InnerTextOrNull();
                                    if (string.IsNullOrEmpty(tVMInfo.TSales))
                                    {

                                    }

                                    TerapeakVehicleMatchingItems.Add(tVMInfo);
                                }
                            }
                            MessagePrinter.PrintMessage($"Processed items - {itemsIds}");

                            var nextPage = htmlDoc.DocumentNode.SelectSingleNode("//button[@data-test = 'nextButton']/span");
                            if (nextPage != null)
                            {
                                IWebElement quoraNextPage = driver.FindElement(By.XPath("//button[@data-test = 'nextButton' and @type = 'button']"));
                                Thread.Sleep(PageWaitSleep);

                                if (quoraNextPage.Text != null)
                                {
                                    endPage = false;
                                    quoraNextPage.Click();
                                    Thread.Sleep(PageWaitSleep);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessagePrinter.PrintMessage("End page found");
                    }

                    driver.Quit();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message + e.StackTrace);
            }

            return true;
        }

        private void ProcessEbayRequest(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            MessagePrinter.PrintMessage($"Processing Item id - {pqi.URL}");

            try
            {
                string postUrl = string.Empty;
                List<ExtWareInfo> localWares = new List<ExtWareInfo>();
                bool limitFlag = true;

                while (limitFlag)
                {
                    string apiKey = string.Empty;
                    lock (_lockObject)
                    {
                        foreach (KeyValuePair<string, bool> keyValuePair in EbayApiIdKeys)
                        {
                            if (!keyValuePair.Value)
                            {
                                apiKey = keyValuePair.Key;
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(apiKey))
                        {
                            MessagePrinter.PrintMessage($"All Keys Limited");
                            cancel = true;
                            return;
                        }
                    }

                    postUrl = $"http://open.api.ebay.com/shopping?callname=GetSingleItem&responseencoding=XML&appid={apiKey/*EbayApiAppId*/}&siteid=0&version=967&ItemID={pqi.URL}&IncludeSelector=TextDescription,ItemSpecifics,Details,ShippingCosts,Compatibility";

                    GetSingleItemResponse responseItem = PostHelper.GetPage(postUrl, out string exceptionStr);
                    if (responseItem != null && responseItem.Ack != "Failure")
                    {
                        TerapeakVehicleMatchingInfo terapeakItem = (TerapeakVehicleMatchingInfo)pqi.Item;

                        if (responseItem.Item == null)
                        {
                            MessagePrinter.PrintMessage($"Item not found on ebay - [{pqi.URL}]", ImportanceLevel.High);
                        }
                        else if (responseItem.Item.ItemCompatibilityList != null)
                        {
                            foreach (var itemCompatibility in responseItem.Item.ItemCompatibilityList)
                            {
                                ExtWareInfo wi = new ExtWareInfo();

                                var compabilityNotesCollection = itemCompatibility.CompatibilityNotes as XmlNode[];
                                if (compabilityNotesCollection != null)
                                    foreach (var compatibilityNote in compabilityNotesCollection)
                                    {
                                        wi.Notes = compatibilityNote.InnerText;
                                    }

                                foreach (var itemCompatibilityValue in itemCompatibility.NameValueList)
                                {
                                    if (itemCompatibilityValue.Name == "Notes")
                                    {
                                        wi.Notes = itemCompatibilityValue.Value;
                                    }
                                    else if (itemCompatibilityValue.Name == "Year")
                                    {
                                        int.TryParse(itemCompatibilityValue.Value, out int Year);

                                        wi.Year = Year;
                                    }
                                    else if (itemCompatibilityValue.Name == "Make")
                                    {
                                        wi.Make = itemCompatibilityValue.Value;
                                    }
                                    else if (itemCompatibilityValue.Name == "Model")
                                    {
                                        wi.Model = itemCompatibilityValue.Value;
                                    }
                                    else if (itemCompatibilityValue.Name == "Trim")
                                    {
                                        wi.Trim = itemCompatibilityValue.Value;
                                    }
                                    else if (itemCompatibilityValue.Name == "Engine")
                                    {
                                        wi.Engine = itemCompatibilityValue.Value;
                                    }
                                }

                                string specifications = string.Empty;
                                if (responseItem.Item.ItemSpecifics != null)
                                {
                                    specifications += $"{_specifications}";
                                    foreach (var itemSpecific in responseItem.Item.ItemSpecifics)
                                    {
                                        if (itemSpecific.Name == "Brand")
                                        {
                                            wi.Brand = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "UPC")
                                        {
                                            wi.UPC = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "Manufacturer Part Number")
                                        {
                                            wi.ManufacturerNumber = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "Interchange Part Number")
                                        {
                                            wi.InterchangePartNumber = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "Other Part Number")
                                        {
                                            wi.OtherPartNumber = itemSpecific.Value;
                                        }
                                        else
                                        {
                                            specifications += $"{itemSpecific.Name}~{itemSpecific.Value}^";
                                        }

                                    }
                                    if (!string.IsNullOrEmpty(specifications))
                                    {
                                        wi.Specifications = specifications.Trim('^');
                                    }
                                }

                                wi.ProductTitle = responseItem.Item.Title;
                                wi.Timestamp = responseItem.Timestamp.ToString();
                                wi.Description = responseItem.Item.Description.Truncate(DescriptionLength);
                                wi.ItemNumber = responseItem.Item.ItemID.ToString();
                                wi.Quantity = responseItem.Item.Quantity;
                                wi.UserId = responseItem.Item.Seller.UserID;
                                wi.ConvertedCurrentPrice = Convert.ToDouble(responseItem.Item.ConvertedCurrentPrice.Value);
                                if (responseItem.Item.DiscountPriceInfo != null)
                                    wi.OriginalRetailPrice = responseItem.Item.DiscountPriceInfo.OriginalRetailPrice.Value.ToString();
                                wi.ListingStatus = responseItem.Item.ListingStatus;
                                wi.QuantitySold = responseItem.Item.QuantitySold.ToString();
                                if (responseItem.Item.ShippingCostSummary.ShippingServiceCost != null)
                                    wi.ShippingServiceCost = responseItem.Item.ShippingCostSummary.ShippingServiceCost.Value.ToString();
                                wi.ShippingType = responseItem.Item.ShippingCostSummary.ShippingType;
                                if (responseItem.Item.ShippingCostSummary.ListedShippingServiceCost != null)
                                    wi.ListedShippingServiceCost = responseItem.Item.ShippingCostSummary.ListedShippingServiceCost.Value.ToString();
                                wi.HitCount = responseItem.Item.HitCount.ToString();
                                wi.StoreName = responseItem.Item.Storefront?.StoreName;
                                wi.ItemCompatibilityCount = responseItem.Item.ItemCompatibilityCount.ToString();
                                wi.PartNumber = responseItem.Item.SKU;

                                wi.Categories = responseItem.Item.PrimaryCategoryName;

                                if (responseItem.Item.PictureURL != null)
                                    foreach (string pictireUrl in responseItem.Item.PictureURL)
                                        wi.ImageUrl += pictireUrl.RemoveParametrs() + ",";
                                if (!string.IsNullOrEmpty(wi.ImageUrl))
                                    wi.ImageUrl = wi.ImageUrl.TrimEnd(',');

                                wi.ItemSold = terapeakItem.ItemSold;
                                wi.AvarageSalePrice = terapeakItem.AvarageSalePrice;
                                wi.Bids = terapeakItem.Bids;
                                wi.AverageShipping = terapeakItem.AverageShipping;
                                wi.FormatStoreFixedPrice = terapeakItem.FormatStoreFixedPrice;
                                wi.Date = terapeakItem.Date;
                                wi.TSales = terapeakItem.TSales;

                                localWares.Add(new ExtWareInfo(wi));
                                //extSett.DataForFile.Add(new ExtWareInfo(wi));
                                //AddWareInfo(wi);
                                //OnItemLoaded(wi);
                            }
                        }

                        #region universals

                        //else
                        //{
                        //    counter1million++;
                        //    if (counter1million > 1000000)
                        //    {
                        //        lock (_lockObject)
                        //        {
                        //            List<ExtWareInfo> itemsForFile = new List<ExtWareInfo>();
                        //            itemsForFile.AddRange(extSett.DataForFile);
                        //            fileCounter++;
                        //            extSett.DataForFile = new List<ExtWareInfo>();
                        //            counter1million = 1;
                        //            string filePath = FileHelper.CreateScrapeFile(fileCounter, itemsForFile);
                        //            MessagePrinter.PrintMessage($"File Created - {filePath}");
                        //        }
                        //    }

                        //    if (counter1million < 1000000)
                        //    {
                        //        ExtWareInfo wi = new ExtWareInfo();

                        //        string specifications = string.Empty;
                        //        if (responseItem.Item.ItemSpecifics != null)
                        //        {
                        //            specifications += $"{_specifications}";
                        //            foreach (var itemSpecific in responseItem.Item.ItemSpecifics)
                        //            {
                        //                if (itemSpecific.Name == "Brand")
                        //                {
                        //                    wi.Brand = itemSpecific.Value;
                        //                }
                        //                else if (itemSpecific.Name == "UPC")
                        //                {
                        //                    wi.UPC = itemSpecific.Value;
                        //                }
                        //                else if (itemSpecific.Name == "Manufacturer Part Number")
                        //                {
                        //                    wi.ManufacturerNumber = itemSpecific.Value;
                        //                }
                        //                else if (itemSpecific.Name == "Interchange Part Number")
                        //                {
                        //                    wi.InterchangePartNumber = itemSpecific.Value;
                        //                }
                        //                else if (itemSpecific.Name == "Other Part Number")
                        //                {
                        //                    wi.OtherPartNumber = itemSpecific.Value;
                        //                }
                        //                else
                        //                {
                        //                    specifications += $"{itemSpecific.Name}~{itemSpecific.Value}^";
                        //                }

                        //            }
                        //            if (!string.IsNullOrEmpty(specifications))
                        //            {
                        //                wi.Specifications = specifications.Trim('^');
                        //            }
                        //        }


                        //        wi.ProductTitle = responseItem.Item.Title;
                        //        wi.Timestamp = responseItem.Timestamp.ToString();
                        //        wi.Description = responseItem.Item.Description.Truncate(DescriptionLength);
                        //        wi.ItemNumber = responseItem.Item.ItemID.ToString();
                        //        wi.Quantity = responseItem.Item.Quantity;
                        //        wi.UserId = responseItem.Item.Seller.UserID;
                        //        wi.ConvertedCurrentPrice = Convert.ToDouble(responseItem.Item.ConvertedCurrentPrice.Value);
                        //        if (responseItem.Item.DiscountPriceInfo != null)
                        //            wi.OriginalRetailPrice =
                        //                responseItem.Item.DiscountPriceInfo.OriginalRetailPrice.Value.ToString();
                        //        wi.ListingStatus = responseItem.Item.ListingStatus;
                        //        wi.QuantitySold = responseItem.Item.QuantitySold.ToString();
                        //        if (responseItem.Item.ShippingCostSummary.ShippingServiceCost != null)
                        //            wi.ShippingServiceCost = responseItem.Item.ShippingCostSummary.ShippingServiceCost.Value
                        //                .ToString();
                        //        wi.ShippingType = responseItem.Item.ShippingCostSummary.ShippingType;
                        //        if (responseItem.Item.ShippingCostSummary.ListedShippingServiceCost != null)
                        //            wi.ListedShippingServiceCost = responseItem.Item.ShippingCostSummary
                        //                .ListedShippingServiceCost.Value.ToString();
                        //        wi.HitCount = responseItem.Item.HitCount.ToString();
                        //        wi.StoreName = responseItem.Item.Storefront?.StoreName;
                        //        wi.ItemCompatibilityCount = responseItem.Item.ItemCompatibilityCount.ToString();
                        //        wi.PartNumber = responseItem.Item.SKU;

                        //        wi.Categories = responseItem.Item.PrimaryCategoryName;

                        //        if (responseItem.Item.PictureURL != null)
                        //            foreach (string pictireUrl in responseItem.Item.PictureURL)
                        //                wi.ImageUrl += pictireUrl.RemoveParametrs() + ",";
                        //        if (!string.IsNullOrEmpty(wi.ImageUrl))
                        //            wi.ImageUrl = wi.ImageUrl.TrimEnd(',');

                        //        wi.ItemSold = terapeakItem.ItemSold;
                        //        wi.AvarageSalePrice = terapeakItem.AvarageSalePrice;
                        //        wi.Bids = terapeakItem.Bids;
                        //        wi.AverageShipping = terapeakItem.AverageShipping;
                        //        wi.FormatStoreFixedPrice = terapeakItem.FormatStoreFixedPrice;
                        //        wi.Date = terapeakItem.Date;
                        //        wi.TSales = terapeakItem.TSales;

                        //        extSett.DataForFile.Add(new ExtWareInfo(wi));
                        //        AddWareInfo(wi);
                        //        OnItemLoaded(wi);
                        //    }
                        //}

                        #endregion

                        if (localWares.Any())
                        {
                            var withoutYearsGroup = localWares.GroupBy(x => new
                            {
                                ((ExtWareInfo)x).PartNumber,
                                ((ExtWareInfo)x).Make,
                                ((ExtWareInfo)x).Model,
                                ((ExtWareInfo)x).Engine
                            });


                            List<ExtWareInfo> yearsGroup = new List<ExtWareInfo>();
                            foreach (var groupInfo in withoutYearsGroup)
                            {
                                var orderedGroupInfo = groupInfo.OrderBy(i => i.Year);

                                ExtWareInfo orderedGroupInfoFirstWare = orderedGroupInfo.First();

                                var firstOldWare = orderedGroupInfo.First(i => i.Make == orderedGroupInfoFirstWare.Make && i.Model == orderedGroupInfoFirstWare.Model && i.Engine == orderedGroupInfoFirstWare.Engine);
                                var lastOldWare = orderedGroupInfo.Last(i => i.Make == orderedGroupInfoFirstWare.Make && i.Model == orderedGroupInfoFirstWare.Model && i.Engine == orderedGroupInfoFirstWare.Engine);

                                orderedGroupInfoFirstWare.StartYear = firstOldWare.Year;
                                orderedGroupInfoFirstWare.EndYear = lastOldWare.Year;

                                yearsGroup.Add(orderedGroupInfoFirstWare);
                            }

                            //FileHelper.CreateScrapeFile(6, yearsGroup);

                            extSett.DataForFile.AddRange(yearsGroup);
                            //AddWareInfo();
                            int counter = 0;

                            //FileHelper.CreateEndFile(FileHelper.GetSettingsPath("EndFileGroup"), extSett.EndFiles);
                        }

                        limitFlag = false;
                    }
                    else if (responseItem?.Errors != null && responseItem.Ack == "Failure")
                    {
                        if (responseItem.Errors.LongMessage.Contains("limited in the number of calls"))
                        {
                            lock (_lockObject)
                            {
                                limitFlag = true;
                                EbayApiIdKeys[apiKey] = true;
                            }
                        }
                        else
                        {
                            limitFlag = false;
                        }

                        MessagePrinter.PrintMessage($"Request to ebay Failure [{pqi.URL}]. {responseItem.Errors.LongMessage}", ImportanceLevel.High);
                    }
                    else
                    {
                        limitFlag = false;
                        MessagePrinter.PrintMessage($"Error while take item from Ebay - [{pqi.URL}]. Error - {exceptionStr}", ImportanceLevel.High);
                    }
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message}", ImportanceLevel.Critical);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage($"Product processed - {pqi.URL}");
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 10)
                act = ProcessEbayRequest;
            else act = null;

            return act;
        }

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
