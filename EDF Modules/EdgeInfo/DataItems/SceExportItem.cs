using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeInfo.DataItems
{
    class SceExportItem
    {
        public string ProdId { get; set; }
        public string ProductType { get; set; }
        public string Warehouse { get; set; }
        public string Supplier { get; set; }
        public double MSRP { get; set; }
        public double Jobber { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
        public string PartNumber { get; set; }
        public string PickupAvailable { get; set; }
        public string Specifications { get; set; }
        public string ProcessingPeriod { get; set; }
    }
}
