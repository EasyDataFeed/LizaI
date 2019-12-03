#region using

using System;
using System.Text.RegularExpressions;

#endregion

namespace Terapeak.DataItems
{
    public class CategoryFilter
    {
        public int DateRange { get; set; }
        public int SiteId { get; set; }
        public int CategoryId { get; set; }
        public string PageName { get; set; }

        public string GetJsonCategoriesReq()
        {
            return "{\"id\":" + CategoryId + "," +
                         " \"siteID\":\"" + SiteId + "\"," +
                         " \"order\":\"Revenue\", " +
                         "\"date_range\":" + DateRange +
                         "}";
        }

        public SellerFilter Convert2SellerFilter()
        {
            var selFilter = new SellerFilter
            {
                DateRange = DateRange,
                SiteId = SiteId,
                CategoryId = CategoryId,
                PageName = PageName
            };
            return selFilter;
        }
    }
}
