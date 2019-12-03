using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffRoadToyo.DataItems
{
    public class FileItems
    {
        public sealed class FileItemsEqualityComparer : IEqualityComparer<FileItems>
        {
            public bool Equals(FileItems x, FileItems y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.PartNumber, y.PartNumber) && string.Equals(x.PartNumber, y.PartNumber);
            }

            public int GetHashCode(FileItems obj)
            {
                unchecked
                {
                    return ((obj.PartNumber != null ? obj.Brand.GetHashCode() : 0) * 397);
                }
            }
        }

        public string Action { get; set; }
        public string ProductType { get; set; }
        public string ProductTitle { get; set; }
        public string AnchorText { get; set; }
        public string SpiderURL { get; set; }
        public string AllowNewBrand { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string METADescription { get; set; }
        public string METAKeywords { get; set; }
        public string GeneralImage { get; set; }
        public string AllowNewCategory { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string SectionCategory { get; set; }
        public string Specifications { get; set; }
        public string Supplier { get; set; }
        public string ReOrderSupplier { get; set; }
        public string Warehouse { get; set; }
        public string ProcessingTime { get; set; }
        public string Combinable { get; set; }
        public string Consolidatable { get; set; }
        public string PickupAvailable { get; set; }
        public string ShippingType { get; set; }
        public string ShippingCarrier1 { get; set; }
        public string ShippingCarrier2 { get; set; }
        public string ShippingCarrier3 { get; set; }
        public string ShippingCarrier4 { get; set; }
        public string ShippingCarrier5 { get; set; }
        public string Allowground { get; set; }
        public string Allow3day { get; set; }
        public string Allow2day { get; set; }
        public string Allownextday { get; set; }
        public string Allowinternational { get; set; }
        public string ShippingGroundRate { get; set; }
        public string Shipping3DayAirRate { get; set; }
        public string Shipping2DayAirRate { get; set; }
        public string ShippingNextDayAirRate { get; set; }
        public string HandlingSurcharge { get; set; }
        public string FileAttachments { get; set; }
        public string Videos { get; set; }
        public string SearchTags { get; set; }
        public string PartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string EAN { get; set; }
        public string UPC { get; set; }
        public string WarningNote { get; set; }
        public string MSRP { get; set; }
        public string Jobber { get; set; }
        public string WebPrice { get; set; }
        public string CostPrice { get; set; }
        public string ItemWeight { get; set; }
        public string ItemHeight { get; set; }
        public string ItemWidth { get; set; }
        public string ItemLength { get; set; }
        public string ShippingWeight { get; set; }
        public string ShippingHeight { get; set; }
        public string ShippingWidth { get; set; }
        public string ShippingLength { get; set; }
    }
}
