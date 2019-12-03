using LumenWorks.Framework.IO.Csv;
using Pinterest.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WheelsScraper;

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
                            Brand = csv["Brand"],
                            MainCategory = csv["Main Category"],
                            SubCategory = csv["Sub Category"],
                            ProdId = csv["Prodid"],
                            Hidden = csv["Hidden"]
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

        public static string CreateSce(string filePath, List<SceExportItem> priceUpdateItems)
        {
            try
            {
                string headers = "General Image,Spider URL,Description,Product Title,Part Number,Brand,Main Category,Sub Category";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (SceExportItem item in priceUpdateItems)
                {
                    string[] productArr = new string[8] { item.GeneralImage, item.SpiderURL, item.Description, item.ProductTitle, item.PartNumber, item.Brand, item.MainCategory, item.SubCategory };
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
                string headers = "Part Number,Brand,Main Category,Sub Category,Spider URL,Pin Url,Changed,ProdId";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (PinterestItem item in pinterestItems)
                {
                    string[] productArr = new string[8] { item.PartNumber, item.Brand, item.MainCategory, item.SubCategory, item.SpiderURL, item.PinUrl, item.Changed , item.ProdId};
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
                        string prodId = string.Empty;

                        if (csv.GetFieldHeaders().Contains("Pin Url"))
                            pinUrl = csv.GetFieldHeaders().Contains("Pin Url") ? csv["Pin Url"] : string.Empty;

                        if (csv.GetFieldHeaders().Contains("Changed"))
                            changed = csv.GetFieldHeaders().Contains("Changed") ? csv["Changed"] : string.Empty;

                        if (csv.GetFieldHeaders().Contains("ProdId"))
                            prodId = csv.GetFieldHeaders().Contains("ProdId") ? csv["ProdId"] : string.Empty;

                        PinterestItem item = new PinterestItem()
                        {
                            PartNumber = csv["Part Number"],
                            Brand = csv["Brand"],
                            MainCategory = csv["Main Category"],
                            SubCategory = csv["Sub Category"],
                            PinUrl = pinUrl,
                            Changed = changed,
                            ProdId = prodId
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
