using OffRoadToyo.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffRoadToyo.Helpers
{
    public class FileHelper
    {
        private const string Separator = ",";

        public static string CreateFile(string filePath, List<FileItems> pinterestItems)
        {
            try
            {
                string headers = "Action,Product Type,Product Title,Anchor Text,Spider URL,Allow new brand,Brand,Description,META Description,META Keywords,General Image,Allow new category,Main Category,Sub Category," +
                    "Section Category,Specifications,Supplier,Re-Order Supplier,Warehouse,Processing Time,Combinable,Consolidatable,pickup available,Shipping Type,Shipping Carrier 1,Shipping Carrier 2,Shipping Carrier 3," +
                    "Shipping Carrier 4,Shipping Carrier 5,allowground,allow3day,allow2day,allownextday,allowinternational,Shipping Ground Rate,Shipping 3-Day Air Rate,Shipping 2-Day Air Rate,Shipping Next-Day Air Rate," +
                    "File Attachments,Videos,Search Tags,Part Number,Manufacturer Part Number,EAN,UPC,Warning Note,MSRP,Jobber,Web Price,Cost Price,Item Weight,Item Height,Item Width,Item Length," +
                    "Shipping Weight,Shipping Height,Shipping Width,Shipping Length";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (FileItems item in pinterestItems)
                {
                    string[] productArr = new string[58] { item.Action, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.AllowNewBrand, item.Brand, item.Description, item.METADescription,
                        item.METAKeywords, item.GeneralImage, item.AllowNewCategory, item.MainCategory, item.SubCategory, item.SectionCategory, item.Specifications, item.Supplier, item.ReOrderSupplier, item.Warehouse,
                        item.ProcessingTime, item.Combinable, item.Consolidatable, item.PickupAvailable, item.ShippingType, item.ShippingCarrier1, item.ShippingCarrier2, item.ShippingCarrier3, item.ShippingCarrier4,
                        item.ShippingCarrier5, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.Allowinternational, item.ShippingGroundRate, item.Shipping3DayAirRate, item.Shipping2DayAirRate,
                        item.ShippingNextDayAirRate, item.FileAttachments, item.Videos, item.SearchTags, item.PartNumber, item.ManufacturerPartNumber, item.EAN, item.UPC, item.WarningNote,
                        item.MSRP, item.Jobber, item.WebPrice, item.CostPrice, item.ItemWeight, item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth, item.ShippingLength};
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
