using LumenWorks.Framework.IO.Csv;
using ScraperEbayApi.DataItems;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScraperEbayApi.Helpers
{
    public class CsvManager
    {
        public static List<EbayCsvItem> ReadFileFilter(string filePath)
        {
            List<EbayCsvItem> itemsList = new List<EbayCsvItem>();
            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        EbayCsvItem item = new EbayCsvItem();

                        if (csv.GetFieldHeaders().Contains("Item ID"))
                            item.EbayItemNumber = csv.GetFieldHeaders().Contains("Item ID") ? csv["Item ID"] : string.Empty;
                        else if (csv.GetFieldHeaders().Contains("Ebay Item Number"))
                            item.EbayItemNumber = csv.GetFieldHeaders().Contains("Ebay Item Number") ? csv["Ebay Item Number"] : string.Empty;

                        if (!string.IsNullOrEmpty(item.EbayItemNumber))
                            itemsList.Add(item);
                    }
                }
            }
            return itemsList;
        }
    }
}
