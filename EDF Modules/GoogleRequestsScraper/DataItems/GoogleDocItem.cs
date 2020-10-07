using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleRequestsScraper.DataItems
{
    public class GoogleDocItem
    {
        public string Keyword { get; set; }
        public string Domain { get; set; }
        public string Position { get; set; }
        public string State { get; set; }
        public string Device { get; set; }
        public string Time { get; set; }
    }
}
