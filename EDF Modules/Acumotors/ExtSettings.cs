using Acumotors.DataItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.Libs.Acumotors
{
    public class ExtSettings
    {
        public string turn14Log { get; set; }
        public string turn14Pass { get; set; }
        public string BrandsFilePath { get; set; }
        public string SpecificationFilePath { get; set; }
        public bool DoBatch { get; set; }
        public double PercentageForMSRP { get; set; }
        public bool doPricePerPound { get; set; }
        public double minSurcharge { get; set; }
        public double surchargePerLb { get; set; }
        public bool Percentage { get; set; }
        public bool UseGoogleSheets { get; set; }
        public string GoogleSheetsLink { get; set; }
        public List<BrandsAlignment> BrandsAlignmentsItems { get; set; }
        public List<SpecificationFile> SpecificationFileItems { get; set; }
        public List<TransferInfoItem> TransferInfoItems { get; set; }
        public List<UpdateInfo> UpdateInfos { get; set; }
        public List<LongPNFile> LongPNFiles { get; set; }
        public ExtSettings()
        {
            BrandsAlignmentsItems = new List<BrandsAlignment>();
            TransferInfoItems = new List<TransferInfoItem>();
            UpdateInfos = new List<UpdateInfo>();
            LongPNFiles = new List<LongPNFile>();
        }
    }
}
