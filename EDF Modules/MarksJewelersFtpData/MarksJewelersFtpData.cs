using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using MarksJewelersFtpData;
using Databox.Libs.MarksJewelersFtpData;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using MarksJewelersFtpData.Extensions;
using MarksJewelersFtpData.SCEapi;
using MarksJewelersFtpData.Helper_Methods;
using MarksJewelersFtpData.Enums;

namespace WheelsScraper
{
    public class MarksJewelersFtpData : BaseScraper
    {
        #region constants

        private const string Separator = ",";
        private const string RaRiamGroupSupplier = "RA Riam Group";
        private const string LeoSchachterDiamondsSupplier = "Leo Schachter Diamonds";
        private const string RDITraidingSupplier = "RDI Trading";
        private const string SaharAtidSupplier = "Sahar Atid";

        private const string DescriptionParagraph =
            "</p><p>This diamond is currently with one of our vendor partners. Please click the link below to schedule an appointment and we will bring it in for you free of charge. All diamonds on the website are excluded from any promotions. Price is final.</p>";

        #endregion

        //public List<string> emptyShapes = new List<string>();

        public List<ExtWareInfo> RaRiamGroupCollection = new List<ExtWareInfo>();
        public List<ExtWareInfo> RosyBlueCollection = new List<ExtWareInfo>();
        public List<ExtWareInfoSceExport> sceItemsExport = new List<ExtWareInfoSceExport>();
        public List<ExtWareInfo> ftp4Collection = new List<ExtWareInfo>();//RDI Trading
        public List<ExtWareInfo> ftp5Collection = new List<ExtWareInfo>();//Sashar Atid
        public List<ExtWareInfo> ftp2Collection = new List<ExtWareInfo>();//Leo Schachter Diamonds

        public List<InventoryQty> quantityFTP = new List<InventoryQty>();
        public List<InternalUse> internalUse = new List<InternalUse>();
        private string _сustomHtmlAbovePrice;
        public MarksJewelersFtpData()
        {
            Name = "MarksJewelersFtpData";
            Url = "https://www.MarksJewelersFtpData.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;
            Complete += MarksJewelersFtpData_Complete;

            SpecialSettings = new ExtSettings();
        }

