using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InvPriceTurn14;
using Databox.Libs.InvPriceTurn14;
using InvPriceTurn14.DataItems;
using InvPriceTurn14.Helpers;
using InvPriceTurn14.Compares;
using System.IO;
using System.Threading;
using LumenWorks.Framework.IO.Csv;
using Ionic.Zip;

namespace WheelsScraper
{
    public class InvPriceTurn14 : BaseScraper
    {
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

        #endregion

        public InvPriceTurn14()
        {
            Name = "InvPriceTurn14";
            Url = "https://www.InvPriceTurn14.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            SpecialSettings = new ExtSettings();
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

        protected bool LoginTurn14()
        {
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
        }

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

            MessagePrinter.PrintMessage($"Read export.");

            string sceExportFilePath = extSett.ExportFilePath;
            extSett.SceItems = CsvManager.ReadSceExport(sceExportFilePath);
            extSett.SceItems = extSett.SceItems.Distinct(new BrandPartNumberEqualityComparer()).ToList();

            foreach (SceItem sceItem in extSett.SceItems)
            {
                foreach (BrandsAlignment brandsAlignment in extSett.BrandsAlignmentsItems)
                {
                    if (string.Equals(sceItem.Brand, brandsAlignment.BrandInSce, StringComparison.OrdinalIgnoreCase))
                    {
                        extSett.TransferInfoItems.Add(new TransferInfoItem()
                        {
                            BrandSce = sceItem.Brand,
                            PartNumberSce = sceItem.PartNumber,
                            ManufacturerPartNumberSce = sceItem.ManufacturerPartNumber,

                            BrandTurn14 = brandsAlignment.BrandInTurn14,
                        });
                    }
                }
            }

            lstProcessQueue.Add(new ProcessQueueItem { ItemType = 7 });

            StartOrPushPropertiesThread();
        }

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
                    Thread.Sleep(1000 * 60 * 4);
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

                                            //if (brand == "Fluidampr")
                                            //    webPrice = csv["Jobber"];

                                            //if (!string.IsNullOrEmpty(costPrice))
                                            //{
                                            //    if (string.IsNullOrEmpty(webPrice))
                                            //        webPrice = GetPriceRules(brand, double.Parse(costPrice)).ToString();

                                            //    if (string.IsNullOrEmpty(msrp))
                                            //        msrp = webPrice;

                                            //    if (double.Parse(costPrice) > double.Parse(msrp))
                                            //        msrp = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

                                            //    if (double.Parse(costPrice) >= double.Parse(webPrice))
                                            //        webPrice = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

                                            //    if (double.Parse(msrp) < double.Parse(webPrice))
                                            //        msrp = webPrice;

                                            //    jobber = webPrice;
                                            //}

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
                                                    transferInfoItem.Msrp = msrp?.Replace(",", string.Empty);
                                                    transferInfoItem.Jobber = jobber?.Replace(",", string.Empty);
                                                    transferInfoItem.WebPrice = webPrice?.Replace(",", string.Empty);
                                                    transferInfoItem.CostPrice = costPrice?.Replace(",", string.Empty);

                                                    transferInfoItem.Turn14WarehouseWesStock = warehouseWesStock;
                                                    transferInfoItem.Turn14WarehouseEastStock = warehouseEastStock;
                                                    transferInfoItem.QuantityTurn14 = stock;
                                                    transferInfoItem.Turn14MfrStock = MfrStockSTR;
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

            return Math.Round(price, 2);
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case 7:
                    act = Turn14Inventory;
                    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }
    }
}
