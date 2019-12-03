using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Acumotors;
using Databox.Libs.Acumotors;
using System.IO;
using System.Threading;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using Acumotors.DataItems;
using Acumotors.Helpers;
using System.Globalization;
using Acumotors.Extensions;
using Google.Apis.Sheets.v4.Data;
using System.Reflection;
using System.Resources;

namespace WheelsScraper
{
    public class Acumotors : BaseScraper
    {
        private const int PartNumberLength = 22;
        private const int DescriptionLength = 30;
        private const int DescriptionLengthCustomTitle = 80;
        private const string WDCode = "REK2";
        private ValueRange googleData;
        //private List<DateToUpdate> DateToUpdate { get; set; }

        public Acumotors()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Name = "Acumotors";
            Url = "https://www.Acumotors.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            //DateToUpdate = new List<DateToUpdate>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var libName = args.Name.ToLower().Replace('.', '_');
            var p1 = libName.IndexOf(',');
            if (p1 != -1)
                libName = libName.Substring(0, p1);
            if (libName.Contains("_resources"))
                return null;
            Assembly asm = null;

            var rmgr = new ResourceManager("Acumotors.Libs", typeof(EmbededLibs).Assembly);
            rmgr.IgnoreCase = true;

            object obj = rmgr.GetObject(libName);
            var asmBytes = ((byte[])(obj));
            if (asmBytes != null)
            {
                asm = Assembly.Load(asmBytes);
            }

            return asm;
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
            MessagePrinter.PrintMessage("Starting Turn14 login...");
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
            {
                string message = "No valid Turn14 login found";
                MessagePrinter.PrintMessage(message);
                throw new Exception(message);
            }

            string loginURL = string.Format("{0}user/login", Url);

            var data = HttpUtility.UrlEncode("username") + "=" + HttpUtility.UrlEncode(login) + "&" +
                    HttpUtility.UrlEncode("password") + "=" + HttpUtility.UrlEncode(pass);

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

