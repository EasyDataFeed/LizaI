using Fabricut.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fabricut.Helpers
{
    public class FileHelper
    {
        private const string Separator = ",";

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static string CreatePriceUpdateFile(string filePath, List<PriceUpdate> priceUpdate)
        {
            try
            {
                string headers = "action,Product Type,prodid,Part Number,MSRP,Jobber,Web Price,Cost Price";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (PriceUpdate item in priceUpdate)
                {
                    string[] productArr = new string[8] { item.Action, item.ProductType,item.ProdId,item.PartNumber,item.MSRP.ToString(),
                        item.Jobber.ToString(),item.WebPrice.ToString(),item.CostPrice.ToString()};
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
