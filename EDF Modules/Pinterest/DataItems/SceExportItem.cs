using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinterest.DataItems
{
    public class SceExportItem
    {
        private sealed class PartNumberBrandEqualityComparer : IEqualityComparer<SceExportItem>
        {
            public bool Equals(SceExportItem x, SceExportItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.PartNumber, y.PartNumber) && string.Equals(x.Brand, y.Brand);
            }

            public int GetHashCode(SceExportItem obj)
            {
                unchecked
                {
                    return ((obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0) * 397) ^ (obj.Brand != null ? obj.Brand.GetHashCode() : 0);
                }
            }
        }

        public static IEqualityComparer<SceExportItem> PartNumberBrandComparer { get; } = new PartNumberBrandEqualityComparer();

        public string GeneralImage { get; set; }
        public string SpiderURL { get; set; }
        public string Description { get; set; }
        public string ProductTitle { get; set; }
        public string PartNumber { get; set; }
        public string Brand { get; set; }
    }
}
