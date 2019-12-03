using System.Collections.Generic;

namespace ScraperEbayApi.DataItems
{
    public class EbayCsvItem
    {
        public class EbayItemNumberEqualityComparer : IEqualityComparer<EbayCsvItem>
        {
            public bool Equals(EbayCsvItem x, EbayCsvItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.EbayItemNumber, y.EbayItemNumber);
            }

            public int GetHashCode(EbayCsvItem obj)
            {
                return (obj.EbayItemNumber != null ? obj.EbayItemNumber.GetHashCode() : 0);
            }
        }


        public string EbayItemNumber { get; set; }
    }
}
