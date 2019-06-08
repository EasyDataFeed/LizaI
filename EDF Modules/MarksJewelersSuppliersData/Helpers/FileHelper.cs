using LumenWorks.Framework.IO.Csv;
using MarksJewelersSuppliersData.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace MarksJewelersSuppliersData.Helpers
{
    public class FileHelper
    {
        private const string Separator = ",";
        private const char ComaSeparator = ',';
        private const int CsvReaderBufferSize = 1024 * 1024 * 100;

        public static string GetSettingsPath(string fileName = null)
        {
            return fileName == null ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory) : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static string CreateJHFile(string filePath, List<SceBatchItem> endFile)
        {
            try
            {
                string headers = "Action,Product Type,Product Title,Anchor Text,Spider URL,Sub Title,Brand,Description,META Description,META Keywords,General Image,Part Number,MSRP,Jobber,Web Price,Cost Price," +
                    "Supplier,Re-Order Supplier,Warehouse,Processing Time,Shipping Type,Shipping Carrier 1,allowground,allow3day,allow2day,allownextday,Shipping Ground Rate,Shipping Next-Day Air Rate,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height," +
                    "Shipping Width,Shipping Length,allow new category,allow new brand,Main Category,Sub Category,Section Category,Cross sell main cat 1,Cross sell sub cat 1,Cross sell sec cat 1,Cross sell main cat 2," +
                    "Cross sell sub cat 2,Cross sell sec cat 2,Cross sell main cat 3,Cross sell sub cat 3,Cross sell sec cat 3,Cross sell main cat 4,Cross sell sub cat 4,Cross sell sec cat 4," +
                    "Cross sell main cat 5,Cross sell sub cat 5,Cross sell sec cat 5,Cross sell main cat 6,Cross sell sub cat 6,Cross sell sec cat 6,Cross sell main cat 7,Cross sell sub cat 7," +
                    "Cross sell sec cat 7,Cross sell main cat 8,Cross sell sub cat 8,Cross sell sec cat 8,Cross sell main cat 9,Cross sell sub cat 9,Cross sell sec cat 9,Cross sell main cat 10," +
                    "Cross sell sub cat 10,Cross sell sec cat 10,Cross sell main cat 11,Cross sell sub cat 11,Cross sell sec cat 11,Cross sell main cat 12,Cross sell sub cat 12,Cross sell sec cat 12," +
                    "Cross sell main cat 13,Cross sell sub cat 13,Cross sell sec cat 13,Cross sell main cat 14,Cross sell sub cat 14,Cross sell sec cat 14,Cross sell main cat 15,Cross sell sub cat 15,Cross sell sec cat 15," +
                    "Cross sell main cat 16,Cross sell sub cat 16,Cross sell sec cat 16,Cross sell main cat 17,Cross sell sub cat 17,Cross sell sec cat 17,Cross sell main cat 18,Cross sell sub cat 18,Cross sell sec cat 18," +
                    "Cross sell main cat 19,Cross sell sub cat 19,Cross sell sec cat 19,Cross sell main cat 20,Cross sell sub cat 20,Cross sell sec cat 20,custom html above qty,Specifications";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

                    foreach (SceBatchItem item in endFile)
                    {
                        string[] productArr = new string[103] { item.Action, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.SubTitle, item.Brand, item.Description, item.METADescription,
                        item.METAKeywords, item.GeneralImage, item.PartNumber, item.MSRP.ToString(), item.Jobber.ToString(), item.WebPrice.ToString(), item.CostPrice.ToString(),
                        item.Supplier, item.ReOrderSupplier, item.Warehouse, item.ProcessingTime, item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.ShippingGroundRate, item.ShippingNextDayAirRate, item.ItemWeight,
                        item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth, item.ShippingLength, item.AllowNewCategory, item.AllowNewBrand, item.MainCategory,
                        item.SubCategory, item.SectionCategory, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.CrossSellMainCat2, item.CrossSellSubCat2,
                        item.CrossSellSecCat2, item.CrossSellMainCat3, item.CrossSellSubCat3, item.CrossSellSecCat3, item.CrossSellMainCat4, item.CrossSellSubCat4, item.CrossSellSecCat4, item.CrossSellMainCat5,
                        item.CrossSellSubCat5, item.CrossSellSecCat5, item.CrossSellMainCat6, item.CrossSellSubCat6, item.CrossSellSecCat6, item.CrossSellMainCat7, item.CrossSellSubCat7, item.CrossSellSecCat7,
                        item.CrossSellMainCat8, item.CrossSellSubCat8, item.CrossSellSecCat8, item.CrossSellMainCat9, item.CrossSellSubCat9, item.CrossSellSecCat9, item.CrossSellMainCat10, item.CrossSellSubCat10, item.CrossSellSecCat10,
                        item.CrossSellMainCat11, item.CrossSellSubCat11, item.CrossSellSecCat11, item.CrossSellMainCat12, item.CrossSellSubCat12, item.CrossSellSecCat12, item.CrossSellMainCat13, item.CrossSellSubCat13, item.CrossSellSecCat13,
                        item.CrossSellMainCat14, item.CrossSellSubCat14, item.CrossSellSecCat14, item.CrossSellMainCat15, item.CrossSellSubCat15, item.CrossSellSecCat15, item.CrossSellMainCat16, item.CrossSellSubCat16, item.CrossSellSecCat16,
                        item.CrossSellMainCat17, item.CrossSellSubCat17, item.CrossSellSecCat17, item.CrossSellMainCat18, item.CrossSellSubCat18, item.CrossSellSecCat18, item.CrossSellMainCat19, item.CrossSellSubCat19, item.CrossSellSecCat19,
                        item.CrossSellMainCat20, item.CrossSellSubCat20, item.CrossSellSecCat20, item.CustomHtmlAboveQty, item.Specifications};
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
                return null;
            }
        }

        public static string CreateNotValidJHFile(string filePath, List<SceBatchItem> endFile)
        {
            try
            {
                string headers = "Action,Product Type,Product Title,Anchor Text,Spider URL,Sub Title,Brand,Description,META Description,META Keywords,General Image,Part Number,MSRP,Jobber,Web Price,Cost Price," +
                    "Supplier,Re-Order Supplier,Warehouse,Processing Time,Shipping Type,Shipping Carrier 1,allowground,allow3day,allow2day,allownextday,Shipping Ground Rate,Shipping Next-Day Air Rate,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height," +
                    "Shipping Width,Shipping Length,allow new category,allow new brand,Main Category,Sub Category,Section Category,Cross sell main cat 1,Cross sell sub cat 1,Cross sell sec cat 1,Cross sell main cat 2," +
                    "Cross sell sub cat 2,Cross sell sec cat 2,Cross sell main cat 3,Cross sell sub cat 3,Cross sell sec cat 3,Cross sell main cat 4,Cross sell sub cat 4,Cross sell sec cat 4," +
                    "Cross sell main cat 5,Cross sell sub cat 5,Cross sell sec cat 5,Cross sell main cat 6,Cross sell sub cat 6,Cross sell sec cat 6,Cross sell main cat 7,Cross sell sub cat 7," +
                    "Cross sell sec cat 7,Cross sell main cat 8,Cross sell sub cat 8,Cross sell sec cat 8,Cross sell main cat 9,Cross sell sub cat 9,Cross sell sec cat 9,Cross sell main cat 10," +
                    "Cross sell sub cat 10,Cross sell sec cat 10,Cross sell main cat 11,Cross sell sub cat 11,Cross sell sec cat 11,Cross sell main cat 12,Cross sell sub cat 12,Cross sell sec cat 12," +
                    "Cross sell main cat 13,Cross sell sub cat 13,Cross sell sec cat 13,Cross sell main cat 14,Cross sell sub cat 14,Cross sell sec cat 14,Cross sell main cat 15,Cross sell sub cat 15,Cross sell sec cat 15," +
                    "Cross sell main cat 16,Cross sell sub cat 16,Cross sell sec cat 16,Cross sell main cat 17,Cross sell sub cat 17,Cross sell sec cat 17,Cross sell main cat 18,Cross sell sub cat 18,Cross sell sec cat 18," +
                    "Cross sell main cat 19,Cross sell sub cat 19,Cross sell sec cat 19,Cross sell main cat 20,Cross sell sub cat 20,Cross sell sec cat 20,Specifications";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

                    foreach (SceBatchItem item in endFile)
                    {
                        string[] productArr = new string[102] { item.Action, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.SubTitle, item.Brand, item.Description, item.METADescription,
                        item.METAKeywords, item.GeneralImage, item.PartNumber, item.MSRP.ToString(), item.Jobber.ToString(), item.WebPrice.ToString(), item.CostPrice.ToString(),
                        item.Supplier, item.ReOrderSupplier, item.Warehouse, item.ProcessingTime, item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.ShippingGroundRate, item.ShippingNextDayAirRate, item.ItemWeight,
                        item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth, item.ShippingLength, item.AllowNewCategory, item.AllowNewBrand, item.MainCategory,
                        item.SubCategory, item.SectionCategory, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.CrossSellMainCat2, item.CrossSellSubCat2,
                        item.CrossSellSecCat2, item.CrossSellMainCat3, item.CrossSellSubCat3, item.CrossSellSecCat3, item.CrossSellMainCat4, item.CrossSellSubCat4, item.CrossSellSecCat4, item.CrossSellMainCat5,
                        item.CrossSellSubCat5, item.CrossSellSecCat5, item.CrossSellMainCat6, item.CrossSellSubCat6, item.CrossSellSecCat6, item.CrossSellMainCat7, item.CrossSellSubCat7, item.CrossSellSecCat7,
                        item.CrossSellMainCat8, item.CrossSellSubCat8, item.CrossSellSecCat8, item.CrossSellMainCat9, item.CrossSellSubCat9, item.CrossSellSecCat9, item.CrossSellMainCat10, item.CrossSellSubCat10, item.CrossSellSecCat10,
                        item.CrossSellMainCat11, item.CrossSellSubCat11, item.CrossSellSecCat11, item.CrossSellMainCat12, item.CrossSellSubCat12, item.CrossSellSecCat12, item.CrossSellMainCat13, item.CrossSellSubCat13, item.CrossSellSecCat13,
                        item.CrossSellMainCat14, item.CrossSellSubCat14, item.CrossSellSecCat14, item.CrossSellMainCat15, item.CrossSellSubCat15, item.CrossSellSecCat15, item.CrossSellMainCat16, item.CrossSellSubCat16, item.CrossSellSecCat16,
                        item.CrossSellMainCat17, item.CrossSellSubCat17, item.CrossSellSecCat17, item.CrossSellMainCat18, item.CrossSellSubCat18, item.CrossSellSecCat18, item.CrossSellMainCat19, item.CrossSellSubCat19, item.CrossSellSecCat19,
                        item.CrossSellMainCat20, item.CrossSellSubCat20, item.CrossSellSecCat20,item.Specifications};
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
                return null;
            }
        }

        public static string CreateTHFile(string filePath, List<SceBatchItem> endFile)
        {
            try
            {
                string headers = "Action,Product Type,Product Title,Anchor Text,Spider URL,Sub Title,Brand,Description,META Description,META Keywords,General Image,Part Number,MSRP,Jobber,Web Price,Cost Price," +
                    "Supplier,Re-Order Supplier,Warehouse,Processing Time,Shipping Type,Shipping Carrier 1,allowground,allow3day,allow2day,allownextday,Shipping Ground Rate,Shipping Next-Day Air Rate,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height," +
                    "Shipping Width,Shipping Length,allow new category,allow new brand,Main Category,Sub Category,Section Category,Cross sell main cat 1,Cross sell sub cat 1,Cross sell sec cat 1,Cross sell main cat 2," +
                    "Cross sell sub cat 2,Cross sell sec cat 2,Cross sell main cat 3,Cross sell sub cat 3,Cross sell sec cat 3,Cross sell main cat 4,Cross sell sub cat 4,Cross sell sec cat 4," +
                    "Cross sell main cat 5,Cross sell sub cat 5,Cross sell sec cat 5,Cross sell main cat 6,Cross sell sub cat 6,Cross sell sec cat 6,Cross sell main cat 7,Cross sell sub cat 7," +
                    "Cross sell sec cat 7,Cross sell main cat 8,Cross sell sub cat 8,Cross sell sec cat 8,Cross sell main cat 9,Cross sell sub cat 9,Cross sell sec cat 9,Cross sell main cat 10," +
                    "Cross sell sub cat 10,Cross sell sec cat 10,Cross sell main cat 11,Cross sell sub cat 11,Cross sell sec cat 11,Cross sell main cat 12,Cross sell sub cat 12,Cross sell sec cat 12," +
                    "Cross sell main cat 13,Cross sell sub cat 13,Cross sell sec cat 13,Cross sell main cat 14,Cross sell sub cat 14,Cross sell sec cat 14,Cross sell main cat 15,Cross sell sub cat 15,Cross sell sec cat 15," +
                    "Cross sell main cat 16,Cross sell sub cat 16,Cross sell sec cat 16,Cross sell main cat 17,Cross sell sub cat 17,Cross sell sec cat 17,Cross sell main cat 18,Cross sell sub cat 18,Cross sell sec cat 18," +
                    "Cross sell main cat 19,Cross sell sub cat 19,Cross sell sec cat 19,Cross sell main cat 20,Cross sell sub cat 20,Cross sell sec cat 20,Cross sell main cat 21,Cross sell sub cat 21,Cross sell sec cat 21," +
                    "Cross sell main cat 22,Cross sell sub cat 22,Cross sell sec cat 22,Cross sell main cat 23,Cross sell sub cat 23,Cross sell sec cat 23,Cross sell main cat 24,Cross sell sub cat 24,Cross sell sec cat 24," +
                    "Cross sell main cat 25,Cross sell sub cat 25,Cross sell sec cat 25,Cross sell main cat 26,Cross sell sub cat 26,Cross sell sec cat 26,Cross sell main cat 27,Cross sell sub cat 27,Cross sell sec cat 27," +
                    "Cross sell main cat 28,Cross sell sub cat 28,Cross sell sec cat 28,Cross sell main cat 29,Cross sell sub cat 29,Cross sell sec cat 29,Cross sell main cat 30,Cross sell sub cat 30,Cross sell sec cat 30," +
                    "Cross sell main cat 31,Cross sell sub cat 31,Cross sell sec cat 31,Cross sell main cat 32,Cross sell sub cat 32,Cross sell sec cat 32,Cross sell main cat 33,Cross sell sub cat 33,Cross sell sec cat 33," +
                    "Cross sell main cat 34,Cross sell sub cat 34,Cross sell sec cat 34,custom html above qty,Specifications";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

                    foreach (SceBatchItem item in endFile)
                    {
                        string[] productArr = new string[145] { item.Action, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.SubTitle, item.Brand, item.Description, item.METADescription,
                        item.METAKeywords, item.GeneralImage, item.PartNumber, item.MSRP.ToString(), item.Jobber.ToString(), item.WebPrice.ToString(), item.CostPrice.ToString(),
                        item.Supplier, item.ReOrderSupplier, item.Warehouse, item.ProcessingTime, item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.ShippingGroundRate, item.ShippingNextDayAirRate, item.ItemWeight,
                        item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth, item.ShippingLength, item.AllowNewCategory, item.AllowNewBrand, item.MainCategory,
                        item.SubCategory, item.SectionCategory, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.CrossSellMainCat2, item.CrossSellSubCat2,
                        item.CrossSellSecCat2, item.CrossSellMainCat3, item.CrossSellSubCat3, item.CrossSellSecCat3, item.CrossSellMainCat4, item.CrossSellSubCat4, item.CrossSellSecCat4, item.CrossSellMainCat5,
                        item.CrossSellSubCat5, item.CrossSellSecCat5, item.CrossSellMainCat6, item.CrossSellSubCat6, item.CrossSellSecCat6, item.CrossSellMainCat7, item.CrossSellSubCat7, item.CrossSellSecCat7,
                        item.CrossSellMainCat8, item.CrossSellSubCat8, item.CrossSellSecCat8, item.CrossSellMainCat9, item.CrossSellSubCat9, item.CrossSellSecCat9, item.CrossSellMainCat10, item.CrossSellSubCat10, item.CrossSellSecCat10,
                        item.CrossSellMainCat11, item.CrossSellSubCat11, item.CrossSellSecCat11, item.CrossSellMainCat12, item.CrossSellSubCat12, item.CrossSellSecCat12, item.CrossSellMainCat13, item.CrossSellSubCat13, item.CrossSellSecCat13,
                        item.CrossSellMainCat14, item.CrossSellSubCat14, item.CrossSellSecCat14, item.CrossSellMainCat15, item.CrossSellSubCat15, item.CrossSellSecCat15, item.CrossSellMainCat16, item.CrossSellSubCat16, item.CrossSellSecCat16,
                        item.CrossSellMainCat17, item.CrossSellSubCat17, item.CrossSellSecCat17, item.CrossSellMainCat18, item.CrossSellSubCat18, item.CrossSellSecCat18, item.CrossSellMainCat19, item.CrossSellSubCat19, item.CrossSellSecCat19,
                        item.CrossSellMainCat20, item.CrossSellSubCat20, item.CrossSellSecCat20, item.CrossSellMainCat21, item.CrossSellSubCat21, item.CrossSellSecCat21, item.CrossSellMainCat22, item.CrossSellSubCat22, item.CrossSellSecCat22,
                        item.CrossSellMainCat23, item.CrossSellSubCat23, item.CrossSellSecCat23, item.CrossSellMainCat24, item.CrossSellSubCat24, item.CrossSellSecCat24, item.CrossSellMainCat25, item.CrossSellSubCat25, item.CrossSellSecCat25,
                        item.CrossSellMainCat26, item.CrossSellSubCat26, item.CrossSellSecCat26, item.CrossSellMainCat27, item.CrossSellSubCat27, item.CrossSellSecCat27, item.CrossSellMainCat28, item.CrossSellSubCat28, item.CrossSellSecCat28,
                        item.CrossSellMainCat29, item.CrossSellSubCat29, item.CrossSellSecCat29, item.CrossSellMainCat30, item.CrossSellSubCat30, item.CrossSellSecCat30, item.CrossSellMainCat31, item.CrossSellSubCat31, item.CrossSellSecCat31,
                        item.CrossSellMainCat32, item.CrossSellSubCat32, item.CrossSellSecCat32, item.CrossSellMainCat33, item.CrossSellSubCat33, item.CrossSellSecCat33, item.CrossSellMainCat34, item.CrossSellSubCat34, item.CrossSellSecCat34, item.CustomHtmlAboveQty, item.Specifications};
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
                return null;
            }
        }

        public static string CreateNotValidTHFile(string filePath, List<SceBatchItem> endFile)
        {
            try
            {
                string headers = "Action,Product Type,Product Title,Anchor Text,Spider URL,Sub Title,Brand,Description,META Description,META Keywords,General Image,Part Number,MSRP,Jobber,Web Price,Cost Price," +
                    "Supplier,Re-Order Supplier,Warehouse,Processing Time,Shipping Type,Shipping Carrier 1,allowground,allow3day,allow2day,allownextday,Shipping Ground Rate,Shipping Next-Day Air Rate,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height," +
                    "Shipping Width,Shipping Length,allow new category,allow new brand,Main Category,Sub Category,Section Category,Cross sell main cat 1,Cross sell sub cat 1,Cross sell sec cat 1,Cross sell main cat 2," +
                    "Cross sell sub cat 2,Cross sell sec cat 2,Cross sell main cat 3,Cross sell sub cat 3,Cross sell sec cat 3,Cross sell main cat 4,Cross sell sub cat 4,Cross sell sec cat 4," +
                    "Cross sell main cat 5,Cross sell sub cat 5,Cross sell sec cat 5,Cross sell main cat 6,Cross sell sub cat 6,Cross sell sec cat 6,Cross sell main cat 7,Cross sell sub cat 7," +
                    "Cross sell sec cat 7,Cross sell main cat 8,Cross sell sub cat 8,Cross sell sec cat 8,Cross sell main cat 9,Cross sell sub cat 9,Cross sell sec cat 9,Cross sell main cat 10," +
                    "Cross sell sub cat 10,Cross sell sec cat 10,Cross sell main cat 11,Cross sell sub cat 11,Cross sell sec cat 11,Cross sell main cat 12,Cross sell sub cat 12,Cross sell sec cat 12," +
                    "Cross sell main cat 13,Cross sell sub cat 13,Cross sell sec cat 13,Cross sell main cat 14,Cross sell sub cat 14,Cross sell sec cat 14,Cross sell main cat 15,Cross sell sub cat 15,Cross sell sec cat 15," +
                    "Cross sell main cat 16,Cross sell sub cat 16,Cross sell sec cat 16,Cross sell main cat 17,Cross sell sub cat 17,Cross sell sec cat 17,Cross sell main cat 18,Cross sell sub cat 18,Cross sell sec cat 18," +
                    "Cross sell main cat 19,Cross sell sub cat 19,Cross sell sec cat 19,Cross sell main cat 20,Cross sell sub cat 20,Cross sell sec cat 20,Cross sell main cat 21,Cross sell sub cat 21,Cross sell sec cat 21," +
                    "Cross sell main cat 22,Cross sell sub cat 22,Cross sell sec cat 22,Cross sell main cat 23,Cross sell sub cat 23,Cross sell sec cat 23,Cross sell main cat 24,Cross sell sub cat 24,Cross sell sec cat 24," +
                    "Cross sell main cat 25,Cross sell sub cat 25,Cross sell sec cat 25,Cross sell main cat 26,Cross sell sub cat 26,Cross sell sec cat 26,Cross sell main cat 27,Cross sell sub cat 27,Cross sell sec cat 27," +
                    "Cross sell main cat 28,Cross sell sub cat 28,Cross sell sec cat 28,Cross sell main cat 29,Cross sell sub cat 29,Cross sell sec cat 29,Cross sell main cat 30,Cross sell sub cat 30,Cross sell sec cat 30," +
                    "Cross sell main cat 31,Cross sell sub cat 31,Cross sell sec cat 31,Cross sell main cat 32,Cross sell sub cat 32,Cross sell sec cat 32,Cross sell main cat 33,Cross sell sub cat 33,Cross sell sec cat 33," +
                    "Cross sell main cat 34,Cross sell sub cat 34,Cross sell sec cat 34,custom html above qty,Specifications";

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(headers);

                    foreach (SceBatchItem item in endFile)
                    {
                        string[] productArr = new string[145] { item.Action, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.SubTitle, item.Brand, item.Description, item.METADescription,
                        item.METAKeywords, item.GeneralImage, item.PartNumber, item.MSRP.ToString(), item.Jobber.ToString(), item.WebPrice.ToString(), item.CostPrice.ToString(),
                        item.Supplier, item.ReOrderSupplier, item.Warehouse, item.ProcessingTime, item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.ShippingGroundRate, item.ShippingNextDayAirRate, item.ItemWeight,
                        item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth, item.ShippingLength, item.AllowNewCategory, item.AllowNewBrand, item.MainCategory,
                        item.SubCategory, item.SectionCategory, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.CrossSellMainCat2, item.CrossSellSubCat2,
                        item.CrossSellSecCat2, item.CrossSellMainCat3, item.CrossSellSubCat3, item.CrossSellSecCat3, item.CrossSellMainCat4, item.CrossSellSubCat4, item.CrossSellSecCat4, item.CrossSellMainCat5,
                        item.CrossSellSubCat5, item.CrossSellSecCat5, item.CrossSellMainCat6, item.CrossSellSubCat6, item.CrossSellSecCat6, item.CrossSellMainCat7, item.CrossSellSubCat7, item.CrossSellSecCat7,
                        item.CrossSellMainCat8, item.CrossSellSubCat8, item.CrossSellSecCat8, item.CrossSellMainCat9, item.CrossSellSubCat9, item.CrossSellSecCat9, item.CrossSellMainCat10, item.CrossSellSubCat10, item.CrossSellSecCat10,
                        item.CrossSellMainCat11, item.CrossSellSubCat11, item.CrossSellSecCat11, item.CrossSellMainCat12, item.CrossSellSubCat12, item.CrossSellSecCat12, item.CrossSellMainCat13, item.CrossSellSubCat13, item.CrossSellSecCat13,
                        item.CrossSellMainCat14, item.CrossSellSubCat14, item.CrossSellSecCat14, item.CrossSellMainCat15, item.CrossSellSubCat15, item.CrossSellSecCat15, item.CrossSellMainCat16, item.CrossSellSubCat16, item.CrossSellSecCat16,
                        item.CrossSellMainCat17, item.CrossSellSubCat17, item.CrossSellSecCat17, item.CrossSellMainCat18, item.CrossSellSubCat18, item.CrossSellSecCat18, item.CrossSellMainCat19, item.CrossSellSubCat19, item.CrossSellSecCat19,
                        item.CrossSellMainCat20, item.CrossSellSubCat20, item.CrossSellSecCat20, item.CrossSellMainCat21, item.CrossSellSubCat21, item.CrossSellSecCat21, item.CrossSellMainCat22, item.CrossSellSubCat22, item.CrossSellSecCat22,
                        item.CrossSellMainCat23, item.CrossSellSubCat23, item.CrossSellSecCat23, item.CrossSellMainCat24, item.CrossSellSubCat24, item.CrossSellSecCat24, item.CrossSellMainCat25, item.CrossSellSubCat25, item.CrossSellSecCat25,
                        item.CrossSellMainCat26, item.CrossSellSubCat26, item.CrossSellSecCat26, item.CrossSellMainCat27, item.CrossSellSubCat27, item.CrossSellSecCat27, item.CrossSellMainCat28, item.CrossSellSubCat28, item.CrossSellSecCat28,
                        item.CrossSellMainCat29, item.CrossSellSubCat29, item.CrossSellSecCat29, item.CrossSellMainCat30, item.CrossSellSubCat30, item.CrossSellSecCat30, item.CrossSellMainCat31, item.CrossSellSubCat31, item.CrossSellSecCat31,
                        item.CrossSellMainCat32, item.CrossSellSubCat32, item.CrossSellSecCat32, item.CrossSellMainCat33, item.CrossSellSubCat33, item.CrossSellSecCat33, item.CrossSellMainCat34, item.CrossSellSubCat34, item.CrossSellSecCat34, item.CustomHtmlAboveQty, item.Specifications};
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
                return null;
            }
        }

        public static List<JohnHardyDataItem> ReadSupplierJohnHardyFile(string filePath)
        {
            //Dictionary<object, object> fileObjects = new Dictionary<object, object>();
            List<JohnHardyDataItem> suppliers = new List<JohnHardyDataItem>();
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                List<string> headers = new List<string>();
                bool headersLine = true;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    if (headersLine)
                    {
                        (parser.ReadFields() ?? throw new InvalidOperationException()).ToList().ForEach(i => headers.Add(i));
                        headersLine = false;
                        //if (!headers.Contains($"Email"))
                        //{
                        //    throw new Exception("Not Found 'email' in input file");
                        //}
                    }
                    var fields = (parser.ReadFields() ?? throw new InvalidOperationException()).ToList();
                    double.TryParse(fields[headers.IndexOf("Retail Price")], out double retailPrice);
                    double.TryParse(fields[headers.IndexOf("Diamond Carat Weight")], out double diamondCaratWeight);

                    JohnHardyDataItem suppliersFile = new JohnHardyDataItem();

                    if (headers.Contains($"Style Long Description"))
                        suppliersFile.StyleLongDescription = fields[headers.IndexOf("Style Long Description")];

                    if (headers.Contains($"Vendor Name"))
                        suppliersFile.VendorName = fields[headers.IndexOf("Vendor Name")];

                    if (headers.Contains($"Product Description"))
                        suppliersFile.ProductDescription = fields[headers.IndexOf("Product Description")];

                    if (headers.Contains($"Style No"))
                        suppliersFile.StyleNo = fields[headers.IndexOf("Style No")];

                    if (headers.Contains($"Retail Price"))
                        suppliersFile.RetailPrice = retailPrice;

                    var index = 0;
                    List<int> indexesMain = new List<int>();
                    List<int> indexesMain2 = new List<int>();
                    List<int> indexesBack = new List<int>();
                    List<int> indexesBack2 = new List<int>();
                    List<int> indexesEdge = new List<int>();
                    List<int> indexesEdge2 = new List<int>();
                    List<int> indexesProfile = new List<int>();
                    List<int> indexesTop = new List<int>();
                    List<int> indexesModel = new List<int>();

                    foreach (var header in headers)
                    {
                        if (header == "Main")
                        {
                            indexesMain.Add(index);
                        }

                        if (header == "Main 2")
                        {
                            indexesMain2.Add(index);
                        }

                        if (header == "Back")
                        {
                            indexesBack.Add(index);
                        }

                        if (header == "Back 2")
                        {
                            indexesBack2.Add(index);
                        }

                        if (header == "Edge")
                        {
                            indexesEdge.Add(index);
                        }

                        if (header == "Edge 2")
                        {
                            indexesEdge2.Add(index);
                        }

                        if (header == "Profile")
                        {
                            indexesProfile.Add(index);
                        }

                        if (header == "Top")
                        {
                            indexesTop.Add(index);
                        }

                        if (header == "Model")
                        {
                            indexesModel.Add(index);
                        }

                        index++;
                    }

                    var colectionIndexMain = 0;

                    foreach (var indexMain in indexesMain)
                    {
                        if (colectionIndexMain == 0)
                            suppliersFile.MainCol1 = fields[indexMain];
                        else if (colectionIndexMain == 1)
                            suppliersFile.MainCol2 = fields[indexMain];
                        else if (colectionIndexMain == 2)
                            suppliersFile.MainCol3 = fields[indexMain];

                        colectionIndexMain++;
                    }

                    var colectionIndexMain2 = 0;

                    foreach (var indexMain2 in indexesMain2)
                    {
                        if (colectionIndexMain2 == 0)
                            suppliersFile.Main2Col1 = fields[indexMain2];
                        else if (colectionIndexMain2 == 1)
                            suppliersFile.Main2Col2 = fields[indexMain2];
                        else if (colectionIndexMain2 == 2)
                            suppliersFile.Main2Col3 = fields[indexMain2];
                        else if (colectionIndexMain2 == 3)
                            suppliersFile.Main2Col4 = fields[indexMain2];

                        colectionIndexMain2++;
                    }

                    var colectionIndexBack = 0;

                    foreach (var indexBack in indexesBack)
                    {
                        if (colectionIndexBack == 0)
                            suppliersFile.BackCol1 = fields[indexBack];
                        else if (colectionIndexBack == 1)
                            suppliersFile.BackCol2 = fields[indexBack];

                        colectionIndexBack++;
                    }

                    var colectionIndexBack2 = 0;

                    foreach (var indexBack2 in indexesBack2)
                    {
                        if (colectionIndexBack2 == 0)
                            suppliersFile.Back2Col1 = fields[indexBack2];
                        else if (colectionIndexBack2 == 1)
                            suppliersFile.Back2Col2 = fields[indexBack2];
                        else if (colectionIndexBack2 == 2)
                            suppliersFile.Back2Col3 = fields[indexBack2];
                        else if (colectionIndexBack2 == 3)
                            suppliersFile.Back2Col4 = fields[indexBack2];

                        colectionIndexBack2++;
                    }

                    var colectionIndexEdge = 0;

                    foreach (var indexEdge in indexesEdge)
                    {
                        if (colectionIndexEdge == 0)
                            suppliersFile.EdgeCol1 = fields[indexEdge];
                        else if (colectionIndexEdge == 1)
                            suppliersFile.EdgeCol2 = fields[indexEdge];
                        else if (colectionIndexEdge == 2)
                            suppliersFile.EdgeCol3 = fields[indexEdge];
                        else if (colectionIndexEdge == 3)
                            suppliersFile.EdgeCol4 = fields[indexEdge];

                        colectionIndexEdge++;
                    }

                    var colectionIndexEdge2 = 0;

                    foreach (var indexEdge2 in indexesEdge2)
                    {
                        if (colectionIndexEdge2 == 0)
                            suppliersFile.Edge2Col1 = fields[indexEdge2];
                        else if (colectionIndexEdge2 == 1)
                            suppliersFile.Edge2Col2 = fields[indexEdge2];
                        else if (colectionIndexEdge2 == 2)
                            suppliersFile.Edge2Col3 = fields[indexEdge2];
                        else if (colectionIndexEdge2 == 3)
                            suppliersFile.Edge2Col4 = fields[indexEdge2];

                        colectionIndexEdge2++;
                    }

                    if (headers.Contains($"Front"))
                        suppliersFile.Front = fields[headers.IndexOf("Front")];

                    var colectionIndexProfile = 0;

                    foreach (var indexProfile in indexesProfile)
                    {
                        if (colectionIndexProfile == 0)
                            suppliersFile.ProfileCol1 = fields[indexProfile];
                        else if (colectionIndexProfile == 1)
                            suppliersFile.ProfileCol2 = fields[indexProfile];

                        colectionIndexProfile++;
                    }

                    var colectionIndexTop = 0;

                    foreach (var indexTop in indexesTop)
                    {
                        if (colectionIndexTop == 0)
                            suppliersFile.TopCol1 = fields[indexTop];
                        else if (colectionIndexTop == 1)
                            suppliersFile.TopCol2 = fields[indexTop];

                        colectionIndexTop++;
                    }

                    var colectionIndexModel = 0;

                    foreach (var indexModel in indexesModel)
                    {
                        if (colectionIndexModel == 0)
                            suppliersFile.ModelCol1 = fields[indexModel];
                        else if (colectionIndexModel == 1)
                            suppliersFile.ModelCol2 = fields[indexModel];
                        else if (colectionIndexModel == 2)
                            suppliersFile.ModelCol3 = fields[indexModel];
                        else if (colectionIndexModel == 3)
                            suppliersFile.ModelCol4 = fields[indexModel];
                        else if (colectionIndexModel == 4)
                            suppliersFile.ModelCol5 = fields[indexModel];
                        else if (colectionIndexModel == 5)
                            suppliersFile.ModelCol6 = fields[indexModel];
                        else if (colectionIndexModel == 6)
                            suppliersFile.ModelCol7 = fields[indexModel];

                        colectionIndexModel++;
                    }

                    if (headers.Contains($"Model 2"))
                        suppliersFile.Model2 = fields[headers.IndexOf("Model 2")];

                    if (headers.Contains($"Gender"))
                        suppliersFile.Gender = fields[headers.IndexOf("Gender")];

                    if (headers.Contains($"Primary Material"))
                        suppliersFile.PrimaryMaterial = fields[headers.IndexOf("Primary Material")];

                    if (headers.Contains($"merch_hier_lvl_collection"))
                        suppliersFile.Merch_hier_lvl_collection = fields[headers.IndexOf("merch_hier_lvl_collection")];

                    if (headers.Contains($"merch_hier_lv2_sub_collection"))
                        suppliersFile.Merch_hier_lv2_sub_collection = fields[headers.IndexOf("merch_hier_lv2_sub_collection")];

                    if (headers.Contains($"Product Type"))
                        suppliersFile.ProductType = fields[headers.IndexOf("Product Type")];

                    if (headers.Contains($"funct_hier_lvl4_item_shape"))
                        suppliersFile.Funct_hier_lvl4_item_shape = fields[headers.IndexOf("funct_hier_lvl4_item_shape")];

                    if (headers.Contains($"stone_id_1"))
                        suppliersFile.Stone_id_1 = fields[headers.IndexOf("stone_id_1")];
                    if (string.IsNullOrEmpty(suppliersFile.Stone_id_1))
                    {
                        suppliersFile.Stone_id_1 = "";
                    }

                    if (headers.Contains($"stone_id_2"))
                        suppliersFile.Stone_id_2 = fields[headers.IndexOf("stone_id_2")];
                    if (string.IsNullOrEmpty(suppliersFile.Stone_id_2))
                    {
                        suppliersFile.Stone_id_2 = "";
                    }

                    if (headers.Contains($"stone_id_3"))
                        suppliersFile.Stone_id_3 = fields[headers.IndexOf("stone_id_3")];
                    if (string.IsNullOrEmpty(suppliersFile.Stone_id_3))
                    {
                        suppliersFile.Stone_id_3 = "";
                    }

                    if (headers.Contains($"stone_id_4"))
                        suppliersFile.Stone_id_4 = fields[headers.IndexOf("stone_id_4")];
                    if (string.IsNullOrEmpty(suppliersFile.Stone_id_4))
                    {
                        suppliersFile.Stone_id_4 = "";
                    }

                    if (headers.Contains($"caratage_of_predom_mat_1"))
                        suppliersFile.Caratage_of_predom_mat_1 = fields[headers.IndexOf("caratage_of_predom_mat_1")];
                    if (string.IsNullOrEmpty(suppliersFile.Caratage_of_predom_mat_1))
                    {
                        suppliersFile.Caratage_of_predom_mat_1 = "";
                    }

                    if (headers.Contains($"caratage_of_predom_mat_2"))
                        suppliersFile.Caratage_of_predom_mat_2 = fields[headers.IndexOf("caratage_of_predom_mat_2")];
                    if (string.IsNullOrEmpty(suppliersFile.Caratage_of_predom_mat_2))
                    {
                        suppliersFile.Caratage_of_predom_mat_2 = "";
                    }

                    if (headers.Contains($"caratage_of_predom_mat_3"))
                        suppliersFile.Caratage_of_predom_mat_3 = fields[headers.IndexOf("caratage_of_predom_mat_3")];
                    if (string.IsNullOrEmpty(suppliersFile.Caratage_of_predom_mat_3))
                    {
                        suppliersFile.Caratage_of_predom_mat_3 = "";
                    }

                    if (headers.Contains($"Closure Type"))
                        suppliersFile.ClosureType = fields[headers.IndexOf("Closure Type")];
                    if (string.IsNullOrEmpty(suppliersFile.ClosureType))
                    {
                        suppliersFile.ClosureType = "";
                    }

                    if (headers.Contains($"Diamond Carat Weight"))
                        suppliersFile.DiamondCaratWeight = diamondCaratWeight;

                    if (headers.Contains($"Size"))
                        suppliersFile.Size = fields[headers.IndexOf("Size")];
                    if (string.IsNullOrEmpty(suppliersFile.Size))
                    {
                        suppliersFile.Size = "";
                    }

                    suppliers.Add(suppliersFile);
                }
            }

            return suppliers;
        }

        public static List<TagHeuerDataItem> ReadSupplierTagHeuerFile(string filePath)
        {
            //Dictionary<object, object> fileObjects = new Dictionary<object, object>();
            List<TagHeuerDataItem> suppliers = new List<TagHeuerDataItem>();
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                List<string> headers = new List<string>();
                bool headersLine = true;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    if (headersLine)
                    {
                        (parser.ReadFields() ?? throw new InvalidOperationException()).ToList().ForEach(i => headers.Add(i));
                        headersLine = false;
                        //if (!headers.Contains($"Email"))
                        //{
                        //    throw new Exception("Not Found 'email' in input file");
                        //}
                    }

                    string price = "";
                    string costPrice = "";
                    string size = "";
                    var fields = (parser.ReadFields() ?? throw new InvalidOperationException()).ToList();

                    TagHeuerDataItem suppliersFile = new TagHeuerDataItem();

                    if (headers.Contains($"Gender"))
                        suppliersFile.Gender = fields[headers.IndexOf("Gender")];
                    if (string.IsNullOrEmpty(suppliersFile.Gender))
                    {
                        suppliersFile.Gender = "";
                    }

                    if (headers.Contains($"Product Title"))
                        suppliersFile.ProductTitle = fields[headers.IndexOf("Product Title")];
                    if (string.IsNullOrEmpty(suppliersFile.ProductTitle))
                    {
                        suppliersFile.ProductTitle = "";
                    }

                    if (headers.Contains($"Case Diameter (MM)"))
                        size = fields[headers.IndexOf("Case Diameter (MM)")].Replace("mm", "");
                    double.TryParse(size, out double caseDiameter);
                    suppliersFile.CaseDiameter = caseDiameter;
                    if (string.IsNullOrEmpty(size))
                    {
                        suppliersFile.CaseDiameter = 0;
                    }

                    if (headers.Contains($"Type of Band"))
                        suppliersFile.TypeOfBand = fields[headers.IndexOf("Type of Band")];
                    if (string.IsNullOrEmpty(suppliersFile.TypeOfBand))
                    {
                        suppliersFile.TypeOfBand = "";
                    }

                    if (headers.Contains($"WEB ID"))
                        suppliersFile.WebId = fields[headers.IndexOf("WEB ID")];
                    if (string.IsNullOrEmpty(suppliersFile.WebId))
                    {
                        suppliersFile.WebId = "";
                    }

                    if (headers.Contains($"Designer"))
                        suppliersFile.Designer = fields[headers.IndexOf("Designer")];
                    if (string.IsNullOrEmpty(suppliersFile.Designer))
                    {
                        suppliersFile.Designer = "";
                    }

                    if (headers.Contains($"Warranty Info"))
                        suppliersFile.WarrantyInfo = fields[headers.IndexOf("Warranty Info")];
                    if (string.IsNullOrEmpty(suppliersFile.WarrantyInfo))
                    {
                        suppliersFile.WarrantyInfo = "";
                    }

                    if (headers.Contains($"MPN"))
                        suppliersFile.MPN = fields[headers.IndexOf("MPN")];
                    if (string.IsNullOrEmpty(suppliersFile.MPN))
                    {
                        suppliersFile.MPN = "";
                    }

                    if (headers.Contains($"Web Price"))
                        price = fields[headers.IndexOf("Web Price")].Replace("$", "");
                        price = price.Replace(",", "");
                    double.TryParse(price, out double webPrice);
                    suppliersFile.WebPrice = webPrice;

                    if (headers.Contains($"Cost"))
                        costPrice = fields[headers.IndexOf("Web Price")].Replace("$", "");
                        costPrice = price.Replace(",", "");
                    double.TryParse(costPrice, out double cost);
                    suppliersFile.Cost = cost;

                    if (headers.Contains($"Case Shape"))
                        suppliersFile.CaseShape = fields[headers.IndexOf("Case Shape")];
                    if (string.IsNullOrEmpty(suppliersFile.CaseShape))
                    {
                        suppliersFile.CaseShape = "";
                    }

                    if (headers.Contains($"Stone ID"))
                        suppliersFile.StoneId = fields[headers.IndexOf("Stone ID")];
                    if (string.IsNullOrEmpty(suppliersFile.StoneId))
                    {
                        suppliersFile.StoneId = "";
                    }

                    if (headers.Contains($"Watch Type"))
                        suppliersFile.WatchType = fields[headers.IndexOf("Watch Type")];
                    if (string.IsNullOrEmpty(suppliersFile.WatchType))
                    {
                        suppliersFile.WatchType = "";
                    }

                    if (headers.Contains($"Collection"))
                        suppliersFile.Collection = fields[headers.IndexOf("Collection")];
                    if (string.IsNullOrEmpty(suppliersFile.Collection))
                    {
                        suppliersFile.Collection = "";
                    }

                    if (headers.Contains($"Movement Type"))
                        suppliersFile.MovementType = fields[headers.IndexOf("Movement Type")];
                    if (string.IsNullOrEmpty(suppliersFile.MovementType))
                    {
                        suppliersFile.MovementType = "";
                    }

                    if (headers.Contains($"Movement Name"))
                        suppliersFile.MovementName = fields[headers.IndexOf("Movement Name")];
                    if (string.IsNullOrEmpty(suppliersFile.MovementName))
                    {
                        suppliersFile.MovementName = "";
                    }

                    if (headers.Contains($"Bezel"))
                        suppliersFile.Bezel = fields[headers.IndexOf("Bezel")];
                    if (string.IsNullOrEmpty(suppliersFile.Bezel))
                    {
                        suppliersFile.Bezel = "";
                    }

                    if (headers.Contains($"Bezel Gemstone Information"))
                        suppliersFile.BezelGemstoneInformation = fields[headers.IndexOf("Bezel Gemstone Information")];
                    if (string.IsNullOrEmpty(suppliersFile.BezelGemstoneInformation))
                    {
                        suppliersFile.BezelGemstoneInformation = "";
                    }

                    if (headers.Contains($"Crystal"))
                        suppliersFile.Crystal = fields[headers.IndexOf("Crystal")];
                    if (string.IsNullOrEmpty(suppliersFile.Crystal))
                    {
                        suppliersFile.Crystal = "";
                    }

                    if (headers.Contains($"Dial"))
                        suppliersFile.Dial = fields[headers.IndexOf("Dial")];
                    if (string.IsNullOrEmpty(suppliersFile.Dial))
                    {
                        suppliersFile.Dial = "";
                    }

                    if (headers.Contains($"Dial Information"))
                        suppliersFile.DialInformation = fields[headers.IndexOf("Dial Information")].Replace("�", "");
                    if (string.IsNullOrEmpty(suppliersFile.DialInformation))
                    {
                        suppliersFile.DialInformation = "";
                    }

                    if (headers.Contains($"Dial Color"))
                        suppliersFile.DialColor = fields[headers.IndexOf("Dial Color")].ToLower();
                    if (string.IsNullOrEmpty(suppliersFile.DialColor))
                    {
                        suppliersFile.DialColor = "";
                    }

                    if (headers.Contains($"Water Resistancy (M)"))
                        suppliersFile.WaterResistancy = fields[headers.IndexOf("Water Resistancy (M)")];
                    if (string.IsNullOrEmpty(suppliersFile.WaterResistancy))
                    {
                        suppliersFile.WaterResistancy = "";
                    }

                    if (headers.Contains($"Bracelet/Strap Material"))
                        suppliersFile.BraceletStrapMaterial = fields[headers.IndexOf("Bracelet/Strap Material")];
                    if (string.IsNullOrEmpty(suppliersFile.BraceletStrapMaterial))
                    {
                        suppliersFile.BraceletStrapMaterial = "";
                    }

                    if (headers.Contains($"Clasp Type"))
                        suppliersFile.ClaspType = fields[headers.IndexOf("Clasp Type")];
                    if (string.IsNullOrEmpty(suppliersFile.ClaspType))
                    {
                        suppliersFile.ClaspType = "";
                    }

                    if (headers.Contains($"Country of Origin"))
                        suppliersFile.CountryOfOrigin = fields[headers.IndexOf("Country of Origin")];
                    if (string.IsNullOrEmpty(suppliersFile.CountryOfOrigin))
                    {
                        suppliersFile.CountryOfOrigin = "";
                    }

                    if (headers.Contains($"Total carat weight"))
                        suppliersFile.TotalCaratWeight = fields[headers.IndexOf("Total carat weight")];
                    if (string.IsNullOrEmpty(suppliersFile.TotalCaratWeight))
                    {
                        suppliersFile.TotalCaratWeight = "";
                    }

                    suppliers.Add(suppliersFile);
                }
            }

            return suppliers;
        }

        public static List<SceExportItem> ReadSceExportFile(string filePath)
        {
            List<SceExportItem> ftpItems = new List<SceExportItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator, CsvReaderBufferSize))
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
                            ProcessingPeriod = csv["Processing Time"],
                            Brand = csv["Brand"]
                        };

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
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
