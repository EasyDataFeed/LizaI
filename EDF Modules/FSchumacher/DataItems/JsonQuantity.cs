using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FSchumacher.DataItems
{
    public class JsonArray
    {
        public JsonInfo InfoList { get; set; }
    }

    public class JsonInfo
    {
        [JsonProperty(PropertyName = "LotNumber")]
        public string LotNumber { get; set; }

        [JsonProperty(PropertyName = "Inventory")]
        public string Inventory { get; set; }

        [JsonProperty(PropertyName = "Warehouse")]
        public string Warehouse { get; set; }

        [JsonProperty(PropertyName = "SoldByUnit")]
        public string SoldByUnit { get; set; }
    }
}
