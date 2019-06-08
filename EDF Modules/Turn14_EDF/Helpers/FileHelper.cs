using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Databox.Libs.Turn14_EDF;
using LumenWorks.Framework.IO.Csv;
using Turn14_EDF.DateItems;
using WheelsScraper;

namespace Turn14_EDF.Helpers
{
    class FileHelper
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
                string headers = "Brand,Manufacturer Part Number,EastStock,WestStock,Qty,Part NumberSce,CentralStock";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (TransferInfoItem item in settings.TransferInfoItems)
                {
                    if (string.IsNullOrEmpty(item.ManufacturerPartNumberTurn14))
                        continue;

                    string[] productArr = new string[7] { item.BrandSce, item.ManufacturerPartNumberSce, item.Turn14WarehouseEastStock, item.Turn14WarehouseWesStock, item.QuantityTurn14, item.PartNumberSce, item.Turn14WarehouseCentralStock };
                    for (int i = 0; i < productArr.Length; i++)
                        if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                            productArr[i] = StringToCSVCell(productArr[i]);

                    string product = String.Join(separator, productArr);
                    sb.AppendLine(product);
                }

                try
                {
                    File.WriteAllText(filePath, sb.ToString());

                    if (settings.DoBatch)
                    {
                        scraper.MessagePrinter.PrintMessage(FTP_UPLOADING);

                        string url = FtpHelper.UploadFileToFtp(scraper.Settings.FtpAddress, scraper.Settings.FtpUsername, scraper.Settings.FtpPassword, fileName, filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");

                            scraper.MessagePrinter.PrintMessage($"{FTP_UPLOADED}: " + urlForBatch);
                        }

                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                    else
                    {
                        scraper.MessagePrinter.PrintMessage($"file created in {filePath}");
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

                if (settings.DoBatch)
                {
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
                            var errMess = string.Format($"Batch upload by URL: {0} failed. Check your {1} connection",
                                url);
                            scraper.MessagePrinter.PrintMessage(errMess, ImportanceLevel.High);
                        }
                    }
                    else
                    {
                        scraper.MessagePrinter.PrintMessage($"File not upload to FTP");
                    }

                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                else
                {
                    scraper.MessagePrinter.PrintMessage($"'Price Update' file created - {filePath}");
                }
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"{e.Message} - when create Turn14 price file");
            }

        }

        #endregion

        public static List<SceExportItem> ReadSceExport(string filePath)
        {
            List<SceExportItem> sceItems = new List<SceExportItem>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        SceExportItem item = new SceExportItem
                        {
                            ProdId = int.Parse(csv["prodid"]),
                            ProductType = int.Parse(csv["Product Type"]),
                            Brand = csv["Brand"],
                            ManufacturerPartNumber = csv["Manufacturer Part Number"],
                            PartNumber = csv["Part Number"]
                        };

                        sceItems.Add(item);
                    }

                    return sceItems;
                }
            }
        }

        public static List<BrandsAlignment> ReadBrandsAlignments(string filePath)
        {
            List<BrandsAlignment> brandsList = new List<BrandsAlignment>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        BrandsAlignment item = new BrandsAlignment();

                        if (csv.GetFieldHeaders().Contains("Brand in SCE"))
                            item.BrandInSce = csv["Brand in SCE"];

                        if (csv.GetFieldHeaders().Contains("Brand in Turn14"))
                            item.BrandInTurn14 = csv["Brand in Turn14"];

                        //if (csv.GetFieldHeaders().Contains("Brand in Premier"))
                        //    item.BrandInPremier = csv["Brand in Premier"];

                        //if (csv.GetFieldHeaders().Contains("Mayer Long"))
                        //    item.BrandInMayerLong = csv["Mayer Long"];

                        //if (csv.GetFieldHeaders().Contains("Mayer Short"))
                        //    item.BrandInMayerShort = csv["Mayer Short"];

                        brandsList.Add(item);
                    }

                    return brandsList;
                }
            }
        }

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
