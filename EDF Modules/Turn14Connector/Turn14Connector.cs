//#define TEST

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using Turn14Connector;
using Databox.Libs.Turn14Connector;
using Turn14Connector.DataItems.Turn14;
using Turn14Connector.Helpers;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Web.Script.Serialization;
using Turn14Connector.DataItems;
using Turn14Connector.DataItems.SCE;
using Turn14Connector.Resources;

#endregion

namespace WheelsScraper
{
    public class Turn14Connector : BaseScraper
    {
        private AllLocationsJson AllLocations { get; set; }
        private bool ScraperStarted { get; set; }
        private Action ProcessAutoSyncOrders { get; set; }

        private static string GlobalOrdersError { get; set; }
        private bool UseProduction = false;

        #region Constans

        private const string TempFileNameFTP = "Turn14ConnectorAddedOrders.json";

        private const string PartNumberForCheck = "PartForCheck";
        private const string BrandForCheck = "BrandForCheck";
        private const string ExportLocalFilePath = @"D:\Altaresh\SVN\VS\EDF Development\EDF Modules\Turn14Connector\EDF\newSceExport1.csv";
        private const bool UseLocalExport = true;

        #endregion

        public Turn14Connector()
        {
            Name = "Turn14Connector";
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();

            SpecialSettings = new ExtSettings();
            AllLocations = new AllLocationsJson();

            Complete += Turn14Connector_Complete;
        }

        #region Complete

        private void Turn14Connector_Complete(object sender, EventArgs e)
        {
            if (Wares.Any())
            {
                List<InventoryUpdateInfo> inventoryUpdateItems = new List<InventoryUpdateInfo>();
                List<PriceUpdateInfo> priceUpdateItems = new List<PriceUpdateInfo>();
                List<ExtWareInfo> extWares = new List<ExtWareInfo>();
                foreach (WareInfo ware in Wares)
                {
                    ExtWareInfo extWare = (ExtWareInfo)ware;

                    if (extSett.InventorySync && !ScraperStarted)
                    {
                        inventoryUpdateItems.Add(new InventoryUpdateInfo(extWare));
                    }

                    if (extSett.PriceSync && !ScraperStarted)
                    {
                        priceUpdateItems.Add(new PriceUpdateInfo(extWare, extSett.ConsiderMapPrice, extSett.PercentageForMsrp, extSett.SurchargePerLb, extSett.MinSurcharge, extSett.DoPricePerPound));
                    }

                    if (ScraperStarted)
                    {
                        List<ExtWareInfo> vehicleWares = ExtWareInfo.ProcessingFitments(extWare, extSett.VehicleInfo);
                        List<ExtWareInfo> groupedWares = ExtWareInfo.GroupWares(vehicleWares);
                        extWares.AddRange(groupedWares);
                    }
                }

                if (ScraperStarted)
                {
                    MessagePrinter.PrintMessage($"Create local price file");
                    string filePath = FileHelper.CreateScrapeFile(FileHelper.GetSettingsPath("Turn14ScrapeInfo.csv"), extWares);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        MessagePrinter.PrintMessage($"Local file created - {filePath}");
                    }
                    else
                    {
                        MessagePrinter.PrintMessage($"Can't create local scrape file", ImportanceLevel.Critical);
                    }
                }
                else
                {
                    if (priceUpdateItems.Count > 0 && extSett.PriceSync)
                    {
                        MessagePrinter.PrintMessage($"Create local price file");
                        string filePath = FileHelper.CreatePriceUpdateFile(FileHelper.GetSettingsPath("Turn14PriceUpdate.csv"), priceUpdateItems);

#if TEST
                        MessagePrinter.PrintMessage($"Local price file created - {filePath}");
#else
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            MessagePrinter.PrintMessage("Upload price batch file to FTP");
                            string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, "Turn14PriceUpdate.csv", filePath, true);
                            if (!string.IsNullOrEmpty(url))
                            {
                                string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                                if (extSett.DoBatch)
                                {
                                    //int batchId = SceApiHelper.BatchUpdate(urlForBatch, Settings);
                                    //MessagePrinter.PrintMessage($"File Batched. BatchId - {batchId}");
                                }
                            }
                            
                            if (File.Exists(filePath))
                                File.Delete(filePath);
                        }
                        else
                        {
                            MessagePrinter.PrintMessage($"Can't create local price file", ImportanceLevel.Critical);
                        }
#endif      
                    }

