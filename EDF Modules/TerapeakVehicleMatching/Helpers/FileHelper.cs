using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TerapeakVehicleMatching.DataItems;

namespace TerapeakVehicleMatching.Helpers
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
                                 ",Make,Model,Start Year,End Year,Trim,Engine,Notes,tTotal Sales,tItemSold,tAvarageSalePrice,tBids,tAverageShipping,tFormatStoreFixedPrice,tDate";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

                    foreach (ExtWareInfo ware in wares)
                    {
                        string[] productArr = new string[38] { ware.Timestamp, ware.ItemNumber, ware.ProductTitle, ware.Brand, ware.PartNumber, ware.Quantity.ToString()
                                                    , ware.QuantitySold,ware.Description,ware.Categories, ware.ConvertedCurrentPrice.ToString(), ware.OriginalRetailPrice, ware.ListingStatus
                                                    , ware.HitCount, ware.UserId, ware.StoreName,ware.ImageUrl, ware.ShippingServiceCost, ware.ShippingType, ware.ListedShippingServiceCost
                                                    , ware.ItemCompatibilityCount, ware.ManufacturerNumber, ware.InterchangePartNumber, ware.OtherPartNumber
                                                    , ware.Specifications, ware.Make, ware.Model, ware.StartYear.ToString(),ware.EndYear.ToString(), ware.Trim, ware.Engine, ware.Notes, ware.TSales, ware.ItemSold
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

        public static string CreateEndFile(string filePath, List<EndFile> wares)
        {
            try
            {
                string separator = ",";
                string headers = "Ebay Item Id,Sce PartNumber,Sce Web Price,Make,Model,Start Year SCE,End Year SCE,Start Year eBay,End Year eBay,Competitor,Ebay Brand,Ebay Category," +
                    "Ebay SKU,Total Sold Terapeak,Total Sold Ebay,Total Sales Terapeak,AvarageSalePrice,EbayPrice";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

                    foreach (EndFile ware in wares)
                    {
                        string[] productArr = new string[18] { ware.EbayItemId, ware.ScePartNumber, ware.SceWebPrice.ToString(), ware.Make, ware.Model
                            , ware.StartYearSCE.ToString(),  ware.EndYearSCE.ToString(),  ware.StartYearEbay.ToString(),  ware.EndYearEbay.ToString()
                            , ware.Competitor, ware.EbayBrand,ware.EbayCategory,   ware.EbaySku, ware.TTotalSold, ware.ETotalSold, ware.TTotalSales
                            , ware.AvarageSalePrice,ware.EbayPrice.ToString() };
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

        public static List<SceExportItem> ReadSceExport(string filePath)
        {
            List<SceExportItem> sceItems = new List<SceExportItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaCharSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        double.TryParse(csv["Web Price"], out double WebPrice);
                        int.TryParse(csv["prodid"], out int prodid);
                        int.TryParse(csv["Start Year"], out int StartYear);
                        int.TryParse(csv["End Year"], out int EndYear);

                        SceExportItem item = new SceExportItem
                        {
                            PartNumber = csv["Part Number"],
                            VehicleMake = csv["Vehicle Make"],
                            ProdId = prodid,
                            VehicleModel = csv["Vehicle Model"],
                            StartYear = StartYear,
                            EndYear = EndYear,
                            WebPrice = WebPrice
                        };

                        if (!string.IsNullOrEmpty(item.VehicleMake))
                            item.VehicleMake = item.VehicleMake.Trim();

                        if (!string.IsNullOrEmpty(item.VehicleModel))
                            item.VehicleModel = item.VehicleModel.Trim();


                        if (!string.IsNullOrEmpty(item.VehicleMake))
                        {
                            sceItems.Add(item);
                        }
                    }
                }

                return sceItems;
            }
        }

        public static List<TerapeakVehicleMatchingInfo> ReadTerapeakFileScraped(string filePath)
        {
            List<TerapeakVehicleMatchingInfo> terapeakItems = new List<TerapeakVehicleMatchingInfo>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaCharSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        TerapeakVehicleMatchingInfo item = new TerapeakVehicleMatchingInfo
                        {
                            ItemId = csv["ItemId"],
                            Bids = csv["Bids"],
                            ItemSold = csv["Total Sold"],
                            TSales = csv["Total Sales"],
                            AvarageSalePrice = csv["AvarageSalePrice"],
                            AverageShipping = csv["AverageShipping"],
                            FormatStoreFixedPrice = csv["FormatStoreFixedPrice"],
                            Date = csv["Date"]
                        };

                        terapeakItems.Add(item);
                    }
                }

                return terapeakItems;
            }
        }

        //							


        public static List<ExtWareInfo> ReadTerapeakFile(string filePath)
        {
            List<ExtWareInfo> terapeakItems = new List<ExtWareInfo>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaCharSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        int.TryParse(csv["Year"], out int Year);
                        double.TryParse(csv["ConvertedCurrentPrice"], out double ebayPrice);

                        ExtWareInfo item = new ExtWareInfo
                        { 
                            Make = csv["Make"],
                            Model = csv["Model"],
                            Year = Year,
                            StoreName = csv["StoreName"],
                            Brand = csv["Brand"],
                            Categories = csv["Categories"],
                            PartNumber = csv["SKU"],
                            ItemSold = csv["tItemSold"],
                            QuantitySold = csv["QuantitySold"],
                            TSales = csv["tTotal Sales"],
                            AvarageSalePrice = csv["tAvarageSalePrice"],
                            ConvertedCurrentPrice = ebayPrice
                        };

                        if (!string.IsNullOrEmpty(item.Make))
                            item.Make = item.Make.Trim();

                        if (!string.IsNullOrEmpty(item.Model))
                            item.Model = item.Model.Trim();

                        terapeakItems.Add(item);
                    }
                }

                return terapeakItems;
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
