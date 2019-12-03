using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.Libs.Kravet
{
    public class ExtSettings
    {
        public string ExportFilePath { get; set; }
        public bool UseExistingExport { get; set; }
        public double WebPriceJober { get; set; }
        public double MSRP { get; set; }
        public bool DoBatch { get; set; }
        public string FTPLogin { get; set; }
        public string FTPPassword { get; set; }
        public string FTPHost { get; set; }
        public string FtpFilePath { get; set; }
    }
}
