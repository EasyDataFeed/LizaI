using ProductsToPinterest.DataItems;
using ProductsToPinterest.Enums;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Databox.Libs.ProductsToPinterest
{
    public class ExtSettings
    {
        public string ExportFilePath { get; set; }
        public bool UseBusinessesAccount { get; set; }
        public bool ChangeWebsite { get; set; }
        public string Brand { get; set; }
        [XmlIgnore] public List<BrandItem> BrandItems { get; set; }

        public ActionType ActionType { get; set; }

        public ExtSettings()
        {
            BrandItems = new List<BrandItem>();
        }
    }
}
