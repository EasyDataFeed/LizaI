using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.Libs.EdgeInfo
{
    public class ExtSettings
    {
        public string FTPLogin { get; set; }
        public string FTPPassword { get; set; }
        public string FTPHost { get; set; }
        public string SupplierFilePath { get; set; }
        public string SupplierFileSkip { get; set; }
        public string SupplierFileZeroSkip { get; set; }
        public bool DoBatch { get; set; }
    }
}
