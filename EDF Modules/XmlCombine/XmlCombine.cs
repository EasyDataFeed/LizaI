#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using XmlCombine;
using Databox.Libs.XmlCombine;
using DevExpress.XtraPrinting.Native;
using XmlCombine.DataItems;
using FileHelper = XmlCombine.Helpers.FileHelper;

#endregion

namespace WheelsScraper
{
    public class XmlCombine : BaseScraper
    {
        public XmlCombine()
        {
            Name = "XmlCombine";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();

            SpecialSettings = new ExtSettings();
        }

        #region Standart Methods

        private ExtSettings extSett
        {
            get
            {
                return (ExtSettings)Settings.SpecialSettings;
            }
        }

        public override Type[] GetTypesForXmlSerialization()
        {
            return new Type[] { typeof(ExtSettings) };
        }

        public override System.Windows.Forms.Control SettingsTab
        {
            get
            {
                var frm = new ucExtSettings();
                frm.Sett = Settings;
                return frm;
            }
        }

        public override WareInfo WareInfoType
        {
            get
            {
                return new ExtWareInfo();
            }
        }

        protected override bool Login()
        {
            return true;
        }

        #endregion

        private bool CheckValidSettings()
        {
            if (string.IsNullOrEmpty(extSett.InputDirectory))
            {
                MessagePrinter.PrintMessage($"Select input directory", ImportanceLevel.Critical);
                return false;
            }

            if (string.IsNullOrEmpty(extSett.OutputDirectory))
            {
                MessagePrinter.PrintMessage($"Select output directory", ImportanceLevel.Critical);
                return false;
            }

            if (extSett.UseFilterFile)
                if (string.IsNullOrEmpty(extSett.FilterFileFilePath))
                {
                    MessagePrinter.PrintMessage($"Select filter file", ImportanceLevel.Critical);
                    return false;
                }

            return true;
        }

