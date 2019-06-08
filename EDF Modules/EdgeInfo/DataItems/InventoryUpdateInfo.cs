using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeInfo.DataItems
{
    public class InventoryUpdateInfo
    {
        public InventoryUpdateInfo() { }

        public InventoryUpdateInfo(IGrouping<string, InventoryUpdateInfo> ob)
        {
            PartNumber = ob.First().PartNumber;
            Qty = ob.Count();
            Supplier = ob.First().Supplier;
            Warehouse = ob.First().Supplier;
        }

        public string PartNumber { get; set; }
        public int Qty { get; set; }
        public string Supplier { get; set; }
        public string Warehouse { get; set; }
    }
}
