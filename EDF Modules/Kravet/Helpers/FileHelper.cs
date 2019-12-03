using Kravet.DataItems;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Kravet.Helpers
{
    public class FileHelper
    {
        private const char ComaSeparator = ',';
        private const string Separator = ",";

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static List<SceExportItem> ReadSceExportFile(string filePath)
        {
            List<SceExportItem> ftpItems = new List<SceExportItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        double.TryParse(csv["MSRP"], out double msrp);
                        double.TryParse(csv["Jobber"], out double jobber);
                        double.TryParse(csv["Web Price"], out double webPrice);
                        double.TryParse(csv["Cost Price"], out double costPrice);

                        SceExportItem item = new SceExportItem
                        {
                            ProdId = csv["Prodid"],
                            ProductType = csv["Product Type"],
                            MSRP = msrp,
                            Jobber = jobber,
                            WebPrice = webPrice,
                            CostPrice = costPrice,
                            PartNumber = csv["Part Number"],
                            Brand = csv["Brand"],
                            f124brand = csv["f124:brand"],
                            ManufacturerPartNumber = csv["Manufacturer Part Number"]
                        };

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
            }
        }

        public static List<FtpDataItem> ReadFtpFile(string filePath)
        {
            List<FtpDataItem> ftpItems = new List<FtpDataItem>();

            try
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                    {
                        csv.DefaultParseErrorAction = ParseErrorAction.AdvanceToNextLine;

                        while (csv.ReadNextRecord())
                        {
                            double.TryParse(csv["WHLS Price"], out double WHLSPrice);

                            FtpDataItem item = new FtpDataItem
                            {
                                ItemPartNumber = csv["Item"],
                                Brand = csv["Brand"],
                                WHLSPrice = WHLSPrice
                            };

                            ftpItems.Add(item);
                        }
                    }

                    return ftpItems;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"FTP file empty", e);
            }

            return null;
        }

        public static string CreatePriceUpdateFile(string filePath, List<PriceUpdateInfo> priceUpdateItems)
        {
            try
            {
                string headers = "action,Product Type,prodid,Part Number,MSRP,Jobber,Web Price,Cost Price";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (PriceUpdateInfo item in priceUpdateItems)
                {
                    string[] productArr = new string[8] { item.Action, item.ProductType, item.ProdId, item.PartNumber, item.MSRP.ToString(), item.Jobber.ToString(),
                        item.WebPrice.ToString(), item.CostPrice.ToString() };
                    for (int i = 0; i < productArr.Length; i++)
                        if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                            productArr[i] = StringToCSVCell(productArr[i]);

                    string product = String.Join(Separator, productArr);
                    sb.AppendLine(product);
                }

                File.WriteAllText(filePath, sb.ToString());

                return filePath;
            }
            catch
            {
                return null;
            }
        }

        private static string StringToCSVCell(string str)
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
