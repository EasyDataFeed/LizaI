using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turn14ApiScraper.DataItems.TEST
{
    public class TestJson
    {
        public List<Products> products { get; set; }
        public int itemCount { get; set; }
        public string status { get; set; }
    }

    public class Products
    {
        public string baseSKU { get; set; }
        public string benchmarkSKU { get; set; }
        public string gender { get; set; }
        public string brand { get; set; }
        public string collection { get; set; }
        public string subCollection { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public double width { get; set; }
        public string metalType { get; set; }
        public double ringSize { get; set; }
        public double cost { get; set; }
        public string itemTitle { get; set; }
        public string description { get; set; }
        public double totalDiamondWeight { get; set; }
        public string stoneSetting { get; set; }
        public double totalDiamondCount { get; set; }
        public string diamondColor { get; set; }
        public string diamondClarity { get; set; }
        public string diamondShape { get; set; }
    }
}
