#region using

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace Databox.Libs.RPMoutletInventory
{
    public class ModuleSettings
    {
        public string LastUploadedJobId { get; set; }

        public string LastDownloadRequestId { get; set; }

        public static string ConfigFile { get; set; }

        private static ModuleSettings _default;

        public static void ReloadConfig(string configFile = null)
        {
            ConfigFile = configFile;
            _default = null;
        }

        public static ModuleSettings Default
        {
            get
            {
                if (_default == null)
                {
                    _default = ReadConfig();
                }
                return _default;
            }
        }

        public void SaveConfig()
        {
            lock (this)
            {
                if (string.IsNullOrEmpty(ConfigFile))
                {
                    ConfigFile = GetConfigFileName();
                }
                var tmpFile = ConfigFile + ".tmp";
                using (var fs = File.Create(tmpFile))
                {
                    var xs = new XmlSerializer(typeof (ModuleSettings));
                    xs.Serialize(fs, _default);
                }
                File.Delete(ConfigFile);
                File.Move(tmpFile, ConfigFile);
            }
        }

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
//                if (!File.Exists(ConfigFile))
//                {
//                    settingsDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//                    ConfigFile = Path.Combine(settingsDir, "MeyerConnector.config");
//                }
            }

            if (File.Exists(ConfigFile))
            {
                try
                {
                    var xs = new XmlSerializer(typeof (ModuleSettings));
                    using (var sr = File.OpenText(ConfigFile))
                    {
                        var xtr = new XmlTextReader(sr);
                        var conf = (ModuleSettings) xs.Deserialize(xtr);
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