using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.Libs.MarksJewelersFtpData
{
    public class ExtSettings
    {
        public bool DownloadRARiamGroup { get; set; }
        public bool DownloadRosyBlue { get; set; }
        public bool DownloadLSDiamonds { get; set; }
        public bool DownloadRDITrading { get; set; }
        public bool DownloadSaharAtid { get; set; }

        public bool DeleteImgRARiamGroup { get; set; }
        public bool DeleteImgRosyBlue { get; set; }
        public bool DeleteImgLSDiamonds { get; set; }
        public bool DeleteImgRDITrading { get; set; }
        public bool DeleteImgSaharAtid { get; set; }

        public string FtpAdress { get; set; }
        public string FtpLogin { get; set; }
        public string FtpPassword { get; set; }
        public bool UploadInventory { get; set; }

    }
}
