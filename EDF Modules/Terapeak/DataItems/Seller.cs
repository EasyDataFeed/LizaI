using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terapeak.DataItems
{
   public class Seller
    {
       public SellerItem[] seller { get; set; }
    }

    public class SellerItem
    {
        public string SellerName { get; set; }
        public decimal Revenue { get; set; }
        public int Listings { get; set; }
        public int SellerId { get; set; }
    }
}
 