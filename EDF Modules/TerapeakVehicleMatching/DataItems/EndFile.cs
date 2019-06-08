using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerapeakVehicleMatching.DataItems
{
    public  class EndFile
    {
        public string EbayItemId { get; set; }
        public string ScePartNumber { get; set; }
        public double SceWebPrice { get; set; }
        public double EbayPrice { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int StartYearSCE { get; set; }
        public int EndYearSCE { get; set; }
        public int StartYearEbay { get; set; }
        public int EndYearEbay { get; set; }
        public string Competitor { get; set; }
        public string EbayBrand { get; set; }
        public string EbayCategory { get; set; }
        public string EbaySku { get; set; }
        public string TTotalSold { get; set; }
        public string ETotalSold { get; set; }
        public string TTotalSales { get; set; }
        public string AvarageSalePrice { get; set; }
    }
}
