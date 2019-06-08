using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Dewalt
{
    public class ExtWareInfo : WareInfo
    {
        public string AnchorText { get; set; }
        public string ProductTitle { get; set; }
        public string BulletPoint { get; set; }
        public string Includes { get; set; }
        public string GeneralImage { get; set; }
        public string ProductDescription { get; set; }
        public string Specifications { get; set; }
        public string METADescription { get; set; }
        public string METAKeywords { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string SectionCategory { get; set; }
    }
}
