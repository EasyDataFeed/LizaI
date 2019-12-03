using InvPricePremier.DataItems;
using System.Collections.Generic;

namespace Databox.Libs.InvPricePremier
{
    public class ExtSettings
    {
        public string ExportFilePath { get; set; }
        public string InvFtpAddress { get; set; }
        public string InvFtpLogin { get; set; }
        public string InvFtpPassword { get; set; }
        public string BrandsFilePath { get; set; }
        public bool PremierInventoryFile { get; set; }
        public bool PremierPriceFile { get; set; }
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
