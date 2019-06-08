using System.Collections.Generic;

namespace Turn14ApiScraper.DataItems.SCE
{
    public class SceExportItem : IEqualityComparer<SceExportItem>
    {

        #region Comparators

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

        #endregion

        #region Properties

        public string PartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string ProdId { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
        public double MSRP { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
        public double Jobber { get; set; }

        #endregion

    }
}
