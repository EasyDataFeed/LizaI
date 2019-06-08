using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fabricut.DataItems;
using LumenWorks.Framework.IO.Csv;

namespace Fabricut.Helpers
{
    public static class CsvManager
    {
        public static List<SceItem> ReadCsvItems(string filePath)
        {
            List<SceItem> csvItemList = new List<SceItem>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        double.TryParse(csv["MSRP"], out double MSRP);
                        double.TryParse(csv["Jobber"], out double Jobber);
                        double.TryParse(csv["Cost Price"], out double CostPrice);
                        double.TryParse(csv["Web Price"], out double WebPrice);

                        SceItem item = new SceItem
                        {
                            AnchorText = csv["Anchor Text"],
                            Brand = csv["Brand"],
                            PrimaryKeyword = csv["primary keyword"],
                            PartNumber = csv["Part Number"],
                            ProdId = csv["prodid"],
                            ProductType = csv["Product Type"],
                            MSRP = MSRP,
                            Jobber = Jobber,
                            CostPrice = CostPrice,
                            WebPrice = WebPrice
                        };

                        csvItemList.Add(item);
                    }
                }
            }
            return csvItemList;
        }
    }
}