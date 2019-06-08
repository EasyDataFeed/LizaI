using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerapeakVehicleMatching.DataItems
{
    public class IndexCollecttionItem
    {
        private sealed class TearapeakIndexExportIndexEqualityComparer : IEqualityComparer<IndexCollecttionItem>
        {
            public bool Equals(IndexCollecttionItem x, IndexCollecttionItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.TearapeakIndex == y.TearapeakIndex && x.ExportIndex == y.ExportIndex;
            }

            public int GetHashCode(IndexCollecttionItem obj)
            {
                unchecked
                {
                    return (obj.TearapeakIndex * 397) ^ obj.ExportIndex;
                }
            }
        }

        public static IEqualityComparer<IndexCollecttionItem> TearapeakIndexExportIndexComparer { get; } = new TearapeakIndexExportIndexEqualityComparer();

        public int TearapeakIndex { get; set; }
        public int ExportIndex { get; set; }
    }
}
