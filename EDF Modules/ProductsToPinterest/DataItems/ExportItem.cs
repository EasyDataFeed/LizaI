using System.Collections.Generic;

namespace ProductsToPinterest.DataItems
{
    public class ExportItem
    {
        private sealed class ProdIdBrandEqualityComparer : IEqualityComparer<ExportItem>
        {
            public bool Equals(ExportItem x, ExportItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.PartNumber, y.PartNumber) && string.Equals(x.Brand, y.Brand);
            }

            public int GetHashCode(ExportItem obj)
            {
                unchecked
                {
                    return ((obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0) * 397) ^ (obj.Brand != null ? obj.Brand.GetHashCode() : 0);
                }
            }
        }

        public static IEqualityComparer<ExportItem> ProdIdBrandComparer { get; } = new ProdIdBrandEqualityComparer();

        public string GeneralImage { get; set; }
        public string SpiderURL { get; set; }
        public string Description { get; set; }
        public string ProductTitle { get; set; }
        public string PartNumber { get; set; }
        public string Brand { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
    }
}
