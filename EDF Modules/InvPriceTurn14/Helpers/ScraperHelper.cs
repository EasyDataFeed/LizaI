using Databox.Libs.InvPriceTurn14;
using InvPriceTurn14.DataItems;
using System;
using System.IO;
using System.Text;
using WheelsScraper;

namespace InvPriceTurn14.Helpers
{
    public static class ScraperHelper
    {
        private const string FTP_UPLOADING = "Upload on FTP";
        private const string FTP_UPLOADED = "Uploaded";

        public static void CreateTurn14File(BaseScraper scraper, ExtSettings settings)
        {
            scraper.MessagePrinter.PrintMessage($"Create Turn14 Inventory File");

            if (settings.TransferInfoItems.Count > 0)
            {
                string fileName = "turn14Inventory.csv";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string separator = ",";
                string headers = "Brand,PartNumber,Manufacturer Part Number,EastStock,WestStock,MfrStock";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (TransferInfoItem item in settings.TransferInfoItems)
                {
                    if (string.IsNullOrEmpty(item.ManufacturerPartNumberTurn14))
                        continue;

                    string[] productArr = new string[6] { item.BrandSce, item.PartNumberTurn14,item.ManufacturerPartNumberSce, item.Turn14WarehouseEastStock, item.Turn14WarehouseWesStock, item.Turn14MfrStock };
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
            string headers = "Brand,Part Number,Manufacturer Part Number,Map,Cost,Retail,Jobber";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headers);

            //int counter = 0;

            foreach (TransferInfoItem item in settings.TransferInfoItems)
            {
                //if (string.IsNullOrEmpty(item.CostPrice) || string.IsNullOrEmpty(item.WebPrice) || string.IsNullOrEmpty(item.Msrp) || string.IsNullOrEmpty(item.Jobber))
                //    continue;

                //counter++;

                string[] productArr = new string[7] { item.BrandSce, item.PartNumberSce, item.ManufacturerPartNumberTurn14, item.WebPrice, item.CostPrice, item.Msrp, item.Jobber };
                for (int i = 0; i < productArr.Length; i++)
                    if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                        productArr[i] = StringToCSVCell(productArr[i]);

                string product = String.Join(separator, productArr);
                sb.AppendLine(product);
            }
            try
            {
                File.WriteAllText(filePath, sb.ToString());
                scraper.MessagePrinter.PrintMessage("File created");
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"{e.Message} - when create Turn14 price file");
            }

            //if (File.Exists(filePath))
            //    File.Delete(filePath);
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
