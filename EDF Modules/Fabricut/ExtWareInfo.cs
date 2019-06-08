using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Fabricut
{
    public class ExtWareInfo : WareInfo
    {
        public string ProductId { get; set; }
        public double? PricePerUnit { get; set; }
        public double? PricePerPeice { get; set; }
        public double? ProcePerHalfPiece { get; set; }
        public int? StockMemos { get; set; }
        public int? StockTotal { get; set; }
        public string UnitSingular { get; set; }
        public string UnitPlural { get; set; }
    }
}
