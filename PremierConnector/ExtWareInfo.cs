using WheelsScraper;

namespace PremierConnector
{
    public class ExtWareInfo : WareInfo
    {
        public double WebPrice { get; set; }
        public string Action { get; set; }
        public int ProductType { get; set; }
        public int Prodid { get; set; }
    }
}
