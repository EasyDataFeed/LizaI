using System;
using System.Collections.Generic;
using System.Linq;
using ScraperApiTurn14;
using Databox.Libs.ScraperApiTurn14;
using System.IO;
using ScraperApiTurn14.DataItems.Turn14;
using System.Net;
using ScraperApiTurn14.DataItems;
using ScraperApiTurn14.Helpers;

namespace WheelsScraper
{
    public class ScraperApiTurn14 : BaseScraper
    {
        #region Constans

        private AllLocationsJson AllLocations { get; set; }
        private bool ScraperStarted { get; set; }
        private const string TempFileNameFTP = "Turn14ConnectorAddedOrders.json";
        private const string PartNumberForCheck = "PartForCheck";
        private const string BrandForCheck = "BrandForCheck";
        private const bool UseLocalExport = true;

        #endregion

        public ScraperApiTurn14()
        {
            Name = "ScraperApiTurn14";
            Url = "https://www.ScraperApiTurn14.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();

            SpecialSettings = new ExtSettings();
            AllLocations = new AllLocationsJson();

            Complete += ScraperApiTurn14_Complete;
        }

        private void ScraperApiTurn14_Complete(object sender, EventArgs e)
        {
            if (Wares.Any())
            {
                List<ExtWareInfo> extWares = new List<ExtWareInfo>();

                int groupedWaresLeft = Wares.Count;
                if (ScraperStarted)
                    MessagePrinter.PrintMessage($"Processing vehicles fitments. SKU Left - [{groupedWaresLeft}]");


                foreach (WareInfo ware in Wares)
                {
                    ExtWareInfo extWare = (ExtWareInfo)ware;

                    if (ScraperStarted)
                    {
                        List<ExtWareInfo> vehicleWares = ExtWareInfo.ProcessingFitments(extWare, extSett.VehicleInfo);
                        List<ExtWareInfo> groupedWares = ExtWareInfo.GroupWares(vehicleWares);
                        extWares.AddRange(groupedWares);

                        groupedWaresLeft--;
                        if (groupedWaresLeft % 1000 == 0)
                            MessagePrinter.PrintMessage($"SKU Left - [{groupedWaresLeft}]");
                    }
                }

                if (ScraperStarted)
                {
                    MessagePrinter.PrintMessage($"Create local file");
                    string filePath = FileHelper.CreateScrapeFile(FileHelper.GetSettingsPath("Turn14ScrapeInfo.csv"), extWares, out string errorMsg);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        MessagePrinter.PrintMessage($"Local file created - {filePath}");
                    }
                    else
                    {
                        MessagePrinter.PrintMessage($"Can't create local scrape file. [{errorMsg}].", ImportanceLevel.Critical);
                    }
                }
            }
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
                frm.LoadTurn14Brands = LoadTurn14Brands;
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

        private void CheckFtpSettings()
        {
            if (!Settings.UseFtp || string.IsNullOrEmpty(Settings.FtpAddress) || string.IsNullOrEmpty(Settings.FtpUsername) || string.IsNullOrEmpty(Settings.FtpPassword))
            {
                throw new Exception("Please setup FTP settings first");
            }
        }

        protected override void RealStartProcess()
        {
            Wares.Clear();
            CheckTurn14Setting();

            if (extSett.BrandsForScraping.data.Any())
            {
                ScraperStarted = true;
                ProcessScrape();
            }

            StartOrPushPropertiesThread();
        }

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

                        lock (this)
                        {
                            lstProcessQueue.Add(new PqiExtended
                            {
                                Item = dataItem,
                                ItemType = ActionTypes.ScrapeItemInfo,
                                TransferInfoItem = transferInfoItem
                            });
                        }
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

                itemInventory = Turn14ApiHelper.GetInventorySingleItem(dataItem);
                pricingInfo = Turn14ApiHelper.GetSingleItemPricing(dataItem);
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
                    }
                }

                if (ScraperStarted)
                    lock (AllLocations)
                        if (!AllLocations.data.Any())
                        {
                            AllLocations = Turn14ApiHelper.GetAllLocations();
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
    }
}
