using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Databox.Libs.RPMoutletInventory;
using RPMoutletInventory.Compares;
using RPMoutletInventory.DataItems;
using RPMoutletInventory.DataItems.Meyer;
using RPMoutletInventory.Models;
using WheelsScraper;

namespace RPMoutletInventory.Helpers
{
    public static class ScraperHelper
    {
        private const string FTP_UPLOADING = "Upload on FTP";
        private const string FTP_UPLOADED = "Uploaded";

        #region Turn 14

        public static void CreateTurn14File(BaseScraper scraper, ExtSettings settings)
        {
            scraper.MessagePrinter.PrintMessage($"Create Turn14 Inventory File");

            if (settings.TransferInfoItems.Count > 0)
            {
                string fileName = "turn14Inventory.csv";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string separator = ",";
                string headers = "Brand,Manufacturer Part Number,EastStock,WestStock,MfrStock";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (TransferInfoItem item in settings.TransferInfoItems)
                {
                    if (string.IsNullOrEmpty(item.ManufacturerPartNumberTurn14))
                        continue;

                    string[] productArr = new string[5] { item.BrandSce, item.ManufacturerPartNumberSce, item.Turn14WarehouseEastStock, item.Turn14WarehouseWesStock, item.Turn14MfrStock };
                    for (int i = 0; i < productArr.Length; i++)
                        if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                            productArr[i] = StringToCSVCell(productArr[i]);

                    string product = String.Join(separator, productArr);
                    sb.AppendLine(product);
                }

                try
                {
                    File.WriteAllText(filePath, sb.ToString());

                    scraper.MessagePrinter.PrintMessage($"file created in {filePath}");

                    scraper.MessagePrinter.PrintMessage(FTP_UPLOADING);

                    string url = FtpHelper.UploadFileToFtp(scraper.Settings.FtpAddress, scraper.Settings.FtpUsername,
                        scraper.Settings.FtpPassword, fileName, filePath, true);
                    if (!string.IsNullOrEmpty(url))
                    {
                        string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");

                        scraper.MessagePrinter.PrintMessage($"{FTP_UPLOADED}: " + urlForBatch);
                    }
                }
                catch (Exception e)
                {
                    scraper.MessagePrinter.PrintMessage($"{e.Message} - when create Turn14 inventory file");
                }
            }
        }

        public static void CreateTurn14BatchPriceFile(BaseScraper scraper, ExtSettings settings)
        {
            scraper.MessagePrinter.PrintMessage($"Create update price file");

            string fileName = "Turn14PriceUpdate.csv";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string separator = ",";
            string headers = "Action,Product Type,prodid,Part Number,Web Price,Cost Price,MSRP,Jobber";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headers);

            int counter = 0;

            foreach (TransferInfoItem item in settings.TransferInfoItems)
            {
                if (string.IsNullOrEmpty(item.CostPrice) || string.IsNullOrEmpty(item.WebPrice) || string.IsNullOrEmpty(item.Msrp) || string.IsNullOrEmpty(item.Jobber))
                    continue;

                counter++;

                string[] productArr = new string[8] { "update", item.ProdTypeSce.ToString(), item.ProdIdSce.ToString(), item.PartNumberSce
                                                    , item.WebPrice, item.CostPrice, item.Msrp, item.Jobber };
                for (int i = 0; i < productArr.Length; i++)
                    if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                        productArr[i] = StringToCSVCell(productArr[i]);

                string product = String.Join(separator, productArr);
                sb.AppendLine(product);
            }
            try
            {
                File.WriteAllText(filePath, sb.ToString());

                scraper.MessagePrinter.PrintMessage(FTP_UPLOADING);

                string url = FtpHelper.UploadFileToFtp(scraper.Settings.FtpAddress, scraper.Settings.FtpUsername,
                    scraper.Settings.FtpPassword, fileName, filePath, true);
                if (!string.IsNullOrEmpty(url))
                {
                    string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");

                    scraper.MessagePrinter.PrintMessage(FTP_UPLOADED + ": " + urlForBatch);
                    try
                    {
                        var batchId = SceApiHelper.BatchUpdate(urlForBatch, scraper.Settings);
                        scraper.MessagePrinter.PrintMessage(
                            $"Batch uploaded. ID: {batchId}, {counter} products batched");
                    }
                    catch (Exception ex)
                    {
                        var errMess = string.Format("Batch upload by URL: {0} failed. Check your {1} connection", url);
                        scraper.MessagePrinter.PrintMessage(errMess, ImportanceLevel.High);
                    }
                }
                else
                {
                    scraper.MessagePrinter.PrintMessage($"File not upload to FTP");
                }
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"{e.Message} - when create Turn14 price file");
            }
            
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        #endregion

        #region Premier

        public static void CreatePremierInventoryFile(BaseScraper scraper, ExtSettings settings)
        {
            scraper.MessagePrinter.PrintMessage($"Create Premier Inventory File");

            string fileName = "PremierInventory.csv";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string separator = ",";
            string headers = "Brand,Manufacturer Part Number,Utah Warehouse,Kentucky Warehouse,Texas Warehouse" +
                             ",California Warehouse,Calgary AB Warehouse,Washington Warehouse,Ponoka AB Warehouse";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headers);

