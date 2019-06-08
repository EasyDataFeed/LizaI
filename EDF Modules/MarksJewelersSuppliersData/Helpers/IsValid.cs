using MarksJewelersSuppliersData.DataItems;
using MarksJewelersSuppliersData.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarksJewelersSuppliersData.Helpers
{
    public class IsValid
    {
        internal static bool ValidateItem(SceBatchItem item)
        {
            if (string.IsNullOrEmpty(item.ProductTitle))
                return false;
            if (string.IsNullOrEmpty(item.SpiderURL))
                return false;
            if (string.IsNullOrEmpty(item.Brand))
                return false;
            if (string.IsNullOrEmpty(item.Description))
                return false;
            if (string.IsNullOrEmpty(item.METADescription))
                return false;
            if (string.IsNullOrEmpty(item.METAKeywords))
                return false;
            if (string.IsNullOrEmpty(item.GeneralImage))
                return false;
            if (string.IsNullOrEmpty(item.MainCategory))
                return false;
            if (string.IsNullOrEmpty(item.SubCategory))
                return false;
            if (string.IsNullOrEmpty(item.Supplier))
                return false;
            if (string.IsNullOrEmpty(item.ProcessingTime))
                return false;
            if (string.IsNullOrEmpty(item.ShippingType))
                return false;
            if (string.IsNullOrEmpty(item.ShippingCarrier1))
                return false;
            if (string.IsNullOrEmpty(item.Allowground))
                return false;
            if (string.IsNullOrEmpty(item.PartNumber))
                return false;
            if (item.MSRP <= 0)
                return false;
            if (item.Jobber <= 0)
                return false;
            if (item.WebPrice <= 0)
                return false;
            if (item.CostPrice <= 0)
                return false;
            if (string.IsNullOrEmpty(item.ItemWeight))
                return false;
            if (string.IsNullOrEmpty(item.ItemHeight))
                return false;
            if (string.IsNullOrEmpty(item.ItemWidth))
                return false;
            if (string.IsNullOrEmpty(item.ItemLength))
                return false;
            if (string.IsNullOrEmpty(item.ShippingWeight))
                return false;
            if (string.IsNullOrEmpty(item.ShippingHeight))
                return false;
            if (string.IsNullOrEmpty(item.ShippingWidth))
                return false;
            if (string.IsNullOrEmpty(item.ShippingLength))
                return false;

            return true;
        }
    }
}
