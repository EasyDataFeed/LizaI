using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kravet.DataItems
{
    public class PriceUpdateInfo
    {
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
        public string Brand { get; set; }
    }
}
