namespace PremierConnector.JsonTypes
{
    public class PremierInventoryItem
    {
        public string ItemNumber { get; set; }
        public Inventory[] Inventory { get; set; }
    }
}