using System;
using System.Collections.Generic;
using System.Linq;
using InvPricePremier;
using Databox.Libs.InvPricePremier;
using InvPricePremier.DataItems;
using InvPricePremier.Helpers;
using InvPricePremier.Compares;
using System.Text.RegularExpressions;
using System.IO;
using Ionic.Zip;
using System.Net;
using LumenWorks.Framework.IO.Csv;

namespace WheelsScraper
{
    public class InvPricePremier : BaseScraper
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
        private bool _processedSuccess;

        #endregion

        public InvPricePremier()
        {
            Name = "InvPricePremier";
            Url = "https://www.InvPricePremier.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

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

        protected override bool Login()
        {
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
                            BrandPremier = brandsAlignment.BrandInPremier
                        });
                    }
                }
            }

            lstProcessQueue.Add(new ProcessQueueItem { ItemType = 8 });

            StartOrPushPropertiesThread();
        }

        protected void ProcessPremierInventory(ProcessQueueItem pqi)
        {
            try
            {
                if (cancel)
                    return;

                MessagePrinter.PrintMessage("Downloading Premier inventory from FTP. Please wait...");
                bool premierProcessed = false;
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
                                    string jobberPrice = csv["JobberPrice"];
                                    string msrp = csv["MSRP"];
                                    string map = csv["Map"];
                                    string yourPrice = csv["Your Price"];

                                    foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
                                    {
                                        if (String.Equals(transferInfoItem.BrandPremier, brand, StringComparison.CurrentCultureIgnoreCase) &&
                                            String.Equals(transferInfoItem.ManufacturerPartNumberSce, manufacturerPartNumber, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            premierProcessed = true;

                                            transferInfoItem.ManufacturerName = brand;
                                            transferInfoItem.PartNumberPremier = partNumber;
                                            transferInfoItem.ManufacturerPartNumberPremier = manufacturerPartNumber;
                                            transferInfoItem.Jobber = jobberPrice;
                                            transferInfoItem.MSRP = msrp;

                                            string[] warehouses = warehouse.TrimEnd(';').Split(';');

                                            int quantity = 0;
                                            
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

                if (extSett.PremierPriceFile)
                    if (extSett.TransferInfoItems.Count > 0 && premierProcessed)
                        ScraperHelper.CreatePremierPriceFile(this, extSett);

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

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case 8:
                    act = ProcessPremierInventory;
                    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }
    }
}
