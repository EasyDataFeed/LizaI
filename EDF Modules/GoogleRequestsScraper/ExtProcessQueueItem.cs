using GoogleRequestsScraper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheelsScraper;

namespace GoogleRequestsScraper
{
    public class ExtProcessQueueItem : ProcessQueueItem
    {
        public DeviceType DeviceType { get; set; }
    }
}
