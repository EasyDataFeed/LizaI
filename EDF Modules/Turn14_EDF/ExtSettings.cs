using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Turn14_EDF.DateItems;

namespace Databox.Libs.Turn14_EDF
{
    public class ExtSettings
    {
        public ExtSettings()
        {
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            TransferInfoItems = new List<TransferInfoItem>();
        }

        public bool DoBatch { get; set; }
        public bool OrdersSync { get; set; }
        public bool InventorySync { get; set; }
        public bool PriceSync { get; set; }
        public string BrandFilePath { get; set; }
        public double PercentageOfCost { get; set; }
        public bool ConsiderMAPPrice { get; set; }
        [XmlIgnore]
        public DateTime DateFrom { get; set; }
        [XmlIgnore]
        public DateTime DateTo { get; set; }
        [XmlIgnore]
        public List<TransferInfoItem> TransferInfoItems { get; set; }
        [XmlIgnore]
        public List<SceExportItem> SceExportItems { get; set; }
        [XmlIgnore]
        public List<BrandsAlignment> BrandsAlignmentsItems { get; set; }
    }
}
