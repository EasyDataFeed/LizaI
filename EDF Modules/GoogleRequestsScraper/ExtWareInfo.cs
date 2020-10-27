using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleRequestsScraper.DataItems;
using WheelsScraper;

namespace GoogleRequestsScraper
{
    public class ExtWareInfo : WareInfo
    {
        public ExtWareInfo()
        {

        }

        public ExtWareInfo(GoogleScrapedItem item)
        {
            Device = item.Device;
            Domain = item.Domain;
            Keyword = item.Keyword;
            Placement = item.Placement;
            State = item.State;
            Time = item.Time;
            Position = item.Position;
            CompanyName = item.CompanyName;
            DumpPageId = item.DumpPageId;
            Title = item.Title;
            UniqueDomains = item.UniqueDomains;
        }

        public string Keyword { get; set; }
        public string Domain { get; set; }
        public string Position { get; set; }
        public string State { get; set; }
        public string Device { get; set; }
        public string Time { get; set; }
        public string Placement { get; set; }
        public string CompanyName { get; set; }
        public string DumpPageId { get; set; }
        public string Title { get; set; }
        public string UniqueDomains { get; set; }
    }
}
