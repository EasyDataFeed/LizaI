using Redfin.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redfin.Helpers
{
    public class FileHelper
    {
        private const string Separator = ",";

        public static string CreateRedfinItemsFile(string filePath, List<RedfinSitemap> items)
        {
            try
            {
                string headers = "Address,City,State,Zip,BuiltYear,Url";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (RedfinSitemap item in items)
                {
                    string[] productArr = new string[6] { item.Address, item.City, item.State, item.Zip, item.BuiltYear, item.Url };
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

        public static string GetSettingsPath(string fileName = null, string folderName = "")
        {
            return fileName == null ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory) : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName, fileName);
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
