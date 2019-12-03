using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeInfo.DataItems
{
    class PriceUpdateInfo
    {

        //Компаратор для удаление дубликатов
        public class PartNumberEqualityComparer : IEqualityComparer<PriceUpdateInfo>
        {
            public bool Equals(PriceUpdateInfo x, PriceUpdateInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.PartNumber, y.PartNumber);
            }

            public int GetHashCode(PriceUpdateInfo obj)
            {
                return (obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0);
            }
        }

        public string Action { get; set; }
        public string ProdId { get; set; }
        public string ProductType { get; set; }
        public string PartNumber { get; set; }
        public double MSRP { get; set; }
        public double CostPrice { get; set; }
        public double WebPrice { get; set; }
        public double Jobber { get; set; }
        public string Warehouse { get; set; }
        public string Supplier { get; set; }
        public string Specification { get; set; }
        public string PickupAvailable {get;set;}
        public string ProcessingPeriod { get; set; }
        public string Featured { get; set; }
        public string CustomHtmlBelowPrice { get; set; }
        public string ShippingType { get; set; } = "dynamic";
        public string ShippingCarrier1 { get; set; } = "FedEX";
        public string ShippingCarrier2 { get; set; }
        public string Allowground { get; set; } = "1";
        public string pickupAvailable { get; set; } = "0";
    }
}
