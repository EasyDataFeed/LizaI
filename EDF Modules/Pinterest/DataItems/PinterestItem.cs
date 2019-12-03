using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinterest.DataItems
{
    public class PinterestItem
    {
        public string SpiderURL { get; set; }
        public string PartNumber { get; set; }
        public string Brand { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string PinUrl { get; set; }
        public string Changed { get; set; }
        public string ProdId { get; set; }
    }
}
