using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace GreenHouseFabricsScraper
{
    public class ExtWareInfo : WareInfo
    {
        public double WholesalePrice { get; set; }
        public int Yards { get; set; }
        public string Specifications { get; set; }
        public string Image { get; set; }
        public string Keywords { get; set; }
        public string Use { get; set; }
    }
}
