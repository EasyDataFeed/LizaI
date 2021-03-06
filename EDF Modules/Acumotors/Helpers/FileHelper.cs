﻿using Acumotors.DataItems;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Acumotors.Helpers
{
    class FileHelper
    {
        private const string Separator = ",";
        private const char ComaCharSeparator = ',';

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static string CreateUpdateFile(string filePath, List<UpdateInfo> priceUpdateItems)
        {
            try
            {
                string headers = "WD Code,Supplier Number,Date Of Snapshot,Time Of Snapshot,Line Code," +
                    "Item Number,Item Package Code,Item Description,About Part,Custom Title,Part Has Core,Quantity On Hand," +
                    "Annualized Sales Quantity,List Price,Cost Price,Core Price,Item Package Quantity,UPC,Custom_attr1,Custom_attr2,Custom_attr3";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (UpdateInfo item in priceUpdateItems)
                {
                    string[] productArr = new string[21] { item.WDCode, item.SupplierNumber, item.DateOfSnapshot.ToString(),
                        item.TimeOfSnapshot.ToString(), item.LineCode, item.ItemNumber, item.ItemPackageCode,
                        item.ItemDescription, item.AboutPart, item.CustomTitle, item.PartHasCore, item.QuantityOnHand, item.AnnualizedSalesQuantity,
                        item.ListPrice.ToString(), item.CostPrice.ToString(), item.CorePrice.ToString(), item.ItemPackageQuantity, item.UPC, item.Custom_attr1, item.Custom_attr2, item.Custom_attr3};
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

        public static string CreateInventoryUpdateFile(string filePath, List<UpdateInfo> priceUpdateItems)
        {
            try
            {
                string headers = "WD Code,Supplier Number,Date Of Snapshot,Time Of Snapshot,Line Code," +
                    "Item Number,Item Package Code,Item Description,Part Has Core,Quantity On Hand," +
                    "Annualized Sales Quantity,List Price,Cost Price,Core Price,Item Package Quantity,UPC,Custom_attr1,Custom_attr2,Custom_attr3";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (UpdateInfo item in priceUpdateItems)
                {
                    string[] productArr = new string[19] { item.WDCode, item.SupplierNumber, item.DateOfSnapshot.ToString(),
                        item.TimeOfSnapshot.ToString(), item.LineCode, item.ItemNumber, item.ItemPackageCode,
                        item.ItemDescription, item.PartHasCore, item.QuantityOnHand, item.AnnualizedSalesQuantity,
                        item.ListPrice.ToString(), item.CostPrice.ToString(), item.CorePrice.ToString(), item.ItemPackageQuantity, item.UPC, item.Custom_attr1, item.Custom_attr2, item.Custom_attr3};
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

        public static string CreateDateToUpdateFile(string filePath, List<DateToUpdate> dateToUpdate)
        {
            try
            {
                string headers = "DateToUpdateProduct,DateToUpdateInventory";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (DateToUpdate item in dateToUpdate)
                {
                    string[] productArr = new string[2] { item.DateToUpdateProduct.ToString(), item.DateToUpdateInventory.ToString()};
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

        public static string CreateLongPNFile(string filePath, List<LongPNFile> longPNUpdateItems)
        {
            try
            {
                string headers = "Item Number";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (LongPNFile item in longPNUpdateItems)
                {
                    string[] productArr = new string[1] { item.LongPartNumber };
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
