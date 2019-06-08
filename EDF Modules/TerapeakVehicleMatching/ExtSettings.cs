using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TerapeakVehicleMatching;
using TerapeakVehicleMatching.DataItems;

namespace Databox.Libs.TerapeakVehicleMatching
{
    public class ExtSettings
    {
        public string Link { get; set; }

        [XmlIgnore]
        public List<string> Brands { get; set; }
        [XmlIgnore]
        public List<string> BrandsForScraping { get; set; }

        [XmlIgnore]
        public List<ExtWareInfo> DataForFile { get; set; }
        [XmlIgnore]
        public List<EndFile> EndFiles { get; set; }
    }
}
