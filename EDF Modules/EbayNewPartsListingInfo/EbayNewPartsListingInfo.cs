#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using Scraper.Shared;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using HtmlAgilityPack;
using EbayNewPartsListingInfo;
using Databox.Libs.EbayNewPartsListingInfo;
using EbayNewPartsListingInfo.DataItems;
using EbayNewPartsListingInfo.Enums;
using EbayNewPartsListingInfo.Extensions;
using EbayNewPartsListingInfo.Helpers;

#endregion

namespace WheelsScraper
{
    public class EbayNewPartsListingInfo : BaseScraper
    {

        #region constants

        //private const string EbayApiAppId = "AdamParr-Comparis-PRD-35d705b3d-641a6ced";
        //private const string EbayApiAppId = "Shopping-Shopping-PRD-debee2917-03c4335b";

        private Dictionary<string, bool> EbayApiIdKeys = new Dictionary<string, bool>()
        {
            {"Shopping-Shopping-PRD-debee2917-03c4335b", false },
            {"DmitriiD-DmitriiD-PRD-7ea9be394-792b53d4", false },
            {"VasylPic-VasylP-PRD-df2fb35bc-0a6844c0", false },
            {"acevudn-acevudn-PRD-4ea94fa4c-3b6c3594", false },
        };

        private const int DescriptionLength = 32767;
        private readonly string _specifications = $"Specification##";

        #endregion

        private int counter1million = 0;
        private int fileCounter = 0;
        public static object _lockObject = new object();

        public EbayNewPartsListingInfo()
        {
            Name = "EbayNewPartsListingInfo";
            Url = "https://www.EbayNewPartsListingInfo.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();

            Complete += EbayNewPartsListingInfo_Complete;

            SpecialSettings = new ExtSettings();
        }

        private void EbayNewPartsListingInfo_Complete(object sender, EventArgs e)
        {
            if (extSett.DataForFile.Count > 0)
            {
                string filePath = FileHelper.CreateFile(fileCounter, extSett.DataForFile);
                MessagePrinter.PrintMessage($"File Created - {filePath}");
            }
            else if (Wares.Count > 0 && !extSett.VehicleCompatibility)
            {
                List<ExtWareInfo> wareInfos = new List<ExtWareInfo>();

                foreach (WareInfo wareInfo in Wares)
                    wareInfos.Add((ExtWareInfo)wareInfo);

                string filePath = FileHelper.CreateFile(fileCounter, wareInfos);
                MessagePrinter.PrintMessage($"File Created - {filePath}");
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
            extSett.DataForFile = new List<ExtWareInfo>();

            lock (this)
                lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = (int)ItemType.ProcessingFile });

            StartOrPushPropertiesThread();
        }