                    if (inventoryUpdateItems.Count > 0 && extSett.InventorySync)
                    {
                        MessagePrinter.PrintMessage($"Create local inventory file");
                        string filePath = FileHelper.CreateInventoryUpdateFile(FileHelper.GetSettingsPath("Turn14InventoryUpdate.csv"), inventoryUpdateItems);

#if TEST
                        MessagePrinter.PrintMessage($"Local inventory file created - {filePath}");
#else
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            MessagePrinter.PrintMessage("Upload inventory file to FTP");
                            string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, "Turn14InventoryUpdate.csv", filePath, true);
                            if (!string.IsNullOrEmpty(url))
                            {
                                string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                                MessagePrinter.PrintMessage($"Invntory file uploaded - {urlForBatch}");
                            }
                            
                            if (File.Exists(filePath))
                                File.Delete(filePath);
                        }
                        else
                        {
                            MessagePrinter.PrintMessage($"Can't create local inventory file", ImportanceLevel.Critical);
                        }
#endif

                    }
                }

            }
        }

        #endregion

        #region Standart Props and Methods

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
                frm.LoadOrders = LoadOrders;
                frm.LoadTurn14Brands = LoadTurn14Brands;
                ProcessAutoSyncOrders = frm.AutoSyncOrders;
                frm.Sett = Settings;
                frm.SyncOrders = SyncOrders;
                frm.ImportOrders = ImportOrders;
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

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;

            switch (item.ItemType)
            {
                case ActionTypes.ScrapeBrandInfo:
                    act = ScrapeBrandInfo;
                    break;
                case ActionTypes.ScrapeItemInfo:
                    act = ScrapeItemInfo;
                    break;
                default:
                    act = null;
                    break;
            }

            return act;
        }

        #endregion

        private void CheckFtpSettings()
        {
            if (!Settings.UseFtp || string.IsNullOrEmpty(Settings.FtpAddress) || string.IsNullOrEmpty(Settings.FtpUsername) || string.IsNullOrEmpty(Settings.FtpPassword))
            {
                throw new Exception("Please setup FTP settings first");
            }
        }

        protected override void RealStartProcess()
        {

#if TEST
            Settings.MaxThreads = 1;
#endif

            CheckFtpSettings();
            Wares.Clear();
            CheckTurn14Setting();

            if (extSett.BrandsForScraping.data.Any())
            {
                ScraperStarted = true;
                ProcessScrape();
            }
            else
            {
                ScraperStarted = false;

                if (extSett.OrdersSync)
                {
                    ProcessOrderSync();
                }

                if (extSett.PriceSync || extSett.InventorySync)
                {
                    ProcessInventoryAndPriceSync();
                }
            }

            StartOrPushPropertiesThread();
        }

        #region Turn14 Info

        private BrandsJson LoadTurn14Brands()
        {
            CheckTurn14Setting();

            BrandsJson brandsJson = Turn14ApiHelper.GetBrands();
            return brandsJson;
        }

        private void CheckTurn14Setting()
        {
            if (string.IsNullOrEmpty(extSett.ClientId) || string.IsNullOrEmpty(extSett.ClientSecret))
                throw new Exception($"Please write Client Id and Client Secret on turn14 setting Tab");

            try
            {
                MessagePrinter.PrintMessage($"Checking Turn14 credentials");
                Turn14ApiHelper.SetAuthInfo(extSett.ClientId, extSett.ClientSecret);
                Turn14ApiHelper.GetToken();
                MessagePrinter.PrintMessage($"Turn14 credentials is valid");
            }
            catch (WebException exc)
            {
                if (exc.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)exc.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            throw new Exception(error);
                        }
                    }
                }
            }
        }

        #endregion

        
        #region Orders Sync

        private void ProcessOrderSync()
        {
            ProcessAutoSyncOrders();
        }

        public List<OrderSync> LoadOrders()
        {
            MessagePrinter.PrintMessage($"Loading SCE orders");
            var orders = SceApiHelper.LoadSceOrders(Settings, extSett.DateFrom, extSett.DateTo);
            MessagePrinter.PrintMessage($"[{orders.Count}] - SCE orders loaded");
            return orders;
        }

        private void SyncOrders(DateTime dateFrom, DateTime dateTo)
        {
            CheckTurn14Setting();
            CheckFtpSettings();

            MessagePrinter.PrintMessage($"Update track numbers...");
            int counter = 0;
            if (!UseProduction)
            {
                //get Shipments
                ShippingOptionsJson shippingOptions = Turn14ApiHelper.GetShippingOptions(UseProduction);

                SandBoxHelper sandBoxHelper = new SandBoxHelper();
                TrackingNumbersJson trackingNumbersJson = sandBoxHelper.GetTrackingNumbersJson();
                //Get Processed Orders
                string localFileName = FileHelper.GetSettingsPath(TempFileNameFTP);
                if (File.Exists(localFileName))
                {
                    localFileName = FileHelper.GetSettingsPath(TempFileNameFTP);
                    List<ProcessedOrder> processedOrders;
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string json = File.ReadAllText(localFileName);
                    processedOrders = jss.Deserialize<List<ProcessedOrder>>(json);

                    //for each track number need to update correct SCE order with this track number
                    foreach (dataTrackingNumbers trackingNumber in trackingNumbersJson.data)
                    {
                        foreach (referencesTrackingNumbers referencesTrackingNumber in trackingNumber.attributes.references)
                        {
                            if (referencesTrackingNumber.type != "Invoice")
                                continue;

                            foreach (ProcessedOrder processedOrder in processedOrders)
                            {
                                if (!string.IsNullOrEmpty(processedOrder.TrackNumber))
                                    continue;

                                if (processedOrder.data.attributes.po_number == referencesTrackingNumber.purchase_order_number)
                                {
                                    processedOrder.TrackNumberDate = DateTime.Now.AddDays(40); //add processed day
                                    processedOrder.TrackNumber = trackingNumber.attributes.tracking_number; //add track number

                                    //UpdateTrackInSCE
                                    //SceApiHelper.UpdateShipmentTrackingNumber(Settings, processedOrder);
                                }
                            }
                        }
                    }

                    RemoveOldProcessedOrders(processedOrders); //remove orders processed more than 30 days ago
                    UpdateProcessedOrders(processedOrders); //update FTP file new orders
                }
            }
            else
            {
                TrackingNumbersJson trackingNumbersJson = Turn14ApiHelper.GetTrackingNumbers(dateFrom, dateTo, UseProduction); //get tracking numbers
                if (trackingNumbersJson.data.Any())
                {
                    List<ProcessedOrder> processedOrders = GetProcessedOrders(); // get processed orders from FTP
                    //for each track number need to update correct SCE order with this track number
                    foreach (dataTrackingNumbers trackingNumber in trackingNumbersJson.data)
                    {
                        foreach (referencesTrackingNumbers referencesTrackingNumber in trackingNumber.attributes.references)
                        {
                            if (referencesTrackingNumber.type != "Invoice")
                                continue;

                            foreach (ProcessedOrder processedOrder in processedOrders)
                            {
                                if (!string.IsNullOrEmpty(processedOrder.TrackNumber))
                                    continue;

                                if (processedOrder.data.attributes.po_number == referencesTrackingNumber.purchase_order_number)
                                {
                                    processedOrder.TrackNumberDate = DateTime.Now; //add processed day
                                    processedOrder.TrackNumber = trackingNumber.attributes.tracking_number; //add track number

                                    //UpdateTrackInSCE
                                    string error = SceApiHelper.UpdateShipmentTrackingNumber(Settings, processedOrder);
                                    if (string.IsNullOrEmpty(error))
                                    {
                                        MessagePrinter.PrintMessage($"Track number updated for order - [{processedOrder.data.attributes.po_number}]. Track number - [{processedOrder.TrackNumber}]", ImportanceLevel.High);
                                    }
                                    else
                                    {
                                        MessagePrinter.PrintMessage($"Track number not updated for order - [{processedOrder.data.attributes.po_number}]. Error - [{error}]", ImportanceLevel.High);
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    RemoveOldProcessedOrders(processedOrders); //remove orders processed more than 30 days ago
                    UpdateProcessedOrders(processedOrders); //update FTP file new orders
                }
            }

            MessagePrinter.PrintMessage($"[{counter}] - track numbers updated");
        }

        private void ImportOrders(DateTime dateFrom, DateTime dateTo, List<OrderSync> ordersToImport)
        {
            CheckFtpSettings(); //if FTP setting don't setup give Exception

            MessagePrinter.PrintMessage($"Import orders to Trun14...");
            int counter = 0; //counter for imported orders
            List<ProcessedOrder> processedOrders = new List<ProcessedOrder>();

            if (!UseProduction)
            {
                SandBoxHelper sandBoxHelper = new SandBoxHelper(); //sandbox object

                //get Shipments
                ShippingOptionsJson shippingOptions = Turn14ApiHelper.GetShippingOptions(UseProduction);
                var turn14OrderQuote = sandBoxHelper.GetTurn14OrderQuote(); //prepare sandbox quote

                OrderQuoteJson quoteRes = Turn14ApiHelper.RequestOrderQuote(turn14OrderQuote, UseProduction, out string errorQuote); //send sandbox OrderQuote request

                var turn14Order = sandBoxHelper.GeTurn14Order(); //prepare sandbox Order
                Turn14OrderJson turn14OrderJson = Turn14ApiHelper.ImportOrder(turn14Order, UseProduction, out string errorOrder); //send sandbox ImportOrder request

                processedOrders.Add(new ProcessedOrder(turn14OrderJson, DateTime.Now));
                counter++;
            }
            else
            {
                //UseProduction = false; //FOR TEST
                //get Shipments
                ShippingOptionsJson shippingOptions = Turn14ApiHelper.GetShippingOptions(UseProduction);

                foreach (OrderSync orderSync in ordersToImport)
                {
                    //orderSync.SceOrder.ShippingAddress.Phone = $"267-468-0350"; //FOR TEST
                    var turn14OrderQuote = new Turn14OrderQuote(orderSync, UseProduction); //convert SceOrder to turn14 Quote for receive available shipping location

                    OrderQuoteJson quoteRes = Turn14ApiHelper.RequestOrderQuote(turn14OrderQuote, UseProduction, out string errorQuote); //send Quote for turn14
                    if (!string.IsNullOrEmpty(errorQuote))
                    {
                        MessagePrinter.PrintMessage($"[{errorQuote}] when processing order - [{orderSync.OrderId}]-", ImportanceLevel.High);
                        continue;
                    }

                    var turn14Order = new Turn14Order(orderSync, quoteRes, shippingOptions, UseProduction); //convert Sce Order to Turn14 order

                    Turn14OrderJson turn14OrderJson = Turn14ApiHelper.ImportOrder(turn14Order, UseProduction, out string errorOrder); // Send Order to Turn14
                    if (!string.IsNullOrEmpty(errorOrder))
                    {
                        MessagePrinter.PrintMessage($"[{errorOrder}] when processing order - [{orderSync.OrderId}]-", ImportanceLevel.High);
                        continue;
                    }

                    //if order imported - remove from grid and mark it in SCE
                    orderSync.Imported = true;
                    processedOrders.Add(new ProcessedOrder(turn14OrderJson, DateTime.Now, orderSync));
                    counter++;

                    if (UseProduction)
                        SceApiHelper.MarkOrderSynced(Settings, orderSync); // mark order in SCE

                    MessagePrinter.PrintMessage($"Order [{orderSync.OrderId}] imported");
                }
            }

            if (processedOrders.Count > 0)
            {
                //get existing orders
                List<ProcessedOrder> ftpProcessedOrders = GetProcessedOrders();

                //add new orders to existing
                ftpProcessedOrders.AddRange(processedOrders);

                //updateFtpFile with new orders
                UpdateProcessedOrders(ftpProcessedOrders);
            }

            MessagePrinter.PrintMessage($"[{counter}] - orders imported");
        }

        private List<ProcessedOrder> GetProcessedOrders()
        {
            string localFileName = FileHelper.GetSettingsPath(TempFileNameFTP);
            List<ProcessedOrder> processedOrders;

            MessagePrinter.PrintMessage($"Downloading processed orders from FTP. Please wait...");
            string ftpFile = RequestHelper.DownloadFTPFile(localFileName, "ftp://" + Settings.FtpAddress, TempFileNameFTP, Settings.FtpUsername, Settings.FtpPassword);
            if (!ftpFile.StartsWith("Error"))
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string json = File.ReadAllText(localFileName);
                processedOrders = jss.Deserialize<List<ProcessedOrder>>(json);
                MessagePrinter.PrintMessage($"{TempFileNameFTP} downloaded from FTP!");
            }
            else
            {
                if (!ftpFile.Contains($"error: (550) File unavailable"))
                    MessagePrinter.PrintMessage($"Processed orders not downloaded from FTP! {ftpFile}", ImportanceLevel.Critical);

                processedOrders = new List<ProcessedOrder>();
            }

            return processedOrders;
        }

        private void UpdateProcessedOrders(List<ProcessedOrder> processedOrders)
        {
            string localFileName = FileHelper.GetSettingsPath(TempFileNameFTP);
            bool succes = false;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string data = jss.Serialize(processedOrders);
            File.WriteAllText(localFileName, data);

            if (UseProduction)
            {
                string ftpFile = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, TempFileNameFTP, localFileName);
                if (!string.IsNullOrEmpty(ftpFile))
                {
                    MessagePrinter.PrintMessage($"Processed Orders updated");
                    succes = true;
                }
                else
                {
                    MessagePrinter.PrintMessage($"Processed orders not updated on FTP please do it manually. File - [{localFileName}] - upload to - [ftp://{Settings.FtpAddress}]", ImportanceLevel.Critical);
                }
            }
            else
            {
                MessagePrinter.PrintMessage($"Processed Orders updated, local File Created - {localFileName}");
                succes = true;
            }

            if (succes && UseProduction)
                File.Delete(localFileName);
        }

        private void RemoveOldProcessedOrders(List<ProcessedOrder> processedOrders)
        {
            List<ProcessedOrder> oldOrders = new List<ProcessedOrder>();

            foreach (ProcessedOrder processedOrder in processedOrders)
            {
                if (processedOrder.TrackNumberDate.AddDays(30) < DateTime.Now)
                {
                    oldOrders.Add(processedOrder);
                }
            }

            foreach (ProcessedOrder oldOrder in oldOrders)
            {
                processedOrders.Remove(oldOrder);
            }
        }

        #endregion

        #region Scraper Settings

        private void ProcessInventoryAndPriceSync()
        {
            if (string.IsNullOrEmpty(extSett.BrandFilePath))
            {
                MessagePrinter.PrintMessage("You must choice file with brands", ImportanceLevel.Critical);
                return;
            }

            extSett.BrandsAlignments = new List<BrandsAlignment>();
            extSett.BrandsAlignments = FileHelper.ReadBrandsAlignments(extSett.BrandFilePath);

            MessagePrinter.PrintMessage($"Downloading SCE export.");

            List<string> exportFiles = new List<string>();
#if TEST
            if (UseLocalExport)
                exportFiles.Add(ExportLocalFilePath);
            else
                exportFiles = SceApiHelper.LoadProductsExport(Settings);
#else
            exportFiles = SceApiHelper.LoadProductsExport(Settings);
#endif
            MessagePrinter.PrintMessage($"SCE export downloaded");

            List<SceExportItem> exportItems = new List<SceExportItem>();

            foreach (string sceExportFilePath in exportFiles)
            {
                exportItems.AddRange(FileHelper.ReadSceExport(sceExportFilePath));
            }

            exportItems = exportItems.Distinct(new SceExportItem()).ToList();

            extSett.TransferInfoItems = new List<TransferInfoItem>();
            foreach (SceExportItem sceItem in exportItems)
            {
                foreach (BrandsAlignment brandsAlignment in extSett.BrandsAlignments)
                {
                    if (sceItem.Brand == brandsAlignment.BrandInSce)
                    {
                        extSett.TransferInfoItems.Add(new TransferInfoItem()
                        {
                            ProdIdSce = sceItem.ProdId,
                            ProdTypeSce = sceItem.ProductType,
                            BrandSce = sceItem.Brand,
                            PartNumberSce = sceItem.PartNumber,
                            ManufacturerPartNumberSce = sceItem.ManufacturerPartNumber,
                            BrandTurn14 = brandsAlignment.BrandInTurn14,
                            WebPrice = sceItem.WebPrice,
                            CostPrice = sceItem.CostPrice,
                            Jobber = sceItem.Jobber,
                            Msrp = sceItem.MSRP
                        });
                    }
                }
            }

#if !TEST
            foreach (string sceExportFilePath in exportFiles)
            {
                if (File.Exists(sceExportFilePath))
                    File.Delete(sceExportFilePath);
            }
#endif
            List<string> unicBrands = new List<string>();
            foreach (var transferInfoItem in extSett.TransferInfoItems)
            {
                if (string.IsNullOrEmpty(transferInfoItem.BrandTurn14))
                    continue;

                if (!unicBrands.Contains(transferInfoItem.BrandTurn14))
                    unicBrands.Add(transferInfoItem.BrandTurn14);
            }

            MessagePrinter.PrintMessage($"[{unicBrands.Count}] - unic brands found in SCE");

            BrandsJson turn14Brands = Turn14ApiHelper.GetBrands();
            int matchedBrands = 0;
            foreach (string brand in unicBrands)
            {
                //#if TEST
                //                        if (brand != BrandForCheck)
                //                            continue;
                //#endif

                foreach (var turn14Brand in turn14Brands.data)
                {
                    if (brand == turn14Brand.attributes.name)
                    {
                        matchedBrands++;


                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                Item = turn14Brand,
                                ItemType = ActionTypes.ScrapeBrandInfo
                            });
                        }

                        break;
                    }
                }
            }

            MessagePrinter.PrintMessage($"[{matchedBrands}] - brands matched with Turn14");
        }

        private void ProcessScrape()
        {
            if (string.IsNullOrEmpty(extSett.VehicleInfoForFitments))
            {
                MessagePrinter.PrintMessage("You must choice file with fitments", ImportanceLevel.Critical);
                return;
            }

            extSett.VehicleInfo = new List<VehicleInfoForFitments>();
            extSett.VehicleInfo = FileHelper.ReadVehicleInfoForFitments(extSett.VehicleInfoForFitments);
            extSett.VehicleInfo = extSett.VehicleInfo.Distinct(VehicleInfoForFitments.VehicleInfoForFitmentsComparer).ToList();
            foreach (var dataBrand in extSett.BrandsForScraping.data)
            {

                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem
                    {
                        Item = dataBrand,
                        ItemType = ActionTypes.ScrapeBrandInfo
                    });
                }
