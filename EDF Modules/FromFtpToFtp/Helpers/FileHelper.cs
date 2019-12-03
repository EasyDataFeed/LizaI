using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FromFtpToFtp.Helpers
{
    public class FileHelper
    {
        private const char ComaSeparator = ',';
        private const string Separator = ",";

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }
    }
}