        protected override void RealStartProcess()
        {
            if (!CheckValidSettings())
                return;

            List<FilterItem> filterItems = new List<FilterItem>();

            if (extSett.UseFilterFile)
            {
                MessagePrinter.PrintMessage($"Read filter file");

                filterItems = FileHelper.ReadFilterItems(extSett.FilterFileFilePath);
                if (filterItems.Count == 0)
                {
                    MessagePrinter.PrintMessage($"not found items in filter file", ImportanceLevel.Critical);
                    return;
                }

                MessagePrinter.PrintMessage($"Filter file readed");
            }

            MessagePrinter.PrintMessage($"Read input directory");
            List<string> files = FileHelper.GetDirectoryXmlFileList(extSett.InputDirectory);
            if (files.Count == 0)
            {
                MessagePrinter.PrintMessage($".xml files not found in this directory - {extSett.InputDirectory}");
                return;
            }
            MessagePrinter.PrintMessage($"files found - {files.Count}");

            List<ACES> fileList = new List<ACES>();
            foreach (string file in files)
            {
                try
                {
                    fileList.Add(FileHelper.ReadFromXmlFile<ACES>(file));
                }
                catch (Exception e)
                {
                    MessagePrinter.PrintMessage($"{e.Message} in this file - {file}", ImportanceLevel.High);
                }
            }

            List<string> documentTitleList = new List<string>();
            foreach (string file in files)
                documentTitleList.Add(Path.GetFileNameWithoutExtension(file)?.Split('_')[extSett.VehicleNameWordPosition-1]);
            documentTitleList = documentTitleList.Distinct().ToList();

            MessagePrinter.PrintMessage($"files with different names - {documentTitleList.Count}");

            foreach (string name in documentTitleList)
            {
                var items = fileList.Where(i => i.Header.DocumentTitle.Contains(name)).ToList();

                if (items.Count == 0)
                    continue;

                ACES acesResultInfo = new ACES();
                List<ACESApp> acesApps = new List<ACESApp>();
                bool firstString = false;
                foreach (ACES acess in items)
                {
                    try
                    {
                        if (!firstString)
                        {
                            acesResultInfo.Header = acess.Header;
                            acesResultInfo.version = acess.version;
                            firstString = true;
                        }

                        var appInfo = acess.App.ToList();
                        acesApps.AddRange(appInfo);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
                acesResultInfo.Header.DocumentTitle = name;
                acesResultInfo.App = acesApps.ToArray();

                //CheckBrand(acesResultInfo.App, filterItems);

                if (extSett.UseFilterFile)
                    SplitFile(filterItems, acesResultInfo, name);

                acesResultInfo.Footer = new ACESFooter { RecordCount = acesResultInfo.App.Count() };
                FileHelper.WriteToXmlFile<ACES>($"{extSett.OutputDirectory}/{name}.xml", acesResultInfo);

                MessagePrinter.PrintMessage($"File Created - {extSett.OutputDirectory}/{name}.xml");
           }

            StartOrPushPropertiesThread();
        }

        private void CheckBrand(ACESApp[] app, List<FilterItem> filterItems)
        {
            foreach (FilterItem filterItem in filterItems)
            {
                if (filterItem.Brand != "VICTOR REINZ") continue;

                var item = app.FirstOrDefault(i => i.Part == filterItem.OEM);
                if (item != null)
                {

                }
            }
        }

        private void SplitFile(List<FilterItem> filterItems, ACES oldAcesResultInfo,string name)
        {
            var groupedFilterImtes = filterItems.GroupBy(i => i.Brand);

            MessagePrinter.PrintMessage($"Split files...");
            List<FilterItem> filterItemsToReplace = new List<FilterItem>();

            foreach (var filterGroup in groupedFilterImtes)
            {
                ACES newAcesResultInfo = new ACES();
                List<ACESApp> acesApps = new List<ACESApp>();

                bool firstString = false;

                foreach (FilterItem filterItem in filterGroup)
                {
                    var items = oldAcesResultInfo.App.Where(i => string.Equals(i.Part, filterItem.OEM)).ToList();

                    if (items.Count == 0)
                        continue;

                    try
                    {
                        if (!firstString)
                        {
                            newAcesResultInfo.Header = oldAcesResultInfo.Header;
                            newAcesResultInfo.version = oldAcesResultInfo.version;
                            firstString = true;
                        }

                        filterItemsToReplace.Add(filterItem);

                        //сначала записать список партов которые потом переименовать и удалить 
                        // и нужно учитывать бренд
                
                        acesApps.AddRange(DeepCopy<List<ACESApp>>(items));

                        //items.ForEach(i => i.Part = i.Part.Replace(filterItem.OEM, filterItem.Manufacturer));
                        //var removeArray = oldAcesResultInfo.App.ToList();
                        //removeArray.RemoveAll(i => string.Equals(i.Part, filterItem.Manufacturer));
                        //oldAcesResultInfo.App = removeArray.ToArray();

                        //acesApps.AddRange(items);
                        //var removeArray = oldAcesResultInfo.App.ToList();
                        //removeArray.RemoveAll(i => string.Equals(i.Part, filterItem.OEM));
                        //oldAcesResultInfo.App = removeArray.ToArray();
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }

                //MessagePrinter.PrintMessage($"items count {acesApps.Count} Make - {filterGroup.Key}");

                if (acesApps.Count > 0)
                {
                    foreach (FilterItem filterItem in filterItemsToReplace)
                    {
                        if(filterItem.Brand != filterGroup.Key) continue;

                        acesApps.ForEach(i => i.Part = i.Part.Replace(filterItem.OEM, filterItem.Manufacturer));

                        //acesApps.ForEach(i =>  i.Part = i.Part.Replace(filterItem.OEM, filterItem.Manufacturer));
                        //var removeArray = oldAcesResultInfo.App.ToList();
                        //removeArray.RemoveAll(i => string.Equals(i.Part, filterItem.Manufacturer));
                        //oldAcesResultInfo.App = removeArray.ToArray();
                    }

                    newAcesResultInfo.Footer = new ACESFooter { RecordCount = acesApps.Count };
                    newAcesResultInfo.App = acesApps.ToArray();

                    FileHelper.WriteToXmlFile<ACES>($"{extSett.OutputDirectory}/{name}-{filterGroup.Key}.xml", newAcesResultInfo);

                    MessagePrinter.PrintMessage($"File Created - {extSett.OutputDirectory}/{name}-{filterGroup.Key}.xml");
                }
            }

            foreach (FilterItem filterItem in filterItemsToReplace)
            {
                //oldAcesResultInfo.App.ForEach(i => i.Part = i.Part.Replace(filterItem.OEM, filterItem.Manufacturer));
                var removeArray = oldAcesResultInfo.App.ToList();
                removeArray.RemoveAll(i => string.Equals(i.Part, filterItem.OEM));
                oldAcesResultInfo.App = removeArray.ToArray();
            }
        }

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
