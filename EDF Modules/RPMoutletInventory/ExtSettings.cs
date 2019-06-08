using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMoutletInventory.DataItems;
using RPMoutletInventory.Enums;

namespace Databox.Libs.RPMoutletInventory
{
    public class ExtSettings
    {
        public bool UseTurn14 { get; set; }
        public bool UsePremier { get; set; }
        public bool UseMeyer { get; set; }
        public bool UseEbay { get; set; }
        public Turn14InventoryType Turn14InventoryType { get; set; }

        public string ebayLog { get; set; }
        public string ebayPass { get; set; }
        public string turn14Log { get; set; }
        public string turn14Pass { get; set; }
        public string motoLog { get; set; }
        public string motoPass { get; set; }
        public string mayerLogin { get; set; }
        public string mayerPass { get; set; }
        public string Email { get; set; }
        public string EbaySecurityToken { get; set; }
        public string InvFtpAddress { get; set; }
        public string InvFtpLogin { get; set; }
        public string InvFtpPassword { get; set; }
        public string apiTokenEB { get; set; }
        public DateTime mayerTokenExp { get; set; }
        public string mayerToken { get; set; }

        public string BrandsFilePath { get; set; }

        public bool Turn14InventoryFile { get; set; }
        public bool PremierInventoryFile { get; set; }
        public bool MeyerInventoryFile { get; set; }
        public bool UpdatePriceInSce { get; set; }
        public string LastDownloadRequestId
        {
            get { return ModuleSettings.Default.LastDownloadRequestId; }
        }
        public string LastUploadedJobId
        {
            get { return ModuleSettings.Default.LastUploadedJobId; }
        }
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