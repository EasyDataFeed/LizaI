using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Acumotors
{
    public class ExtWareInfo : WareInfo
    {
        public string PrimaryVendor { get; set; }
        public string InternalPartNumber { get; set; }
        public string Description { get; set; }
        public string CoreCharge { get; set; }
        public string Stock { get; set; }
        public string Cost { get; set; }
        public string Map { get; set; }
        public string Weight { get; set; }
    }
}
