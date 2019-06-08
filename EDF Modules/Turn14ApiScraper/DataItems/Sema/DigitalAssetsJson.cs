using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turn14ApiScraper.DataItems.Sema
{
    public class DigitalAssetsJson
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<DigitalAssets> digitalAssets { get; set; }
    }

    public class DigitalAssets
    {
        public string PartNumber { get; set; }
        public string Link { get; set; }
        public string AssetTypeCode { get; set; }
        public string FileName { get; set; }
        public string RecordModifiedDate { get; set; }
    }
}
