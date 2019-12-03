using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EbayNewPartsListingInfo;

namespace Databox.Libs.EbayNewPartsListingInfo
{
    public class ExtSettings
    {
        public string EbayItemsFilePath { get; set; }
        public bool VehicleCompatibility { get; set; }
        public bool FullDescription { get; set; }

        [XmlIgnore]
        public List<ExtWareInfo> DataForFile{ get; set; }
        [XmlIgnore]
        public List<List<ExtWareInfo>> AllWaresInfoFile { get; set; }
    }
}
