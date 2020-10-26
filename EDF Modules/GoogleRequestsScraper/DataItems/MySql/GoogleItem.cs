using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleRequestsScraper.DataItems.MySql
{
    public class GoogleItem
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string Domain { get; set; }
        public string Position { get; set; }
        public string State { get; set; }
        public string Device { get; set; }
        public string Time { get; set; }
        public string Placement { get; set; }
        public string CompanyName { get; set; }
    }
}
