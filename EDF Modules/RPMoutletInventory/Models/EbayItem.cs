using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMoutletInventory.Models
{
   public class EbayItem
    {
       public string ProductTitle { get; set; }
       public string PartNumber { get; set; }
       public string EbayId { get; set; }
       public string Brand { get; set; }


        public bool Found { get; set; }
        public string BrandSce { get; set; }
        public string ManufacturerPartSce { get; set; }
    }
}
