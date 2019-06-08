using Pinterest.DataItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.Libs.Pinterest
{
    public class ExtSettings
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ExportFilePath { get; set; }
        public bool UseExistingExport { get; set; }
        public bool UseBusinessesAccount { get; set; }
        public string Brand { get; set; }
        public List<BrandItem> BrandItems { get; set; }
    }
}
