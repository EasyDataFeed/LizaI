using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn14ApiScraper.DataItems.Turn14
{
    public class TrackingNumbersJson
    {
        public List<dataTrackingNumbers> data { get; set; }
    }

    public class dataTrackingNumbers
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesTrackingNumbers attributes { get; set; }
    }

    public class attributesTrackingNumbers
    {
        public string tracking_number { get; set; }
        public string service { get; set; }
        public List<referencesTrackingNumbers> references { get; set; }
    }

    public class referencesTrackingNumbers
    {
        public string type { get; set; }
        public string id { get; set; }
        public string number { get; set; }
        public string purchase_order_number { get; set; }
    }
}