#if TEST
                break;
#endif
            }
        }

        private void ScrapeBrandInfo(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            dataBrands brandData = (dataBrands)pqi.Item;
            int page = 1;
            MessagePrinter.PrintMessage($"Receiving info for brand - [{brandData.attributes.name}]");

            try
            {
                bool lastPageFound = false;
                do
                {
                    var itemInfo = Turn14ApiHelper.GetItemsByBrand(brandData, page);

                    int.TryParse(itemInfo.meta.total_pages, out int lastPage);
                    if (page == lastPage)
                    {
                        lastPageFound = true;
                    }
                    else
                    {
                        page++;
                    }

                    foreach (dataItems dataItem in itemInfo.data)
                    {
                        if (!PartFoundInSCE(dataItem, out TransferInfoItem transferInfoItem))
                            continue;

                        //#if TEST
                        //                        if(dataItem.attributes.part_number != PartNumberForCheck)
                        //                            continue;
                        //#endif

                        lock (this)
                        {
                            lstProcessQueue.Add(new PqiExtended
                            {
                                Item = dataItem,
                                ItemType = ActionTypes.ScrapeItemInfo,
                                TransferInfoItem = transferInfoItem
                            });
                        }

#if TEST
                        break;
#endif
                    }

                } while (!lastPageFound);
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this Brand - [{brandData.attributes.name}] and this page - [{page}]", ImportanceLevel.High);
            }

            MessagePrinter.PrintMessage($"[{brandData.attributes.name}] - Brand info Received");
            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        private bool PartFoundInSCE(dataItems dataItem, out TransferInfoItem transferItem)
        {
            transferItem = new TransferInfoItem();

            if (ScraperStarted)
                return true;
            
            if (extSett.InventorySync || extSett.PriceSync)
            {
                var attributeInfo = dataItem.attributes;

                string brand = attributeInfo.brand;
                string partNumber = attributeInfo.part_number;
                string manufacturerPartNumber = attributeInfo.mfr_part_number;

                int productMatched = 0;
                foreach (TransferInfoItem transferInfoItem in extSett.TransferInfoItems)
                {
                    if (transferInfoItem.BrandTurn14 == brand && transferInfoItem.ManufacturerPartNumberSce.ToLower() == manufacturerPartNumber.ToLower())
                    {
                        productMatched++;

                        transferItem = transferInfoItem;
                    }
                }

                if (productMatched > 1)
                {
                    MessagePrinter.PrintMessage($"Product [{brand}] [{partNumber}] has several prodId in SCE", ImportanceLevel.High);
                    return false;
                }
                else if (productMatched == 1)
                {
                    return true;
                }
            }

            return false;
        }

        private void ScrapeItemInfo(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            var pqiInfo = (PqiExtended)pqi;

            dataItems dataItem = (dataItems)pqiInfo.Item;
            TransferInfoItem transferInfoItem = (TransferInfoItem)pqiInfo.TransferInfoItem;

            try
            {
                SingleItemDataJson itemDataInfo = new SingleItemDataJson();
                SingleItemPricingJson pricingInfo = new SingleItemPricingJson();
                InventorySingleItemJson itemInventory = new InventorySingleItemJson();

                if (extSett.InventorySync || ScraperStarted)
                {
                    itemInventory = Turn14ApiHelper.GetInventorySingleItem(dataItem);
                }

                if (extSett.PriceSync || ScraperStarted)
                {
                    pricingInfo = Turn14ApiHelper.GetSingleItemPricing(dataItem);
                }

                if (ScraperStarted)
                    itemDataInfo = Turn14ApiHelper.GetSingleItemData(dataItem);

                FullItemInfoJson fullItem = new FullItemInfoJson
                {
                    DataItemJson = dataItem,
                    SingleItemPricingJson = pricingInfo,
                    InventorySingleItemJson = itemInventory,
                    SingleItemDataJson = itemDataInfo
                };

                PopulateItemInfo(fullItem, transferInfoItem);

            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this brand - [{dataItem.attributes.brand}] and this ItemInfo - [{dataItem.attributes.part_number}]", ImportanceLevel.High);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        private void PopulateItemInfo(FullItemInfoJson fullItem, TransferInfoItem transferInfoItem)
        {
            var attributeInfo = fullItem.DataItemJson.attributes;

            try
            {
                string brand = attributeInfo.brand;
                string partNumber = attributeInfo.part_number;
                string manufacturerPartNumber = attributeInfo.mfr_part_number;

                string title = attributeInfo.product_name;
                string upc = attributeInfo.barcode;
                string mainCategory = attributeInfo.category;
                string subCategory = attributeInfo.subcategory;
                string description = attributeInfo.part_description;
                string length = string.Empty;
                string width = string.Empty;
                string height = string.Empty;
                string weight = string.Empty;

                if (attributeInfo.dimensions != null)
                {
                    if (attributeInfo.dimensions.Any())
                    {
                        if (attributeInfo.dimensions.Count > 1)
                        {

                        }

                        length = attributeInfo.dimensions.First().length;
                        width = attributeInfo.dimensions.First().width;
                        height = attributeInfo.dimensions.First().height;
                        weight = attributeInfo.dimensions.First().weight;
                    }
                }

                double msrp = 0;
                double costPrice = 0;
                double webPrice = 0;
                double jobber = 0;

                var pricingInfo = fullItem.SingleItemPricingJson.data?.attributes;
                if (pricingInfo != null)
                {
                    costPrice = pricingInfo.purchase_cost;

                    if (pricingInfo.pricelists.Any())
                    {
                        if (pricingInfo.pricelists.Count > 1)
                        {

                        }

                        foreach (var pricelist in pricingInfo.pricelists)
                        {
                            switch (pricelist.name.ToLower())
                            {
                                case "jobber":
                                    jobber = pricelist.price;
                                    break;
                                case "map":
                                    webPrice = pricelist.price;
                                    break;
                                case "retail":
                                    msrp = pricelist.price;
                                    break;
                            }
                        }
                    }
                }

                string generalImage = string.Empty;
                string attachments = string.Empty;
                string fitments = string.Empty;

                var filesInfo = fullItem.SingleItemDataJson.data;
                if (filesInfo != null)
                {
                    if (filesInfo.Any())
                    {
                        if (filesInfo.Count > 1)
                        {

                        }

                        if (filesInfo.FirstOrDefault()?.files != null)
                        {
                            foreach (var itemData in filesInfo.FirstOrDefault()?.files)
                            {
                                switch (itemData.file_extension.ToLower())
                                {
                                    case "jpg":
                                        generalImage += itemData.links.First(i => i.width == itemData.links.Max(m => m.width)).url + ",";
                                        break;
                                    case "png":
                                        generalImage += itemData.links.First(i => i.width == itemData.links.Max(m => m.width)).url + ",";
                                        break;
                                    case "pdf":
                                        attachments += itemData.links.First(i => i.width == itemData.links.Max(m => m.width)).url + ",";
                                        break;
                                }
                            }
                        }

                        generalImage = generalImage.Trim(',');

                        List<string> fitmentsList = new List<string>();
                        if (filesInfo.FirstOrDefault()?.vehicle_fitments != null)
                        {
                            foreach (var itemFitments in filesInfo.FirstOrDefault()?.vehicle_fitments)
                            {
                                if (!fitmentsList.Contains(itemFitments.vehicle_id))
                                    fitmentsList.Add(itemFitments.vehicle_id);
                            }

                            foreach (string fitment in fitmentsList)
                            {
                                fitments += fitment + ",";
                            }

                            fitments = fitments.Trim(',');
                        }

                        //LINQ
                        //string fitment1 = string.Empty;
                        //fitment1 = filesInfo.First().vehicle_fitments.Aggregate(fitment1, (current, itemFitments) => current + (itemFitments.vehicle_id + ","));
                        //fitment1 = fitment1.Trim(',');

                    }
                }

                int stock = 0;
                int manufacturerStock = 0;

                var inventoryInfo = fullItem.InventorySingleItemJson.data;
                if (inventoryInfo != null)
                {
                    if (inventoryInfo.Any())
                    {
                        if (inventoryInfo.Count > 1)
                        {

                        }

                        var inventoryAttributes = inventoryInfo.FirstOrDefault()?.attributes;
                        if (inventoryAttributes != null)
                        {
                            int.TryParse(inventoryAttributes.manufacturer?.stock, out int mfrStock);
                            manufacturerStock = mfrStock;

                            if (inventoryAttributes.inventory != null)
                                foreach (KeyValuePair<string, string> inventoryValuePair in inventoryAttributes.inventory)
                                {
                                    if (attributeInfo.warehouse_availability == null)
                                        continue;

                                    foreach (var warehouseAvailability in attributeInfo.warehouse_availability)
                                    {
                                        if (warehouseAvailability.location_id == inventoryValuePair.Key)
                                        {
                                            int.TryParse(inventoryValuePair.Value, out int currentWarehouseStock);
                                            stock += currentWarehouseStock;
                                            break;
                                        }
                                    }

                                    //LINQ
                                    //if (attributeInfo.warehouse_availability.Any(warehouseAvailability => warehouseAvailability.location_id == inventoryValuePair.Key))
                                    //{
                                    //    int.TryParse(inventoryValuePair.Value, out int currentWarehouseStock);
                                    //    stock += currentWarehouseStock;
                                    //}
                                }
                        }
                    }
                }

                ExtWareInfo wi = new ExtWareInfo(title, description, partNumber, manufacturerPartNumber, brand, mainCategory, subCategory
                    , upc, length, height, width, weight, stock, manufacturerStock, msrp, jobber, webPrice, costPrice, generalImage
                    , attachments, fitments, transferInfoItem.ProdIdSce, transferInfoItem.ProdTypeSce, transferInfoItem.PartNumberSce);

                AddWareInfo(wi);
                OnItemLoaded(wi);

                MessagePrinter.PrintMessage($"[{attributeInfo.part_number}] info received");
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this brand - [{attributeInfo.brand}] and PN - [{attributeInfo.part_number}]", ImportanceLevel.High);
            }
        }

        #endregion

    }
}
