using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerapeakVehicleMatching.DataItems
{
    public class SceExportItem
    {
        public class SceExportItemEqualityComparer : IEqualityComparer<SceExportItem>
        {
            public bool Equals(SceExportItem x, SceExportItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.ProdId == y.ProdId && string.Equals(x.PartNumber, y.PartNumber) && x.WebPrice.Equals(y.WebPrice) && string.Equals(x.VehicleMake, y.VehicleMake) && string.Equals(x.VehicleModel, y.VehicleModel) && x.StartYear == y.StartYear && x.EndYear == y.EndYear && string.Equals(x.Brand, y.Brand);
            }

            public int GetHashCode(SceExportItem obj)
            {
                unchecked
                {
                    var hashCode = obj.ProdId;
                    hashCode = (hashCode * 397) ^ (obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.WebPrice.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.VehicleMake != null ? obj.VehicleMake.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.VehicleModel != null ? obj.VehicleModel.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.StartYear;
                    hashCode = (hashCode * 397) ^ obj.EndYear;
                    hashCode = (hashCode * 397) ^ (obj.Brand != null ? obj.Brand.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public int ProdId { get; set; }
        public string PartNumber { get; set; }
        public double WebPrice { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Brand { get; set; }
    }
}