        protected override void RealStartProcess()
        {
            if (extSett.UseGoogleSheets)
            {
                googleData = GoogleDocApiHelper.GetGoogleData(extSett);
            }

            //extSett.BrandsFilePath = @"D:\Leprizoriy\Work SCE\EDF\EDF Modules\Acumotors\EDF\WHI Brand Code List.csv";

            extSett.UpdateInfos = new List<UpdateInfo>();

            if (string.IsNullOrEmpty(extSett.BrandsFilePath))
            {
                MessagePrinter.PrintMessage("You must choice File with brands", ImportanceLevel.Critical);
                return;
            }

            extSett.BrandsAlignmentsItems = new List<BrandsAlignment>();
            extSett.BrandsAlignmentsItems = CsvManager.ReadBrandFile(extSett.BrandsFilePath);

            if (!string.IsNullOrEmpty(extSett.SpecificationFilePath))
            {
                extSett.SpecificationFileItems = new List<SpecificationFile>();
                extSett.SpecificationFileItems = CsvManager.ReadSpecificationFile(extSett.SpecificationFilePath);
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
                    MessagePrinter.PrintMessage($"Login to Turn14 failed", ImportanceLevel.Critical);
                    lstProcessQueue.Clear();
                    pqi.Processed = true;
                    return;
                }

                int counter = 0;
                string csvFile = string.Empty;
                //string csvFile = @"D:\Leprizoriy\Work SCE\EDF\WheelsScraper 2\WheelsScraper\bin\Debug\turnfourteen.csv";

                List<TransferInfoItem> turn14Export = new List<TransferInfoItem>();

                MessagePrinter.PrintMessage("Download Turn14 export");

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
                    MessagePrinter.PrintMessage(string.Format("Turn14 Inventory not downloaded!"));
                }
                else
                {
                    MessagePrinter.PrintMessage("Turn14 Inventory downloaded!");
                    if (File.Exists(csvFile))
                    {
                        MessagePrinter.PrintMessage(string.Format("Reading all inventory items from the file {0}", csvFile));
                        using (var sr = File.OpenText(csvFile))
                        {
                            using (var csv = new CsvReader(sr, true, ','))
                            {
                                while (csv.ReadNextRecord())
                                {
                                    double.TryParse(csv["Cost"], out double cost);
                                    double.TryParse(csv["Map"], out double map);
                                    double.TryParse(csv["CoreCharge"], out double coreCharge);
                                    double.TryParse(csv["Weight"], out double weight);

                                    double currentPrice = 0;

                                    if (extSett.doPricePerPound)
                                    {
                                        weight = Math.Round(weight, MidpointRounding.AwayFromZero);
                                        currentPrice = extSett.minSurcharge + (extSett.surchargePerLb * weight);
                                    }

                                    TransferInfoItem item = new TransferInfoItem
                                    {
                                        Brand = csv["PrimaryVendor"],
                                        PartNumber = csv["PartNumber"],
                                        Description = csv["Description"],
                                        CoreCharge = coreCharge,
                                        Map = map,
                                        CostPrice = cost,
                                        Stock = csv["Stock"],
                                        Weight = weight,
                                        CurrentPrice = currentPrice
                                    };

                                    //if (item.PartNumber == "1036600")
                                    //{
                                    //    if (extSett.doPricePerPound)
                                    //    {
                                    //        weight = Math.Round(weight, MidpointRounding.AwayFromZero);
                                    //        currentPrice = extSett.minSurcharge + (extSett.surchargePerLb * weight);
                                    //        item.CurrentPrice = currentPrice;
                                    //    }
                                    //}

                                    turn14Export.Add(item);
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

                string datestr = $"{DateTime.Now:yyyyMMdd}";
                string timestr = $"{DateTime.Now:HH:mm:ss}";
                string date = $"{DateTime.Now:yyMMdd}";

                foreach (var turn14 in turn14Export)
                {
                    foreach (var brand in extSett.BrandsAlignmentsItems)
                    {
                        if (turn14.Brand.ToLower() == brand.Brand.ToLower() && !string.IsNullOrEmpty(brand.BrandCode))
                        {
                            if (turn14.PartNumber == "42-100")
                            {

                            }

                            if (extSett.Percentage)
                            {
                                extSett.PercentageForMSRP = brand.Surcharge;
                            }

                            var turn14Item = new UpdateInfo();
                            turn14Item.Brand = brand.Brand;
                            turn14Item.WDCode = WDCode;
                            turn14Item.SupplierNumber = "1";
                            turn14Item.DateOfSnapshot = datestr;
                            turn14Item.TimeOfSnapshot = timestr;
                            turn14Item.LineCode = brand.BrandCode;
                            turn14Item.ItemNumber = turn14.PartNumber;
                            turn14Item.ItemPackageCode = "EA";
                            turn14Item.ItemDescription = turn14.Description;
                            turn14Item.AboutPart = turn14.Description;
                            turn14Item.CustomTitle = turn14.Description;
                            turn14Item.QuantityOnHand = turn14.Stock;
                            turn14Item.AnnualizedSalesQuantity = "";
                            turn14Item.ListPrice = Math.Round(turn14.CoreCharge + turn14.CostPrice + ((turn14.CoreCharge + turn14.CostPrice) * (extSett.PercentageForMSRP / 100)) + turn14.CurrentPrice, 2);
                            turn14Item.CostPrice = turn14Item.ListPrice;
                            turn14Item.ItemPackageQuantity = "1";
                            turn14Item.UPC = "Does not apply";

                            if (turn14.CoreCharge != 0)
                            {
                                turn14Item.PartHasCore = "Y";
                                turn14Item.CorePrice = turn14.CoreCharge;
                            }
                            else
                            {
                                turn14Item.PartHasCore = "N";
                                turn14Item.CorePrice = 0;
                            }

                            if (turn14Item.ListPrice < turn14.Map)
                            {
                                //MessagePrinter.PrintMessage($"{turn14Item.ItemNumber} - old price {turn14Item.ListPrice}, new price {turn14.Map}", ImportanceLevel.Mid);
                                turn14Item.ListPrice = turn14.Map;
                                turn14Item.CostPrice = turn14Item.ListPrice;
                            }

                            if (turn14Item.ItemNumber.Length > PartNumberLength)
                            {
                                var longPN = new LongPNFile();
                                longPN.LongPartNumber = turn14Item.ItemNumber;
                                extSett.LongPNFiles.Add(longPN);

                                string partNumber = turn14Item.ItemNumber.Truncate(PartNumberLength);
                                MessagePrinter.PrintMessage($"Cut ItemNumber [{turn14Item.ItemNumber}] to " +
                                    $"[{partNumber}]", ImportanceLevel.Mid);

                                turn14Item.ItemNumber = partNumber;
                            }

                            if (turn14Item.ItemDescription.Length > DescriptionLength)
                            {
                                turn14Item.ItemDescription = turn14Item.ItemDescription.Truncate(DescriptionLength);
                            }

                            if (turn14Item.CustomTitle.Length > DescriptionLengthCustomTitle)
                            {
                                turn14Item.CustomTitle = turn14Item.CustomTitle.Truncate(DescriptionLengthCustomTitle);
                            }

                            if (string.IsNullOrEmpty(turn14Item.ItemDescription))
                            {
                                turn14Item.ItemDescription = turn14Item.ItemNumber + turn14.Brand;
                            }

                            extSett.UpdateInfos.Add(turn14Item);
                        }
                    }
                }

                foreach (var updateItem in extSett.UpdateInfos)
                {
                    foreach (var specification in extSett.SpecificationFileItems)
                    {
                        if (updateItem.ItemNumber == specification.PartNumber.Replace("@", "") && updateItem.LineCode == specification.LineCode)
                        {
                            updateItem.Custom_attr1 = specification.Custom_attr1;
                            updateItem.Custom_attr2 = specification.Custom_attr2;
                            updateItem.Custom_attr3 = specification.Custom_attr3;
                            updateItem.AboutPart = specification.AboutPart;
                            updateItem.CustomTitle = specification.CustomTitle;
                        }
                    }
                }

                if (extSett.UseGoogleSheets)
                {
                    foreach (var turn14Item in extSett.UpdateInfos)
                    {
                        foreach (var item in googleData.Values)
                        {
                            if (item[0].ToString() == turn14Item.Brand && item[1].ToString() == turn14Item.ItemNumber)
                            {
                                double.TryParse(item[2].ToString(), out double price);

                                if (price != 0)
                                {
                                    turn14Item.ListPrice = price;
                                }
                                else
                                {
                                    MessagePrinter.PrintMessage($"{turn14Item.ItemNumber} has price '0'.", ImportanceLevel.High);
                                }
                            }
                        }
                    }
                }

                //if (extSett.LongPNFiles.Count > 0)
                //{
                //    MessagePrinter.PrintMessage($"Create local file");
                //    string filePath = FileHelper.CreateLongPNFile(FileHelper.GetSettingsPath($"acumotorsLongPN.csv"), extSett.LongPNFiles);
                //    if (!string.IsNullOrEmpty(filePath))
                //    {
                //        MessagePrinter.PrintMessage($"Local file created {filePath}");
                //    }
                //}

                if (extSett.UpdateInfos.Count > 0)
                {
                    bool submited6 = false;
                    var dateToUpdateInventory = CheckDateToUpdateFile();
                    if (dateToUpdateInventory != null)
                    {
                        DateTime changedDate = DateTime.Now;
                        bool dateIsEmpty = false;
                        foreach (var item in dateToUpdateInventory)
                        {
                            if (item.DateToUpdateProduct != null)
                            {
                                if (DateTime.Now.AddHours(-6) < item.DateToUpdateProduct.Value)
                                    submited6 = true;

                                changedDate = item.DateToUpdateProduct.Value.AddHours(6);
                            }
                            else
                            {
                                dateIsEmpty = true;
                            }
                        }

                        if (submited6 == false)
                        {
                            if (dateIsEmpty == false)
                            {
                                if (DateTime.Now < changedDate)
                                    MessagePrinter.PrintMessage($"Product file will load to ftp approximately on {changedDate}");

                                while (DateTime.Now < changedDate)
                                {
                                    if (DateTime.Now > changedDate)
                                        break;
                                    else
                                        Thread.Sleep(10 * 60 * 1000);
                                }
                            }

                            foreach (var item in dateToUpdateInventory)
                            {
                                if (item.DateToUpdateInventory != null)
                                {
                                    changedDate = item.DateToUpdateInventory.Value.AddHours(6);
                                }
                                else
                                {
                                    dateIsEmpty = true;
                                }
                            }

                            if (dateIsEmpty == false)
                            {
                                if (DateTime.Now < changedDate)
                                    MessagePrinter.PrintMessage($"Product file will load to ftp approximately on {changedDate}");

                                while (DateTime.Now < changedDate)
                                {
                                    if (DateTime.Now > changedDate)
                                        break;
                                    else
                                        Thread.Sleep(10 * 60 * 1000);
                                }
                            }
                        }
                    }

                    if (submited6 == false)
                    {
                        MessagePrinter.PrintMessage($"Create local file");
                        string filePath = FileHelper.CreateUpdateFile(FileHelper.GetSettingsPath($"acumotors{date}.csv"), extSett.UpdateInfos);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            MessagePrinter.PrintMessage("Upload file to FTP");
                            string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, $"acumotors{date}.csv", filePath, true);
                            if (!string.IsNullOrEmpty(url))
                            {
                                MessagePrinter.PrintMessage($"File uploaded to FTP {url}");

                                //string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                                //if (extSett.DoBatch)
                                //{
                                //    int batchId = SceApiHelper.BatchUpdate(urlForBatch, Settings);
                                //    MessagePrinter.PrintMessage($"File Batched. BatchId - {batchId}");
                                //}
                            }

                            MessagePrinter.PrintMessage($"Local file created {filePath}");
                            //if (File.Exists(filePath))
                            //    File.Delete(filePath);

                            var dateToUpdateInfo = CheckDateToUpdateFile();
                            if (dateToUpdateInfo != null)
                            {
                                foreach (var item in dateToUpdateInfo)
                                {
                                    item.DateToUpdateProduct = DateTime.Now;
                                }

                                string dateToUpdateFilePath = FileHelper.CreateDateToUpdateFile(FileHelper.GetSettingsPath($"AcumotorsDateToUpdate.csv"), dateToUpdateInfo);
                            }
                            else
                            {
                                var dateToUpdate = new DateToUpdate();
                                dateToUpdate.DateToUpdateProduct = DateTime.Now;
                                List<DateToUpdate> dateToUpdateInventorys = new List<DateToUpdate>();
                                dateToUpdateInventorys.Add(dateToUpdate);

                                string dateToUpdateFilePath = FileHelper.CreateDateToUpdateFile(FileHelper.GetSettingsPath($"AcumotorsDateToUpdate.csv"), dateToUpdateInventorys);
                            }
                        }
                    }
                }

                if (extSett.UpdateInfos.Count > 0)
                {
                    var dateToUpdateProduct = CheckDateToUpdateFile();
                    if (dateToUpdateProduct != null)
                    {
                        DateTime changedDate = DateTime.Now;
                        bool dateIsEmpty = false;

                        foreach (var item in dateToUpdateProduct)
                        {
                            if (item.DateToUpdateProduct != null)
                            {
                                changedDate = item.DateToUpdateProduct.Value.AddHours(6);
                            }
                            else
                            {
                                dateIsEmpty = true;
                            }
                        }

                        if (dateIsEmpty == false)
                        {
                            if (DateTime.Now < changedDate)
                                MessagePrinter.PrintMessage($"Inventory file will load to ftp approximately on {changedDate}");

                            while (DateTime.Now < changedDate)
                            {
                                if (DateTime.Now > changedDate)
                                    break;
                                else
                                    Thread.Sleep(10 * 60 * 1000);
                            }

                            MessagePrinter.PrintMessage($"Create local file");
                            string filePath = FileHelper.CreateInventoryUpdateFile(FileHelper.GetSettingsPath($"acumotors{date}.csv"), extSett.UpdateInfos);
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                MessagePrinter.PrintMessage("Upload file to FTP");
                                string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, $"acumotors{date}.csv", filePath, true);
                                if (!string.IsNullOrEmpty(url))
                                {
                                    MessagePrinter.PrintMessage($"File uploaded to FTP {url}");

                                    //string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                                    //if (extSett.DoBatch)
                                    //{
                                    //    int batchId = SceApiHelper.BatchUpdate(urlForBatch, Settings);
                                    //    MessagePrinter.PrintMessage($"File Batched. BatchId - {batchId}");
                                    //}
                                }

                                MessagePrinter.PrintMessage($"Local file created {filePath}");
                                //if (File.Exists(filePath))
                                //    File.Delete(filePath);

                                var dateToUpdateInfo = CheckDateToUpdateFile();
                                if (dateToUpdateInfo != null)
                                {
                                    foreach (var item in dateToUpdateInfo)
                                    {
                                        item.DateToUpdateInventory = DateTime.Now;
                                    }

                                    string dateToUpdateFilePath = FileHelper.CreateDateToUpdateFile(FileHelper.GetSettingsPath($"AcumotorsDateToUpdate.csv"), dateToUpdateInfo);
                                }
                                else
                                {
                                    var dateToUpdate = new DateToUpdate();
                                    dateToUpdate.DateToUpdateInventory = DateTime.Now;
                                    List<DateToUpdate> dateToUpdateProducts = new List<DateToUpdate>();
                                    dateToUpdateProducts.Add(dateToUpdate);

                                    string dateToUpdateFilePath = FileHelper.CreateDateToUpdateFile(FileHelper.GetSettingsPath($"AcumotorsDateToUpdate.csv"), dateToUpdateProducts);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessagePrinter.PrintMessage($"Error - {ex.Message}. {ex.StackTrace}");
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        private List<DateToUpdate> CheckDateToUpdateFile()
        {
            var acumotorsDateToUpdateFilePath = FileHelper.GetSettingsPath("AcumotorsDateToUpdate.csv");

            if (File.Exists(acumotorsDateToUpdateFilePath))
            {
                var dateToUpdate = CsvManager.ReadDateToUpdateFile(acumotorsDateToUpdateFilePath);
                return dateToUpdate;
            }
            else
                return null;
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

        protected void ProcessBrandsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brands list processed");
            StartOrPushPropertiesThread();
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