        private void MarksJewelersFtpData_Complete(object sender, EventArgs e)
        {
            //emptyShapes = emptyShapes.Distinct().ToList();

            if (quantityFTP.Count > 0 && internalUse.Count != 0)
            {
                string fileName = "InternalUse.csv";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string separator = ",";
                string headers = "action,prodid,Product Type,Part Number,internaluse";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (InternalUse item in internalUse)
                {
                    string[] productArr = new string[5] { item.Action, item.ProdId, item.ProductType, item.PartNumber, item.InternalUseValue };

                    for (int i = 0; i < productArr.Length; i++)
                        if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                            productArr[i] = StringToCSVCell(productArr[i]);

                    string product = String.Join(separator, productArr);
                    sb.AppendLine(product);
                }

                File.WriteAllText(filePath, sb.ToString());

                MessagePrinter.PrintMessage($"file created in {filePath}");

                try
                {
                    MessagePrinter.PrintMessage($"Upload on FTP");

                    string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, fileName, filePath, true);
                    if (!string.IsNullOrEmpty(url))
                    {
                        string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                        BatchUpdate(urlForBatch, Settings);
                        MessagePrinter.PrintMessage("File uploaded: " + urlForBatch);
                        MessagePrinter.PrintMessage("File Batched: ");
                    }
                }
                catch (System.Exception y)
                {
                    MessagePrinter.PrintMessage($"{y.Message} - when upload Premier file on FTP");
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            if (quantityFTP.Count > 0 && extSett.UploadInventory == true)
            {
                string fileName = "MJ_Inventory.csv";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string separator = ",";
                string headers = "Part number,Qty";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (InventoryQty item in quantityFTP)
                {
                    string[] productArr = new string[2] { item.PartNumber, item.Quantity.ToString() };

                    for (int i = 0; i < productArr.Length; i++)
                        if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                            productArr[i] = StringToCSVCell(productArr[i]);

                    string product = String.Join(separator, productArr);
                    sb.AppendLine(product);
                }

                File.WriteAllText(filePath, sb.ToString());

                MessagePrinter.PrintMessage($"file created in {filePath}");

                try
                {
                    MessagePrinter.PrintMessage($"Upload on FTP");

                    string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, fileName, filePath, true);
                    if (!string.IsNullOrEmpty(url))
                    {
                        MessagePrinter.PrintMessage("File uploaded: " + url);
                    }
                }
                catch (System.Exception y)
                {
                    MessagePrinter.PrintMessage($"{y.Message} - when upload Premier file on FTP");
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            if (extSett.DownloadRosyBlue)
            {
                if (RosyBlueCollection.Count > 0)
                {
                    string fileName = "MarksJuwelersFTP3RosyBlue.csv";
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    string separator = ",";
                    string headers = "Action,prodid,Product Type,Product Title,Anchor Text,Spider URL,Screen Title,Sub Title,Brand,Description,META Description,Main Category,Sub Category,General Image" +
                        ",Specifications,Processing Time,Shipping Type,Shipping Carrier 1,Allowground,Allow3day,Allow2day,Allownextday,Live Inventory,Supplier,Re-Order Supplier,File Attachments" +
                        ",Part Number,Manufacturer Part Number,MSRP,Jobber,Web Price,Cost Price,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height,Shipping Width,Shipping Length," +
                        "Cross sell main cat 1,Cross sell sub cat 1,Cross sell sec cat 1,Videos,internaluse,custom html above price,Checkout Rule";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(headers);

                    foreach (ExtWareInfo item in RosyBlueCollection)
                    {
                        if (item.Action.ToLower() == "update" || item.Action.ToLower() == "add")
                        {
                            if (string.IsNullOrEmpty(item.PanoramImage))
                            {
                                item.PanoramImage = $"3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                            else
                            {
                                item.PanoramImage = $"~!~3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                        }

                        string[] productArr = new string[47] { item.Action, item.ProductId, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.ScreenTitle, item.SubTitle
                        , item.BrandItem, item.DescriptionItem, item.METADescription, item.MainCategory, item.SubCategory, item.GeneralImage, item.Specifications, item.ProcessingTime
                        , item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.LiveInventory, item.Supplier
                        , item.ReOrderSupplier, item.FileAttachments, item.PartNumberItem, item.ManufacturerPartNumber, item.MSRPItem.ToString(), item.JobberItem.ToString(), item.WebPrice.ToString()
                        , item.CostPrice.ToString(), item.ItemWeight, item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth
                        , item.ShippingLength, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.PanoramImage, item.InternalUse, item.CustomHtmlAbovePrice, item.CheckoutRule };

                        for (int i = 0; i < productArr.Length; i++)
                            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                                productArr[i] = StringToCSVCell(productArr[i]);

                        string product = String.Join(separator, productArr);
                        sb.AppendLine(product);
                    }

                    File.WriteAllText(filePath, sb.ToString());

                    MessagePrinter.PrintMessage($"file created in {filePath}");

                    try
                    {
                        MessagePrinter.PrintMessage($"Upload on FTP");

                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername,
                            Settings.FtpPassword, fileName, filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            //BatchUpdate(urlForBatch, Settings); было закомментровано
                            //MessagePrinter.PrintMessage("File uploaded: " + urlForBatch);
                            //MessagePrinter.PrintMessage("File Batched: ");
                        }
                    }
                    catch (Exception y)
                    {
                        MessagePrinter.PrintMessage($"{y.Message} - when upload Premier file on FTP");
                    }
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if (extSett.DownloadRARiamGroup)
            {
                if (RaRiamGroupCollection.Count > 0)
                {
                    string fileName = "MarksJuwelersFTP1RARiamGroup.csv";
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    string separator = ",";
                    string headers = "Action,prodid,Product Type,Product Title,Anchor Text,Spider URL,Screen Title,Sub Title,Brand,Description,META Description,Main Category,Sub Category,General Image" +
                        ",Specifications,Processing Time,Shipping Type,Shipping Carrier 1,Allowground,Allow3day,Allow2day,Allownextday,Live Inventory,Supplier,Re-Order Supplier,File Attachments" +
                        ",Part Number,MSRP,Jobber,Web Price,Cost Price,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height,Shipping Width,Shipping Length,Cross sell main cat 1," +
                        "Cross sell sub cat 1,Cross sell sec cat 1,Videos,internaluse,custom html above price,Checkout Rule";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(headers);

                    foreach (ExtWareInfo item in RaRiamGroupCollection)
                    {
                        if (item.Action.ToLower() == "update" || item.Action.ToLower() == "add")
                        {
                            if (string.IsNullOrEmpty(item.PanoramImage))
                            {
                                item.PanoramImage = $"3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                            else
                            {
                                item.PanoramImage = $"~!~3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                        }

                        string[] productArr = new string[46] { item.Action, item.ProductId, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.ScreenTitle, item.SubTitle
                        , item.BrandItem, item.DescriptionItem, item.METADescription, item.MainCategory, item.SubCategory, item.GeneralImage, item.Specifications, item.ProcessingTime
                        , item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.LiveInventory, item.Supplier
                        , item.ReOrderSupplier, item.FileAttachments, item.PartNumberItem, item.MSRPItem.ToString(), item.JobberItem.ToString(), item.WebPrice.ToString()
                        , item.CostPrice.ToString(), item.ItemWeight, item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth
                        , item.ShippingLength, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.PanoramImage, item.InternalUse, item.CustomHtmlAbovePrice, item.CheckoutRule };

                        for (int i = 0; i < productArr.Length; i++)
                            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                                productArr[i] = StringToCSVCell(productArr[i]);

                        string product = String.Join(separator, productArr);
                        sb.AppendLine(product);
                    }

                    File.WriteAllText(filePath, sb.ToString());

                    MessagePrinter.PrintMessage($"file created in {filePath}");

                    try
                    {
                        MessagePrinter.PrintMessage($"Upload on FTP");

                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername,
                            Settings.FtpPassword, fileName, filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            BatchUpdate(urlForBatch, Settings);
                            MessagePrinter.PrintMessage("File uploaded: " + urlForBatch);
                            MessagePrinter.PrintMessage("File Batched: ");
                        }

                    }
                    catch (Exception y)
                    {
                        MessagePrinter.PrintMessage($"{y.Message} - when upload Premier file on FTP");
                    }
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if (extSett.DownloadRDITrading)
            {
                if (ftp4Collection.Count > 0)
                {
                    string fileName = "MarksJuwelersFTP4RDITrading.csv";
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    string separator = ",";
                    string headers = "Action,prodid,Product Type,Product Title,Anchor Text,Spider URL,Screen Title,Sub Title,Brand,Description,META Description,Main Category,Sub Category,General Image" +
                        ",Specifications,Processing Time,Shipping Type,Shipping Carrier 1,Allowground,Allow3day,Allow2day,Allownextday,Live Inventory,Supplier,Re-Order Supplier,File Attachments" +
                        ",Part Number,MSRP,Jobber,Web Price,Cost Price,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height,Shipping Width,Shipping Length,Cross sell main cat 1," +
                        "Cross sell sub cat 1,Cross sell sec cat 1,Videos,internaluse,custom html above price,Checkout Rule";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(headers);

                    foreach (ExtWareInfo item in ftp4Collection)
                    {
                        if (item.Action.ToLower() == "update" || item.Action.ToLower() == "add")
                        {
                            if (string.IsNullOrEmpty(item.PanoramImage))
                            {
                                item.PanoramImage = $"3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                            else
                            {
                                item.PanoramImage = $"~!~3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                        }

                        string[] productArr = new string[46] { item.Action, item.ProductId, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.ScreenTitle, item.SubTitle
                        , item.BrandItem, item.DescriptionItem, item.METADescription, item.MainCategory, item.SubCategory, item.GeneralImage, item.Specifications, item.ProcessingTime
                        , item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.LiveInventory, item.Supplier
                        , item.ReOrderSupplier, item.FileAttachments, item.PartNumberItem, item.MSRPItem.ToString(), item.JobberItem.ToString(), item.WebPrice.ToString()
                        , item.CostPrice.ToString(), item.ItemWeight, item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth
                        , item.ShippingLength,item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.PanoramImage, item.InternalUse, item.CustomHtmlAbovePrice, item.CheckoutRule };

                        for (int i = 0; i < productArr.Length; i++)
                            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                                productArr[i] = StringToCSVCell(productArr[i]);

                        string product = String.Join(separator, productArr);
                        sb.AppendLine(product);
                    }

                    File.WriteAllText(filePath, sb.ToString());

                    MessagePrinter.PrintMessage($"file created in {filePath}");

                    try
                    {
                        MessagePrinter.PrintMessage($"Upload on FTP");

                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername,
                            Settings.FtpPassword, fileName, filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            BatchUpdate(urlForBatch, Settings);
                            MessagePrinter.PrintMessage("File uploaded: " + urlForBatch);
                            MessagePrinter.PrintMessage("File Batched: ");
                        }
                    }
                    catch (Exception y)
                    {
                        MessagePrinter.PrintMessage($"{y.Message} - when upload Premier file on FTP");
                    }
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if (extSett.DownloadSaharAtid)
            {
                if (ftp5Collection.Count > 0)
                {
                    string fileName = "MarksJuwelersFTP5SaharAtid.csv";
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    string separator = ",";
                    string headers = "Action,prodid,Product Type,Product Title,Anchor Text,Spider URL,Screen Title,Sub Title,Brand,Description,META Description,Main Category,Sub Category,General Image" +
                        ",Specifications,Processing Time,Shipping Type,Shipping Carrier 1,Allowground,Allow3day,Allow2day,Allownextday,Live Inventory,Supplier,Re-Order Supplier,File Attachments" +
                        ",Part Number,MSRP,Jobber,Web Price,Cost Price,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height,Shipping Width,Shipping Length,Cross sell main cat 1," +
                        "Cross sell sub cat 1,Cross sell sec cat 1,Videos,internaluse,custom html above price,Checkout Rule";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(headers);

                    foreach (ExtWareInfo item in ftp5Collection)
                    {
                        if (item.Action.ToLower() == "update" || item.Action.ToLower() == "add")
                        {
                            if (string.IsNullOrEmpty(item.PanoramImage))
                            {
                                item.PanoramImage = $"3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                            else
                            {
                                item.PanoramImage = $"~!~3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                        }

                        string[] productArr = new string[46] { item.Action, item.ProductId, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.ScreenTitle, item.SubTitle
                        , item.BrandItem, item.DescriptionItem, item.METADescription, item.MainCategory, item.SubCategory, item.GeneralImage, item.Specifications, item.ProcessingTime
                        , item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.LiveInventory, item.Supplier
                        , item.ReOrderSupplier, item.FileAttachments, item.PartNumberItem, item.MSRPItem.ToString(), item.JobberItem.ToString(), item.WebPrice.ToString()
                        , item.CostPrice.ToString(), item.ItemWeight, item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth
                        , item.ShippingLength, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1, item.PanoramImage, item.InternalUse, item.CustomHtmlAbovePrice, item.CheckoutRule };

                        for (int i = 0; i < productArr.Length; i++)
                            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                                productArr[i] = StringToCSVCell(productArr[i]);

                        string product = String.Join(separator, productArr);
                        sb.AppendLine(product);
                    }

                    File.WriteAllText(filePath, sb.ToString());

                    MessagePrinter.PrintMessage($"file created in {filePath}");

                    try
                    {
                        MessagePrinter.PrintMessage($"Upload on FTP");

                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername,
                            Settings.FtpPassword, fileName, filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            BatchUpdate(urlForBatch, Settings);
                            MessagePrinter.PrintMessage("File uploaded: " + urlForBatch);
                            MessagePrinter.PrintMessage("File Batched: ");
                        }
                    }
                    catch (Exception y)
                    {
                        MessagePrinter.PrintMessage($"{y.Message} - when upload Premier file on FTP");
                    }
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if (extSett.DownloadLSDiamonds)
            {
                if (ftp2Collection.Count > 0)
                {
                    string fileName = "MarksJuwelersFTP2LSDiamonds.csv";
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    string separator = ",";
                    string headers =
                        "Action,prodid,Product Type,Product Title,Anchor Text,Spider URL,Screen Title,Sub Title,Brand,Description,META Description,Main Category,Sub Category,General Image" +
                        ",Specifications,Processing Time,Shipping Type,Shipping Carrier 1,Allowground,Allow3day,Allow2day,Allownextday,Live Inventory,Supplier,Re-Order Supplier,File Attachments" +
                        ",Part Number,MSRP,Jobber,Web Price,Cost Price,Item Weight,Item Height,Item Width,Item Length,Shipping Weight,Shipping Height,Shipping Width,Shipping Length,custom html below price," +
                        "Checkout Rule,Cross sell main cat 1,Cross sell sub cat 1,Cross sell sec cat 1,Videos,internaluse,custom html above price";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(headers);

                    foreach (ExtWareInfo item in ftp2Collection)
                    {
                        if (!string.IsNullOrEmpty(item.PanoramImage) || string.Equals(item.Action, "removeall"))
                        {
                            if (!ProcessPanoramImage(item, extSett.DeleteImgLSDiamonds, LeoSchachterDiamondsSupplier))
                                item.PanoramImage = string.Empty;
                        }

                        if (item.Action.ToLower() == "update" || item.Action.ToLower() == "add")
                        {
                            if (string.IsNullOrEmpty(item.PanoramImage))
                            {
                                item.PanoramImage = $"3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                            else
                            {
                                item.PanoramImage = $"~!~3^Marks Jewelers Store^https://player.vimeo.com/video/347130418";
                            }
                        }

                        string[] productArr = new string[47] { item.Action, item.ProductId, item.ProductType, item.ProductTitle, item.AnchorText, item.SpiderURL, item.ScreenTitle, item.SubTitle
                        , item.BrandItem, item.DescriptionItem, item.METADescription, item.MainCategory, item.SubCategory, item.GeneralImage, item.Specifications, item.ProcessingTime
                        , item.ShippingType, item.ShippingCarrier1, item.Allowground, item.Allow3day, item.Allow2day, item.Allownextday, item.LiveInventory, item.Supplier
                        , item.ReOrderSupplier, item.FileAttachments, item.PartNumberItem, item.MSRPItem.ToString(), item.JobberItem.ToString(), item.WebPrice.ToString()
                        , item.CostPrice.ToString(), item.ItemWeight, item.ItemHeight, item.ItemWidth, item.ItemLength, item.ShippingWeight, item.ShippingHeight, item.ShippingWidth
                        , item.ShippingLength, item.CustomHtmlBelowPrice, item.CheckoutRule, item.CrossSellMainCat1, item.CrossSellSubCat1, item.CrossSellSecCat1,item.PanoramImage
                        , item.InternalUse,item.CustomHtmlAbovePrice };

                        for (int i = 0; i < productArr.Length; i++)
                            if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
                                productArr[i] = StringToCSVCell(productArr[i]);

                        string product = String.Join(separator, productArr);
                        sb.AppendLine(product);
                    }

                    File.WriteAllText(filePath, sb.ToString());

                    MessagePrinter.PrintMessage($"file created in {filePath}");

                    try
                    {
                        MessagePrinter.PrintMessage($"Upload on FTP");

                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername,
                            Settings.FtpPassword, fileName, filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string modelurlForBatch = "http://efilestorage.com/scefiles/sce.elena_gmail.com/Sanmar/Sanmar.csv";
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            BatchUpdate(urlForBatch, Settings);
                            MessagePrinter.PrintMessage("File uploaded: " + urlForBatch);
                            MessagePrinter.PrintMessage("File Batched: ");
                        }
                    }
                    catch (Exception y)
                    {
                        MessagePrinter.PrintMessage($"{y.Message} - when upload Premier file on FTP");
                    }
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }

        private bool ProcessPanoramImage(ExtWareInfo item, bool deleteImages, string supplier)
        {
            try
            {
                string folderName = $"{supplier} {item.PartNumberItem}";
                string folderPath = $"ftp://{extSett.FtpAdress}/{folderName}";

                if (FtpHelper.CheckIfFileExists(folderPath, extSett.FtpLogin, extSett.FtpPassword))
                {
                    if (deleteImages && string.Equals(item.Action, "removeall"))
                    {
                        MessagePrinter.PrintMessage($"Delete images for - {folderName}");
                        RequestHelper.FtpDeleteDirectory(folderName, extSett.FtpAdress, extSett.FtpLogin, extSett.FtpPassword);

                        return false;
                    }
                    else
                    {
                        item.PanoramImage = $"5^{item.PartNumberItem}^https://ecidegat.sirv.com/{folderName}/{folderName}.spin";
                    }
                }
                else
                {
                    if (string.Equals(item.Action, "removeall"))
                        return false;

                    MessagePrinter.PrintMessage($"Processing images for - {folderName}");
                    List<Dictionary<string, string>> fileList = new List<Dictionary<string, string>>();

                    for (int i = 1; i <= 150; i++)
                    {
                        fileList.Add(RequestHelper.DownloadImage($"https://img.diacertimages.com/images/{item.PartNumberItem}/cam1/cam1_zoom_{i.ToString().FormatImageNumber()}.jpg"
                            , item.PartNumberItem
                            , $"cam1_zoom_{i.ToString().FormatImageNumber()}.jpg"));
                    }

                    int cointer = 0;

                    foreach (Dictionary<string, string> dictionary in fileList)
                    {
                        if (cointer == 0)
                        {
                            cointer++;
                            RequestHelper.FtpCreateFolder(extSett.FtpAdress, folderName, extSett.FtpLogin, extSett.FtpPassword);
                        }

                        RequestHelper.UploadFtpFile(dictionary.First().Key, folderName, extSett.FtpLogin, extSett.FtpPassword);

                        Thread.Sleep(200);
                        if (File.Exists(dictionary.First().Key))
                            File.Delete(dictionary.First().Key);
                    }

                    Thread.Sleep(1000);
                    FileHelper.DeleteDirectory(FileHelper.GetSettingsPath(item.PartNumberItem));
                    item.PanoramImage = $"5^{item.PartNumberItem}^https://ecidegat.sirv.com/{folderName}/{folderName}.spin";
                    //  5^69067^https://pirevoil.sirv.com/Spins/69067/69067.spin
                }

                MessagePrinter.PrintMessage($"Image for - {folderName} processed");
            }
            catch
            {
                MessagePrinter.PrintMessage($"problem with panoram image in part - {item.PartNumberItem}", ImportanceLevel.High);
                return false;
            }

            return true;
        }

        public static string StringToCSVCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
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
            if (!FtpHelper.CheckConnection(extSett.FtpAdress, extSett.FtpLogin, extSett.FtpPassword))
                throw new Exception($"Check FTP credentials for SIRV");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MarksJewelersFtpData.Resources.CustomHtmlAbovePrice.txt";
            _сustomHtmlAbovePrice = string.Empty;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                _сustomHtmlAbovePrice = reader.ReadToEnd();
            }

            RaRiamGroupCollection.Clear();
            RosyBlueCollection.Clear();
            ftp2Collection.Clear();
            ftp4Collection.Clear();
            ftp5Collection.Clear();
            quantityFTP.Clear();
            internalUse.Clear();

            bool someChecked = false;

            if (extSett.DownloadRARiamGroup == true)
            {
                someChecked = true;
                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem { ItemType = 1 });
                }
            }

            if (extSett.DownloadRosyBlue == true)
            {
                someChecked = true;
                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem { ItemType = 2 });
                }
            }

            if (extSett.DownloadRDITrading == true)
            {
                someChecked = true;
                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem { ItemType = 3 });
                }
            }

            if (extSett.DownloadLSDiamonds == true)
            {
                someChecked = true;
                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem { ItemType = 4 });
                }
            }
            if (extSett.DownloadSaharAtid == true)
            {
                someChecked = true;
                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem { ItemType = 5 });
                }
            }


            if (someChecked)
            {
                MessagePrinter.PrintMessage("SCEExport - downloading");
                string sceExportFilePath = LoadProductsExport(Settings);
                MessagePrinter.PrintMessage("SCEExport- downloaded");

                sceItemsExport = ReadSceExport(sceExportFilePath);
                sceItemsExport = sceItemsExport.Distinct(new ExtWareInfoSceExport.BrandPartNumberEqualityComparer()).ToList();

                if (File.Exists(sceExportFilePath))
                {
                    File.Delete(sceExportFilePath);
                }
            }

            StartOrPushPropertiesThread();
        }

        /// <summary>
        /// ----------------------------------------------------------ProcessDownloadLSDiamonds---------------------------------------------------------
        /// </summary>


        private void ProcessDownloadLSDiamonds(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            const string actionAdd = "add";
            const string brand = "Marks Jewelers";
            const string productType = "1";
            const string mainCategory = "Diamonds";
            const string subCategory = "Diamonds";
            const string processingTime = "1";
            const string shippingType = "dynamic";
            const string shippingCarrier1 = "FedEx";
            const string allowground = "1";
            const string allow3day = "1";
            const string allow2day = "1";
            const string allownextday = "1";
            const string liveInventory = "1";
            const string supplier = "Leo Schachter Diamonds";
            const string reOrderSupplier = "Leo Schachter Diamonds";
            const string itemWeight = "1";
            const string itemHeight = "1";
            const string itemWidth = "1";
            const string itemLength = "1";
            const string shippingWeight = "1";
            const string shippingHeight = "1";
            const string shippingWidth = "1";
            const string shippingLength = "1";
            const string chekoutRule = "3";

            double length;
            double width;
            double height;

            MessagePrinter.PrintMessage("Leo Schachter Diamonds processing");

            MessagePrinter.PrintMessage("Download file from Leo Schachter Diamonds");

            string downloadFTPFile = DownloadFTPFile("LeoSchachterDiamonds@efilestorage.com", "LSDiamonds!", "ftp://efilestorage.com/MarksJewelers.csv", "lsDiamonds_Data.csv");
            if (string.IsNullOrEmpty(downloadFTPFile))
            {
                MessagePrinter.PrintMessage("File from FTP - not downloaded", ImportanceLevel.High);

                return;
            }

            List<LSDiamond> dataFTPFile = ReadSceExportLSD(downloadFTPFile);
            if (File.Exists(downloadFTPFile))
            {
                File.Delete(downloadFTPFile);
            }

            List<LSDiamond> itemsForRemoveByShape = new List<LSDiamond>();
            foreach (var diamond in dataFTPFile)
            {
                var sh = diamond.ShapeLong(diamond.Shape.TrimEnd());
                if (string.IsNullOrEmpty(sh))
                {
                    itemsForRemoveByShape.Add(diamond);
                }
            }

            foreach (LSDiamond ftp1 in itemsForRemoveByShape)
            {
                dataFTPFile.Remove(ftp1);
            }

            if (dataFTPFile != null)
            {
                MessagePrinter.PrintMessage("Parsing FTP complete");
            }
            MessagePrinter.PrintMessage("Compare Data");

            foreach (var item in dataFTPFile)
            {
                bool ftpItemFound = false;
                ExtWareInfo product = new ExtWareInfo();
                InventoryQty qty = new InventoryQty();
                foreach (var itemSCE in sceItemsExport)
                {
                    product = new ExtWareInfo();

                    product.Action = actionAdd;
                    product.CheckoutRule = chekoutRule;

                    product.CustomHtmlBelowPrice = "<p class=\"wrap-appoint-btn\"> <img src=\"/files//marks_2/appointment_button2.png\" data-appointlet-organization=\"marks-jewelers\" data-appointlet-field-sku=\"" + item.Id.TrimEnd() + "\" class=\"appointment_button\"> </p>";

                    product.CustomHtmlAbovePrice = _сustomHtmlAbovePrice;

                    if (item.ShapeLong(item.Shape.TrimEnd()) == "")
                    {
                        continue;
                    }

                    product.ProductType = productType;
                    product.ProductTitle = String.Format("{0:0.00}", item.Carat) + " Carat " + item.CutGradeLong(item.Cut.TrimEnd()) + item.ShapeLong(item.Shape.TrimEnd()) + " Diamond | " + item.Id.TrimEnd() + " | Marks Jewelers";
                    product.AnchorText = String.Format("{0:0.00}", item.Carat) + " Carat " + item.CutGradeLong(item.Cut.TrimEnd()) + item.ShapeLong(item.Shape.TrimEnd()) + " Diamond";
                    product.SpiderURL = (String.Format("{0:0.00}", item.Carat) + "-Carat-" + item.CutGradeLongSpiderURL(item.Cut.TrimEnd()) + "-" + item.ShapeLong(item.Shape.TrimEnd()) + "-Diamond-" + item.Id.TrimEnd() + "-L01").Replace("--", "-");
                    product.ScreenTitle = String.Format("{0:0.00}", item.Carat) + " Carat " + item.CutGradeLong(item.Cut.TrimEnd()) + item.ShapeLong(item.Shape.TrimEnd()) + " Diamond";
                    product.SubTitle = Metods.ColorHardCode(item.Color.TrimEnd()) + " Color " + Metods.ClarityHardCode(item.Clarity.TrimEnd()) + " Clarity ";
                    product.BrandItem = brand;
                    product.DescriptionItem = "This is " + String.Format("{0:0.00}", item.Carat) + " Carat " + item.CutGradeLong(item.Cut.TrimEnd()) + item.ShapeLong(item.Shape.TrimEnd()) + " Diamond" + " has " + item.SymmetryLongLeo(item.Symmetry.Trim()) +
                        " Symmetry, " + item.PolishLong(item.Polish.Trim()) + " Polish, " + item.CertificateName(item.CertificateLab.TrimEnd());

                    product.DescriptionItem += DescriptionParagraph;

                    product.METADescription = "This is " + String.Format("{0:0.00}", item.Carat) + " Carat " + item.CutGradeLong(item.Cut.TrimEnd()) + item.ShapeLong(item.Shape.TrimEnd()) + " Diamond" + " has " + item.SymmetryLongLeo(item.Symmetry.Trim()) +
                        " Symmetry, " + item.PolishLong(item.Polish.Trim()) + " Polish, " + item.CertificateName(item.CertificateLab.TrimEnd());
                    product.MainCategory = mainCategory;
                    product.SubCategory = subCategory;
                    ImageMaker subCat = new ImageMaker();
                    product.CrossSellMainCat1 = "Diamonds by Shape";
                    product.CrossSellSubCat1 = subCat.ShapeShare(item.Shape.TrimEnd());

                    string measurements = item.Measurements.TrimEnd();
                    string[] tokens = measurements.Split('x');
                    length = double.Parse(tokens[0]);
                    width = double.Parse(tokens[1]);
                    height = double.Parse(tokens[2]);

                    product.GeneralImage = subCat.ImageMake(item.Shape.TrimEnd());
                    product.PanoramImage = item.VideosURL;
                    if ((product.GeneralImage != null || product.GeneralImage != "") && (product.PanoramImage != ""))
                    {
                        product.GeneralImage = product.GeneralImage + ",https://img.diacertimages.com/images/" + item.Id.TrimEnd() + "/cam1/cam1_zoom_001.jpg";
                    }

                    product.Specifications = "Specifications##Shape~" + item.ShapeLong(item.Shape.TrimEnd()) + "^Carat~" + String.Format("{0:0.00}", item.Carat) +
                            "^Color~" + Metods.ColorHardCode(item.Color.TrimEnd()) + "^Clarity~" + Metods.ClarityHardCode(item.Clarity.TrimEnd()) + item.CutGradeLongSpecification(item.Cut.TrimEnd()) +
                            "^Polish~" + item.PolishLong(item.Polish.Trim()) + item.SymmetryLong(item.Symmetry.Trim()) +
                            item.FluorescenceLong(item.Fluorescence.TrimEnd()) + item.CertificateLabSpecialization(item.CertificateLab.TrimEnd(), item.CertificateID) + "^Total Depth~" +
                            item.DepthPercentage.Trim() + "^Table Size~" + item.Table.Trim() + "^Length~" + length + "^Width~" + width + "^Height~" + height + "^L/W Ratio~" + Math.Round((length / width), 2);
                    product.ProcessingTime = processingTime;
                    product.ShippingType = shippingType;
                    product.ShippingCarrier1 = shippingCarrier1;
                    product.Allowground = allowground;
                    product.Allow3day = allow3day;
                    product.Allow2day = allow2day;
                    product.Allownextday = allownextday;
                    product.LiveInventory = liveInventory;
                    product.Supplier = supplier;
                    product.ReOrderSupplier = reOrderSupplier;
                    product.FileAttachments = "Certificate^^" + item.CertificateURL;
                    product.PartNumberItem = item.Id.TrimEnd();
                    qty.PartNumber = product.PartNumberItem;
                    product.MSRPItem = Math.Round((item.TotalPrice / 0.77), 2);
                    product.JobberItem = Math.Round((item.TotalPrice / 0.77), 2);
                    product.WebPrice = Math.Round((item.TotalPrice / 0.77), 2);
                    product.CostPrice = Math.Round((item.TotalPrice), 2);
                    product.CrossSellSecCat1 = subCat.CostRange(product.WebPrice);
                    product.ItemWeight = itemWeight;
                    product.ItemHeight = itemHeight;
                    product.ItemWidth = itemWidth;
                    product.ItemLength = itemLength;
                    product.ShippingWeight = shippingWeight;
                    product.ShippingHeight = shippingHeight;
                    product.ShippingWidth = shippingWidth;
                    product.ShippingLength = shippingLength;

                    if (itemSCE.PartNumber == item.Id.TrimEnd() && itemSCE.Brand == brand && itemSCE.Supplier == supplier)
                    {
                        product.Action = "update";
                        product.ProductId = itemSCE.Prodid;
                        product.InternalUse = "0";
                        ftpItemFound = true;
                        qty.Quantity = 1;
                        quantityFTP.Add(qty);

                        ftp2Collection.Add(product);
                    }

                }
                if (!ftpItemFound)
                {
                    product.Action = "add";
                    product.InternalUse = "0";
                    qty.Quantity = 1;
                    quantityFTP.Add(qty);
                    ftp2Collection.Add(product);
                }
            }
            foreach (var trueLink in ftp2Collection)
            {
                trueLink.FileAttachments = UrlCheck(trueLink.FileAttachments.Replace("Certificate^^", ""));
            }

            List<DeleteItem> notExistingParts = new List<DeleteItem>();

            foreach (var itemSCE in sceItemsExport)
            {
                bool flagFound = false;

                foreach (var itemFTP1 in dataFTPFile)
                {
                    if (itemSCE.PartNumber == itemFTP1.Id.TrimEnd() && itemSCE.Supplier == supplier)
                    {
                        flagFound = true;
                        break;
                    }
                }

                if (flagFound == false && itemSCE.Supplier == supplier)
                {
                    InternalUse intlUse = new InternalUse();
                    intlUse.Action = "update";
                    intlUse.PartNumber = itemSCE.PartNumber;
                    intlUse.ProdId = itemSCE.Prodid;
                    intlUse.ProductType = itemSCE.ProductType;
                    intlUse.InternalUseValue = "1";

                    internalUse.Add(intlUse);

                    InventoryQty qty = new InventoryQty();
                    DeleteItem itemDel = new DeleteItem();
                    itemDel.PartNumber = itemSCE.PartNumber;
                    itemDel.ProdId = itemSCE.Prodid;
                    itemDel.Brand = itemSCE.Brand;
                    itemDel.ProductType = itemSCE.ProductType;
                    qty.Quantity = 0;
                    qty.PartNumber = itemSCE.PartNumber;
                    quantityFTP.Add(qty);
                }
            }

            notExistingParts = notExistingParts.Distinct(new PartNumberProdIdEqualityComparer()).ToList();

            foreach (var itemNoExs in notExistingParts)
            {
                ExtWareInfo wi = new ExtWareInfo();

                wi.Action = "removeall";
                wi.PartNumberItem = itemNoExs.PartNumber;
                wi.ProductId = itemNoExs.ProdId;
                wi.ProductType = itemNoExs.ProductType;
            }



            pqi.Processed = true;
            MessagePrinter.PrintMessage("Leo Schachter Diamonds processed");
            StartOrPushPropertiesThread();
        }



        /// <summary>
        /// ----------------------------------------------------------ProcessDownloadSaharAtid---------------------------------------------------------
        /// </summary>




        private void ProcessDownloadSaharAtid(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            const string actionAdd = "add";
            const string brand = "Marks Jewelers";
            const string productType = "1";
            const string mainCategory = "Diamonds";
            const string subCategory = "Diamonds";
            const string processingTime = "1";
            const string shippingType = "dynamic";
            const string shippingCarrier1 = "FedEx";
            const string allowground = "1";
            const string allow3day = "1";
            const string allow2day = "1";
            const string allownextday = "1";
            const string liveInventory = "1";
            const string supplier = "Sahar Atid";
            const string reOrderSupplier = "Sahar Atid";
            const string itemWeight = "1";
            const string itemHeight = "1";
            const string itemWidth = "1";
            const string itemLength = "1";
            const string shippingWeight = "1";
            const string shippingHeight = "1";
            const string shippingWidth = "1";
            const string shippingLength = "1";
            const string chekoutRule = "3";
            double length;
            double width;
            double height;

            MessagePrinter.PrintMessage("SaharAtid processing");
            MessagePrinter.PrintMessage("Download file from Sahar Atid");

            string downloadFTPFile = DownloadFTPFile("SaharAtid@efilestorage.com", "SaharAtid123!", "ftp://efilestorage.com/DIAMOND-LIST.csv", "SaharAtid_Data.csv");
            if (string.IsNullOrEmpty(downloadFTPFile))
            {
                MessagePrinter.PrintMessage("File from FTP - not downloaded", ImportanceLevel.High);

                return;
            }

            List<SaharAtid> dataFTPFile = ReadSceExportSaharAtid(downloadFTPFile);
            if (File.Exists(downloadFTPFile))
            {
                File.Delete(downloadFTPFile);
            }

            List<SaharAtid> itemsForRemoveByShape = new List<SaharAtid>();
            foreach (var diamond in dataFTPFile)
            {
                var sh = diamond.ShapeLong(diamond.Shape.TrimEnd());
                if (string.IsNullOrEmpty(sh))
                {
                    itemsForRemoveByShape.Add(diamond);
                }
            }

            foreach (SaharAtid ftp1 in itemsForRemoveByShape)
            {
                dataFTPFile.Remove(ftp1);
            }

            if (dataFTPFile != null)
            {
                MessagePrinter.PrintMessage("Parsing FTP complete");
            }
            MessagePrinter.PrintMessage("Compare Data");

            foreach (var itemFTP5 in dataFTPFile)
            {
                bool ftpItemFound = false;
                ExtWareInfo product = new ExtWareInfo();
                InventoryQty qty = new InventoryQty();
                foreach (var itemSCE in sceItemsExport)
                {
                    product = new ExtWareInfo();
                    product.Action = actionAdd;
                    product.ProductType = productType;
                    product.CheckoutRule = chekoutRule;

                    product.CustomHtmlBelowPrice = "<p class=\"wrap-appoint-btn\"> <img src=\"/files//marks_2/appointment_button2.png\" data-appointlet-organization=\"marks-jewelers\" data-appointlet-field-sku=\"" + itemFTP5.Stock.TrimEnd() + "\" class=\"appointment_button\"> </p>";

                    product.CustomHtmlAbovePrice = _сustomHtmlAbovePrice;
                    product.ProductTitle = String.Format("{0:0.00}", itemFTP5.Carat) + " Carat " + itemFTP5.CutGradeLong(itemFTP5.Cut.TrimEnd()) + itemFTP5.ShapeLong(itemFTP5.Shape.TrimEnd()) + " Diamond | " +
                        itemFTP5.Stock.TrimEnd() + " | Marks Jewelers";
                    product.AnchorText = String.Format("{0:0.00}", itemFTP5.Carat) + " Carat " + itemFTP5.CutGradeLong(itemFTP5.Cut.TrimEnd()) + itemFTP5.ShapeLong(itemFTP5.Shape.TrimEnd()) + " Diamond";
                    product.SpiderURL = (String.Format("{0:0.00}", itemFTP5.Carat) + "-Carat-" + itemFTP5.CutGradeLongSpiderURLSahar(itemFTP5.Cut.TrimEnd()) + "-" + itemFTP5.ShapeLong(itemFTP5.Shape.TrimEnd()) + "-Diamond-" + itemFTP5.Stock + "-S01").Replace("--", "-");
                    product.ScreenTitle = String.Format("{0:0.00}", itemFTP5.Carat) + " Carat " + itemFTP5.CutGradeLong(itemFTP5.Cut.TrimEnd()) + itemFTP5.ShapeLong(itemFTP5.Shape.TrimEnd()) + " Diamond";
                    product.SubTitle = Metods.ColorHardCodeSaharAtid(itemFTP5.Color.TrimEnd()) + Metods.ClarityHardCode(itemFTP5.Clarity.TrimEnd()) + " Clarity";
                    product.BrandItem = brand;
                    product.DescriptionItem = "This is " + String.Format("{0:0.00}", itemFTP5.Carat) + " Carat " + itemFTP5.CutGradeLong(itemFTP5.Cut.TrimEnd()) + itemFTP5.ShapeLong(itemFTP5.Shape.TrimEnd()) + " Diamond has " +
                            itemFTP5.SymmetryLong(itemFTP5.Symmetry.TrimEnd()) + " Symmetry, " + itemFTP5.PolishLong(itemFTP5.Polish.TrimEnd()) + " Polish and " + itemFTP5.Lab.TrimEnd() + " Certificate";
                    product.DescriptionItem += DescriptionParagraph;

                    product.METADescription = "This is " + String.Format("{0:0.00}", itemFTP5.Carat) + " Carat " + itemFTP5.CutGradeLong(itemFTP5.Cut.TrimEnd()) + itemFTP5.ShapeLong(itemFTP5.Shape.TrimEnd()) + " Diamond has " +
                            itemFTP5.SymmetryLong(itemFTP5.Symmetry.TrimEnd()) + " Symmetry, " + itemFTP5.PolishLong(itemFTP5.Polish.TrimEnd()) + " Polish and " + itemFTP5.Lab.TrimEnd() + " Certificate";
                    product.MainCategory = mainCategory;
                    product.SubCategory = subCategory;
                    ImageMaker subCatSahar = new ImageMaker();
                    product.CrossSellMainCat1 = "Diamonds by Shape";
                    product.CrossSellSubCat1 = subCatSahar.ShapeShare(itemFTP5.Shape.TrimEnd());

                    string measurements = itemFTP5.Measurements.TrimEnd();
                    string[] tokens = measurements.Split('x');
                    length = double.Parse(tokens[0]);
                    width = double.Parse(tokens[1]);
                    height = double.Parse(tokens[2]);


                    product.GeneralImage = subCatSahar.ImageMake(itemFTP5.Shape.TrimEnd());



                    product.PanoramImage = itemFTP5.DiamondImage;

                    if (string.IsNullOrEmpty(itemFTP5.DiamondImage))
                    {
                        product.GeneralImage = subCatSahar.ImageMake(itemFTP5.Shape.TrimEnd());

                    }
                    else
                    {
                        product.GeneralImage = product.GeneralImage + "," + itemFTP5.DiamondImage.Replace("scanresult.html", "cam1/cam1_normal_001.jpg") + "," + itemFTP5.DiamondImage.Replace("scanresult.html", "cam1/cam1_normal_038.jpg")
                        + "," + itemFTP5.DiamondImage.Replace("scanresult.html", "cam1/cam1_normal_075.jpg");
                    }
                    product.Specifications = "Specifications##Shape~" + itemFTP5.ShapeLong(itemFTP5.Shape.TrimEnd()) + "^Carat~" + String.Format("{0:0.00}", itemFTP5.Carat) +
                                   itemFTP5.ColorLongSpecification(itemFTP5.Color.Trim()) + "^Clarity~" + Metods.ClarityHardCode(itemFTP5.Clarity.Trim()) + itemFTP5.CutGradeLongSpecification(itemFTP5.Cut.TrimEnd()) +
                                   "^Polish~" + itemFTP5.PolishLong(itemFTP5.Polish.Trim()) + "^Symmetry~" + itemFTP5.SymmetryLong(itemFTP5.Symmetry.Trim()) +
                                   "^Fluorescence~" + itemFTP5.FluorescenceLong(itemFTP5.FluorescenceIntensity.TrimEnd()) + "^Lab Name~" + itemFTP5.Lab.TrimEnd() +
                                   "^Certificate No~" + itemFTP5.CertificateNumber.TrimEnd() + "^Total Depth~" + itemFTP5.DepthPr.TrimEnd() +
                                   "^Table Size~" + itemFTP5.TablePr.Trim() + "^Length~" + length + "^Width~" + width + "^Height~" + height + "^L/W Ratio~" + Math.Round((length / width), 2);
                    product.ProcessingTime = processingTime;
                    product.ShippingType = shippingType;
                    product.ShippingCarrier1 = shippingCarrier1;
                    product.Allowground = allowground;
                    product.Allow3day = allow3day;
                    product.Allow2day = allow2day;
                    product.Allownextday = allownextday;
                    product.LiveInventory = liveInventory;
                    product.Supplier = supplier;
                    product.ReOrderSupplier = reOrderSupplier;

                    product.FileAttachments = "Certificate^^http://dtol-cert-images.s3-website-us-east-1.amazonaws.com/GIA_pdf/" + itemFTP5.CertificateNumber.TrimEnd() + ".pdf";
                    product.PartNumberItem = itemFTP5.Stock.TrimEnd();
                    qty.PartNumber = product.PartNumberItem;
                    product.CostPrice = itemFTP5.PricePerCarat * itemFTP5.Carat;
                    product.MSRPItem = product.CostPrice / 0.77;
                    product.JobberItem = product.CostPrice / 0.77;
                    product.WebPrice = product.CostPrice / 0.77;



                    product.CrossSellSecCat1 = subCatSahar.CostRange(product.WebPrice);
                    product.ItemWeight = itemWeight;
                    product.ItemHeight = itemHeight;
                    product.ItemWidth = itemWidth;
                    product.ItemLength = itemLength;
                    product.ShippingWeight = shippingWeight;
                    product.ShippingHeight = shippingHeight;
                    product.ShippingWidth = shippingWidth;
                    product.ShippingLength = shippingLength;

                    if (itemSCE.PartNumber == itemFTP5.Stock.TrimEnd() && itemSCE.Brand == brand && itemSCE.Supplier == supplier)
                    {
                        product.Action = "update";
                        product.ProductId = itemSCE.Prodid;
                        product.InternalUse = "0";
                        qty.Quantity = 1;
                        quantityFTP.Add(qty);
                        ftpItemFound = true;

                        ftp5Collection.Add(product);
                    }

                }
                if (!ftpItemFound)
                {
                    product.Action = "add";
                    product.InternalUse = "0";
                    qty.Quantity = 1;
                    quantityFTP.Add(qty);

                    ftp5Collection.Add(product);
                }

            }
            foreach (var trueLink in ftp5Collection)
            {
                trueLink.FileAttachments = UrlCheck(trueLink.FileAttachments.Replace("Certificate^^", ""));
            }

            List<DeleteItem> notExistingParts = new List<DeleteItem>();
            foreach (var itemSCE in sceItemsExport)
            {
                bool flagFound = false;

                foreach (var itemFTP5 in dataFTPFile)
                {
                    if (itemSCE.PartNumber == itemFTP5.Stock.TrimEnd() && itemSCE.Supplier == supplier)
                    {
                        flagFound = true;
                        break;
                    }
                }

                if (flagFound == false && itemSCE.Supplier == supplier)
                {
                    InternalUse intlUse = new InternalUse();
                    intlUse.Action = "update";
                    intlUse.PartNumber = itemSCE.PartNumber;
                    intlUse.ProdId = itemSCE.Prodid;
                    intlUse.ProductType = itemSCE.ProductType;
                    intlUse.InternalUseValue = "1";

                    internalUse.Add(intlUse);

                    DeleteItem item = new DeleteItem();
                    InventoryQty qty = new InventoryQty();
                    item.PartNumber = itemSCE.PartNumber;
                    item.ProdId = itemSCE.Prodid;
                    item.Brand = itemSCE.Brand;
                    item.ProductType = itemSCE.ProductType;
                    qty.Quantity = 0;
                    qty.PartNumber = itemSCE.PartNumber;
                    quantityFTP.Add(qty);
                }
            }

            notExistingParts = notExistingParts.Distinct(new PartNumberProdIdEqualityComparer()).ToList();

            foreach (var item in notExistingParts)
            {
                ExtWareInfo wi = new ExtWareInfo();

                wi.Action = "removeall";
                wi.PartNumberItem = item.PartNumber;
                wi.ProductId = item.ProdId;
                wi.ProductType = item.ProductType;

                //ftp5Collection.Add(wi);
            }


            pqi.Processed = true;
            MessagePrinter.PrintMessage("SaharAtid processed");
            StartOrPushPropertiesThread();
        }



        /// <summary>
        /// ----------------------------------------------------------ProcessDownloadRDITrading-------------------------------------------------------
        /// </summary>





        private void ProcessDownloadRDITrading(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            const string actionAdd = "add";
            const string brand = "Marks Jewelers";
            const string productType = "1";
            const string mainCategory = "Diamonds";
            const string subCategory = "Diamonds";
            const string processingTime = "1";
            const string shippingType = "dynamic";
            const string shippingCarrier1 = "FedEx";
            const string allowground = "1";
            const string allow3day = "1";
            const string allow2day = "1";
            const string allownextday = "1";
            const string liveInventory = "1";
            const string supplier = "RDI Trading";
            const string reOrderSupplier = "RDI Trading";
            const string itemWeight = "1";
            const string itemHeight = "1";
            const string itemWidth = "1";
            const string itemLength = "1";
            const string shippingWeight = "1";
            const string shippingHeight = "1";
            const string shippingWidth = "1";
            const string shippingLength = "1";
            const string chekoutRule = "3";

            MessagePrinter.PrintMessage("RDI Trading processing");

            MessagePrinter.PrintMessage("Download file from RDI Trading");

            string downloadFTPFile = DownloadFTPFile("RDITrading@efilestorage.com", "RDITrading123!", "ftp://efilestorage.com/diamonds.csv", "RDI_Trading_Data.csv");
            if (string.IsNullOrEmpty(downloadFTPFile))
            {
                MessagePrinter.PrintMessage("File from FTP - not downloaded", ImportanceLevel.High);

                return;
            }

            string downloadFTPFileDallasOffice = DownloadFTPFile("dallas_mj@efilestorage.com", "dallas_mj123!", "ftp://efilestorage.com/1304-diamonds.csv", "1304-diamonds.csv");

            if (string.IsNullOrEmpty(downloadFTPFileDallasOffice))
            {
                MessagePrinter.PrintMessage("File from FTP - not downloaded", ImportanceLevel.High);

                return;
            }

            List<RDITrading> dataFTPFile = ReadSceExportRDITrading(downloadFTPFile);
            if (File.Exists(downloadFTPFile))
            {
                File.Delete(downloadFTPFile);
            }

            List<RDITrading> dataFTPFileDallasOffice = ReadSceExportRDITrading(downloadFTPFileDallasOffice);

            if (File.Exists(downloadFTPFileDallasOffice))
            {
                File.Delete(downloadFTPFileDallasOffice);
            }

            foreach (var item in dataFTPFileDallasOffice)
            {
                dataFTPFile.Add(item);
            }

            List<RDITrading> itemsForRemoveByShape = new List<RDITrading>();
            foreach (var diamond in dataFTPFile)
            {
                var sh = diamond.ShapeLong(diamond.Shape.TrimEnd());
                if (string.IsNullOrEmpty(sh))
                {
                    itemsForRemoveByShape.Add(diamond);
                }
            }

            foreach (RDITrading ftp1 in itemsForRemoveByShape)
            {
                dataFTPFile.Remove(ftp1);
            }

            if (dataFTPFile != null)
            {
                MessagePrinter.PrintMessage("Parsing FTP complete");
            }
            MessagePrinter.PrintMessage("Compare Data");

            foreach (var itemFTP4 in dataFTPFile)
            {
                bool ftpItemFound = false;
                ExtWareInfo product = new ExtWareInfo();
                InventoryQty qty = new InventoryQty();

                foreach (var itemSCE in sceItemsExport)
                {
                    product = new ExtWareInfo();
                    product.Action = actionAdd;
                    product.ProductType = productType;
                    product.CheckoutRule = chekoutRule;

                    product.CustomHtmlBelowPrice = "<p class=\"wrap-appoint-btn\"> <img src=\"/files//marks_2/appointment_button2.png\" data-appointlet-organization=\"marks-jewelers\" data-appointlet-field-sku=\"" + itemFTP4.VendorStockNumber.TrimEnd() + "\" class=\"appointment_button\"> </p>";
                    product.CustomHtmlAbovePrice = _сustomHtmlAbovePrice;

                    product.ProductTitle = String.Format("{0:0.00}", itemFTP4.Wieght) + " Carat " + itemFTP4.CutGradeLong(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + " Diamond | " +
                        itemFTP4.VendorStockNumber.TrimEnd() + " | Marks Jewelers";
                    product.AnchorText = String.Format("{0:0.00}", itemFTP4.Wieght) + " Carat " + itemFTP4.CutGradeLong(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + " Diamond";
                    product.SpiderURL = (String.Format("{0:0.00}", itemFTP4.Wieght) + "-Carat-" + itemFTP4.CutGradeLongSpyderURLRDI(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + "-" + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + "-Diamond-" + itemFTP4.VendorStockNumber.TrimEnd() + "-R02").Replace("--", "-");
                    product.ScreenTitle = String.Format("{0:0.00}", itemFTP4.Wieght) + " Carat " + itemFTP4.CutGradeLong(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + " Diamond";
                    product.SubTitle = Metods.ColorHardCode(itemFTP4.Color.TrimEnd()) + " Color " + Metods.ClarityHardCode(itemFTP4.Clarity.TrimEnd()) + " Clarity";
                    product.BrandItem = brand;
                    if ((itemFTP4.SymmetryLong(itemFTP4.Symmetry.TrimEnd())) == "" && itemFTP4.PolishLong(itemFTP4.Polish.TrimEnd()) == "" && itemFTP4.Lab.TrimEnd() == "")
                    {
                        product.DescriptionItem = "This is " + String.Format("{0:0.00}", itemFTP4.Wieght) + " Carat " + itemFTP4.CutGradeLong(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + " Diamond";
                    }
                    else
                    {
                        product.DescriptionItem = "This is " + String.Format("{0:0.00}", itemFTP4.Wieght) + " Carat " + itemFTP4.CutGradeLong(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + " Diamond has " +
                                itemFTP4.SymmetryLong(itemFTP4.Symmetry.TrimEnd()) + itemFTP4.PolishLong(itemFTP4.Polish.TrimEnd()) + itemFTP4.Lab.TrimEnd() + " Certificate";
                    }
                    product.DescriptionItem += DescriptionParagraph;

                    if ((itemFTP4.SymmetryLong(itemFTP4.Symmetry.TrimEnd())) == "" && itemFTP4.PolishLong(itemFTP4.Polish.TrimEnd()) == "" && itemFTP4.Lab.TrimEnd() == "")
                    {
                        product.METADescription = "This is " + String.Format("{0:0.00}", itemFTP4.Wieght) + " Carat " + itemFTP4.CutGradeLong(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + " Diamond";
                    }
                    else
                    {
                        product.METADescription = "This is " + String.Format("{0:0.00}", itemFTP4.Wieght) + " Carat " + itemFTP4.CutGradeLong(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + " Diamond has " +
                                itemFTP4.SymmetryLong(itemFTP4.Symmetry.TrimEnd()) + itemFTP4.PolishLong(itemFTP4.Polish.TrimEnd()) + itemFTP4.Lab.TrimEnd() + " Certificate";
                    }

                    product.MainCategory = mainCategory;
                    product.SubCategory = subCategory;
                    ImageMaker subCatRDI = new ImageMaker();
                    product.CrossSellMainCat1 = "Diamonds by Shape";
                    product.CrossSellSubCat1 = subCatRDI.ShapeShare(itemFTP4.Shape.TrimEnd());
                    product.GeneralImage = subCatRDI.ImageMake(itemFTP4.Shape.TrimEnd());



                    product.Specifications = "Specifications##Shape~" + itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd()) + "^Carat~" + String.Format("{0:0.00}", itemFTP4.Wieght) +
                                       "^Color~" + Metods.ColorHardCode(itemFTP4.Color.TrimEnd()) + "^Clarity~" + Metods.ClarityHardCode(itemFTP4.Clarity.TrimEnd()) + itemFTP4.CutGradeLongSpecification(itemFTP4.CutGrade.TrimEnd(), itemFTP4.ShapeLong(itemFTP4.Shape.TrimEnd())) +
                                       itemFTP4.PolishLongSpecification(itemFTP4.Polish.Trim()) + itemFTP4.SymmetryLongSpecification(itemFTP4.Symmetry.Trim()) +
                                       itemFTP4.FluorescenceLong(itemFTP4.FluorescenceIntensit.TrimEnd()) + itemFTP4.LabLong(itemFTP4.Lab.TrimEnd()) +
                                       itemFTP4.CertificateLong(itemFTP4.Certificate.TrimEnd()) + itemFTP4.DepthPrLong(itemFTP4.DepthPr.TrimEnd()) +
                                       itemFTP4.TablePrLong(itemFTP4.TablePr.Trim()) + "^Length~" + itemFTP4.Length + "^Width~" + itemFTP4.Width + "^Height~" + itemFTP4.Depth + "^L/W Ratio~" + Math.Round((itemFTP4.Length / itemFTP4.Width), 2);
                    product.ProcessingTime = processingTime;
                    product.ShippingType = shippingType;
                    product.ShippingCarrier1 = shippingCarrier1;
                    product.Allowground = allowground;
                    product.Allow3day = allow3day;
                    product.Allow2day = allow2day;
                    product.Allownextday = allownextday;
                    product.LiveInventory = liveInventory;
                    product.Supplier = supplier;
                    product.ReOrderSupplier = reOrderSupplier;

                    if (!string.IsNullOrWhiteSpace(itemFTP4.CertificateImage) && !string.IsNullOrEmpty(itemFTP4.CertificateImage))
                    {
                        product.FileAttachments = "Certificate^^" + itemFTP4.CertificateImage.TrimEnd();
                    }
                    else product.FileAttachments = "";


                    product.PartNumberItem = itemFTP4.VendorStockNumber.TrimEnd();
                    qty.PartNumber = product.PartNumberItem;
                    product.MSRPItem = itemFTP4.Price / 0.77;
                    product.JobberItem = itemFTP4.Price / 0.77;
                    product.WebPrice = itemFTP4.Price / 0.77;
                    product.CostPrice = itemFTP4.Price;
                    product.CrossSellSecCat1 = subCatRDI.CostRange(product.WebPrice);
                    product.ItemWeight = itemWeight;
                    product.ItemHeight = itemHeight;
                    product.ItemWidth = itemWidth;
                    product.ItemLength = itemLength;
                    product.ShippingWeight = shippingWeight;
                    product.ShippingHeight = shippingHeight;
                    product.ShippingWidth = shippingWidth;
                    product.ShippingLength = shippingLength;
                    if (itemSCE.PartNumber == itemFTP4.VendorStockNumber.TrimEnd() && itemSCE.Brand == brand && itemSCE.Supplier == supplier)
                    {
                        product.Action = "update";
                        product.ProductId = itemSCE.Prodid;
                        product.InternalUse = "0";
                        qty.Quantity = 1;
                        quantityFTP.Add(qty);
                        ftpItemFound = true;

                        ftp4Collection.Add(product);
                    }

                }
                if (!ftpItemFound)
                {
                    product.Action = "add";
                    product.InternalUse = "0";
                    qty.Quantity = 1;
                    quantityFTP.Add(qty);
                    ftp4Collection.Add(product);
                }

            }
            List<DeleteItem> notExistingParts = new List<DeleteItem>();
            foreach (var itemSCE in sceItemsExport)
            {
                bool flagFound = false;

                foreach (var itemFTP4 in dataFTPFile)
                {
                    if (itemSCE.PartNumber == itemFTP4.VendorStockNumber.TrimEnd() && itemSCE.Supplier == supplier)
                    {
                        flagFound = true;
                        break;
                    }
                }




                if (flagFound == false && itemSCE.Supplier == supplier)
                {
                    InternalUse intlUse = new InternalUse();
                    intlUse.Action = "update";
                    intlUse.PartNumber = itemSCE.PartNumber;
                    intlUse.ProdId = itemSCE.Prodid;
                    intlUse.ProductType = itemSCE.ProductType;
                    intlUse.InternalUseValue = "1";

                    internalUse.Add(intlUse);

                    DeleteItem item = new DeleteItem();
                    item.PartNumber = itemSCE.PartNumber;
                    item.ProdId = itemSCE.Prodid;
                    item.Brand = itemSCE.Brand;
                    item.ProductType = itemSCE.ProductType;

                    InventoryQty qty = new InventoryQty();
                    qty.Quantity = 0;
                    qty.PartNumber = itemSCE.PartNumber;
                    quantityFTP.Add(qty);
                }
            }

            notExistingParts = notExistingParts.Distinct(new PartNumberProdIdEqualityComparer()).ToList();

            foreach (var item in notExistingParts)
            {
                ExtWareInfo wi = new ExtWareInfo();

                wi.Action = "removeall";
                wi.PartNumberItem = item.PartNumber;
                wi.ProductId = item.ProdId;
                wi.ProductType = item.ProductType;
            }


            pqi.Processed = true;
            MessagePrinter.PrintMessage("Rdi Trading processed");
            StartOrPushPropertiesThread();
        }

        /// <summary>
        /// --------------------------------------------------------ProcessDownloadRARiamGroup---------------------------------------------------------------
        /// </summary>-
        protected void ProcessDownloadRARiamGroup(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            const string actionAdd = "add";
            const string actionUpdate = "update";
            const string brand = "Marks Jewelers";
            const string productType = "1";
            const string mainCategory = "Diamonds";
            const string subCategory = "Diamonds";
            const string processingTime = "1";
            const string shippingType = "dynamic";
            const string shippingCarrier1 = "FedEx";
            const string allowground = "1";
            const string allow3day = "1";
            const string allow2day = "1";
            const string allownextday = "1";
            const string liveInventory = "1";
            const string supplier = "RA Riam Group";
            const string reOrderSupplier = "RA Riam Group";
            const string itemWeight = "1";
            const string itemHeight = "1";
            const string itemWidth = "1";
            const string itemLength = "1";
            const string shippingWeight = "1";
            const string shippingHeight = "1";
            const string shippingWidth = "1";
            const string shippingLength = "1";
            const string chekoutRule = "3";


            MessagePrinter.PrintMessage("RA Riam Group processing");

            MessagePrinter.PrintMessage("Download file from RARiamGroup FTP");
            string filePathFTP1 = DownloadFTPFile("RARiamGroup@efilestorage.com", "RARGroup123!", "ftp://efilestorage.com/ALTR%20FEED%20MARKS.CSV", "ALTR FEED MARKS.csv");

            if (string.IsNullOrEmpty(filePathFTP1))
            {
                MessagePrinter.PrintMessage("File from FTP - not downloaded", ImportanceLevel.High);

                return;
            }


            List<ExtWareInfoFTP1> ftpItemsFTP1 = ReadSceExportFTP1(filePathFTP1);
            if (File.Exists(filePathFTP1))
            {
                File.Delete(filePathFTP1);
            }

            List<ExtWareInfoFTP1> itemsForRemoveByShape = new List<ExtWareInfoFTP1>();
            foreach (var diamond in ftpItemsFTP1)
            {
                var sh = diamond.ShapeLong(diamond.Shape.TrimEnd());
                if (string.IsNullOrEmpty(sh))
                {
                    itemsForRemoveByShape.Add(diamond);
                }
            }

            foreach (ExtWareInfoFTP1 ftp1 in itemsForRemoveByShape)
            {
                ftpItemsFTP1.Remove(ftp1);
            }

            if (ftpItemsFTP1 != null)
            {
                MessagePrinter.PrintMessage("Parsing FTP complete");
            }
            MessagePrinter.PrintMessage("Compare Data");


            foreach (var itemFTP1 in ftpItemsFTP1)
            {
                if (itemFTP1.Shape.TrimEnd().ToLower() == "rose" || itemFTP1.Shape.TrimEnd().ToLower() == "tr" || itemFTP1.Shape.TrimEnd().ToLower() == "r111" || itemFTP1.Shape.TrimEnd().ToLower() == "oct")
                {
                    continue;
                }
                if (Metods.ColorHardCodeRARIAM(itemFTP1.Color.TrimEnd()).Contains("not much"))
                {
                    continue;
                }
                else
                {

                    bool ftpItemFound = false;
                    ExtWareInfo product = new ExtWareInfo();
                    InventoryQty qty = new InventoryQty();
                    foreach (var itemSCE in sceItemsExport)
                    {
                        product = new ExtWareInfo();

                        product.Action = actionAdd;
                        product.ProductType = productType;
                        product.CheckoutRule = chekoutRule;

                        product.CustomHtmlBelowPrice = "<p class=\"wrap-appoint-btn\"> <img src=\"/files//marks_2/appointment_button2.png\" data-appointlet-organization=\"marks-jewelers\" data-appointlet-field-sku=\"" + itemFTP1.Lot.TrimEnd() + "\" class=\"appointment_button\"> </p>";

                        product.CustomHtmlAbovePrice = _сustomHtmlAbovePrice;

                        product.ProductTitle = String.Format("{0:0.00}", itemFTP1.Weight) + " Carat " + itemFTP1.CutGradeLong(itemFTP1.CutGrade.TrimEnd()) +
                            itemFTP1.ShapeLong(itemFTP1.Shape.TrimEnd()) + " Lab Created Diamond | " + itemFTP1.Lot.TrimEnd() + " | Marks Jewelers";
                        product.AnchorText = String.Format("{0:0.00}", itemFTP1.Weight) + " Carat " + itemFTP1.CutGradeLong(itemFTP1.CutGrade.TrimEnd()) +
                            itemFTP1.ShapeLong(itemFTP1.Shape.TrimEnd()) + " Lab Created Diamond";
                        product.SpiderURL = (String.Format("{0:0.00}", itemFTP1.Weight) + "-Carat-" + itemFTP1.CutGradeLongSpiderURLRARIAM(itemFTP1.CutGrade.TrimEnd()) + "-" +
                            itemFTP1.ShapeLong(itemFTP1.Shape.TrimEnd()) + "-Lab-Created-Diamond-" + itemFTP1.Lot.TrimEnd() + "-R01").Replace("--", "-");
                        product.ScreenTitle = String.Format("{0:0.00}", itemFTP1.Weight) + " Carat " + itemFTP1.CutGradeLong(itemFTP1.CutGrade.TrimEnd()) +
                            itemFTP1.ShapeLong(itemFTP1.Shape.TrimEnd()) + " Lab Created Diamond";


                        product.SubTitle = Metods.ColorHardCode(itemFTP1.Color.TrimEnd()) + " Color, " + Metods.ClarityHardCode(itemFTP1.Clarity.TrimEnd()) + " Clarity";
                        product.BrandItem = brand;
                        product.DescriptionItem = "This is " + String.Format("{0:0.00}", itemFTP1.Weight) + " Carat " + itemFTP1.CutGradeLong(itemFTP1.CutGrade.TrimEnd()) +
                            itemFTP1.ShapeLong(itemFTP1.Shape.TrimEnd()) +
                            " Diamond" + " has " + itemFTP1.SymmetryLong(itemFTP1.Symmetry.Trim()) + " Symmetry, " + itemFTP1.PolishLong(itemFTP1.Polish.Trim()) + " Polish and " +
                            itemFTP1.Lab.TrimEnd() + " Certificate";
                        product.DescriptionItem += DescriptionParagraph;

                        product.METADescription = "This is " + String.Format("{0:0.00}", itemFTP1.Weight) + " Carat " + itemFTP1.CutGradeLong(itemFTP1.CutGrade.TrimEnd()) +
                            itemFTP1.ShapeLong(itemFTP1.Shape.TrimEnd()) +
                            " Diamond" + " has " + itemFTP1.SymmetryLong(itemFTP1.Symmetry.Trim()) + " Symmetry, " + itemFTP1.PolishLong(itemFTP1.Polish.Trim()) + " Polish and " +
                            itemFTP1.Lab.TrimEnd() + " Certificate";
                        product.MainCategory = mainCategory;
                        product.SubCategory = subCategory;
                        ImageMaker subCatRARIAM = new ImageMaker();
                        product.CrossSellMainCat1 = "Diamonds by Shape";
                        product.CrossSellSubCat1 = subCatRARIAM.ShapeShare(itemFTP1.Shape.TrimEnd());
                        product.GeneralImage = subCatRARIAM.ImageMake(itemFTP1.Shape.TrimEnd());


                        product.Specifications = "Specifications##Shape~" + itemFTP1.ShapeLong(itemFTP1.Shape.TrimEnd()) + "^Carat~" + String.Format("{0:0.00}", itemFTP1.Weight) +
                            "^Color~" + Metods.ColorHardCode(itemFTP1.Color.Trim()) + "^Clarity~" + Metods.ClarityHardCode(itemFTP1.Clarity.Trim()) + itemFTP1.CutGradeLongSpecification(itemFTP1.CutGrade.TrimEnd()) +
                            "^Polish~" + itemFTP1.PolishLong(itemFTP1.Polish.Trim()) + "^Symmetry~" + itemFTP1.SymmetryLong(itemFTP1.Symmetry.Trim()) +
                            "^Fluorescence~None^Lab Name~" + itemFTP1.Lab.Trim() + "^Certificate No~" + itemFTP1.Certificate.Trim() + "^Total Depth~" +
                            itemFTP1.DepthPr.Trim() + "^Table Size~" + itemFTP1.TablePr.Trim() + "^Length~" + itemFTP1.Length + "^Width~" + itemFTP1.Width + "^Height~" +
                            itemFTP1.Depth.Trim() + "^L/W Ratio~" + Math.Round((itemFTP1.Length) / (itemFTP1.Width), 2);
                        product.ProcessingTime = processingTime;
                        product.ShippingType = shippingType;
                        product.ShippingCarrier1 = shippingCarrier1;
                        product.Allowground = allowground;
                        product.Allow3day = allow3day;
                        product.Allow2day = allow2day;
                        product.Allownextday = allownextday;
                        product.LiveInventory = liveInventory;
                        product.Supplier = supplier;
                        product.ReOrderSupplier = reOrderSupplier;
                        product.FileAttachments = "";
                        try
                        {
                            if (itemFTP1.Lab == "IGI")
                            {
                                product.FileAttachments = "Certificate^^http://igionline.com/PDF/2017/" + itemFTP1.Certificate.TrimEnd() + ".pdf";
                            }
                            if (itemFTP1.Lab == "GCAL")
                            {
                                product.FileAttachments = "Certificate^^http://images.gemfacts.com/GCALimages/Certs/GCAL" + itemFTP1.Certificate.TrimEnd() + ".pdf";
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        product.PartNumberItem = itemFTP1.Lot.TrimEnd();
                        qty.PartNumber = product.PartNumberItem;
                        product.CostPrice = itemFTP1.PriceStone;
                        product.MSRPItem = product.CostPrice / 0.6;
                        product.JobberItem = product.CostPrice / 0.6;
                        product.WebPrice = product.CostPrice / 0.6;

                        product.CrossSellSecCat1 = subCatRARIAM.CostRange(product.WebPrice);
                        product.ItemWeight = itemWeight;
                        product.ItemHeight = itemHeight;
                        product.ItemWidth = itemWidth;
                        product.ItemLength = itemLength;
                        product.ShippingWeight = shippingWeight;
                        product.ShippingHeight = shippingHeight;
                        product.ShippingWidth = shippingWidth;
                        product.ShippingLength = shippingLength;

                        if (itemSCE.PartNumber == itemFTP1.Lot.TrimEnd() && itemSCE.Brand == brand && itemSCE.Supplier == supplier && itemFTP1.Shape.TrimEnd() != "Rose")
                        {
                            product.Action = "update";
                            product.ProductId = itemSCE.Prodid;
                            product.InternalUse = "0";
                            qty.Quantity = 1;
                            quantityFTP.Add(qty);
                            ftpItemFound = true;

                            RaRiamGroupCollection.Add(product);
                        }
                    }


                    if (!ftpItemFound && itemFTP1.Shape.TrimEnd() != "Rose")
                    {
                        product.Action = "add";
                        product.InternalUse = "0";
                        qty.Quantity = 1;
                        quantityFTP.Add(qty);
                        RaRiamGroupCollection.Add(product);
                    }
                }
            }
            foreach (var trueLink in RaRiamGroupCollection)
            {
                trueLink.FileAttachments = UrlCheckRARiam(trueLink.FileAttachments.Replace("Certificate^^", ""));
            }

            List<DeleteItem> notExistingParts = new List<DeleteItem>();
            foreach (var itemSCE in sceItemsExport)
            {
                bool flagFound = false;

                foreach (var itemFTP1 in ftpItemsFTP1)
                {
                    if (itemSCE.PartNumber == itemFTP1.Lot.TrimEnd() && itemSCE.Supplier == supplier)
                    {
                        flagFound = true;
                        break;
                    }
                }

                if (flagFound == false && itemSCE.Supplier == supplier)
                {
                    InternalUse intlUse = new InternalUse();
                    intlUse.Action = "update";
                    intlUse.PartNumber = itemSCE.PartNumber;
                    intlUse.ProdId = itemSCE.Prodid;
                    intlUse.ProductType = itemSCE.ProductType;
                    intlUse.InternalUseValue = "1";

                    internalUse.Add(intlUse);

                    InventoryQty qty = new InventoryQty();
                    DeleteItem item = new DeleteItem();
                    item.PartNumber = itemSCE.PartNumber;
                    item.ProdId = itemSCE.Prodid;
                    item.Brand = itemSCE.Brand;
                    item.ProductType = itemSCE.ProductType;
                    qty.Quantity = 0;
                    qty.PartNumber = itemSCE.PartNumber;
                    quantityFTP.Add(qty);
                }
            }

            notExistingParts = notExistingParts.Distinct(new PartNumberProdIdEqualityComparer()).ToList();

            foreach (var item in notExistingParts)
            {
                ExtWareInfo wi = new ExtWareInfo();

                wi.Action = "removeall";
                wi.PartNumberItem = item.PartNumber;
                wi.ProductId = item.ProdId;
                wi.ProductType = item.ProductType;
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("RA Riam Group processed");
            StartOrPushPropertiesThread();
        }

        public static List<ExtWareInfoFTP1> ReadSceExportFTP1(string filePath)
        {
            List<ExtWareInfoFTP1> ftp1 = new List<ExtWareInfoFTP1>();


            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        ExtWareInfoFTP1 item = new ExtWareInfoFTP1();
                        item.Lot = csv["Lot"];
                        item.Shape = csv["Shape"];
                        item.Weight = double.Parse(csv["Weight"]);
                        item.Color = csv["Color"];
                        item.Clarity = csv["Clarity"];
                        item.Length = double.Parse(csv["Length"]);
                        item.Width = double.Parse(csv["Width"]);
                        item.Depth = csv["Depth"];
                        item.CutGrade = csv["Cut Grade"];
                        item.Lab = csv["Lab"];
                        item.PriceStone = double.Parse(csv["Price/stone"]);
                        item.DepthPr = csv["Depth %"];
                        item.TablePr = csv["Table %"];
                        item.Polish = csv["Polish"];
                        item.Symmetry = csv["Symmetry"];
                        item.Certificate = csv["Certificate ."];


                        ftp1.Add(item);

                    }

                    return ftp1;
                }
            }
        }


        public class ExtWareInfoFTP1
        {
            public string Lot { get; set; }
            public string Shape { get; set; }
            public double Weight { get; set; }
            public string Color { get; set; }
            public string Clarity { get; set; }
            public double Length { get; set; }
            public double Width { get; set; }
            public string Depth { get; set; }
            public string CutGrade { get; set; }
            public string Lab { get; set; }
            public double PriceStone { get; set; }
            public string DepthPr { get; set; }
            public string TablePr { get; set; }
            public string Polish { get; set; }
            public string Symmetry { get; set; }
            public string Certificate { get; set; }
            public string ShapeLong(string name)
            {
                if (name.Equals("RD") || name.Equals("BR") || name.Equals("R89")
                    || name.Equals("Round") || name.Equals("R111") || name.Equals("Rose"))
                {
                    name = "Round";

                }
                else if (name.Equals("CU") || name.Equals("Cushion"))
                {
                    name = "Cushion";

                }
                else if (name.Equals("OV") || name.Equals("Oval"))
                {
                    name = "Oval";

                }
                else if (name.Equals("RAD") || name.Equals("Radiant") || name.Equals("RA"))
                {
                    name = "Radiant";

                }
                else if (name.Equals("AS") || name.Equals("Asscher"))
                {
                    name = "Asscher";

                }
                else if (name.Equals("HT") || name.Equals("Heart") || name.Equals("HS"))
                {
                    name = "Heart";
                }
                else if (name.Equals("PC") || name.Equals("Princess") || name.Equals("PR"))
                {
                    name = "Princess";

                }
                else if (name.Equals("EM") || name.Equals("EME") || name.Equals("Emerald"))
                {
                    name = "Emerald";

                }
                else if (name.Equals("PS") || name.Equals("Pear"))
                {
                    name = "Pear";

                }
                else if (name.Equals("MS") || name.Equals("MQ") || name.Equals("MR") || name.Equals("Marquise"))
                {
                    name = "Marquise";

                }
                else name = "";
                return name;
            }
            public string CutGradeLongSpecification(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Cut~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Cut~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Cut~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "^Cut~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Cut~Excellent";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || string.IsNullOrWhiteSpace(name) || name.Equals("R89"))
                {
                    name = "";
                }
                else name = "";
                return name;
            }

            public string CutGradeLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Cut ";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Cut ";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent Cut ";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair Cut ";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent Cut ";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("R89"))
                {
                    name = "";
                }
                else name = "";
                return name;
            }
            public string CutGradeLongSpiderURLRARIAM(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good-Cut";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very-Good-Cut";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent-Cut";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair-Cut";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent-Cut";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("R89"))
                {
                    name = "";
                }
                else name = "";
                return name;
            }

            public string PolishLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good";
                }
                else name = "";
                return name;
            }
            public string SymmetryLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Ideal";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good";
                }
                else name = "";
                return name;
            }
        }

        public string DownloadFTPFile(string login, string password, string filePath, string filename)
        {
            string fileName = filename;
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            try
            {
                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential(login, password);

                    byte[] fileData = request.DownloadData(filePath);

                    using (FileStream file = File.Create(inputFilePath))
                    {
                        file.Write(fileData, 0, fileData.Length);
                        file.Close();
                    }
                }

                return inputFilePath;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message, ImportanceLevel.Critical);
                return null;
            }
        }
        /// <summary>
        /// ----------------------------------------------------------ProcessDownloadRosyBlue---------------------------------------------------------
        /// </summary>
        private void ProcessDownloadRosyBlue(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            const string actionAdd = "add";
            const string actionRemove = "remove";
            const string actionUpdate = "update";
            const string brand = "Marks Jewelers";
            const string productType = "1";
            const string mainCategory = "Diamonds";
            const string subCategory = "Diamonds";
            const string processingTime = "1";
            const string shippingType = "dynamic";
            const string shippingCarrier1 = "FedEx";
            const string allowground = "1";
            const string allow3day = "1";
            const string allow2day = "1";
            const string allownextday = "1";
            const string liveInventory = "1";
            const string supplier = "Rosy Blue";
            const string reOrderSupplier = "Rosy Blue";
            const string itemWeight = "1";
            const string itemHeight = "1";
            const string itemWidth = "1";
            const string itemLength = "1";
            const string shippingWeight = "1";
            const string shippingHeight = "1";
            const string shippingWidth = "1";
            const string shippingLength = "1";
            const string chekoutRule = "3";
            double length;
            double width;
            double height;


            MessagePrinter.PrintMessage("Rosy Blue processing");

            MessagePrinter.PrintMessage("Download file from Rosy Blue");

            string downloadFTPFile = @"D:\Work\Projects\FTP FEED\FTP3 Rosy Blue\MarksJewelers_example.csv";
            if (string.IsNullOrEmpty(downloadFTPFile))
            {
                MessagePrinter.PrintMessage("File from FTP - not downloaded", ImportanceLevel.High);

                return;
            }

            List<RosyBlue> dataFTPFile = ReadSceExportRB(downloadFTPFile);
            if (dataFTPFile != null)
            {
                MessagePrinter.PrintMessage("Parsing FTP complete");
            }
            MessagePrinter.PrintMessage("Compare Data");


            foreach (var itemFTP3 in dataFTPFile)
            {
                bool ftpItemFound = false;
                ExtWareInfo product = new ExtWareInfo();
                foreach (var itemSCE in sceItemsExport)
                {
                    product = new ExtWareInfo();
                    product.Action = actionAdd;
                    product.ProductType = productType;
                    product.CheckoutRule = chekoutRule;

                    product.ProductTitle = String.Format("{0:0.00}", itemFTP3.Carat) + " Carat " + itemFTP3.CutGradeLong(itemFTP3.Cut.TrimEnd()) + itemFTP3.ShapeLong(itemFTP3.Shape.TrimEnd()) + " Diamond | " +
                        itemFTP3.VendorSku.TrimEnd() + " | Marks Jewelers";
                    product.AnchorText = String.Format("{0:0.00}", itemFTP3.Carat) + " Carat " + itemFTP3.CutGradeLong(itemFTP3.Cut.TrimEnd()) + itemFTP3.ShapeLong(itemFTP3.Shape.TrimEnd()) + " Diamond";
                    product.SpiderURL = String.Format("{0:0.00}", itemFTP3.Carat) + " Carat " + itemFTP3.CutGradeLong(itemFTP3.Cut.TrimEnd()) + itemFTP3.ShapeLong(itemFTP3.Shape.TrimEnd()) + " Diamond " + itemFTP3.VendorSku;
                    product.ScreenTitle = String.Format("{0:0.00}", itemFTP3.Carat) + " Carat " + itemFTP3.CutGradeLong(itemFTP3.Cut.TrimEnd()) + itemFTP3.ShapeLong(itemFTP3.Shape.TrimEnd()) + " Diamond";
                    product.SubTitle = Metods.ColorHardCode(itemFTP3.Color.TrimEnd()) + " Color " + Metods.ClarityHardCode(itemFTP3.Clarity.TrimEnd()) + " Clarity";
                    product.BrandItem = brand;
                    product.DescriptionItem = "This is " + String.Format("{0:0.00}", itemFTP3.Carat) + " Carat " + itemFTP3.CutGradeLong(itemFTP3.Cut.TrimEnd()) + itemFTP3.ShapeLong(itemFTP3.Shape.TrimEnd()) + " Diamond has " +
                        itemFTP3.SymmetryRosy(itemFTP3.Symmetry.TrimEnd()) + itemFTP3.PolishRosy(itemFTP3.Polish.TrimEnd());
                    product.DescriptionItem += DescriptionParagraph;

                    product.METADescription = "This is " + String.Format("{0:0.00}", itemFTP3.Carat) + " Carat " + itemFTP3.CutGradeLong(itemFTP3.Cut.TrimEnd()) + itemFTP3.ShapeLong(itemFTP3.Shape.TrimEnd()) + " Diamond has " +
                        itemFTP3.SymmetryRosy(itemFTP3.Symmetry.TrimEnd()) + itemFTP3.PolishRosy(itemFTP3.Polish.TrimEnd());
                    product.MainCategory = mainCategory;
                    product.SubCategory = subCategory;
                    ImageMaker subCatRosy = new ImageMaker();
                    product.CrossSellMainCat1 = "Diamonds by Shape";
                    product.CrossSellSubCat1 = subCatRosy.ShapeShare(itemFTP3.Shape.TrimEnd());

                    string measurements = itemFTP3.Measurements.TrimEnd();
                    string[] tokens = measurements.Split('-', 'x');
                    length = double.Parse(tokens[0]);
                    width = double.Parse(tokens[1]);
                    height = double.Parse(tokens[2]);

                    string diamondImage = itemFTP3.DiamondImage.TrimEnd();
                    string[] tokensDI = diamondImage.Split('=');


                    product.GeneralImage = subCatRosy.ImageMake(itemFTP3.Shape.TrimEnd());

                    product.Specifications = "Specifications##Shape~" + itemFTP3.ShapeLong(itemFTP3.Shape.TrimEnd()) + "^Carat~" + String.Format("{0:0.00}", itemFTP3.Carat) +
                               "^Color~" + Metods.ColorHardCode(itemFTP3.Color.Trim()) + "^Clarity~" + Metods.ClarityHardCode(itemFTP3.Clarity.Trim()) + itemFTP3.CutGradeLongSpecification(itemFTP3.Cut.TrimEnd()) +
                               itemFTP3.PolishLong(itemFTP3.Polish.Trim()) + itemFTP3.SymmetryLong(itemFTP3.Symmetry.Trim()) +
                               "^Fluorescence~" + itemFTP3.FluorescenceLong(itemFTP3.Flurescence.TrimEnd()) + itemFTP3.DepthRosyB(itemFTP3.Depth.TrimEnd()) +
                               itemFTP3.TableRosyB(itemFTP3.Table.Trim()) + "^Length~" + length + "^Width~" + width + "^Height~" + height + "^L/W Ratio~" + Math.Round((length / width), 2);
                    product.ProcessingTime = processingTime;
                    product.ShippingType = shippingType;
                    product.ShippingCarrier1 = shippingCarrier1;
                    product.Allowground = allowground;
                    product.Allow3day = allow3day;
                    product.Allow2day = allow2day;
                    product.Allownextday = allownextday;
                    product.LiveInventory = liveInventory;
                    product.Supplier = supplier;
                    product.ReOrderSupplier = reOrderSupplier;
                    product.PartNumberItem = Guid.NewGuid().ToString("N");
                    product.ManufacturerPartNumber = itemFTP3.VendorSku.TrimEnd();

                    product.CostPrice = itemFTP3.Price * itemFTP3.Carat;
                    product.MSRPItem = product.CostPrice / 0.77;
                    product.JobberItem = product.CostPrice / 0.77;
                    product.WebPrice = product.CostPrice / 0.77;
                    product.CrossSellSecCat1 = subCatRosy.CostRange(product.WebPrice);
                    product.ItemWeight = itemWeight;
                    product.ItemHeight = itemHeight;
                    product.ItemWidth = itemWidth;
                    product.ItemLength = itemLength;
                    product.ShippingWeight = shippingWeight;
                    product.ShippingHeight = shippingHeight;
                    product.ShippingWidth = shippingWidth;
                    product.ShippingLength = shippingLength;

                    if (itemSCE.PartNumber == itemFTP3.VendorSku.TrimEnd() && itemSCE.Brand == brand && itemSCE.Supplier == supplier)
                    {
                        product.Action = "update";
                        product.ProductId = itemSCE.Prodid;
                        product.InternalUse = "0";
                        ftpItemFound = true;

                        RosyBlueCollection.Add(product);
                    }

                }
                if (!ftpItemFound)
                {
                    product.Action = "add";
                    product.InternalUse = "0";
                    RosyBlueCollection.Add(product);
                }

            }
            List<DeleteItem> notExistingParts = new List<DeleteItem>();
            foreach (var itemSCE in sceItemsExport)
            {
                bool flagFound = false;

                foreach (var itemFTP3 in dataFTPFile)
                {
                    if (itemSCE.PartNumber == itemFTP3.VendorSku.TrimEnd() && itemSCE.Supplier == supplier)
                    {
                        flagFound = true;
                        break;
                    }
                }

                if (flagFound == false && itemSCE.Supplier == supplier)
                {
                    DeleteItem item = new DeleteItem();
                    item.PartNumber = itemSCE.PartNumber;
                    item.ProdId = itemSCE.Prodid;
                    item.Brand = itemSCE.Brand;
                    item.ProductType = itemSCE.ProductType;

                    notExistingParts.Add(item);
                }
            }

            notExistingParts = notExistingParts.Distinct(new PartNumberProdIdEqualityComparer()).ToList();

            foreach (var item in notExistingParts)
            {
                ExtWareInfo wi = new ExtWareInfo();

                wi.Action = "removeall";
                wi.PartNumberItem = item.PartNumber;
                wi.ProductId = item.ProdId;
                wi.ProductType = item.ProductType;

                RosyBlueCollection.Add(wi);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Rosy Blue processed");
            StartOrPushPropertiesThread();
        }

        private class DeleteItem
        {
            public string PartNumber { get; set; }
            public string ProdId { get; set; }
            public string Brand { get; set; }
            public string ProductType { get; set; }



        }

        private class PartNumberProdIdEqualityComparer : IEqualityComparer<DeleteItem>
        {
            public bool Equals(DeleteItem x, DeleteItem y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.PartNumber, y.PartNumber);
            }

            public int GetHashCode(DeleteItem obj)
            {
                unchecked
                {
                    return ((obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0) * 397);
                }
            }
        }

        public List<RosyBlue> ReadSceExportRB(string filePath)
        {
            List<RosyBlue> rosyBlue = new List<RosyBlue>();


            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    try
                    {

                        while (csv.ReadNextRecord())
                        {
                            RosyBlue item = new RosyBlue();
                            item.VendorSku = csv["Vendor Sku"];
                            item.Cert = csv["Cert"];
                            item.Lab = csv["Lab"];
                            item.Shape = csv["Shape"];
                            item.Carat = double.Parse(csv["Carat"]);
                            item.Color = csv["Color"];
                            item.Clarity = csv["Clarity"];
                            item.Cut = csv["Cut"];
                            item.Depth = csv["Depth"];
                            item.Table = csv["Table"];
                            item.Polish = csv["Polish"];
                            item.Symmetry = csv["Symmetry"];
                            item.Flurescence = csv["Flurescence"];
                            item.Culet = csv["Culet"];
                            item.Girdle = csv["Girdle"];
                            item.Measurements = csv["Measurements"];
                            item.Price = double.Parse(csv["Price"]);
                            item.Eyeclean = csv["Eyeclean"];
                            item.Milky = csv["Milky"];
                            item.Onmemo = csv["Onmemo"];
                            item.Inhand = csv["Inhand"];
                            item.DiamondVideo = csv["Diamond Video"];
                            item.DiamondImage = csv["Diamond Image"];


                            rosyBlue.Add(item);


                        }
                    }
                    catch (Exception e)
                    {
                        MessagePrinter.PrintMessage("Error" + e.Message);
                    }

                    return rosyBlue;
                }
            }
        }
        public class RosyBlue
        {
            public string VendorSku { get; set; }
            public string Cert { get; set; }
            public string Lab { get; set; }
            public string Shape { get; set; }
            public double Carat { get; set; }
            public string Color { get; set; }
            public string Clarity { get; set; }
            public string Cut { get; set; }
            public string Depth { get; set; }
            public string Table { get; set; }
            public string Polish { get; set; }
            public string Symmetry { get; set; }
            public string Flurescence { get; set; }
            public string Culet { get; set; }
            public string Girdle { get; set; }
            public string Measurements { get; set; }
            public double Price { get; set; }
            public string Eyeclean { get; set; }
            public string Milky { get; set; }
            public string Onmemo { get; set; }
            public string Inhand { get; set; }
            public string DiamondVideo { get; set; }
            public string DiamondImage { get; set; }
            public string ShapeLong(string name)
            {
                if (name.Equals("RD") || name.Equals("BR") || name.Equals("R89") || name.Equals("Round"))
                {
                    name = "Round";

                }
                else if (name.Equals("CU") || name.Equals("Cushion") || name.Equals("CMB") || name.Equals("CB") || name.TrimEnd('*').Equals("PB"))
                {
                    name = "Cushion";

                }
                else if (name.Equals("OV") || name.Equals("Oval") || name.TrimEnd('*').Equals("OB") || name.Equals("OB"))
                {
                    name = "Oval";

                }
                else if (name.Equals("RAD") || name.Equals("Radiant") || name.Equals("RA"))
                {
                    name = "Radiant";

                }
                else if (name.Equals("AS") || name.Equals("Asscher"))
                {
                    name = "Asscher";

                }
                else if (name.Equals("HT") || name.Equals("Heart"))
                {
                    name = "Heart";
                }
                else if (name.Equals("PC") || name.Equals("Princess") || name.Equals("PR"))
                {
                    name = "Princess";

                }
                else if (name.Equals("EM") || name.Equals("EME") || name.Equals("Emerald"))
                {
                    name = "Emerald";

                }
                else if (name.Equals("PS") || name.Equals("Pear") || name.Equals("PMB") || name.Equals("PB"))
                {
                    name = "Pear";

                }
                else if (name.Equals("MS") || name.Equals("MQ") || name.Equals("MR") || name.Equals("Marquise"))
                {
                    name = "Marquise";

                }
                else name = "";
                return name;
            }
            public string PolishRosy(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Polish";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Polish";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent Polish";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair Polish";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Ideal Polish";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good Polish";
                }
                else if (name.Equals(""))
                {
                    name = "";
                }
                else name = "";
                return name;
            }
            public string SymmetryRosy(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Symmetry, ";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Symmetry, ";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent Symmetry, ";
                }
                else if (name.Equals("FA") || name.Equals("FAIR") || name.Equals("F"))
                {
                    name = "Fair Symmetry, ";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL") || name.Equals("Ideal"))
                {
                    name = "Ideal Symmetry, ";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good Symmetry, ";
                }
                else name = "";
                return name;
            }
            public string CutGradeLongSpecification(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Cut~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Cut~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Cut~Ideal";
                }
                else if (name.Equals("FA") || name.Equals("FAIR") || name.Equals("F"))
                {
                    name = "^Cut~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Cut~Ideal";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }
                else name = "";
                return name;
            }

            public string CutGradeLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Cut ";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Cut ";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Ideal Cut ";
                }
                else if (name.Equals("FA") || name.Equals("FAIR") || name.Equals("F"))
                {
                    name = "Fair Cut ";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Ideal Cut ";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("") || string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }
                return name;
            }

            public string PolishLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Polish~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Polish~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Polish~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "^Polish~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Polish~Ideal";
                }
                else if (name.Equals("NA"))
                {
                    name = "^Polish~Good";
                }
                else name = "";
                return name;
            }
            public string SymmetryLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Symmetry~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Symmetry~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Symmetry~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "^Symmetry~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Symmetry~Ideal";
                }
                else if (name.Equals("NA"))
                {
                    name = "^Symmetry~Good";
                }
                else name = "";
                return name;
            }
            public string DepthRosyB(string name)
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name))
                {
                    return name = "^Total Depth~" + name;
                }
                else return "";
            }
            public string TableRosyB(string name)
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name))
                {
                    return name = "^Table Size~" + name;
                }
                else return "";
            }
            public string FluorescenceLong(string name)
            {
                if (name.Equals("NON") || name.Equals(""))
                {
                    name = "None";
                }
                else if (name.Equals("FNT"))
                {
                    name = "Faint";
                }
                else if (name.Equals("MED"))
                {
                    name = "Medium";
                }
                else if (name.Equals("STG"))
                {
                    name = "Strong Slight";
                }
                return name;
            }

        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 1)
                act = ProcessDownloadRARiamGroup;
            else if (item.ItemType == 2)
                act = ProcessDownloadRosyBlue;
            else if (item.ItemType == 3)
                act = ProcessDownloadRDITrading;
            else if (item.ItemType == 4)
                act = ProcessDownloadLSDiamonds;
            else if (item.ItemType == 5)
                act = ProcessDownloadSaharAtid;



            else act = null;

            return act;
        }



        public static List<ExtWareInfoSceExport> ReadSceExport(string filePath)
        {

            List<ExtWareInfoSceExport> sceExports = new List<ExtWareInfoSceExport>();
            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        ExtWareInfoSceExport itemSce = new ExtWareInfoSceExport();
                        itemSce.ProductType = csv["Product Type"];
                        itemSce.Prodid = csv["Prodid"];
                        itemSce.Brand = csv["Brand"];
                        itemSce.Supplier = csv["Supplier"];
                        itemSce.PartNumber = csv["Part Number"];

                        sceExports.Add(itemSce);
                    }
                    return sceExports;
                }
            }
        }

        public class ExtWareInfoSceExport
        {
            public class BrandPartNumberEqualityComparer : IEqualityComparer<ExtWareInfoSceExport>
            {
                public bool Equals(ExtWareInfoSceExport x, ExtWareInfoSceExport y)
                {
                    if (ReferenceEquals(x, y)) return true;
                    if (ReferenceEquals(x, null)) return false;
                    if (ReferenceEquals(y, null)) return false;
                    if (x.GetType() != y.GetType()) return false;
                    return string.Equals(x.Brand, y.Brand) && string.Equals(x.PartNumber, y.PartNumber);
                }

                public int GetHashCode(ExtWareInfoSceExport obj)
                {
                    unchecked
                    {
                        return ((obj.Brand != null ? obj.Brand.GetHashCode() : 0) * 397) ^ (obj.PartNumber != null ? obj.PartNumber.GetHashCode() : 0);
                    }
                }
            }

            public string ProductType { get; set; }
            public string Prodid { get; set; }
            public string Brand { get; set; }
            public string Supplier { get; set; }
            public string PartNumber { get; set; }
        }

        public static sceApi GetApiClient(ScraperSettings settings)
        {
            if (string.IsNullOrEmpty(settings.SCEAccessKey) || string.IsNullOrEmpty(settings.SCEAPIKey) ||
                string.IsNullOrEmpty(settings.SCEAPISecret))
            {
                throw new ArgumentNullException("API fields have to be filled!");
            }
            var client = new sceApi();
            var auth = new AuthHeaderAPI
            {
                ApiAccessKey = settings.SCEAccessKey,
                ApiKey = settings.SCEAPIKey,
                ApiSecretKey = settings.SCEAPISecret
            };
            client.AuthHeaderAPIValue = auth;
            client.Timeout = 300 * 60 * 1000;
            return client;
        }

        public static string LoadProductsExport(ScraperSettings settings)
        {
            var client = GetApiClient(settings);
            var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
            var zippedFile = fileName.Replace(".csv", ".zip");
            var prodBytes = client.GetFullProductsExport(Separator);
            try
            {
                using (var fs = File.OpenWrite(zippedFile))
                {
                    fs.Write(prodBytes, 0, prodBytes.Length);
                }

                using (var zipArc = ZipFile.Read(zippedFile))
                {
                    foreach (var z in zipArc)
                    {
                        z.FileName = Path.GetFileName(fileName);
                        z.Extract(Path.GetDirectoryName(fileName), ExtractExistingFileAction.OverwriteSilently);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            finally
            {
                if (File.Exists(zippedFile))
                {
                    File.Delete(zippedFile);
                }
            }
            return fileName;
        }
        public static int BatchUpdate(string sourceFile, ScraperSettings settings)
        {
            var client = GetApiClient(settings);
            var resultId = client.RunProductBatch(sourceFile, true);
            return resultId;
        }

        public List<RDITrading> ReadSceExportRDITrading(string filePath)
        {
            List<RDITrading> rosyBlue = new List<RDITrading>();

            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    try
                    {
                        while (csv.ReadNextRecord())
                        {
                            string color = csv["Color"];
                            string price = csv["Price"];
                            string length = "";
                            string width = "";
                            string depth = "";

                            length = csv["Length"];
                            width = csv["Width"];
                            depth = csv["Depth"];
                            if (!string.IsNullOrEmpty(price) && !string.IsNullOrEmpty(color))
                            {
                                RDITrading item = new RDITrading();
                                item.Shape = csv["Shape"];
                                item.Wieght = double.Parse(csv["Wieght"]);
                                item.Color = csv["Color"];
                                item.Clarity = csv["Clarity"];

                                if (!string.IsNullOrEmpty(length))
                                {
                                    item.Length = double.Parse(csv["Length"]);
                                }
                                if (!string.IsNullOrEmpty(width))
                                {
                                    item.Width = double.Parse(csv["Width"]);
                                }
                                if (!string.IsNullOrEmpty(depth))
                                {
                                    item.Depth = double.Parse(csv["Depth"]);
                                }
                                item.CutGrade = csv["Cut Grade"];
                                item.Lab = csv["Lab"];
                                item.Price = double.Parse(csv["Price"]);
                                item.DepthPr = csv["Depth %"];
                                item.TablePr = csv["Table %"];
                                item.Polish = csv["Polish"];
                                item.Symmetry = csv["Symmetry"];
                                item.FluorescenceIntensit = csv["Fluorescence Intensit"];
                                item.Certificate = csv["Certificate"];
                                item.VendorStockNumber = csv["VendorStockNumber"];
                                item.CertificateImage = csv["CertificateImage"];

                                rosyBlue.Add(item);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessagePrinter.PrintMessage("Error" + e.Message);
                    }

                    return rosyBlue;
                }
            }
        }
        public class RDITrading
        {
            public string Shape { get; set; }
            public double Wieght { get; set; }
            public string Color { get; set; }
            public string Clarity { get; set; }
            public double Length { get; set; }
            public double Width { get; set; }
            public double Depth { get; set; }
            public string CutGrade { get; set; }
            public string Lab { get; set; }
            public double Price { get; set; }
            public string DepthPr { get; set; }
            public string TablePr { get; set; }
            public string Polish { get; set; }
            public string Symmetry { get; set; }
            public string FluorescenceIntensit { get; set; }
            public string Certificate { get; set; }
            public string VendorStockNumber { get; set; }
            public string CertificateImage { get; set; }







            public string ShapeLong(string name)
            {
                if (name.Equals("RD") || name.Equals("BR") || name.Equals("R89") || name.Equals("Round") || name.Equals("OM"))
                {
                    name = "Round";

                }
                else if (name.Equals("CU") || name.Equals("Cushion") || name.Equals("CMB"))
                {
                    name = "Cushion";

                }
                else if (name.Equals("OV") || name.Equals("Oval"))
                {
                    name = "Oval";

                }
                else if (name.Equals("RAD") || name.Equals("Radiant") || name.Equals("RA"))
                {
                    name = "Radiant";

                }
                else if (name.Equals("AS") || name.Equals("Asscher"))
                {
                    name = "Asscher";

                }
                else if (name.Equals("HT") || name.Equals("Heart") || name.Equals("HS"))
                {
                    name = "Heart";
                }
                else if (name.Equals("PC") || name.Equals("Princess") || name.Equals("PR"))
                {
                    name = "Princess";

                }
                else if (name.Equals("EM") || name.Equals("EME") || name.Equals("Emerald") || name.Equals("EC") || name.Equals("SQEM"))
                {
                    name = "Emerald";

                }
                else if (name.Equals("PS") || name.Equals("Pear"))
                {
                    name = "Pear";

                }
                else if (name.Equals("MS") || name.Equals("MQ") || name.Equals("MR") || name.Equals("Marquise"))
                {
                    name = "Marquise";

                }
                else name = "";
                return name;
            }
            public string CutGradeLongSpecification(string name, string shape)
            {
                if (shape == "Round" && name == "")
                {
                    return name = "^Cut~Good";
                }
                else
                {
                    if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                    {
                        name = "^Cut~Good";
                    }
                    else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                    {
                        name = "^Cut~Very Good";
                    }
                    else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                    {
                        name = "^Cut~Excellent";
                    }
                    else if (name.Equals("FA") || name.Equals("FAIR"))
                    {
                        name = "^Cut~Fair";
                    }
                    else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                    {
                        name = "^Cut~Ideal";
                    }
                    else if (name.Equals("NA") || name.Equals("N/A") || string.IsNullOrWhiteSpace(name) || name.Equals("R89"))
                    {
                        name = "";
                    }
                    else name = "";
                }
                return name;
            }

            public string CutGradeLong(string name, string shape)
            {
                if (shape == "Round" && name == "")
                {
                    return name = "Good Cut ";
                }
                else
                {

                    if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                    {
                        name = "Good Cut ";
                    }
                    else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                    {
                        name = "Very Good Cut ";
                    }
                    else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                    {
                        name = "Excellent Cut ";
                    }
                    else if (name.Equals("FA") || name.Equals("FAIR"))
                    {
                        name = "Fair Cut ";
                    }
                    else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                    {
                        name = "Excellent Cut ";
                    }
                    else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("") || string.IsNullOrWhiteSpace(name) || name.Equals("R89"))
                    {
                        name = "";
                    }
                }
                
                return name;
            }
            public string CutGradeLongSpyderURLRDI(string name, string shape)
            {
                if (shape == "Round" && name == "")
                {
                    return name = "Good-Cut";
                }
                else
                {

                    if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                    {
                        name = "Good-Cut";
                    }
                    else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                    {
                        name = "Very-Good-Cut";
                    }
                    else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                    {
                        name = "Excellent-Cut";
                    }
                    else if (name.Equals("FA") || name.Equals("FAIR"))
                    {
                        name = "Fair-Cut";
                    }
                    else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                    {
                        name = "Excellent-Cut";
                    }
                    else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("") || string.IsNullOrWhiteSpace(name) || name.Equals("R89"))
                    {
                        name = "";
                    }
                }
                
                return name;
            }
            public string SymmetryLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Symmetry, ";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Symmetry, ";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent Symmetry, ";
                }
                else if (name.Equals("FA") || name.Equals("FAIR") || name.Equals("F"))
                {
                    name = "Fair Symmetry, ";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL") || name.Equals("Ideal"))
                {
                    name = "Excellent Symmetry, ";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good Symmetry, ";
                }
                else name = "";
                return name;
            }

            public string PolishLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Polish and ";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Polish and ";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent Polish and ";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair Polish and ";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent Polish and ";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good Polish and ";
                }
                else if (name.Equals(""))
                {
                    name = "";
                }
                else name = "";
                return name;
            }
            public string CertificateLong(string name)
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name))
                {
                    return name = "^Certificate No~" + name;
                }
                else return "";
            }
            public string DepthPrLong(string name)
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name))
                {
                    return name = "^Total Depth~" + name;
                }
                else return "";
            }
            public string TablePrLong(string name)
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name))
                {
                    return name = "^Table Size~" + name;
                }
                else return "";
            }

            public string LabLong(string name)
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name))
                {
                    return name = "^Lab Name~" + name;
                }
                else return "";
            }

            public string PolishLongSpecification(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Polish~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Polish~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Polish~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "^Polish~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Polish~Excellent";
                }
                else if (name.Equals("NA"))
                {
                    name = "^Polish~Good";
                }
                else if (name.Equals(""))
                {
                    name = "";
                }
                else name = "";
                return name;
            }


            public string SymmetryLongSpecification(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Symmetry~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Symmetry~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Symmetry~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR") || name.Equals("F"))
                {
                    name = "^Symmetry~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL") || name.Equals("Ideal"))
                {
                    name = "^Symmetry~Excellent";
                }
                else if (name.Equals("NA"))
                {
                    name = "^Symmetry~Good";
                }
                else if (name.Equals(""))
                {
                    name = "";
                }

                return name;
            }
            public string FluorescenceLong(string name)
            {
                if (name.Equals("NON") || name.Equals("") || name.Equals("N"))
                {
                    name = "^Fluorescence~None";
                }
                else if (name.Equals("FNT") || name.Equals("F"))
                {
                    name = "^Fluorescence~Faint";
                }
                else if (name.Equals("MED") || name.Equals("M"))
                {
                    name = "^Fluorescence~Medium";
                }
                else if (name.Equals("STG") || name.Equals("VS") || name.Equals("VSL"))
                {
                    name = "^Fluorescence~Very Strong";
                }
                else if (name.Equals("S"))
                {
                    name = "^Fluorescence~Strong";
                }
                return name;
            }

        }
        public List<SaharAtid> ReadSceExportSaharAtid(string filePath)
        {
            List<SaharAtid> saharAtid = new List<SaharAtid>();


            using (var sr = File.OpenText(filePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    try
                    {

                        while (csv.ReadNextRecord())
                        {
                            SaharAtid item = new SaharAtid();
                            item.Stock = csv["Stock #"];
                            item.Availability = csv["Availability"];
                            item.Shape = csv["Shape"];
                            item.Carat = double.Parse(csv["Carat"]);
                            item.Color = csv["Color"];
                            item.Clarity = csv["Clarity"];
                            item.Cut = csv["Cut"];
                            item.Polish = csv["Polish"];
                            item.Symmetry = csv["Symmetry"];
                            item.FluorescenceIntensity = csv["Fluorescence Intensity"];
                            item.FluorescenceColor = csv["Fluorescence Color"];
                            item.Measurements = csv["Measurements"];
                            item.Lab = csv["Lab"];
                            item.CertificateNumber = csv["Certificate Number"];
                            item.PricePerCarat = double.Parse(csv["Price Per Carat"]);
                            item.Rap = csv["% Rap"];
                            item.FancyColor = csv["Fancy Color"];
                            item.FancyColorIntensity = csv["Fancy Color Intensity"];
                            item.FancyColorOvertone = csv["Fancy Color Overtone"];
                            item.DepthPr = csv["Depth %"];
                            item.TablePr = csv["Table %"];
                            item.GirdleCondition = csv["Girdle Condition"];
                            item.CuletSize = csv["Culet Size"];
                            item.CrownHeight = csv["Crown Height"];
                            item.CrownAngle = csv["Crown Angle"];
                            item.PavilionDepth = csv["Pavilion Depth"];
                            item.Comment = csv["Comment"];
                            item.Country = csv["Country"];
                            item.Location = csv["Location"];
                            item.CertificateFilename = csv["Certificate Filename"];
                            item.DiamondImage = csv["Diamond Image"];
                            item.MemberComments = csv["Member Comments"];


                            saharAtid.Add(item);

                        }
                    }
                    catch (Exception e)
                    {
                        MessagePrinter.PrintMessage("Error" + e.Message);
                    }

                    return saharAtid;
                }
            }
        }
        public class SaharAtid
        {
            public string Stock { get; set; }
            public string Availability { get; set; }
            public string Shape { get; set; }
            public double Carat { get; set; }
            public string Color { get; set; }
            public string Clarity { get; set; }
            public string Cut { get; set; }
            public string Polish { get; set; }
            public string Symmetry { get; set; }
            public string FluorescenceIntensity { get; set; }
            public string FluorescenceColor { get; set; }
            public string Measurements { get; set; }
            public string Lab { get; set; }
            public string CertificateNumber { get; set; }
            public double PricePerCarat { get; set; }
            public string Rap { get; set; }
            public string FancyColor { get; set; }
            public string FancyColorIntensity { get; set; }
            public string FancyColorOvertone { get; set; }
            public string DepthPr { get; set; }
            public string TablePr { get; set; }
            public string GirdleCondition { get; set; }
            public string CuletSize { get; set; }
            public string CrownHeight { get; set; }
            public string CrownAngle { get; set; }
            public string PavilionDepth { get; set; }
            public string Comment { get; set; }
            public string Country { get; set; }
            public string Location { get; set; }
            public string CertificateFilename { get; set; }
            public string DiamondImage { get; set; }
            public string MemberComments { get; set; }

            public string ShapeLong(string name)
            {
                if (name.Equals("RD") || name.Equals("BR") || name.Equals("R89") || name.Equals("Round"))
                {
                    name = "Round";

                }
                else if (name.Equals("CU") || name.Equals("Cushion"))
                {
                    name = "Cushion";

                }
                else if (name.Equals("OV") || name.Equals("Oval"))
                {
                    name = "Oval";

                }
                else if (name.Equals("RAD") || name.Equals("Radiant") || name.Equals("RA"))
                {
                    name = "Radiant";

                }
                else if (name.Equals("AS") || name.Equals("Asscher"))
                {
                    name = "Asscher";

                }
                else if (name.Equals("HT") || name.Equals("Heart"))
                {
                    name = "Heart";
                }
                else if (name.Equals("PC") || name.Equals("Princess") || name.Equals("PR"))
                {
                    name = "Princess";

                }
                else if (name.Equals("EM") || name.Equals("EME") || name.Equals("Emerald"))
                {
                    name = "Emerald";

                }
                else if (name.Equals("PS") || name.Equals("Pear"))
                {
                    name = "Pear";

                }
                else if (name.Equals("MS") || name.Equals("MQ") || name.Equals("MR") || name.Equals("Marquise"))
                {
                    name = "Marquise";

                }
                else name = "";
                return name;
            }
            public string CutGradeLongSpecification(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Cut~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Cut~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Cut~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "^Cut~Good";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Cut~Ideal";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }
                else name = "";
                return name;
            }
            
            public string CutGradeLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Cut ";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Cut ";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent Cut ";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair Cut ";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Ideal Cut ";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("") || string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }
                
                return name;
            }
            public string CutGradeLongSpiderURLSahar(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good-Cut";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very-Good-Cut";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent-Cut";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair-Cut";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Ideal-Cut";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("") || string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }
                
                return name;
            }
            public string ColorLong(string name)
            {
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name))
                {
                    if (name.Equals("FC"))
                    {
                        return "M Color ";
                    }
                    return name = name + " Color ";
                }
                else return "";
            }
            public string ColorLongSpecification(string name)
            {
                switch (name)
                {
                    case "E":
                        return "^Color~E";

                    case "D":
                        return "^Color~D";
                    case "F":
                        return "^Color~F";
                    case "G":
                        return "^Color~G";
                    case "H":
                        return "^Color~H";
                    case "I":
                        return "^Color~I";
                    case "J":
                        return "^Color~J";
                    case "K":
                        return "^Color~K";
                    case "L":
                        return "^Color~L";
                    case "M":
                        return "^Color~M";

                    default:
                        return "^Color~M";
                }

            }


            public string PolishLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Ideal";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good";
                }
                else name = "";
                return name;
            }
            public string SymmetryLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Ideal";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good";
                }
                
                return name;
            }
            public string FluorescenceLong(string name)
            {
                if (name.Equals("NONE") || name.Equals("") || name.Equals("N"))
                {
                    name = "None";
                }
                else if (name.Equals("FNT") || name.Equals("FAINT"))
                {
                    name = "Faint";
                }
                else if (name.Equals("MED") || name.Equals("MEDIUM"))
                {
                    name = "Medium";
                }
                else if (name.Equals("STG") || name.Equals("STRONG"))
                {
                    name = "Strong";
                }
                else if (name.Equals("VERY STRONG"))
                {
                    name = "Very Strong";
                }
                return name;
            }

        }

        public List<LSDiamond> ReadSceExportLSD(string filePath)
        {
            List<LSDiamond> lsDiamond = new List<LSDiamond>();

            try
            {


                using (var reader = new StreamReader(filePath))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadToEnd();
                        var values = line.Split('\r');

                        try
                        {
                            foreach (var col in values)
                            {
                                LSDiamond itemLSD = new LSDiamond();
                                var arrayOfValues = col.Split(',');

                                if (col == "\n")
                                {
                                    continue;
                                }

                                itemLSD.Id = arrayOfValues[0].Replace("\n", "").ToString();
                                if (itemLSD.Id == "Id" || col == "\n")
                                {
                                    continue;
                                }
                                itemLSD.VideosURL = arrayOfValues[1];
                                itemLSD.CertificateLab = arrayOfValues[2];
                                itemLSD.CertificateURL = arrayOfValues[3];
                                itemLSD.CertificateID = arrayOfValues[4];
                                itemLSD.Price = arrayOfValues[5];
                                itemLSD.TotalPrice = Double.Parse(arrayOfValues[6]);
                                itemLSD.DiscountPrc = arrayOfValues[7];
                                itemLSD.Shape = arrayOfValues[8];
                                itemLSD.Carat = Double.Parse(arrayOfValues[9]);
                                itemLSD.Cut = arrayOfValues[10];
                                itemLSD.Color = arrayOfValues[11];
                                itemLSD.Clarity = arrayOfValues[12];
                                itemLSD.Fluorescence = arrayOfValues[13];
                                itemLSD.Polish = arrayOfValues[14];
                                itemLSD.Symmetry = arrayOfValues[15];
                                itemLSD.Table = arrayOfValues[16];
                                itemLSD.GirdleCondition = arrayOfValues[17];
                                itemLSD.Girdle = arrayOfValues[18];
                                itemLSD.Culet = arrayOfValues[19];
                                itemLSD.CrownHeight = arrayOfValues[20];
                                itemLSD.CrownAngle = arrayOfValues[21];
                                itemLSD.PavilionDepth = arrayOfValues[22];
                                itemLSD.PavilionAngle = arrayOfValues[23];
                                itemLSD.DepthPercentage = arrayOfValues[24];
                                itemLSD.Measurements = arrayOfValues[25];
                                itemLSD.StarLength = arrayOfValues[26];
                                itemLSD.LowerHalfLength = arrayOfValues[27];
                                itemLSD.Inscription = arrayOfValues[28].Replace("\"", "");
                                itemLSD.LabComments = "";
                                itemLSD.KeyToSymbols = "";
                                itemLSD.Location = "";

                                if (lsDiamond.Count == 70)
                                {

                                }

                                lsDiamond.Add(itemLSD);

                            }

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }




                    }


                }

            }
            catch (Exception ex)
            {


            }
            return lsDiamond;
        }

        public class LSDiamond
        {
            public string Id { get; set; }
            public string VideosURL { get; set; }
            public string CertificateLab { get; set; }
            public string CertificateURL { get; set; }
            public string CertificateID { get; set; }
            public string Price { get; set; }
            public double TotalPrice { get; set; }
            public string DiscountPrc { get; set; }
            public string Shape { get; set; }
            public double Carat { get; set; }
            public string Cut { get; set; }
            public string Color { get; set; }
            public string Clarity { get; set; }
            public string Fluorescence { get; set; }
            public string Polish { get; set; }
            public string Symmetry { get; set; }
            public string Table { get; set; }
            public string GirdleCondition { get; set; }
            public string Girdle { get; set; }
            public string Culet { get; set; }
            public string CrownHeight { get; set; }
            public string CrownAngle { get; set; }
            public string PavilionDepth { get; set; }
            public string PavilionAngle { get; set; }
            public string DepthPercentage { get; set; }
            public string Measurements { get; set; }
            public string StarLength { get; set; }
            public string LowerHalfLength { get; set; }
            public string Inscription { get; set; }
            public string LabComments { get; set; }
            public string KeyToSymbols { get; set; }
            public string Location { get; set; }
            public string ShapeLong(string name)
            {
                if (name.Equals("RD") || name.Equals("BR") || name.Equals("R89") || name.Equals("Round"))
                {
                    name = "Round";

                }
                else if (name.Equals("CU") || name.Equals("Cushion"))
                {
                    name = "Cushion";

                }
                else if (name.Equals("OV") || name.Equals("Oval"))
                {
                    name = "Oval";

                }
                else if (name.Equals("RAD") || name.Equals("Radiant") || name.Equals("RA"))
                {
                    name = "Radiant";

                }
                else if (name.Equals("AS") || name.Equals("Asscher"))
                {
                    name = "Asscher";

                }
                else if (name.Equals("HT") || name.Equals("Heart"))
                {
                    name = "Heart";
                }
                else if (name.Equals("PC") || name.Equals("Princess") || name.Equals("PR"))
                {
                    name = "Princess";

                }
                else if (name.Equals("EM") || name.Equals("EME") || name.Equals("Emerald"))
                {
                    name = "Emerald";

                }
                else if (name.Equals("PS") || name.Equals("Pear"))
                {
                    name = "Pear";

                }
                else if (name.Equals("MS") || name.Equals("MQ") || name.Equals("MR") || name.Equals("Marquise"))
                {
                    name = "Marquise";

                }
                else name = "";
                return name;
            }

            public double CaratFormat(double carat)
            {
                if (carat >= 1)
                {
                    return carat;
                }
                else
                {
                    return carat;
                }
            }
            public string CutGradeLongSpecification(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Cut~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Cut~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Cut~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "^Cut~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Cut~Excellent";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || string.IsNullOrWhiteSpace(name))
                {
                    name = "";
                }
                else name = "";
                return name;
            }

            public string CutGradeLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good Cut ";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good Cut ";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent Cut ";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair Cut ";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent Cut ";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("") || name.Equals("R89"))
                {
                    name = "";
                }
                else name = "";
                return name;
            }
            public string CutGradeLongSpiderURL(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good-Cut";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very-Good-Cut";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent-Cut";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair-Cut";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent-Cut";
                }
                else if (name.Equals("NA") || name.Equals("N/A") || name.Equals("") || name.Equals("R89"))
                {
                    name = "";
                }
                else name = "";
                return name;
            }


            public string PolishLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good";
                }
                else name = "";
                return name;
            }
            public string SymmetryLongLeo(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "Excellent";
                }
                else if (name.Equals("NA"))
                {
                    name = "Good";
                }
                else name = "";
                return name;
            }
            public string SymmetryLong(string name)
            {
                if (name.Equals("GD") || name.Equals("GOOD") || name.Equals("G"))
                {
                    name = "^Symmetry~Good";
                }
                else if (name.Equals("VG") || name.Equals("VERY GOOD"))
                {
                    name = "^Symmetry~Very Good";
                }
                else if (name.Equals("EX") || name.Equals("EXCELLENT"))
                {
                    name = "^Symmetry~Excellent";
                }
                else if (name.Equals("FA") || name.Equals("FAIR"))
                {
                    name = "^Symmetry~Fair";
                }
                else if (name.Equals("ID") || name.Equals("IDEAL") || name.Equals("IL"))
                {
                    name = "^Symmetry~Excellent";
                }
                else if (name.Equals("NA"))
                {
                    name = "^Symmetry~Good";
                }
                else name = "";
                return name;
            }
            public string FluorescenceLong(string name)
            {
                if (name.Equals("NON") || name.Equals("") || name.Equals("NONE"))
                {
                    name = "^Fluorescence~None";
                }
                else if (name.Equals("FNT") || name.Equals("FAINT"))
                {
                    name = "^Fluorescence~Faint";
                }
                else if (name.Equals("MED") || name.Equals("MEDIUM"))
                {
                    name = "^Fluorescence~Medium";
                }
                else if (name.Equals("VERY STRONG") || name.Equals("VS") || name.Equals("VSL"))
                {
                    name = "^Fluorescence~Very Strong";
                }
                else if (name.Equals("STRONG"))
                {
                    name = "^Fluorescence~Strong";
                }
                return name;
            }

            public string CertificateName(string name)
            {
                if (name.Equals("GIA"))
                {
                    return "GCAL Certificate";
                }
                else return "IGI Certificate";
            }
            public string CertificateLabSpecialization(string name, string certId)
            {
                if (name.Equals("IGI"))
                {
                    return name = "^Lab Name~IGI^Certificate No~" + certId;
                }
                else if (name.Equals("AGS"))
                {
                    return name = "^Lab Name~AGS^Certificate No~" + certId;
                }
                else if (name.Equals("EGL IL"))
                {
                    return name = "^Lab Name~EGL IL^Certificate No~" + certId;
                }
                else if (name.Equals("EGL US"))
                {
                    return name = "^Lab Name~EGL US^Certificate No~" + certId;
                }
                else return "";
            }


        }
        public string UrlCheck(string url)
        {
            try
            {
                var testedUrl = PageRetriever.ReadFromServer(url, true);

                if (testedUrl != null)
                {
                    if (testedUrl != "0")
                    {
                        return "Certificate^^" + url;
                    }
                }
            }
            catch (Exception)
            {

            }
            return "";
        }

        public string UrlCheckRARiam(string url)
        {
            try
            {
                var testedUrl = PageRetriever.ReadFromServer(url, true);

                if (testedUrl != null)
                {
                    if (testedUrl != "0")
                    {
                        return "Certificate^^" + url;
                    }
                }
            }
            catch (Exception)
            {
                try
                {

                    string[] arrayUrl = url.Split('/');
                    var urlJpg = "http://www.gemfacts.com/Grading/images/" + arrayUrl[arrayUrl.Length - 1].Replace("GCAL", "").Replace(".pdf", "") + ".jpg";
                    var testedUrl = PageRetriever.ReadFromServer(urlJpg, true);

                    if (testedUrl != null)
                    {
                        if (testedUrl != "0")
                        {
                            return "Certificate^^" + urlJpg;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return "";
        }

    }

}
