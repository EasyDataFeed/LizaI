using Acumotors.DataItems;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Acumotors.Helpers
{
    public class CsvManager
    {
        public static List<BrandsAlignment> ReadBrandFile(string filePath)
        {
            List<BrandsAlignment> brandFile = new List<BrandsAlignment>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        double.TryParse(csv["Surcharge"], out double surcharge);

                        BrandsAlignment item = new BrandsAlignment
                        {
                            Brand = csv["Brand"],
                            BrandCode = csv["Brand Code"],
                            Surcharge = surcharge
                        };

                        brandFile.Add(item);
                    }

                    return brandFile;
                }
            }
        }

        public static List<SpecificationFile> ReadSpecificationFile(string filePath)
        {
            List<SpecificationFile> specificationFile = new List<SpecificationFile>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        SpecificationFile item = new SpecificationFile
                        {
                            PartNumber = csv["Part Number"],
                            LineCode = csv["Line Code"],
                            Custom_attr1 = csv["Custom_attr1"],
                            Custom_attr2 = csv["Custom_attr2"],
                            Custom_attr3 = csv["Custom_attr3"],
                            AboutPart = csv["About Part"],
                            CustomTitle = csv["Custom Title"]
                        };

                        specificationFile.Add(item);
                    }

                    return specificationFile;
                }
            }
        }

        public static List<DateToUpdate> ReadDateToUpdateFile(string filePath)
        {
            List<DateToUpdate> specificationFile = new List<DateToUpdate>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        

                        DateToUpdate item = new DateToUpdate
                        {
                            DateToUpdateProduct = DateTime.TryParse(csv["DateToUpdateProduct"], out DateTime dateToUpdateProduct)? (DateTime?)dateToUpdateProduct : null,
                            DateToUpdateInventory = DateTime.TryParse(csv["DateToUpdateInventory"], out DateTime dateToUpdateInventory)? (DateTime?)dateToUpdateInventory : null
                        };

                        specificationFile.Add(item);
                    }

                    return specificationFile;
                }
            }
        }
    }
}
