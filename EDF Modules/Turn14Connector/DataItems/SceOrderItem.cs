namespace Turn14Connector.DataItems
{
    public class SceOrderItem
    {
        public int ProductId { get; set; }
        public string PartNumber { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Tax { get; set; }
    }
}
