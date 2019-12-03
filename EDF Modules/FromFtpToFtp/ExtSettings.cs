using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.Libs.FromFtpToFtp
{
    public class ExtSettings
    {
        public string FTPLogin { get; set; }
        public string FTPPassword { get; set; }
        public string FTPHost { get; set; }
        public string FtpFilePath { get; set; }
        public string ToFTPLogin { get; set; }
        public string ToFTPPassword { get; set; }
        public string ToFTPHost { get; set; }
    }
}
