using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terapeak.DataItems
{
   public class CategoryResearch
    {
       public int category_id { get; set; }
       public string category_fullname { get; set; }
       public string category_name { get; set; }
       public int[] childList { get; set; }
    }
}
