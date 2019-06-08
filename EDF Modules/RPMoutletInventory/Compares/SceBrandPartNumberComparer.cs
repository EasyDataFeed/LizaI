using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPMoutletInventory.DataItems;

namespace RPMoutletInventory.Compares
{
    public class BrandPartNumberEqualityComparer : IEqualityComparer<SceItem>
    {
        public bool Equals(SceItem x, SceItem y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.Brand, y.Brand) && string.Equals(x.PartNumber, y.PartNumber) && string.Equals(x.ManufacturerPartNumber, y.ManufacturerPartNumber);
        }

        public int GetHashCode(SceItem obj)
        {
            unchecked
            {
                return ((obj.Brand != null ? obj.Brand.GetHashCode() : 0) * 397) ^ (obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0) ^ (obj.ManufacturerPartNumber != null ? obj.ManufacturerPartNumber.GetHashCode() : 0);
            }
        }
    }
}
