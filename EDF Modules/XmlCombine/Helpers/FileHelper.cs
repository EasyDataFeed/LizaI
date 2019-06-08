#region using

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LumenWorks.Framework.IO.Csv;
using XmlCombine.DataItems;

#endregion

namespace XmlCombine.Helpers
{
    public static class FileHelper
    {
        private const char ComaDelimiter = ',';

        public static List<string> GetDirectoryXmlFileList(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.xml").ToList(); ;
        }

        public static List<FilterItem> ReadFilterItems(string filePath)
        {
            List<FilterItem> filterItems = new List<FilterItem>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaDelimiter))
                {
                    while (csv.ReadNextRecord())
                    {
                        filterItems.Add(new FilterItem
                        {
                            OEM = csv["OEM #"],
                            Manufacturer = csv["Manufacturer #"],
                            Brand = csv["Brand"]
                        });
                    }
                }
            }

            return filterItems;
        }

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
                writer.Flush();
            }
            finally
            {
                writer?.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}
