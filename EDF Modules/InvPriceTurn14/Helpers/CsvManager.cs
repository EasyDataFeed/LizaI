using InvPriceTurn14.DataItems;
using LumenWorks.Framework.IO.Csv;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InvPriceTurn14.Helpers
{
    public static class CsvManager
    {
        public static List<SceItem> ReadSceExport(string filePath)
        {
            List<SceItem> sceItems = new List<SceItem>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        SceItem item = new SceItem
                        {
                            Brand = csv["Brand"],
                            ManufacturerPartNumber = csv["Manufacturer Part Number"],
                            PartNumber = csv["Part Number"]
                        };

                        sceItems.Add(item);
                    }

                    return sceItems;
                }
            }
        }

        public static List<BrandsAlignment> ReadBrandsAlignments(string filePath)
        {
            List<BrandsAlignment> brandsList = new List<BrandsAlignment>();


            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        BrandsAlignment item = new BrandsAlignment();

                        if (csv.GetFieldHeaders().Contains("Brand in Export"))
                            item.BrandInSce = csv["Brand in Export"];

                        if (csv.GetFieldHeaders().Contains("Brand in Turn14"))
                            item.BrandInTurn14 = csv["Brand in Turn14"];

                        brandsList.Add(item);
                    }

                    return brandsList;
                }
            }
        }
    }
}
