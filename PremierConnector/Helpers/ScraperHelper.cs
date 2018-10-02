#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PremierConnector.DataItems;
using WheelsScraper;

#endregion

namespace PremierConnector.Helpers
{
    public static class ScraperHelper
    {
        private const string FTP_UPLOADING = "Upload on FTP";
        private const string FTP_UPLOADED = "Uploaded";

        #region Premier

        public static void CreatePremierInventoryFile(BaseScraper scraper, List<TransferInfoItem> items) 
        {
            scraper.MessagePrinter.PrintMessage($"Create Premier Inventory File");

            string fileName = "PremierInventory.csv";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string separator = ",";
            string headers = "Brand,Part Number,Utah Warehouse,Kentucky Warehouse,Texas Warehouse" +
                             ",California Warehouse,Calgary AB Warehouse,Washington Warehouse,Ponoka AB Warehouse";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headers);

            foreach (TransferInfoItem item in items)
            {
                if (string.IsNullOrEmpty(item.PartNumberPremier))
                    continue;

                string[] productArr = new string[9] { item.BrandPremier, item.PartNumberPremier, item.PremierUtahWarehouse, item.PremierKentuckyWarehouse
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
