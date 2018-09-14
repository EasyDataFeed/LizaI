using System.Collections.Generic;

namespace PremierConnector.JsonTypes
{
    public class SalesOrder
    {
        public string CustomerPurchaseOrderNumber { get; set; }
        public string Note { get; set; }
        public bool SignatureRequired { get; set; }
        public string ShipMethod { get; set; }
        public string WarehouseCode { get; set; }
        public ShipToAddress ShipToAddress { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; }
    }
}
