using AccountsCRMFieldsUpdater.DataItems;
using AccountsCRMFieldsUpdater.Extensions;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AccountsCRMFieldsUpdater.Helpers
{
    class FileHelper
    {
        private const string Separator = ",";

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static List<CustomerInfo> ReadCustomersFile(string filePath, out List<string> headersInfo)
        {
            //Dictionary<object, object> fileObjects = new Dictionary<object, object>();
            List<CustomerInfo> customersData = new List<CustomerInfo>();
            headersInfo = new List<string>();
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
                        headersInfo.AddRange(headers);
                        if (!headers.Contains($"Email"))
                        {
                            throw new Exception("Not Found 'email' in input file");
                        }
                    }
                    var fields = (parser.ReadFields() ?? throw new InvalidOperationException()).ToList();

                    CustomerInfo customerData = new CustomerInfo();

                    customerData.Email = fields[headers.IndexOf("Email")];

                    if (headers.Contains($"Website"))
                        customerData.Website = fields[headers.IndexOf("Website")];

                    if (headers.Contains($"Billing Company"))
                        customerData.BillingCompany = fields[headers.IndexOf("Billing Company")];

                    if (headers.Contains($"First name"))
                        customerData.FirstName = fields[headers.IndexOf("First name")];

                    if (headers.Contains($"Last name"))
                        customerData.LastName = fields[headers.IndexOf("Last name")];

                    if (headers.Contains($"Billing Address"))
                        customerData.BillingAddress = fields[headers.IndexOf("Billing Address")];

                    if (headers.Contains($"Phone 1"))
                        customerData.Phone1 = fields[headers.IndexOf("Phone 1")];

                    if (headers.Contains($"City"))
                        customerData.City = fields[headers.IndexOf("City")];

                    if (headers.Contains($"State"))
                        customerData.State = fields[headers.IndexOf("State")];

                    if (headers.Contains($"Zip"))
                        customerData.Zip = fields[headers.IndexOf("Zip")];

                    if (headers.Contains($"Country"))
                        customerData.Country = fields[headers.IndexOf("Country")];

                    foreach (string header in headers)
                    {
                        if (header.StartsWith("af:"))
                        {
                            string cleanedHeader = header.TrimStart("af:");
                            string data = fields[headers.IndexOf(header)];
                            customerData.AccountCustomFields.Add($"{cleanedHeader}^{data}");
                        }
                        else if (header.StartsWith("cf:"))
                        {
                            string cleanedHeader = header.TrimStart("cf:");
                            string data = fields[headers.IndexOf(header)];
                            customerData.ContactCustomFields.Add($"{cleanedHeader}^{data}");
                        }
                    }

                    customersData.Add(customerData);
                }
            }

            return customersData;
        }

        public static string CreateNotValidFile(string filePath, List<CustomerInfo> customerInfos)
        {
            try
            {
                string headers = "Email,Website,BillingCompany,FirstName,LastName,Phone1," +
                    "BillingAddress,City,State,Zip,Country";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (CustomerInfo item in customerInfos)
                {
                    string[] productArr = new string[11] { item.Email, item.Website, item.BillingCompany,
                        item.FirstName, item.LastName, item.Phone1, item.BillingAddress,
                        item.City, item.State, item.Zip, item.Country };
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
