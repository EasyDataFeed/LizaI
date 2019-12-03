using LumenWorks.Framework.IO.Csv;
using ProductsToPinterest.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace ProductsToPinterest.Helpers
{
    class FileHelper
    {
        private const char ComaSeparator = ',';
        private const string Separator = ",";

        public static List<ExportItem> ReadExportFile(string filePath)
        {
            List<ExportItem> ftpItems = new List<ExportItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        ExportItem item = new ExportItem
                        {
                            GeneralImage = csv["General Image"],
                            SpiderURL = csv["Spider URL"],
                            Description = csv["Description"],
                            ProductTitle = csv["Product Title"],
                            PartNumber = csv["Part Number"],
                            Brand = csv["Brand"],
                            MainCategory = csv["Main Category"],
                            SubCategory = csv["Sub Category"]
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
                            Brand = csv["Brand"],
                            MainCategory = csv["Main Category"],
                            SubCategory = csv["Sub Category"]
                        };

                        brandItems.Add(item);
                    }
                }

                return brandItems;
            }
        }

        public static string CreatePinterestAddItem(string filePath, List<PinterestItem> pinterestItems)
        {
            try
            {
                string headers = "Part Number,Brand,Main Category,Sub Category,Pin Url,Changed,ProdId";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (PinterestItem item in pinterestItems)
                {
                    string[] productArr = new string[7] { item.PartNumber, item.Brand, item.MainCategory, item.SubCategory, item.PinUrl, item.Changed, item.ProdId };
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
                        string pinUrl = string.Empty;
                        string changed = string.Empty;

                        if (csv.GetFieldHeaders().Contains("Pin Url"))
                            pinUrl = csv.GetFieldHeaders().Contains("Pin Url") ? csv["Pin Url"] : string.Empty;

                        if (csv.GetFieldHeaders().Contains("Changed"))
                            changed = csv.GetFieldHeaders().Contains("Changed") ? csv["Changed"] : string.Empty;

                        PinterestItem item = new PinterestItem()
                        {
                            PartNumber = csv["Part Number"],
                            Brand = csv["Brand"],
                            MainCategory = csv["Main Category"],
                            SubCategory = csv["Sub Category"],
                            PinUrl = pinUrl,
                            Changed = changed
                        };

                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public static string UploadFileToFTP(BaseScraper scraper, string fileName, string filePath)
        {
            var settings = scraper.Settings;
            var resultUrl = "";
            resultUrl = FtpHelper.UploadFileToFtp(settings.FtpAddress.Trim(),
            settings.FtpUsername.Trim(),
            settings.FtpPassword.Trim(), fileName, filePath);
            if (!string.IsNullOrEmpty(resultUrl))
            {
                if (!string.IsNullOrEmpty(settings.ImagesPublicPath))
                {
                    var p1 = settings.ImagesPublicPath;
                    if (!p1.EndsWith("/"))
                        p1 += "/";
                    resultUrl = new Uri(new Uri(p1), Path.GetFileName(resultUrl)).ToString();
                }
            }
            return resultUrl;
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
