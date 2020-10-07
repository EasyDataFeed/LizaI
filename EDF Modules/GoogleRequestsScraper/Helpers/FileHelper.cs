using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GoogleRequestsScraper.DataItems;
using LumenWorks.Framework.IO.Csv;

namespace GoogleRequestsScraper.Helpers
{
    public class FileHelper
    {
        private const char ComaSeparator = ',';
        private const string Separator = ",";
        public static string GetSettingsPath(string fileName = null, string folderName = "")
        {
            return fileName == null
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory)
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName, fileName);
        }

        public static string GetSettingsDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        }

        public static List<GeotargetsDataItem> ReadGeotargetsFile(string filePath)
        {
            List<GeotargetsDataItem> ftpItems = new List<GeotargetsDataItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        GeotargetsDataItem item = new GeotargetsDataItem
                        {
                            Name = csv["Name"],
                            CanonicalName = csv["Canonical Name"],
                            CountryCode = csv["Country Code"],
                            Check = csv["Check"]
                        };

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
            }
        }

        //public static List<StatesDataItem> ReadStatesFile(string filePath)
        //{
        //    List<StatesDataItem> ftpItems = new List<StatesDataItem>();

        //    using (StreamReader sr = File.OpenText(filePath))
        //    {
        //        using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
        //        {
        //            while (csv.ReadNextRecord())
        //            {
        //                StatesDataItem item = new StatesDataItem
        //                {
        //                    City = csv["City"],
        //                    CountryCode = csv["Country Code"],
        //                    StateCode = csv["Country Code"]
        //                };

        //                ftpItems.Add(item);
        //            }
        //        }

        //        return ftpItems;
        //    }
        //}

        public static List<DomainsDataItem> ReadDomainsFile(string filePath)
        {
            List<DomainsDataItem> ftpItems = new List<DomainsDataItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator))
                {
                    while (csv.ReadNextRecord())
                    {
                        DomainsDataItem item = new DomainsDataItem
                        {
                            Legal = csv["legal"],
                            Website = csv["website"]
                        };

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
            }
        }

        public static List<StatesDataItem> FillCountryStateZip()
        {
            var countryStateZipFileInfo = FileHelper.ReadAllResourceLines("GoogleRequestsScraper.Resources.States.txt");

            var headers = countryStateZipFileInfo[0].Split('\t');
            var stateInd = headers.FindIndex(h => string.Equals(h, "State"));
            var stateCodeInd = headers.FindIndex(h => string.Equals(h, "State Code"));

            List<StatesDataItem> items = new List<StatesDataItem>();
            for (int i = 1; i < countryStateZipFileInfo.Length - 1; i++)
            {
                var item = new StatesDataItem
                {
                    //CountryCode = countryStateZipFileInfo[i].Split('\t')[countryCodeInd],
                    //StateAbbr = countryStateZipFileInfo[i].Split('\t')[stateAbbrInd],
                    State = countryStateZipFileInfo[i].Split('\t')[stateInd],
                    StateCode = countryStateZipFileInfo[i].Split('\t')[stateCodeInd]
                };
                items.Add(item);
            }

            return items;
        }

        public static string[] ReadAllResourceLines(string resourceName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return EnumerateLines(reader).ToArray();
            }
        }

        private static IEnumerable<string> EnumerateLines(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public static void CreateFile(string fileName, string filedData)
        {
            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(fileName);
                writer.Write(filedData);
                writer.Flush();
            }
            finally
            {
                writer?.Close();
            }
        }

        public static void CreateZipFile(string fileName, IEnumerable<string> files)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            ZipArchive zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
            foreach (string file in files)
            {
                zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
            }

            zip.Dispose();
        }

        public static void DeleteSettingsFiles(List<string> files)
        {
            foreach (string filePath in files)
            {
                File.Delete(filePath);
            }
        }


        public static string CreateGoogleScrapedItemsFile(string filePath, List<GoogleScrapedItem> items)
        {
            string headers =
                $"keyword,domain,position,state,device,time,placement";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(headers);

                foreach (GoogleScrapedItem item in items)
                {
                    try
                    {
                        string[] productArr = new string[7]
                        {
                            item.Keyword, item.Domain, item.Position, item.State, item.Device,
                            item.Time, item.Placement
                        };
                        for (int i = 0; i < productArr.Length; i++)
                            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                                productArr[i] = StringToCsvCell(productArr[i]);

                        string line = String.Join(Separator, productArr);
                        writer.WriteLine(line);
                    }
                    catch { /*ignored*/ }
                }
            }

            return filePath;
        }

        public static string StringToCsvCell(string str)
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
