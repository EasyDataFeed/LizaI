using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.Libs.EbayNewParts
{
    public class ExtSettings
    {
        public List<string> SotreIdsToScrap { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool FlgScrap { get; set; }
        public int CurrentCountItems { get; set; }
        public bool LastCheck { get; set; }
    }
}
