using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace GreenHouseFabrics
{
    public class ExtWareInfo : WareInfo
    {
        public string Prodid { get; set; }

        public string ProductType { get; set; }

        public double WebPrice { get; set; }

        public string Inventory { get; set; }
    }
}
