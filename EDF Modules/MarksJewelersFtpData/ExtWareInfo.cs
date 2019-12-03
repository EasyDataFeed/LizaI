using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace MarksJewelersFtpData
{
    public class ExtWareInfo : WareInfo
    {
        public string Action { get; set; }
        public string ProductType { get; set; }
        public string ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string AnchorText { get; set; }
        public string SpiderURL { get; set; }
        public string ScreenTitle { get; set; }
        public string SubTitle { get; set; }
        public string BrandItem { get; set; }
        public string DescriptionItem { get; set; }
        public string METADescription { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string GeneralImage { get; set; }
        public string Specifications { get; set; }
        public string ProcessingTime { get; set; }
        public string ShippingType { get; set; }
        public string ShippingCarrier1 { get; set; }
        public string Allowground { get; set; }
        public string Allow3day { get; set; }
        public string Allow2day { get; set; }
        public string Allownextday { get; set; }
        public string LiveInventory { get; set; }
        public string Supplier { get; set; }
        public string ReOrderSupplier { get; set; }
        public string FileAttachments { get; set; }
        public string PartNumberItem { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public double MSRPItem { get; set; }
        public double JobberItem { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
        public string ItemWeight { get; set; }
        public string ItemHeight { get; set; }
        public string ItemWidth { get; set; }
        public string ItemLength { get; set; }
        public string ShippingWeight { get; set; }
        public string ShippingHeight { get; set; }
        public string ShippingWidth { get; set; }
        public string ShippingLength { get; set; }
        public string CheckoutRule { get; set; }
        public string CustomHtmlBelowPrice { get; set; }
        public string CustomHtmlAbovePrice { get; set; }
        public string CrossSellMainCat1 { get; set; }
        public string CrossSellSubCat1 { get; set; }
        public string CrossSellSecCat1 { get; set; }
        public string PanoramImage { get; set; }
        public string InternalUse { get; set; }
    }
}
