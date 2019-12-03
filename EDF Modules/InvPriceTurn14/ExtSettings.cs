using InvPriceTurn14.DataItems;
using System.Collections.Generic;

namespace Databox.Libs.InvPriceTurn14
{
    public class ExtSettings
    {
        public string ExportFilePath { get; set; }
        public string turn14Log { get; set; }
        public string turn14Pass { get; set; }
        public string BrandsFilePath { get; set; }
        public bool Turn14InventoryFile { get; set; }
        public bool UpdatePriceInSce { get; set; }
        public List<SceItem> SceItems { get; set; }
        public List<BrandsAlignment> BrandsAlignmentsItems { get; set; }
        public List<TransferInfoItem> TransferInfoItems { get; set; }
        public ExtSettings()
        {
            SceItems = new List<SceItem>();
            BrandsAlignmentsItems = new List<BrandsAlignment>();
            TransferInfoItems = new List<TransferInfoItem>();
        }
    }
}
