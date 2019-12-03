using ScraperEbayApi;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Databox.Libs.ScraperEbayApi
{
    public class ExtSettings
    {
        public string EbayItemsFilePath { get; set; }
        public bool VehicleCompatibility { get; set; }
        public bool FullDescription { get; set; }
        public string Key { get; set; }

        [XmlIgnore]
        public List<ExtWareInfo> DataForFile { get; set; }
        [XmlIgnore]
        public List<List<ExtWareInfo>> AllWaresInfoFile { get; set; }
    }
}
