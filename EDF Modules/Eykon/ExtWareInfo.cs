using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Eykon
{
    public class ExtWareInfo : WareInfo
    {
        public string Brand { get; set; }
        public string SubBrand { get; set; }
        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string AnchorText { get; set; }
        public string MetaDescription { get; set; }
        public string GeneralImageTags { get; set; }
        public string Image { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string CrossSellSubCategory1 { get; set; }
        public string CrossSellSubCategory2 { get; set; }
        public string Suppliers { get; set; }
        public string Warehouse { get; set; }
        public string Shipping { get; set; }
        public string PartNumber { get; set; }
        public string Price { get; set; }
        public string Dimensions { get; set; }
        public string GrabDocuments { get; set; }
        public string Specifications { get; set; }
        public string ProductType { get; set; }
        public string ProductTypeSpec { get; set; }
        public string ColorSpecification { get; set; }
        public string Color { get; set; }
    }
}
