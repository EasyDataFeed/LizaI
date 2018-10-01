namespace PremierConnector
{
    public class ProductsPartNumber
    {
        public string PartNumber { get; set; }
        public int Prodid { get; set; }
        public int ProductType { get; set; }

        public override string ToString()
        {
            return PartNumber;
        }
    }
}
