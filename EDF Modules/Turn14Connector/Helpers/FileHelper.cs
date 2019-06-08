#region using

using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Turn14Connector.DataItems;
using Turn14Connector.DataItems.SCE;

#endregion

namespace Turn14Connector.Helpers
{
    class FileHelper
    {

        #region Constants

        private const char ComaCharSeparator = ',';
        private const string Separator = ",";

        #endregion

        #region Read Files

        public static List<SceExportItem> ReadSceExport(string filePath)
        {
            List<SceExportItem> sceItems = new List<SceExportItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaCharSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        double.TryParse(csv["MSRP"], out double MSRP);
                        double.TryParse(csv["Web Price"], out double WebPrice);
                        double.TryParse(csv["Cost Price"], out double CostPrice);
                        double.TryParse(csv["Jobber"], out double Jobber);

                        SceExportItem item = new SceExportItem
                        {
                            PartNumber = csv["Part Number"],
                            ManufacturerPartNumber = csv["Manufacturer Part Number"],
                            ProdId = csv["prodid"],
                            ProductType = csv["Product Type"],
                            Brand = csv["Brand"],
                            MSRP = MSRP,
                            WebPrice = WebPrice,
                            CostPrice = CostPrice,
                            Jobber = Jobber
                        };

                        sceItems.Add(item);
                    }
                }

                return sceItems;
            }
        }

        public static List<BrandsAlignment> ReadBrandsAlignments(string filePath)
        {
            List<BrandsAlignment> brandsList = new List<BrandsAlignment>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ComaCharSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        BrandsAlignment item = new BrandsAlignment();

                        if (csv.GetFieldHeaders().Contains("Brand in SCE"))
                            item.BrandInSce = csv["Brand in SCE"];
                        else
                            throw new Exception("Miss 'Brand in SCE' column in Brand file");

                        if (csv.GetFieldHeaders().Contains("Brand in Turn14"))
                            item.BrandInTurn14 = csv["Brand in Turn14"];
                        else
                            throw new Exception("Miss 'Brand in Turn14' column in Brand file");

                        brandsList.Add(item);
                    }

                    return brandsList;
                }
            }
        }

        public static List<VehicleInfoForFitments> ReadVehicleInfoForFitments(string filePath)
        {
            List<VehicleInfoForFitments> vehicleInfo = new List<VehicleInfoForFitments>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        VehicleInfoForFitments infoForFitments = new VehicleInfoForFitments
                        {
                            MakeName = csv["makename"],
                            ModelName = csv["modelname"],
                            YearId = csv["yearid"],
                            SubModelName = csv["submodelname"],
                            Engine = csv["engine"],
                            Liter = csv["Liter"],
                            BlockType = csv["BlockType"],
                            Cylinders = csv["Cylinders"],
                            FuelTypeName = csv["FuelTypeName"],
                            FuelDeliveryTypeName = csv["FuelDeliveryTypeName"],
                            CC = csv["CC"],
                            VehicleId = csv["vehicleid"],
                        };

                        vehicleInfo.Add(infoForFitments);
                    }

                    return vehicleInfo;
                }
            }
        }

        #endregion

        #region Create Files

        public static string CreateScrapeFile(string filePath, List<ExtWareInfo> wares)
        {
            try
            {
                string headers = "Title,Description,Brand,Part Number,ManufacturerNumber,MainCategory,SubCategory,Upc,Length,Height,Width,Weight" +
                                 ",Stock,ManufacturerStock,Msrp,Web Price,Cost Price,Jobber,General Images,Attachments,Fitments,Make,Model,Submodel,Engine,Start Year,End Year,VehicleId,YearID";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (ExtWareInfo ware in wares)
                {
                    string[] productArr = new string[29] { ware.Title,ware.Description,ware.Brand,ware.PartNumber,ware.ManufacturerNumber
                        ,ware.MainCategory, ware.SubCategory, ware.Upc, ware.Length,ware.Height,ware.Width,ware.Weight,ware.Stock.ToString()
                        ,ware.ManufacturerStock.ToString(),ware.Msrp.ToString(),ware.WebPrice.ToString(),ware.CostPrice.ToString(),ware.Jober.ToString()
                        ,ware.GeneralImages,ware.Attachments,ware.Fitments,ware.Make,ware.Model,ware.Submodel,ware.Engine,ware.StartYear.ToString(),ware.EndYear.ToString()
                        ,ware.VehicleId.ToString(),ware.YearId.ToString() };
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

        public static string CreatePriceUpdateFile(string filePath, List<PriceUpdateInfo> priceUpdateItems)
        {
            try
            {
                string headers = "action,prodid,Product Type,Brand,Part Number,MSRP,Jobber,Web Price,Cost Price";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (PriceUpdateInfo item in priceUpdateItems)
                {
                    string[] productArr = new string[9] { item.Action, item.ProdId, item.ProductType,item.Brand,item.PartNumber
                        ,item.MSRP.ToString(),item.Jobber.ToString(),item.WebPrice.ToString(),item.CostPrice.ToString() };
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
                string headers = "Brand,Part Number,Manufacturer Part Number,Stock,Manufacturer Stock";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (InventoryUpdateInfo item in priceUpdateItems)
                {
                    string[] productArr = new string[5] { item.Brand, item.PartNumber, item.ManufacturerPartNumber, item.Stock.ToString(), item.ManufacturerStock.ToString() };
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

        #endregion

        #region Help Methods

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



        #endregion

    }
}
