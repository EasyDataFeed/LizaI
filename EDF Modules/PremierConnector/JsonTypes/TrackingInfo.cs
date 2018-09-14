namespace PremierConnector.JsonTypes
{
    public class TrackingInfo
    {
        public string SalesOrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerPurchaseOrderNumber { get; set; }
        public Tracking[] Tracking { get; set; }
    }
}
