using Pinterest.DataItems;
using Pinterest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Databox.Libs.Pinterest
{
    public class ExtSettings
    {
        public string ExportFilePath { get; set; }
        public bool UseExistingExport { get; set; }
        public bool UseBusinessesAccount { get; set; }
        public bool ChangeWebsite { get; set; }
        public bool SkipHidden { get; set; }
        public string Brand { get; set; }
        [XmlIgnore] public List<BrandItem> BrandItems { get; set; }

        public ActionType ActionType { get; set; }

        public ExtSettings()
        {
            BrandItems = new List<BrandItem>();
        }
    }
}
