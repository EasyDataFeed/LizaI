using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarksJewelersSuppliersData.DataItems
{
    public class SceBatchItem
    {
        public class SceBatchItemProductTitleEqualityComparer : IEqualityComparer<SceBatchItem>
        {
            public bool Equals(SceBatchItem x, SceBatchItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.SpiderURL, y.SpiderURL);
            }

            public int GetHashCode(SceBatchItem obj)
            {
                return (obj.SpiderURL != null ? obj.SpiderURL.GetHashCode() : 0);
            }
        }

        public string Action { get; set; }
        public string ProductType { get; set; }
        public string ProductTitle { get; set; }
        public string AnchorText { get; set; }
        public string SpiderURL { get; set; }
        public string SubTitle { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string METADescription { get; set; }
        public string METAKeywords { get; set; }
        public string GeneralImage { get; set; }
        public string PartNumber { get; set; }
        public double MSRP { get; set; }
        public double Jobber { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
        public string Supplier { get; set; }
        public string ReOrderSupplier { get; set; }
        public string Warehouse { get; set; }
        public string ProcessingTime { get; set; }
        public string ShippingType { get; set; }
        public string ShippingCarrier1 { get; set; }
        public string Allowground { get; set; }
        public string Allow3day { get; set; }
        public string Allow2day { get; set; }
        public string Allownextday { get; set; }
        public string ShippingGroundRate { get; set; }
        public string ShippingNextDayAirRate { get; set; }
        public string ItemWeight { get; set; }
        public string ItemHeight { get; set; }
        public string ItemWidth { get; set; }
        public string ItemLength { get; set; }
        public string ShippingWeight { get; set; }
        public string ShippingHeight { get; set; }
        public string ShippingWidth { get; set; }
        public string ShippingLength { get; set; }
        public string AllowNewCategory { get; set; }
        public string AllowNewBrand { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string SectionCategory { get; set; }
        public string CrossSellMainCat1 { get; set; }
        public string CrossSellSubCat1 { get; set; }
        public string CrossSellSecCat1 { get; set; }
        public string Specifications { get; set; }
        public string CrossSellMainCat2 { get; set; }
        public string CrossSellSubCat2 { get; set; }
        public string CrossSellSecCat2 { get; set; }
        public string CrossSellMainCat3 { get; set; }
        public string CrossSellSubCat3 { get; set; }
        public string CrossSellSecCat3 { get; set; }
        public string CrossSellMainCat4 { get; set; }
        public string CrossSellSubCat4 { get; set; }
        public string CrossSellSecCat4 { get; set; }
        public string CrossSellMainCat5 { get; set; }
        public string CrossSellSubCat5 { get; set; }
        public string CrossSellSecCat5 { get; set; }
        public string CrossSellMainCat6 { get; set; }
        public string CrossSellSubCat6 { get; set; }
        public string CrossSellSecCat6 { get; set; }
        public string CrossSellMainCat7 { get; set; }
        public string CrossSellSubCat7 { get; set; }
        public string CrossSellSecCat7 { get; set; }
        public string CrossSellMainCat8 { get; set; }
        public string CrossSellSubCat8 { get; set; }
        public string CrossSellSecCat8 { get; set; }
        public string CrossSellMainCat9 { get; set; }
        public string CrossSellSubCat9 { get; set; }
        public string CrossSellSecCat9 { get; set; }
        public string CrossSellMainCat10 { get; set; }
        public string CrossSellSubCat10 { get; set; }
        public string CrossSellSecCat10 { get; set; }
        public string CrossSellMainCat11 { get; set; }
        public string CrossSellSubCat11 { get; set; }
        public string CrossSellSecCat11 { get; set; }
        public string CrossSellMainCat12 { get; set; }
        public string CrossSellSubCat12 { get; set; }
        public string CrossSellSecCat12 { get; set; }
        public string CrossSellMainCat13 { get; set; }
        public string CrossSellSubCat13 { get; set; }
        public string CrossSellSecCat13 { get; set; }
        public string CrossSellMainCat14 { get; set; }
        public string CrossSellSubCat14 { get; set; }
        public string CrossSellSecCat14 { get; set; }
        public string CrossSellMainCat15 { get; set; }
        public string CrossSellSubCat15 { get; set; }
        public string CrossSellSecCat15 { get; set; }
        public string CrossSellMainCat16 { get; set; }
        public string CrossSellSubCat16 { get; set; }
        public string CrossSellSecCat16 { get; set; }
        public string CrossSellMainCat17 { get; set; }
        public string CrossSellSubCat17 { get; set; }
        public string CrossSellSecCat17 { get; set; }
        public string CrossSellMainCat18 { get; set; }
        public string CrossSellSubCat18 { get; set; }
        public string CrossSellSecCat18 { get; set; }
        public string CrossSellMainCat19 { get; set; }
        public string CrossSellSubCat19 { get; set; }
        public string CrossSellSecCat19 { get; set; }
        public string CrossSellMainCat20 { get; set; }
        public string CrossSellSubCat20 { get; set; }
        public string CrossSellSecCat20 { get; set; }
        public string CrossSellMainCat21 { get; set; }
        public string CrossSellSubCat21 { get; set; }
        public string CrossSellSecCat21 { get; set; }
        public string CrossSellMainCat22 { get; set; }
        public string CrossSellSubCat22 { get; set; }
        public string CrossSellSecCat22 { get; set; }
        public string CrossSellMainCat23 { get; set; }
        public string CrossSellSubCat23 { get; set; }
        public string CrossSellSecCat23 { get; set; }
        public string CrossSellMainCat24 { get; set; }
        public string CrossSellSubCat24 { get; set; }
        public string CrossSellSecCat24 { get; set; }
        public string CrossSellMainCat25 { get; set; }
        public string CrossSellSubCat25 { get; set; }
        public string CrossSellSecCat25 { get; set; }
        public string CrossSellMainCat26 { get; set; }
        public string CrossSellSubCat26 { get; set; }
        public string CrossSellSecCat26 { get; set; }
        public string CrossSellMainCat27 { get; set; }
        public string CrossSellSubCat27 { get; set; }
        public string CrossSellSecCat27 { get; set; }
        public string CrossSellMainCat28 { get; set; }
        public string CrossSellSubCat28 { get; set; }
        public string CrossSellSecCat28 { get; set; }
        public string CrossSellMainCat29 { get; set; }
        public string CrossSellSubCat29 { get; set; }
        public string CrossSellSecCat29 { get; set; }
        public string CrossSellMainCat30 { get; set; }
        public string CrossSellSubCat30 { get; set; }
        public string CrossSellSecCat30 { get; set; }
        public string CrossSellMainCat31 { get; set; }
        public string CrossSellSubCat31 { get; set; }
        public string CrossSellSecCat31 { get; set; }
        public string CrossSellMainCat32 { get; set; }
        public string CrossSellSubCat32 { get; set; }
        public string CrossSellSecCat32 { get; set; }
        public string CrossSellMainCat33 { get; set; }
        public string CrossSellSubCat33 { get; set; }
        public string CrossSellSecCat33 { get; set; }
        public string CrossSellMainCat34 { get; set; }
        public string CrossSellSubCat34 { get; set; }
        public string CrossSellSecCat34 { get; set; }
        public string CustomHtmlAboveQty { get; set; }
        public int Index { get; set; }
    }
}
