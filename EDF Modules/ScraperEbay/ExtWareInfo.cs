using System.Collections.Generic;
using WheelsScraper;

namespace ScraperEbay
{
    public class ExtWareInfo : WareInfo
    {
        public string EbayItemNumber { get; set; }
        public string Sold { get; set; }

        public class EbayItemNumberSoldEqualityComparer : IEqualityComparer<ExtWareInfo>
        {
            public bool Equals(ExtWareInfo x, ExtWareInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.EbayItemNumber, y.EbayItemNumber) && string.Equals(x.Sold, y.Sold);
            }

            public int GetHashCode(ExtWareInfo obj)
            {
                unchecked
                {
                    return ((obj.EbayItemNumber != null ? obj.EbayItemNumber.GetHashCode() : 0) * 397) ^ (obj.Sold != null ? obj.Sold.GetHashCode() : 0);
                }
            }
        }
    }
}
