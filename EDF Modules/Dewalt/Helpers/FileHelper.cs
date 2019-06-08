using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dewalt.Helpers
{
    class FileHelper
    {
        public static string CreateReportFile(List<string> batchInfo, bool errorList = false)
        {
            try
            {
                string fileName;
                if (errorList)
                    fileName = $"ErrorReport-{DateTime.Now:MM-dd-yyyy-hh-mm}.csv";
                else
                    fileName = $"NotProductPageReport-{DateTime.Now:MM-dd-yyyy-hh-mm}.csv";

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string separator = ",";
                string headers = "URL";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (string item in batchInfo)
                {
                    string[] productArr = new string[1] { item };
                    string product = String.Join(separator, productArr);
                    sb.AppendLine(product);
                }

                File.WriteAllText(filePath, sb.ToString());

                return filePath;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
