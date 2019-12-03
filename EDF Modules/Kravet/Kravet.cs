using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Kravet;
using Databox.Libs.Kravet;
using Kravet.Helpers;
using System.IO;
using Ionic.Zip;
using Kravet.DataItems;

namespace WheelsScraper
{
    public class Kravet : BaseScraper
    {
        //private const string FTPLogin = "mahone";
        //private const string FTPPassword = "m0onSton3";
        //private const string FTPHost = "ftp://file.kravet.com";

        public Kravet()
        {
            Name = "Kravet";
            Url = "https://www.Kravet.com/";
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

            List<FtpDataItem> ftpDataItems = FileHelper.ReadFtpFile(csvFile);

            if (ftpDataItems.Count > 0)
            {
                MessagePrinter.PrintMessage($"FTP file was read");
            }
            else
            {
                MessagePrinter.PrintMessage($"FTP file empty", ImportanceLevel.Critical);
                return;
            }

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }

            List<string> sceFiles = new List<string>();
            List<PriceUpdateInfo> priceUpdateItems = new List<PriceUpdateInfo>();

            if (extSett.UseExistingExport)
            {
                MessagePrinter.PrintMessage($"Use existing export");
                sceFiles = new List<string>() { extSett.ExportFilePath };
            }
            else
            {
                MessagePrinter.PrintMessage($"Download SCE export... please wait");
                sceFiles = SceApiHelper.LoadProductsExport(Settings);

                if (sceFiles.Count > 0)
                {
                    MessagePrinter.PrintMessage($"SCE export downloaded");
                }
                else
                {
                    MessagePrinter.PrintMessage($"SCE export not downloaded, please try again later", ImportanceLevel.Critical);
                    return;
                }
            }

            MessagePrinter.PrintMessage($"Read SCE export... please wait");
            List<SceExportItem> sceExportItems = new List<SceExportItem>();

            foreach (string sceFile in sceFiles)
            {
                sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile));
            }

            foreach (SceExportItem sceExportItem in sceExportItems)
            {
                if (sceExportItem.ManufacturerPartNumber.Contains("@"))
                    sceExportItem.ManufacturerPartNumber = sceExportItem.ManufacturerPartNumber.TrimStart('@');

                foreach (FtpDataItem ftpDataItem in ftpDataItems)
                {
                    if (sceExportItem.f124brand == ftpDataItem.Brand && sceExportItem.ManufacturerPartNumber == ftpDataItem.ItemPartNumber)
                    {
                        var priceItem = new PriceUpdateInfo
                        {
                            Action = "update",
                            ProdId = sceExportItem.ProdId,
                            ProductType = sceExportItem.ProductType,
                            PartNumber = sceExportItem.PartNumber,
                            WebPrice = ftpDataItem.WHLSPrice + (extSett.WebPriceJober * ftpDataItem.WHLSPrice / 100),
                            Jobber = ftpDataItem.WHLSPrice + (extSett.WebPriceJober * ftpDataItem.WHLSPrice / 100),
                            MSRP = ftpDataItem.WHLSPrice + (extSett.MSRP * ftpDataItem.WHLSPrice / 100),
                            CostPrice = ftpDataItem.WHLSPrice
                        };

                        if (priceItem.WebPrice > priceItem.MSRP)
                        {
                            priceItem.MSRP = priceItem.WebPrice;
                        }

                        if (priceItem.CostPrice > priceItem.MSRP)
                        {
                            priceItem.CostPrice = priceItem.MSRP;
                        }

                        priceUpdateItems.Add(priceItem);
                    }
                }
            }

            if (priceUpdateItems.Count > 0)
            {
                MessagePrinter.PrintMessage($"Create local price file");
                string filePath = FileHelper.CreatePriceUpdateFile(FileHelper.GetSettingsPath("KravetPriceUpdate.csv"), priceUpdateItems);
                if (!string.IsNullOrEmpty(filePath))
                {
                    MessagePrinter.PrintMessage("Upload price batch file to FTP");
                    if (extSett.DoBatch)
                    {
                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, "KravetPriceUpdate.csv", filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            int batchId = SceApiHelper.BatchUpdate(urlForBatch, Settings);
                            MessagePrinter.PrintMessage($"File Batched. BatchId - {batchId}");
                        }

                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                    else
                    {
                        MessagePrinter.PrintMessage($"File created - {filePath}");
                    }
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
