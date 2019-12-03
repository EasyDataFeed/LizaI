using ScraperApiTurn14.DataItems.Turn14;

namespace ScraperApiTurn14.DataItems
{
    public class FullItemInfoJson
    {
        public dataItems DataItemJson { get; set; }
        public SingleItemPricingJson SingleItemPricingJson { get; set; }
        public SingleItemDataJson SingleItemDataJson { get; set; }
        public InventorySingleItemJson InventorySingleItemJson { get; set; }
    }
}