            foreach (TransferInfoItem item in settings.TransferInfoItems)
            {
                if (string.IsNullOrEmpty(item.ManufacturerPartNumberPremier))
                    continue;

                string[] productArr = new string[9] { item.BrandSce, item.ManufacturerPartNumberSce, item.PremierUtahWarehouse, item.PremierKentuckyWarehouse
                                                        , item.PremierTexasWarehouse, item.PremierCaliforniaWarehouse, item.PremierCalgaryABWarehouse, item.PremierWashingtonWarehouse
                                                        , item.PremierPonokaABWarehouse };
                for (int i = 0; i < productArr.Length; i++)
                    if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                        productArr[i] = StringToCSVCell(productArr[i]);

                string product = String.Join(separator, productArr);
                sb.AppendLine(product);
            }
            try
            {
                File.WriteAllText(filePath, sb.ToString());

                scraper.MessagePrinter.PrintMessage($"file created in {filePath}");

                scraper.MessagePrinter.PrintMessage(FTP_UPLOADING);
                string url = FtpHelper.UploadFileToFtp(scraper.Settings.FtpAddress, scraper.Settings.FtpUsername,
                    scraper.Settings.FtpPassword, fileName, filePath, true);
                if (!string.IsNullOrEmpty(url))
                {
                    string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");

                    scraper.MessagePrinter.PrintMessage($"{FTP_UPLOADED}: " + urlForBatch);
                }
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"{e.Message} - when create Premier inventory file");
            }
        }

        #endregion

        #region Meyer

        public static void CreateMeyerIntentoryFile(BaseScraper scraper, ExtSettings settings)
        {
            scraper.MessagePrinter.PrintMessage($"Create Meyer Inventory File");
            string fileName = "MeyerInventory.csv";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string separator = ",";
            string headers = "Brand Sce,Brand Meyer Long,Brand Meyer Short,Manufacturer Part Number,Stock,Warehouse";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headers);

            foreach (TransferInfoItem item in settings.TransferInfoItems)
            {
                if (string.IsNullOrEmpty(item.ManufacturerPartNumberMeyer))
                    continue;

                string[] productArr = new string[6] { item.BrandSce, item.BrandMeyerLong, item.BrandMeyerShort, item.ManufacturerPartNumberSce, item.QuantityMeyer, item.MeyerWarehouse };
                for (int i = 0; i < productArr.Length; i++)
                    if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                        productArr[i] = StringToCSVCell(productArr[i]);

                string product = String.Join(separator, productArr);
                sb.AppendLine(product);
            }

            try
            {
                File.WriteAllText(filePath, sb.ToString());

                scraper.MessagePrinter.PrintMessage($"file created in {filePath}");

                scraper.MessagePrinter.PrintMessage(FTP_UPLOADING);

                string url = FtpHelper.UploadFileToFtp(scraper.Settings.FtpAddress, scraper.Settings.FtpUsername,
                    scraper.Settings.FtpPassword, fileName, filePath, true);
                if (!string.IsNullOrEmpty(url))
                {
                    string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");

                    scraper.MessagePrinter.PrintMessage($"{FTP_UPLOADED}: " + urlForBatch);
                }
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"{e.Message} - when create Meyer inventory file");
            }
        }

        #endregion

        #region Ebay

        public static void CreateEbayIntentoryFile(BaseScraper scraper, ExtSettings settings, List<EbayItem> ebayItems)
        {
            scraper.MessagePrinter.PrintMessage($"Create Ebay Products File");
            string fileName = "EbayProducts.csv";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string separator = ",";
            string headers = "SCE Brand,SCE Manufacturer Part Number,Ebay Brand,Ebay Manufacturer Part Number,Ebay ID,Matched";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headers);

            foreach (EbayItem item in ebayItems)
            {
                string[] productArr = new string[6] { item.BrandSce, item.ManufacturerPartSce, item.Brand, item.PartNumber, item.EbayId, item.Found == true? "Yes" : "No" };
                for (int i = 0; i < productArr.Length; i++)
                    if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                        productArr[i] = StringToCSVCell(productArr[i]);

                string product = String.Join(separator, productArr);
                sb.AppendLine(product);
            }

            try
            {
                File.WriteAllText(filePath, sb.ToString());

                scraper.MessagePrinter.PrintMessage($"file created in {filePath}");

                //scraper.MessagePrinter.PrintMessage(FTP_UPLOADING);

                //string url = FtpHelper.UploadFileToFtp(scraper.Settings.FtpAddress, scraper.Settings.FtpUsername,
                //    scraper.Settings.FtpPassword, fileName, filePath, true);
                //if (!string.IsNullOrEmpty(url))
                //{
                //    string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");

                //    scraper.MessagePrinter.PrintMessage($"{FTP_UPLOADED}: " + urlForBatch);
                //}
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"{e.Message} - when create Meyer inventory file");
            }
        }

        #endregion
        public static string StringToCSVCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }
    }
}
