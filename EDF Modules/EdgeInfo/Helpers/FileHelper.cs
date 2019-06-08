using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EdgeInfo.DataItems;
using LumenWorks.Framework.IO.Csv;

namespace EdgeInfo.Helpers
{
    class FileHelper
    {
        private const char ComaSeparator = ',';
        private const string Separator = ",";

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static List<FtpDataItem> ReadFtpFile(string filePath)
        {
            List<FtpDataItem> ftpItems = new List<FtpDataItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        double.TryParse(csv["itCost"], out double itCost);
                        double.TryParse(csv["itCurrentPrice"], out double itCurrentPrice);

                        FtpDataItem item = new FtpDataItem
                        {
                            ItStyleCode = csv["itVendStyleCode"],
                            ItSize = csv["itSize"],
                            ItCost = itCost,
                            ItCurrentPrice = itCurrentPrice,
                            ItVendorId = csv["itVendorId"]
                        };

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
            }
        }

        public static List<ProcessingPeriod> ReadProcessingPeriod(string filePath)
        {
            List<ProcessingPeriod> items = new List<ProcessingPeriod>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        ProcessingPeriod item = new ProcessingPeriod
                        {
                            SupplierName = csv["Supplier Name"],
                            SupplierDeliveryTime = csv["Supplier Delivery Time (Ships In:)"],
                            EdgeName = csv["Edge Name"]
                        };

                        items.Add(item);
                    }
                }

                return items;
            }
        }

        public static List<SupplierItems> ReadSupplierFile(string filePath2)
        {
            List<SupplierItems> items = new List<SupplierItems>();

            using (StreamReader sr = File.OpenText(filePath2))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        SupplierItems item = new SupplierItems
                        {
                            SupplierName = csv["Supplier Name"],
                        };

                        items.Add(item);
                    }
                }

                return items;
            }
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
                            Supplier = csv["Supplier"],
                            Warehouse = csv["Warehouse"],
                            MSRP = msrp,
                            Jobber = jobber,
                            WebPrice = webPrice,
                            CostPrice = costPrice,
                            PartNumber = csv["Part Number"],
                            PickupAvailable = csv["pickup available"],
                            Specifications = csv["Specifications"],
                            ProcessingPeriod = csv["Processing Time"]
                        };

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
            }
        }

        public static string CreatePriceUpdateFile(string filePath, List<PriceUpdateInfo> priceUpdateItems)
        {
            try
            {
                string headers = "action,Product Type,prodid,Part Number,Supplier,Warehouse,MSRP,Jobber,Web Price,Cost Price,Processing Time,Specifications,Featured";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (PriceUpdateInfo item in priceUpdateItems)
                {
                    string[] productArr = new string[13] { item.Action, item.ProductType,item.ProdId,item.PartNumber,item.Supplier
                        ,item.Warehouse,item.MSRP.ToString(),item.Jobber.ToString()
                        ,item.WebPrice.ToString(),item.CostPrice.ToString(),item.ProcessingPeriod,item.Specification,item.Featured };
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

        public static string CreateInventoryUpdateFile(string filePath, List<InventoryUpdateInfo> priceUpdateItems)
        {
            try
            {
                string headers = "Part Number,Qty,Supplier,Warehouse";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (InventoryUpdateInfo item in priceUpdateItems)
                {
                    string[] productArr = new string[4] { item.PartNumber, item.Qty.ToString(),item.Supplier,item.Warehouse };
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
