using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn14ApiScraper.DataItems.Turn14
{
    public class AuthJson
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}
