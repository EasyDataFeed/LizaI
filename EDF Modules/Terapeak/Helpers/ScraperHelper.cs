using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terapeak.Helpers
{
   public static class ScraperHelper
    {
       public static string GetCategoriesListUrl(string token, string pagename)
       {
           return String.Format("https://sell.terapeak.com/services/ebay/category/structure?" +
                                "token={0}" +
                                "&ulpagename={1}", token, pagename);
       }

        public static string GetSellersListUrl(string token, string pagename)
       {
           return String.Format("https://sell.terapeak.com/services/ebay/legacy/categoryresearch/categorysellers?" +
                                "token={0}" +
                                "&ulpagename={1}&", token, pagename);
       }
    }
}
