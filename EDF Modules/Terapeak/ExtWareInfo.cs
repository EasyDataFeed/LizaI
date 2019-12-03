using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Terapeak
{
    public class ExtWareInfo : WareInfo
    {
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalListings { get; set; }
        public string Category { get; set; }
    }
}
