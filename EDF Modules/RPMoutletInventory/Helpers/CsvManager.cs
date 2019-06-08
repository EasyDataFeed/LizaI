using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using RPMoutletInventory.DataItems;

namespace RPMoutletInventory.Helpers
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
                            ProdId = int.Parse(csv["prodid"]),
                            ProductType = int.Parse(csv["Product Type"]),
                            Brand = csv["Brand"],
                            ManufacturerPartNumber = csv["Manufacturer Part Number"],
                            PartNumber = csv["Part Number"],
                            Upc = csv["UPC"],
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

                        if (csv.GetFieldHeaders().Contains("Brand in SCE"))
                            item.BrandInSce = csv["Brand in SCE"];

                        if (csv.GetFieldHeaders().Contains("Brand in Turn14"))
                            item.BrandInTurn14 = csv["Brand in Turn14"];

                        if (csv.GetFieldHeaders().Contains("Brand in Premier"))
                            item.BrandInPremier = csv["Brand in Premier"];

                        if (csv.GetFieldHeaders().Contains("Mayer Long"))
                            item.BrandInMayerLong = csv["Mayer Long"];

                        if (csv.GetFieldHeaders().Contains("Mayer Short"))
                            item.BrandInMayerShort = csv["Mayer Short"];

                        if (csv.GetFieldHeaders().Contains("Brand in Ebay"))
                            item.BrandInEbay = csv["Brand in Ebay"];

                        brandsList.Add(item);
                    }

                    return brandsList;
                }
            }
        }
    }
}
