using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace InvPriceTurn14.Store_Modules
{
    public class ModuleSettings
    {
        public static string ConfigFile { get; set; }
        private static ModuleSettings _default;

        public static string GetConfigFileName()
        {
            var settingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "EDF");
            var configFile = Path.Combine(settingsDir, "EbayFileExchanger.config");
            Directory.CreateDirectory(settingsDir);
            return configFile;
        }

        protected static ModuleSettings ReadConfig()
        {
            if (string.IsNullOrEmpty(ConfigFile))
            {
                ConfigFile = GetConfigFileName();
            }

            if (File.Exists(ConfigFile))
            {
                try
                {
                    var xs = new XmlSerializer(typeof(ModuleSettings));
                    using (var sr = File.OpenText(ConfigFile))
                    {
                        var xtr = new XmlTextReader(sr);
                        var conf = (ModuleSettings)xs.Deserialize(xtr);
                        return conf;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Parsing config failed", e);
                }
            }
            return new ModuleSettings();
        }
    }
}
