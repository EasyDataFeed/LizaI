using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using LoadSceExport;
using Databox.Libs.LoadSceExport;
using LoadSceExport.Helpers;
using LoadSceExport.DataItems;

namespace WheelsScraper
{
    public class LoadSceExport : BaseScraper
    {
        public LoadSceExport()
        {
            Name = "LoadSceExport";
            Url = "https://www.LoadSceExport.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
        }

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

        //public override System.Windows.Forms.Control SettingsTab
        //{
        //	get
        //	{
        //		var frm = new ucExtSettings();
        //		frm.Sett = Settings;
        //		return frm;
        //	}
        //}

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

        protected override void RealStartProcess()
        {
            var wi = new ExtWareInfo();
            List<string> sceFiles = SceApiHelper.LoadProductsExport(Settings);
            //List<string> sceFiles = new List<string>() { @"E:\Leprizoriy\Work SCE\EDF\WheelsScraper 2\WheelsScraper\bin\Debug\newSceExport1.csv" };

            if (sceFiles.Count > 0)
            {
                MessagePrinter.PrintMessage($"SCE export downloaded");
            }
            else
            {
                MessagePrinter.PrintMessage($"SCE export not downloaded, please try again later", ImportanceLevel.Critical);
                return;
            }

            MessagePrinter.PrintMessage($"Read SCE export... please wait");
            List<ExtWareInfo> sceExportItems = new List<ExtWareInfo>();
            foreach (string sceFile in sceFiles)
            {
                sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile));
            }

            foreach (var item in sceExportItems)
            {
                AddWareInfo(item);
                OnItemLoaded(item);
            }

            MessagePrinter.PrintMessage("SCE export readed");
            StartOrPushPropertiesThread();
        }
    }
}
