using Turn14ApiScraper.DataItems.Turn14;

namespace Turn14ApiScraper.DataItems
{
    public class FullItemInfoJson
    {
        public dataItems DataItemJson { get; set; }
        public SingleItemPricingJson SingleItemPricingJson { get; set; }
        public SingleItemDataJson SingleItemDataJson { get; set; }
        public InventorySingleItemJson InventorySingleItemJson { get; set; }
    }
}
