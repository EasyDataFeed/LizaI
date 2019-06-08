using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Milwaukeetool
{
    public class ExtWareInfo : WareInfo
    {
        private sealed class MilwaukeetoolEqualityComparer : IEqualityComparer<ExtWareInfo>
        {
            public bool Equals(ExtWareInfo x, ExtWareInfo y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.PartNumber, y.PartNumber);
            }

            public int GetHashCode(ExtWareInfo obj)
            {
                unchecked
                {
                    return (obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0);
                }
            }
        }

        public static IEqualityComparer<ExtWareInfo> MilwaukeetoolPartNumberComparer { get; } = new MilwaukeetoolEqualityComparer();

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
