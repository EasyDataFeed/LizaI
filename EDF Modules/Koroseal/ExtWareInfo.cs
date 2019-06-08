using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Koroseal
{
    public class ExtWareInfo : WareInfo
    {
        public string ProductTitle { get; set; }
        public string GeneralImage { get; set; }
        public string Specifications { get; set; }
        public string ProductType { get; set; }
        public string Title { get; set; }
        public string AnchorText { get; set; }
        public string ProductDescription { get; set; }
        public string GeneralImageTags { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string SpecificColor { get; set; }
        public string Suppliers { get; set; }
        public string Warehouse { get; set; }
        public string ProductPartNumber { get; set; }
        public string Shipping { get; set; }
        public double Price { get; set; }
        public string Dimensions { get; set; }
        public string CrossSellSubCategory1 { get; set; }
        public string CrossSellSubCategory2 { get; set; }
    }
}
