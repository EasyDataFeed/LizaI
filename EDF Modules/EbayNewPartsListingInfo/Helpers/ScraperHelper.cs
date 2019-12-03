using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Databox.Libs.EbayNewPartsListingInfo;
using WheelsScraper;

namespace EbayNewPartsListingInfo.Helpers
{
    public static class ScraperHelper
    {
        public static void CreateFullInfoFiles(BaseScraper scraper, ExtSettings extSett)
        {
            int fileCount = 0;
            //foreach (var waresList in extSett.AllWaresInfo)
            //{
            //    fileCount++;
            //    string fileName = $"DbVehicle{fileCount}.csv";
            //    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            //    string separator = ",";
            //    string headers = "Action, Product Type, Product Title, Anchor Text, Spider URL, Allow New Brand, Brand, Description, Additional Description, META Description, META Keywords" +
            //        ", General Image, Allow New Category, Main Category, Sub Category, Specifications, Distr Name, Part Number, Manufacturer Part Number, UPC, Application Specific Image" +
            //        ", Application Specifications, Warning Note, MSRP, Jobber, Web Price, Cost Price, Item Weight, Item Height, Item Width, Item Length" +
            //        ", Shipping Weight, Shipping Height, Shipping Width, Shipping Length, Data Source, Vehicle Make, Vehicle Model, Vehicle Sub Model, Start Year, End Year" +
            //        ", Engine, Aspiration, Cylinder Type, Drive Type, Brakes, ABS, Transmission, Doors, Body Type, Bed Type, Navigation, Climate Control, Radio, Heated Seats" +
            //        ", Sunroof, Vehicle Engine Options, Vehicle Interior Options, Vehicle Exterior Options, Vehicle Warning Note, Create Date, UpdateDate, DeleteDate, Create Date For Price" +
            //        ", Update Date For Price, Delete Date For Price ";

            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine(headers);

            //    foreach (ExtWareInfo ware in waresList)
            //    {
            //        string[] productArr = new string[66] { ware.Action, ware.ProductType, ware.ProductTitle, ware.AnchorText, ware.SpiderUrl, "1", ware.BrandName, ware.Description, ware.AdditionalDescription
            //                            , ware.METADescription, ware.METAKeywords, ware.GeneralImage, "1", ware.MainCategory, ware.SubCategory, ware.Specifications, ware.DistrName, ware.PartNumb
            //                            , ware.ManPartNumb, ware.UPC, ware.ApplicationSpecificImage, ware.ApplicationSpecifications, ware.WarningNote, ware.MSRP.ToString(), ware.Jobber
            //                            , ware.WebPrice.ToString(), ware.CostPrice.ToString(), ware.ItemWeight.ToString(), ware.ItemHeight.ToString(), ware.ItemWidth.ToString()
            //                            , ware.ItemLength.ToString(), ware.ShippingWeight.ToString(), ware.ShippingHeight.ToString(), ware.ShippingWidth.ToString(), ware.ShippingLength.ToString()
            //                            , ware.DataSource, ware.VehicleMake, ware.VehicleModel, ware.VehicleSubModel, ware.StartYear, ware.EndYear, ware.Engine, ware.Aspiration, ware.CylinderType
            //                            , ware.DriveType, ware.Brakes, ware.ABS, ware.Transmission, ware.Doors, ware.BodyType, ware.BedType, ware.Navigation, ware.ClimateControl, ware.Radio
            //                            , ware.HeatedSeats, ware.Sunroof, ware.VehicleEngineOptions, ware.VehicleInteriorOptions, ware.VehicleExteriorOptions, ware.VehicleWarningNote
            //                            , ware.CreateDate.ToString(), ware.UpdateDate.ToString(), ware.DeleteDate.ToString(), ware.CreateDatePrice.ToString(), ware.UpdateDateForPrice.ToString()
            //                            , ware.DeleteDatePrice.ToString()};
            //        for (int i = 0; i < productArr.Length; i++)
            //            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
            //            {
            //                if (productArr[i].Contains("\""))
            //                    productArr[i] = productArr[i].Replace("\"", "\"\"");
            //                if (productArr[i].Contains(","))
            //                    productArr[i] = $"\"{productArr[i]}\"";
            //                if (productArr[i].Contains("\r"))
            //                    productArr[i] = productArr[i].Replace("\r", string.Empty);
            //                if (productArr[i].Contains("\n"))
            //                    productArr[i] = productArr[i].Replace("\n", string.Empty);
            //            }
            //        string product = String.Join(separator, productArr);
            //        sb.AppendLine(product);
            //    }

            //    try
            //    {
            //        if (File.Exists(filePath))
            //            File.Delete(filePath);

            //        File.WriteAllText(filePath, sb.ToString());
            //    }
            //    catch { }

            //    scraper.MessagePrinter.PrintMessage($"File created {filePath}");
            //}
        }
    }
}