        protected void ProcessFile(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                List<EbayCsvItem> ebayItems = new List<EbayCsvItem>();
                ebayItems = CsvManager.ReadFileFilter(extSett.EbayItemsFilePath);

                ebayItems = ebayItems.Distinct(new EbayCsvItem.EbayItemNumberEqualityComparer()).ToList();

                int counter = 0;
                foreach (EbayCsvItem ebayItem in ebayItems)
                {
                    //if (counter < 100)
                    //{
                    counter++;
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = (int)ItemType.ProcessingEbayRequest,
                            URL = ebayItem.EbayItemNumber,
                        });
                    }
                    //}
                    //else
                    //{
                    //    break;
                    //}

                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message}", ImportanceLevel.Critical);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("File processed");
            StartOrPushPropertiesThread();
        }

        private void ProcessEbayRequest(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                string postUrl = string.Empty;
                //pqi.URL = "153031854409";

                bool limitFlag = true;

                while (limitFlag)
                {
                    string apiKey = string.Empty;
                    lock (_lockObject)
                    {
                        foreach (KeyValuePair<string, bool> keyValuePair in EbayApiIdKeys)
                        {
                            if (!keyValuePair.Value)
                            {
                                apiKey = keyValuePair.Key;
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(apiKey))
                        {
                            MessagePrinter.PrintMessage($"All Keys Limited");
                            cancel = true;
                            return;
                        }
                    }

                    if (extSett.FullDescription)
                        postUrl =
                           $"http://open.api.ebay.com/shopping?callname=GetSingleItem&responseencoding=XML&appid={apiKey}&siteid=0&version=967&ItemID={pqi.URL}&IncludeSelector=Description,ItemSpecifics,Details,ShippingCosts,Compatibility";
                    else
                        postUrl =
                            $"http://open.api.ebay.com/shopping?callname=GetSingleItem&responseencoding=XML&appid={apiKey}&siteid=0&version=967&ItemID={pqi.URL}&IncludeSelector=TextDescription,ItemSpecifics,Details,ShippingCosts,Compatibility";

                    GetSingleItemResponse responseItem = PostHelper.GetPage(postUrl, out string exceptionStr);
                    if (responseItem != null && responseItem.Ack != "Failure")
                    {
                        if (responseItem.Item == null)
                        {
                            MessagePrinter.PrintMessage($"Item not found on ebay - [{pqi.URL}]", ImportanceLevel.High);
                        }
                        else if (responseItem.Item.ItemCompatibilityList != null)
                        {
                            counter1million += responseItem.Item.ItemCompatibilityCount;
                            if (counter1million > 1000000)
                            {
                                lock (_lockObject)
                                {
                                    List<ExtWareInfo> itemsForFile = new List<ExtWareInfo>();
                                    itemsForFile.AddRange(extSett.DataForFile);

                                    fileCounter++;
                                    extSett.DataForFile = new List<ExtWareInfo>();
                                    counter1million = responseItem.Item.ItemCompatibilityCount;

                                    string filePath = FileHelper.CreateFile(fileCounter, itemsForFile);
                                    MessagePrinter.PrintMessage($"File Created - {filePath}");
                                }
                            }

                            if (counter1million < 1000000)
                            {
                                foreach (var itemCompatibility in responseItem.Item.ItemCompatibilityList)
                                {
                                    ExtWareInfo wi = new ExtWareInfo();

                                    var compabilityNotesCollection = itemCompatibility.CompatibilityNotes as XmlNode[];
                                    if (compabilityNotesCollection != null)
                                        foreach (var compatibilityNote in compabilityNotesCollection)
                                        {
                                            wi.Notes = compatibilityNote.InnerText;
                                        }

                                    foreach (var itemCompatibilityValue in itemCompatibility.NameValueList)
                                    {
                                        if (itemCompatibilityValue.Name == "Notes")
                                        {
                                            wi.Notes = itemCompatibilityValue.Value;
                                        }
                                        else if (itemCompatibilityValue.Name == "Year")
                                        {
                                            wi.Year = itemCompatibilityValue.Value;
                                        }
                                        else if (itemCompatibilityValue.Name == "Make")
                                        {
                                            wi.Make = itemCompatibilityValue.Value;
                                        }
                                        else if (itemCompatibilityValue.Name == "Model")
                                        {
                                            wi.Model = itemCompatibilityValue.Value;
                                        }
                                        else if (itemCompatibilityValue.Name == "Trim")
                                        {
                                            wi.Trim = itemCompatibilityValue.Value;
                                        }
                                        else if (itemCompatibilityValue.Name == "Engine")
                                        {
                                            wi.Engine = itemCompatibilityValue.Value;
                                        }
                                    }

                                    string specifications = string.Empty;
                                    if (responseItem.Item.ItemSpecifics != null)
                                    {
                                        specifications += $"{_specifications}";
                                        foreach (var itemSpecific in responseItem.Item.ItemSpecifics)
                                        {
                                            if (itemSpecific.Name == "Brand")
                                            {
                                                wi.Brand = itemSpecific.Value;
                                            }
                                            else if (itemSpecific.Name == "UPC")
                                            {
                                                wi.UPC = itemSpecific.Value;
                                            }
                                            else if (itemSpecific.Name == "Manufacturer Part Number")
                                            {
                                                wi.ManufacturerNumber = itemSpecific.Value;
                                            }
                                            else if (itemSpecific.Name == "Interchange Part Number")
                                            {
                                                wi.InterchangePartNumber = itemSpecific.Value;
                                            }
                                            else if (itemSpecific.Name == "Other Part Number")
                                            {
                                                wi.OtherPartNumber = itemSpecific.Value;
                                            }
                                            else
                                            {
                                                specifications += $"{itemSpecific.Name}~{itemSpecific.Value}^";
                                            }

                                        }
                                        if (!string.IsNullOrEmpty(specifications))
                                        {
                                            wi.Specifications = specifications.Trim('^');
                                        }
                                    }

                                    wi.ProductTitle = responseItem.Item.Title;
                                    wi.Timestamp = responseItem.Timestamp.ToString();
                                    wi.Description = responseItem.Item.Description?.Truncate(DescriptionLength);

                                    //if(!wi.Description.Contains("https://newparts.com/files/eBay/shippingImg.png"))
                                    //    continue;

                                    wi.ItemNumber = responseItem.Item.ItemID.ToString();
                                    wi.Quantity = responseItem.Item.Quantity;
                                    wi.UserId = responseItem.Item.Seller?.UserID;
                                    wi.ConvertedCurrentPrice = Convert.ToDouble(responseItem.Item.ConvertedCurrentPrice.Value);
                                    if (responseItem.Item.DiscountPriceInfo != null)
                                        wi.OriginalRetailPrice = responseItem.Item.DiscountPriceInfo.OriginalRetailPrice.Value.ToString();
                                    wi.ListingStatus = responseItem.Item.ListingStatus;
                                    wi.QuantitySold = responseItem.Item.QuantitySold.ToString();
                                    if (responseItem.Item.ShippingCostSummary.ShippingServiceCost != null)
                                        wi.ShippingServiceCost = responseItem.Item.ShippingCostSummary.ShippingServiceCost.Value.ToString();
                                    wi.ShippingType = responseItem.Item.ShippingCostSummary.ShippingType;
                                    if (responseItem.Item.ShippingCostSummary.ListedShippingServiceCost != null)
                                        wi.ListedShippingServiceCost = responseItem.Item.ShippingCostSummary.ListedShippingServiceCost.Value.ToString();
                                    wi.HitCount = responseItem.Item.HitCount.ToString();
                                    wi.StoreName = responseItem.Item.Storefront?.StoreName;
                                    wi.ItemCompatibilityCount = responseItem.Item?.ItemCompatibilityCount.ToString();
                                    wi.PartNumber = responseItem.Item.SKU;
                                    wi.Categories = responseItem.Item.PrimaryCategoryName;

                                    if (responseItem.Item.PictureURL != null)
                                        foreach (string pictireUrl in responseItem.Item.PictureURL)
                                            wi.ImageUrl += pictireUrl.RemoveParametrs() + ",";
                                    if (!string.IsNullOrEmpty(wi.ImageUrl))
                                        wi.ImageUrl = wi.ImageUrl.TrimEnd(',');

                                    if (!extSett.VehicleCompatibility)
                                    {
                                        wi.Notes = string.Empty;
                                        wi.Make = string.Empty;
                                        wi.Model = string.Empty;
                                        wi.Year = string.Empty;
                                        wi.Engine = string.Empty;
                                        wi.Trim = string.Empty;

                                        AddWareInfo(wi);
                                        OnItemLoaded(wi);
                                        break;
                                    }


                                    extSett.DataForFile.Add(new ExtWareInfo(wi));
                                    //AddWareInfo(wi);
                                    //OnItemLoaded(wi);
                                }
                            }

                        }
                        else
                        {
                            counter1million++;
                            if (counter1million > 1000000)
                            {
                                lock (_lockObject)
                                {
                                    List<ExtWareInfo> itemsForFile = new List<ExtWareInfo>();
                                    itemsForFile.AddRange(extSett.DataForFile);

                                    fileCounter++;
                                    extSett.DataForFile = new List<ExtWareInfo>();
                                    counter1million = 1;

                                    string filePath = FileHelper.CreateFile(fileCounter, itemsForFile);
                                    MessagePrinter.PrintMessage($"File Created - {filePath}");
                                }
                            }

                            if (counter1million < 1000000)
                            {

                                ExtWareInfo wi = new ExtWareInfo();

                                string specifications = string.Empty;
                                if (responseItem.Item.ItemSpecifics != null)
                                {
                                    specifications += $"{_specifications}";
                                    foreach (var itemSpecific in responseItem.Item.ItemSpecifics)
                                    {
                                        if (itemSpecific.Name == "Brand")
                                        {
                                            wi.Brand = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "UPC")
                                        {
                                            wi.UPC = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "Manufacturer Part Number")
                                        {
                                            wi.ManufacturerNumber = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "Interchange Part Number")
                                        {
                                            wi.InterchangePartNumber = itemSpecific.Value;
                                        }
                                        else if (itemSpecific.Name == "Other Part Number")
                                        {
                                            wi.OtherPartNumber = itemSpecific.Value;
                                        }
                                        else
                                        {
                                            specifications += $"{itemSpecific.Name}~{itemSpecific.Value}^";
                                        }

                                    }
                                    if (!string.IsNullOrEmpty(specifications))
                                    {
                                        wi.Specifications = specifications.Trim('^');
                                    }
                                }


                                wi.ProductTitle = responseItem.Item.Title;
                                wi.Timestamp = responseItem.Timestamp.ToString();
                                wi.Description = responseItem.Item.Description?.Truncate(DescriptionLength);

                                wi.ItemNumber = responseItem.Item.ItemID.ToString();
                                wi.Quantity = responseItem.Item.Quantity;
                                wi.UserId = responseItem.Item.Seller?.UserID;
                                wi.ConvertedCurrentPrice = Convert.ToDouble(responseItem.Item.ConvertedCurrentPrice.Value);
                                if (responseItem.Item.DiscountPriceInfo != null)
                                    wi.OriginalRetailPrice =
                                        responseItem.Item.DiscountPriceInfo.OriginalRetailPrice.Value.ToString();
                                wi.ListingStatus = responseItem.Item.ListingStatus;
                                wi.QuantitySold = responseItem.Item.QuantitySold.ToString();
                                if (responseItem.Item.ShippingCostSummary.ShippingServiceCost != null)
                                    wi.ShippingServiceCost = responseItem.Item.ShippingCostSummary.ShippingServiceCost.Value
                                        .ToString();
                                wi.ShippingType = responseItem.Item.ShippingCostSummary.ShippingType;
                                if (responseItem.Item.ShippingCostSummary.ListedShippingServiceCost != null)
                                    wi.ListedShippingServiceCost = responseItem.Item.ShippingCostSummary
                                        .ListedShippingServiceCost.Value.ToString();
                                wi.HitCount = responseItem.Item.HitCount.ToString();
                                wi.StoreName = responseItem.Item.Storefront?.StoreName;
                                wi.ItemCompatibilityCount = responseItem.Item.ItemCompatibilityCount.ToString();
                                wi.PartNumber = responseItem.Item?.SKU;
                                wi.Categories = responseItem.Item?.PrimaryCategoryName;

                                if (responseItem.Item.PictureURL != null)
                                    foreach (string pictireUrl in responseItem.Item.PictureURL)
                                        wi.ImageUrl += pictireUrl.RemoveParametrs() + ",";
                                if (!string.IsNullOrEmpty(wi.ImageUrl))
                                    wi.ImageUrl = wi.ImageUrl.TrimEnd(',');

                                //if (wi.Description.Contains("https://newparts.com/files/eBay/shippingImg.png"))
                                //{
                                //    extSett.DataForFile.Add(new ExtWareInfo(wi));
                                //    AddWareInfo(wi);
                                //    OnItemLoaded(wi);
                                //}

                                extSett.DataForFile.Add(new ExtWareInfo(wi));
                                AddWareInfo(wi);
                                OnItemLoaded(wi);
                            }
                        }

                        limitFlag = false;
                    }
                    else if (responseItem?.Errors != null && responseItem.Ack == "Failure")
                    {
                        if (responseItem.Errors.LongMessage.Contains("limited in the number of calls"))
                        {
                            lock (_lockObject)
                            {
                                limitFlag = true;
                                EbayApiIdKeys[apiKey] = true;    
                            }
                        }

                        MessagePrinter.PrintMessage($"Request to ebay Failure [{pqi.URL}]. {responseItem.Errors.LongMessage}", ImportanceLevel.High);
                    }
                    else
                    {
                        MessagePrinter.PrintMessage($"Error while take item from Ebay - [{pqi.URL}]. Error - {exceptionStr}", ImportanceLevel.High);
                    }
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
                case (int)ItemType.ProcessingFile:
                    act = ProcessFile;
                    break;
                case (int)ItemType.ProcessingEbayRequest:
                    act = ProcessEbayRequest;
                    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }
    }
}
