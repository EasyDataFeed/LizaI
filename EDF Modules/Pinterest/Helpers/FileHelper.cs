using LumenWorks.Framework.IO.Csv;
using Pinterest.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pinterest.Helpers
{
    class FileHelper
    {
        private const char ComaSeparator = ',';
        private const string Separator = ",";

        public static List<SceExportItem> ReadSceExportFile(string filePath)
        {
            List<SceExportItem> ftpItems = new List<SceExportItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        SceExportItem item = new SceExportItem
                        {
                            GeneralImage = csv["General Image"],
                            SpiderURL = csv["Spider URL"],
                            Description = csv["Description"],
                            ProductTitle = csv["Product Title"],
                            PartNumber = csv["Part Number"],
                            Brand = csv["Brand"]
                        };

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
            }
        }

        public static List<BrandItem> ReadBrandFile(string filePath)
        {
            List<BrandItem> brandItems = new List<BrandItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        BrandItem item = new BrandItem
                        {
                            Brand = csv["Brand"]
                        };

                        brandItems.Add(item);
                    }
                }

                return brandItems;
            }
        }

        public static string CreateSce(string filePath, List<SceExportItem> priceUpdateItems)
        {
            try
            {
                string headers = "General Image,Spider URL,Description,Product Title,Part Number,Brand";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (SceExportItem item in priceUpdateItems)
                {
                    string[] productArr = new string[6] { item.GeneralImage, item.SpiderURL, item.Description, item.ProductTitle, item.PartNumber, item.Brand };
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

        public static string CreatePinterestAddItem(string filePath, List<PinterestItem> pinterestItems)
        {
            try
            {
                string headers = "Part Number,Brand,Spider URL";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (PinterestItem item in pinterestItems)
                {
                    string[] productArr = new string[3] { item.PartNumber, item.Brand, item.SpiderURL};
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

        public static List<PinterestItem> ReadPinterestAddItem(string filePath)
        {
            List<PinterestItem> items = new List<PinterestItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        PinterestItem item = new PinterestItem()
                        {
                            PartNumber = csv["Part Number"],
                            Brand = csv["Brand"],
                        };

                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
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
