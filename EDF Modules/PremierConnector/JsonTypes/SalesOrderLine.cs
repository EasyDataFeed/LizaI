namespace PremierConnector.JsonTypes
{
    public class SalesOrderLine
    {
        public string ItemNumber { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string ShipMethod { get; set; }
        public string WarehouseCode { get; set; }
    }
}
