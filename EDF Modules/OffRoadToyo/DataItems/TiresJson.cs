using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OffRoadToyo.DataItems
{
    public class TiresJson
    {
        public List<tires> tires { get; set; }
    }

    public class tires
    {
        public string title { get; set; }
        public string description { get; set; }
        public string warranty { get; set; }
        public string image { get; set; }
        public string sizes_url { get; set; }
    }
}
