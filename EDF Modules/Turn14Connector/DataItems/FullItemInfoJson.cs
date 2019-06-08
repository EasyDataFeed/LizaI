using Turn14Connector.DataItems.Turn14;

namespace Turn14Connector.DataItems
{
    public class FullItemInfoJson
    {
        public dataItems DataItemJson { get; set; }
        public SingleItemPricingJson SingleItemPricingJson { get; set; }
        public SingleItemDataJson SingleItemDataJson { get; set; }
        public InventorySingleItemJson InventorySingleItemJson { get; set; }
    }
}
