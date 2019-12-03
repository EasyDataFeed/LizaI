#region using

using System;

#endregion

namespace Terapeak.DataItems
{
    public class SellerFilter : CategoryFilter, ICloneable
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string CategoryFullName { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public string GetJsonSellersReq()
        {
            return
                "{\"id\":\"" + CategoryId + "\"" +
                ",\"siteID\":\"" + SiteId + "\"" +
                ",\"date_range\":" + DateRange +
                ",\"currency\":\"1\"" +
                ",\"limit\":" + Limit +
                ",\"offset\":" + Offset +
                ",\"order\":\"revenue\"" +
                ",\"orderDir\":\"desc\"}";
        }
    }
}
