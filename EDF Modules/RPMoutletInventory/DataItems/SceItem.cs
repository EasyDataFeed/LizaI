using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPMoutletInventory.DataItems
{
    public class SceItem
    {
        public int ProdId { get; set; }
        public int ProductType { get; set; }
        public string Brand { get; set; }
        public string PartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string Upc { get; set; }
    }
}
