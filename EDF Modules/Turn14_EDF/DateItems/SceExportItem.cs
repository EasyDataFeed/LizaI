using System.Collections.Generic;

namespace Turn14_EDF.DateItems
{
    public class SceExportItem : IEqualityComparer<SceExportItem>
    {
        public bool Equals(SceExportItem x, SceExportItem y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.Brand, y.Brand) && string.Equals(x.PartNumber, y.PartNumber) && string.Equals(x.ManufacturerPartNumber, y.ManufacturerPartNumber);
        }

        public int GetHashCode(SceExportItem obj)
        {
            unchecked
            {
                return ((obj.Brand != null ? obj.Brand.GetHashCode() : 0) * 397) ^ (obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0) ^ (obj.ManufacturerPartNumber != null ? obj.ManufacturerPartNumber.GetHashCode() : 0);
            }
        }

        public int ProdId { get; set; }
        public int ProductType { get; set; }
        public string Brand { get; set; }
        public string PartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }

    }
}
