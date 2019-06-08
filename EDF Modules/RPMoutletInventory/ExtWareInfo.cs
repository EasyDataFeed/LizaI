using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace RPMoutletInventory
{
    public class ExtWareInfo : WareInfo
    {
        public bool Changed { get; set; }
        public int ProductId { get; set; }
        public string EbayId { get; set; }
        public int Available { get; set; }
        public string PartNum { get; set; }
    }
}
