namespace PremierConnector.JsonTypes
{
    public class PremierPricingItem
    {
        public int Id { get; set; }
        public string ItemNumber { get; set; }
        public Pricing[] Pricing { get; set; }
    }
}
