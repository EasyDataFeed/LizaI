using WheelsScraper;

namespace Insidefabric
{
    public class ExtWareInfo : WareInfo
    {
        public string SKU { get; set; }
        public string SpiderUrl { get; set; }
        public string ProductType { get; set; }
        public string METAKeywords { get; set; }
        public string Manufacturer { get; set; }
        public string AnchorText { get; set; }
        public string DetailsProductType { get; set; }
        public string GeneralImage { get; set; }
        public string WebPrice { get; set; }
        public string RetailPrice { get; set; }
        public string PriceUnit { get; set; }
        public string MinimumIndicator { get; set; }
        public string Categories { get; set; }
        public string RelatedProducts { get; set; }
        public string RelatedSKU { get; set; }
        public string Specification { get; set; }
        public string FeaturedProducts { get; set; }
        public string CrossSells { get; set; }
        public string Discontinued { get; set; }
    }
}
