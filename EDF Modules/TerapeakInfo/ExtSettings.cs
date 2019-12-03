using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TerapeakInfo;

namespace Databox.Libs.TerapeakInfo
{
    public class ExtSettings
    {
        public string Link { get; set; }
        public bool FullDescription { get; set; }
        public bool VehicleCompatibility { get; set; }
        [XmlIgnore]
        public List<ExtWareInfo> DataForFile { get; set; }
    }
}
