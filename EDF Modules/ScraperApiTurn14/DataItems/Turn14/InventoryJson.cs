using System;
using System.Collections.Generic;

namespace ScraperApiTurn14.DataItems.Turn14
{
    public class InventoryJson
    {
        public List<dataInventory> data { get; set; }
        public metaInventory meta { get; set; }
        public linksInventory links { get; set; }
    }

    public class dataInventory
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesInventory attributes { get; set; }
        public relationshipsInventory relationships { get; set; }
    }

    public class attributesInventory
    {
        public Dictionary<String, string> inventory { get; set; }
        public manufacturerInventory manufacturer { get; set; }
    }

    public class InventoryInfo
    {
        public Dictionary<String, string> inventory { get; set; }
    }

    public class manufacturerInventory
    {
        public string stock { get; set; }
        public DateTime esd { get; set; }
    }

    public class relationshipsInventory
    {
        public itemInventory item { get; set; }
    }

    public class itemInventory
    {
        public string links { get; set; }
    }

    public class metaInventory
    {
        public int total_pages { get; set; }
    }

    public class linksInventory
    {
        public string self { get; set; }
        public string last { get; set; }
        public string next { get; set; }
    }
}
