#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Fabricut;
using Databox.Libs.Fabricut;
using Fabricut.DataItems;
using Fabricut.Enums;
using Fabricut.Helpers;

#endregion

namespace WheelsScraper
{
    public class Fabricut : BaseScraper
    {
        private const string LOAD_PRODUCT_EXPORT = "Download SCE store export";
        private const string DOWNLOADED = "Export downloaded";
        private const string PROCESSING_EXPORT = "Processing export file.";

        private List<PriceUpdate> PriceUpdate { get; set; }

        public Fabricut()
        {
            Name = "Fabricut";
            Url = "https://www.Fabricut.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
            Complete += Fabricut_Complete;
        }

        private void Fabricut_Complete(object sender, EventArgs e)
        {
            //создание файла после скрапинга
            if (PriceUpdate.Count > 0)
            {
                MessagePrinter.PrintMessage($"Create local price file");
                string filePath = FileHelper.CreatePriceUpdateFile(FileHelper.GetSettingsPath("FabricutPriceUpdate.csv"), PriceUpdate);
                if (!string.IsNullOrEmpty(filePath))
                {
                    MessagePrinter.PrintMessage("Upload price batch file");
                    string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, "FabricutPriceUpdate.csv", filePath, true);
                    if (!string.IsNullOrEmpty(url))
                    {
                        string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                        if (extSett.DoBatch)
                        {
                            int batchId = SceApiHelper.BatchUpdate(urlForBatch, Settings);
                            MessagePrinter.PrintMessage($"File Batched. BatchId - {batchId}");
                        }
                    }

                    //удалить локальный файл
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
            else
            {
                MessagePrinter.PrintMessage($"Nothing to update");
            }
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

        protected override void RealStartProcess()
        {
            PriceUpdate = new List<PriceUpdate>();
            lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = (int)ItemType.ProcessingCsv });
            StartOrPushPropertiesThread();
        }

        protected void ProcessCsv(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                MessagePrinter.PrintMessage(LOAD_PRODUCT_EXPORT);

                string exportFilePath = SceApiHelper.LoadProductsExport(Settings);

                //string exportFilePath = @"D:\Leprizoriy\Work SCE\EDF\WheelsScraper 2\WheelsScraper\bin\debug\13521aad-5e59-43a0-b4d6-2061dc542ad6.csv";
                MessagePrinter.PrintMessage(DOWNLOADED);

                MessagePrinter.PrintMessage(PROCESSING_EXPORT);
                List<SceItem> sceItems = CsvManager.ReadCsvItems(exportFilePath);
                if (sceItems != null)
                {
                    foreach (SceItem sceItem in sceItems)
                    {
                        if (sceItem.Brand == "Fabricut")
                        {
                            string productId = sceItem.AnchorText.Substring(0, sceItem.AnchorText.IndexOf(" ")).Trim();
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem()
                                {
                                    ItemType = (int)ItemType.ProcessingRequests,
                                    Name = productId,
                                    Item = sceItems,
                                });
                            }
                        }
                    }
                }

                if (File.Exists(exportFilePath))
                    File.Delete(exportFilePath);
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message}", ImportanceLevel.Critical);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Export file processed");
            StartOrPushPropertiesThread();
        }

        protected void ProcessRequest(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                JsonStockInfoItem stockInfoItem = RequestHelper.GetStockItem(pqi.Name);
                if (stockInfoItem != null)
                {
                    var sceItems = (List<SceItem>)pqi.Item;
                    int counter = 0;
                    foreach (SceItem sceItem in sceItems)
                    {
                        if (sceItem.PartNumber == pqi.Name && sceItem.Brand == "Fabricut")
                        {
                            counter++;
                            double msrp = Convert.ToDouble(stockInfoItem.Pricing.PricePerUnit * 2);
                            double jobber = msrp * 0.8;
                            double webPrice = msrp * 0.8;
                            double costPrice = Convert.ToDouble(stockInfoItem.Pricing.PricePerUnit);

                            if (msrp == 0 || jobber == 0 || webPrice == 0 || costPrice == 0)
                            {
                                MessagePrinter.PrintMessage($"{sceItem.PartNumber} has 0 price", ImportanceLevel.High);
                                continue;
                            }
                            PriceUpdate.Add(new PriceUpdate
                            {
                                Action = "Update",
                                ProdId = sceItem.ProdId,
                                ProductType = sceItem.ProductType,
                                PartNumber = sceItem.PartNumber,
                                MSRP = msrp,
                                Jobber = jobber,
                                WebPrice = webPrice,
                                CostPrice = costPrice
                            });
                        }else if(counter > 1)
                        {

                        }
                    }

                    ExtWareInfo wi = new ExtWareInfo();

                    wi.ProductId = pqi.Name;

                    wi.PricePerPeice = stockInfoItem.Pricing.PricePerPiece;
                    wi.PricePerUnit = stockInfoItem.Pricing.PricePerUnit;
                    wi.ProcePerHalfPiece = stockInfoItem.Pricing.PricePerHalfPiece;

                    wi.StockMemos = stockInfoItem.Stock.Current.Memos;
                    wi.StockTotal = stockInfoItem.Stock.Current.Total;

                    wi.UnitPlural = stockInfoItem.Stock.Unit.Long.Plural;
                    wi.UnitSingular = stockInfoItem.Stock.Unit.Long.Singular;

                    AddWareInfo(wi);
                    OnItemLoaded(wi);
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message}", ImportanceLevel.Critical);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Product processed");
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case (int)ItemType.ProcessingCsv:
                    act = ProcessCsv;
                    break;
                case (int)ItemType.ProcessingRequests:
                    act = ProcessRequest;
                    break;
                default:
                    act = null;
                    break;
            }

            return act;
        }
    }
}
