using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fabricut.DataItems
{
    public class SceItem
    {
        public string AnchorText { get; set; }      
        public string Brand { get; set; }      
        public string PrimaryKeyword { get; set; }
        public string ProductType { get; set; }
        public string ProdId { get; set; }
        public string PartNumber { get; set; }
        public double MSRP { get; set; }
        public double Jobber { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
    }
}
