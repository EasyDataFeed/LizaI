using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MarksJewelersFtpData.Helper_Methods
{
    class FileHelper
    {
        public static string GetSettingsPath(string fileName, string partNumber)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, partNumber)))
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, partNumber));
            
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, partNumber, fileName);
        }

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath);
        }
    }
}
