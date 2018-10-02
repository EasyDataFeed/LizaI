namespace PremierConnector.JsonTypes
{
    public class Tracking
    {
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public bool IsDropShip { get; set; }
        public PackageItem[] PackageItems { get; set; }
    }
}
