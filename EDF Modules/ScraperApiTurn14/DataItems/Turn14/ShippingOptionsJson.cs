using System.Collections.Generic;

namespace ScraperApiTurn14.DataItems.Turn14
{
    public class ShippingOptionsJson
    {
        public List<dataShippingOptions> data { get; set; }
    }

    public class dataShippingOptions
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesShippingOptions attributes { get; set; }
    }

    public class attributesShippingOptions
    {
        public string transportation_name { get; set; }
        public string carrier_name { get; set; }
    }
}
