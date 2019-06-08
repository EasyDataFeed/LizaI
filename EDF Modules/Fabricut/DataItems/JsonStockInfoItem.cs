using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Fabricut.DataItems
{
    public class JsonStockInfoItem
    {
        [JsonProperty(PropertyName = "pricing")]
        public Pricing Pricing { get; set; }

        [JsonProperty(PropertyName = "stock")]
        public Stock Stock { get; set; }
    }

    public class Pricing
    {
        [JsonProperty(PropertyName = "per_unit")]
        public double? PricePerUnit { get; set; }

        [JsonProperty(PropertyName = "per_piece")]
        public double? PricePerPiece { get; set; }

        [JsonProperty(PropertyName = "per_halfpiece")]
        public double? PricePerHalfPiece { get; set; }
    }

    public class Stock
    {
        [JsonProperty(PropertyName = "current")]
        public Current Current { get; set; }

        [JsonProperty(PropertyName = "unit")]
        public Unit Unit { get; set; }
    }

    public class Current
    {
        [JsonProperty(PropertyName = "memos")]
        public int? Memos { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int? Total { get; set; }
    }

    public class Unit
    {
        [JsonProperty(PropertyName = "long")]
        public Long Long { get; set; }
    }

    public class Long
    {
        [JsonProperty(PropertyName = "Singular")]
        public string Singular { get; set; }

        [JsonProperty(PropertyName = "Plural")]
        public string Plural { get; set; }
    }
}
