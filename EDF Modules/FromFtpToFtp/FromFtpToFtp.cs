using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using FromFtpToFtp;
using Databox.Libs.FromFtpToFtp;
using System.IO;
using Ionic.Zip;
using FromFtpToFtp.Helpers;

namespace WheelsScraper
{
    public class FromFtpToFtp : BaseScraper
    {
        public FromFtpToFtp()
        {
            Name = "FromFtpToFtp";
            Url = "https://www.FromFtpToFtp.com/";
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

        protected override void RealStartProcess()
        {
            string localFilePath = FileHelper.GetSettingsPath("Mahone.zip");

            MessagePrinter.PrintMessage($"Download FTP file");
            string ftpFileName = FTPHelper.DownloadFtpFile(extSett.FTPLogin, extSett.FTPPassword, extSett.FTPHost, extSett.FtpFilePath, localFilePath);
            string csvFile = LoadCsvFile(localFilePath);

            if (!string.IsNullOrEmpty(csvFile))
            {
                MessagePrinter.PrintMessage($"FTP file downloaded");
                MessagePrinter.PrintMessage($"Upload file to FTP");
                string url = FtpHelper.UploadFileToFtp(extSett.ToFTPHost, extSett.ToFTPLogin, extSett.ToFTPPassword, "Mahone.csv", csvFile, true);
                MessagePrinter.PrintMessage($"File uploaded to FTP");

                if (File.Exists(csvFile))
                {
                    File.Delete(csvFile);
                }
            }

            StartOrPushPropertiesThread();
        }

        private string LoadCsvFile(string localFilePath)
        {
            try
            {
                if (File.Exists(localFilePath))
                {
                    MessagePrinter.PrintMessage(string.Format("File Mahone.zip downloaded!", localFilePath));
                    string unzippedFile = localFilePath.Replace(".zip", ".csv");
                    using (var zipArc = ZipFile.Read(localFilePath))
                    {
                        zipArc[0].FileName = Path.GetFileName(unzippedFile);
                        zipArc[0].Extract(Path.GetDirectoryName(unzippedFile),
                            ExtractExistingFileAction.OverwriteSilently);
                        MessagePrinter.PrintMessage(string.Format("File Mahone.zip unzipped!", unzippedFile));
                    }
                    File.Delete(localFilePath);
                    MessagePrinter.PrintMessage(string.Format("File Mahone.zip deleted!", localFilePath));
                    return unzippedFile;
                }
                else
                {
                    MessagePrinter.PrintMessage(string.Format("File Mahone.zip not found!", localFilePath),
                        ImportanceLevel.Critical);
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message, ImportanceLevel.Critical);
            }
            return string.Empty;
        }

        protected void ProcessBrandsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brands list processed");
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 1)
                act = ProcessBrandsListPage;
            else act = null;

            return act;
        }
    }
}
