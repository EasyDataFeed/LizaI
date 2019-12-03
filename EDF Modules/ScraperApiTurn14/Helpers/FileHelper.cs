using LumenWorks.Framework.IO.Csv;
using ScraperApiTurn14.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScraperApiTurn14.Helpers
{
    class FileHelper
    {

        #region Constants

        private const char ComaCharSeparator = ',';
        private const string Separator = ",";

        #endregion

        #region Read Files

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

        public static string CreateScrapeFile(string filePath, List<ExtWareInfo> wares, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                string headers = "Title,Description,Brand,Part Number,ManufacturerNumber,MainCategory,SubCategory,Upc,Length,Height,Width,Weight" +
                                 ",Stock,ManufacturerStock,Msrp,Web Price,Cost Price,Jobber,General Images,Attachments,Fitments,Make,Model,Submodel,Engine,Start Year,End Year,VehicleId,YearID";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

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

                        string line = String.Join(Separator, productArr);

                        writer.WriteLine(line);
                    }
                }

                return filePath;
            }
            catch (Exception exc)
            {
                errorMsg = exc.Message;
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

        #endregion
    }
}
