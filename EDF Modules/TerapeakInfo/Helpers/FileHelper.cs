using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TerapeakInfo.DataItems;
using TerapeakInfo.Extensions;

namespace TerapeakInfo.Helpers
{
    public static class FileHelper
    {
        private const char ComaCharSeparator = ',';

        //public static string CreateFile(int fileCounter, List<ExtWareInfo> items)
        //{
        //    string fileName = $"TerapeakListingInfo{fileCounter}.csv";
        //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        //    string separator = ",";
        //    string headers = "Timestamp,ItemID,Product Title,Brand,SKU,Quantity,QuantitySold,Description,Categories,ConvertedCurrentPrice,OriginalRetailPrice" +
        //                     ",ListingStatus,HitCount,UserId,StoreName,Pictures,ShippingServiceCost,ShippingType,ListedShippingServiceCost" +
        //                     ",ItemCompatibilityCount,ManufacturerNumber,InterchangePartNumber,OtherPartNumber,Specifications" +
        //                     ",Make,Model,Year,Trim,Engine,Notes,Total Sales";

        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine(headers);

        //    foreach (ExtWareInfo ware in items)
        //    {
        //        string[] productArr = new string[31] { ware.Timestamp, ware.ItemNumber, ware.ProductTitle, ware.Brand, ware.PartNumber, ware.Quantity.ToString()
        //                                            , ware.QuantitySold,ware.Description,ware.Categories, ware.ConvertedCurrentPrice.ToString(), ware.OriginalRetailPrice, ware.ListingStatus
        //                                            , ware.HitCount, ware.UserId, ware.StoreName,ware.ImageUrl, ware.ShippingServiceCost, ware.ShippingType, ware.ListedShippingServiceCost
        //                                            , ware.ItemCompatibilityCount, ware.ManufacturerNumber, ware.InterchangePartNumber, ware.OtherPartNumber
        //                                            , ware.Specifications, ware.Make, ware.Model, ware.Year, ware.Trim, ware.Engine, ware.Notes, ware.TSales };
        //        for (int i = 0; i < productArr.Length; i++)
        //            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
        //                productArr[i] = StringToCSVCell(productArr[i]);

        //        string product = String.Join(separator, productArr);
        //        sb.AppendLine(product);
        //    }

        //    try
        //    {
        //        if (File.Exists(filePath))
        //            File.Delete(filePath);

        //        File.WriteAllText(filePath, sb.ToString());

        //        return filePath;
        //    }
        //    catch { }
        //    return null;
        //}

        public static string CreateScrapeFile(int fileCounter, List<ExtWareInfo> wares)
        {
            try
            {
                string fileName = $"TerapeakListingInfo{fileCounter}.csv";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string separator = ",";

                string headers = "Timestamp,ItemID,Product Title,Brand,SKU,Quantity,QuantitySold,Description,Categories,ConvertedCurrentPrice,OriginalRetailPrice" +
                                 ",ListingStatus,HitCount,UserId,StoreName,Pictures,ShippingServiceCost,ShippingType,ListedShippingServiceCost" +
                                 ",ItemCompatibilityCount,ManufacturerNumber,InterchangePartNumber,OtherPartNumber,Specifications" +
                                 ",Make,Model,Year,Trim,Engine,Notes,tTotal Sales,tItemSold,tAvarageSalePrice,tBids,tAverageShipping,tFormatStoreFixedPrice,tDate";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

                    foreach (ExtWareInfo ware in wares)
                    {
                        string[] productArr = new string[37] { ware.Timestamp, ware.ItemNumber, ware.ProductTitle, ware.Brand, ware.PartNumber, ware.Quantity.ToString()
                                                    , ware.QuantitySold,ware.Description,ware.Categories, ware.ConvertedCurrentPrice.ToString(), ware.OriginalRetailPrice, ware.ListingStatus
                                                    , ware.HitCount, ware.UserId, ware.StoreName,ware.ImageUrl, ware.ShippingServiceCost, ware.ShippingType, ware.ListedShippingServiceCost
                                                    , ware.ItemCompatibilityCount, ware.ManufacturerNumber, ware.InterchangePartNumber, ware.OtherPartNumber
                                                    , ware.Specifications, ware.Make, ware.Model, ware.Year, ware.Trim, ware.Engine, ware.Notes, ware.TSales, ware.ItemSold
                                                    , ware.AvarageSalePrice,ware.Bids,ware.AverageShipping,ware.FormatStoreFixedPrice,ware.Date };
                        for (int i = 0; i < productArr.Length; i++)
                            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                                productArr[i] = StringToCSVCell(productArr[i]);

                        string line = String.Join(separator, productArr);

                        writer.WriteLine(line);
                    }
                }

                return filePath;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        private static string StringToCSVCell(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

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
