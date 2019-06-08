using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acumotors.DataItems
{
    public class UpdateInfo
    {
        public string WDCode { get; set; }
        public string SupplierNumber { get; set; }
        public string DateOfSnapshot { get; set; }
        public string TimeOfSnapshot { get; set; }
        public string LineCode { get; set; }
        public string ItemNumber { get; set; }
        public string ItemPackageCode { get; set; }
        public string ItemDescription { get; set; }
        public string PartHasCore { get; set; }
        public string QuantityOnHand { get; set; }
        public string AnnualizedSalesQuantity { get; set; }
        public double ListPrice { get; set; }
        public double CostPrice { get; set; }
        public double CorePrice { get; set; }
        public string ItemPackageQuantity { get; set; }
        public string UPC { get; set; }
    }
}
