using System.Collections.Generic;

namespace ScraperApiTurn14.DataItems
{
    public class VehicleInfoForFitments
    {
        private sealed class VehicleInfoForFitmentsEqualityComparer : IEqualityComparer<VehicleInfoForFitments>
        {
            public bool Equals(VehicleInfoForFitments x, VehicleInfoForFitments y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.MakeName, y.MakeName) && string.Equals(x.ModelName, y.ModelName) && string.Equals(x.YearId, y.YearId) && string.Equals(x.SubModelName, y.SubModelName) && string.Equals(x.Engine, y.Engine) && string.Equals(x.Liter, y.Liter) && string.Equals(x.BlockType, y.BlockType) && string.Equals(x.Cylinders, y.Cylinders) && string.Equals(x.FuelTypeName, y.FuelTypeName) && string.Equals(x.FuelDeliveryTypeName, y.FuelDeliveryTypeName) && string.Equals(x.CC, y.CC) && string.Equals(x.VehicleId, y.VehicleId);
            }

            public int GetHashCode(VehicleInfoForFitments obj)
            {
                unchecked
                {
                    var hashCode = (obj.MakeName != null ? obj.MakeName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.ModelName != null ? obj.ModelName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.YearId != null ? obj.YearId.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.SubModelName != null ? obj.SubModelName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Engine != null ? obj.Engine.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Liter != null ? obj.Liter.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.BlockType != null ? obj.BlockType.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Cylinders != null ? obj.Cylinders.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.FuelTypeName != null ? obj.FuelTypeName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.FuelDeliveryTypeName != null ? obj.FuelDeliveryTypeName.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CC != null ? obj.CC.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.VehicleId != null ? obj.VehicleId.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public static IEqualityComparer<VehicleInfoForFitments> VehicleInfoForFitmentsComparer { get; } = new VehicleInfoForFitmentsEqualityComparer();

        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string YearId { get; set; }
        public string SubModelName { get; set; }
        public string Engine { get; set; }
        public string Liter { get; set; }
        public string BlockType { get; set; }
        public string Cylinders { get; set; }
        public string FuelTypeName { get; set; }
        public string FuelDeliveryTypeName { get; set; }
        public string CC { get; set; }
        public string VehicleId { get; set; }
    }
}
