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
    }
}
