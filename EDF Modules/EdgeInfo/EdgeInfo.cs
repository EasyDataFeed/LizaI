﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using EdgeInfo;
using Databox.Libs.EdgeInfo;
using EdgeInfo.DataItems;
using EdgeInfo.Helpers;

namespace WheelsScraper
{
    public class EdgeInfo : BaseScraper
    {
        private const string pickupSpec = "^PickUp~<span class=\"pickup12\">Available In Store</span>";
        private const string pickupNotAvailableSpec = "^PickUp~<span class=\"pickup12\">Available For Special Order</span>";
        private const string pickupEmptySpec = "PickUp~<span class=\"pickup12\">Available In Store</span>";
        private const string pickupClassSpec = "PickUp~<span class=\"pickup12\">";
        private const string SpecHead = "Specifications##";

        public EdgeInfo()
        {
            Name = "EdgeInfo";
            Url = "https://www.EdgeInfo.com/";
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
            try
            {
                if (string.IsNullOrEmpty(extSett.SupplierFileSkip))
                {
                    MessagePrinter.PrintMessage($"Please select File for supplier skip", ImportanceLevel.Critical);
                    return;
                }

                List<SupplierItems> supplierItems = FileHelper.ReadSupplierFile(extSett.SupplierFileSkip);
                if (supplierItems.Count == 0)
                {
                    MessagePrinter.PrintMessage($"File for supplier skip is empty", ImportanceLevel.Critical);
                    return;
                }

                List<SupplierItems> supplierZeroItems = FileHelper.ReadSupplierFile(extSett.SupplierFileZeroSkip);
                if (supplierZeroItems.Count == 0)
                {
                    MessagePrinter.PrintMessage($"File for supplier's zero inventory is empty", ImportanceLevel.High);
                }
                
                string ftpFileName = FileHelper.GetSettingsPath("ftpFile.csv");
                
                MessagePrinter.PrintMessage($"Download FTP file");
                string localFilePath = RequestHelper.DownloadLatesFile(extSett.FTPLogin, extSett.FTPPassword, extSett.FTPHost, ftpFileName);
                if (string.IsNullOrEmpty(localFilePath))
                {
                    MessagePrinter.PrintMessage($"Can't download FTP file", ImportanceLevel.Critical);
                    return;
                }

                //считатать ftp export
                MessagePrinter.PrintMessage($"Read FTP file");

                List<FtpDataItem> ftpDataItems = FileHelper.ReadFtpFile(localFilePath);
                if (ftpDataItems.Count > 0)
                {
                    MessagePrinter.PrintMessage($"FTP file was read");
                }
                else
                {
                    MessagePrinter.PrintMessage($"FTP file empty", ImportanceLevel.Critical);
                    return;
                }

                //удалить фтп файл
                if (File.Exists(ftpFileName))
                {
                    File.Delete(ftpFileName);
                }
                
                MessagePrinter.PrintMessage($"Download SCE export... please wait");
                List<string> sceFiles = SceApiHelper.LoadProductsExport(Settings);

                if (sceFiles.Count > 0)
                {
                    MessagePrinter.PrintMessage($"SCE export downloaded");
                }
                else
                {
                    MessagePrinter.PrintMessage($"SCE export not downloaded, please try again later", ImportanceLevel.Critical);
                    return;
                }

                //считывание Sce Export
                MessagePrinter.PrintMessage($"Read SCE export... please wait");
                List<SceExportItem> sceExportItems = new List<SceExportItem>();
                foreach (string sceFile in sceFiles)
                {
                    sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile));
                }

                List<string> processing = new List<string>() { extSett.SupplierFilePath };

                List<ProcessingPeriod> processingPeriod = new List<ProcessingPeriod>();
                foreach (string file in processing)
                {
                    processingPeriod.AddRange(FileHelper.ReadProcessingPeriod(file));
                }

                if (sceExportItems.Count > 0)
                {
                    MessagePrinter.PrintMessage($"SCE export was read");
                }
                else
                {
                    MessagePrinter.PrintMessage($"SCE export empty", ImportanceLevel.Critical);
                    return;
                }
                
                foreach (string sceFile in sceFiles)
                {
                    if (File.Exists(sceFile))
                    {
                        File.Delete(sceFile);
                    }
                }
                
                List<PriceUpdateInfo> priceUpdateItems = new List<PriceUpdateInfo>();
                
                List<InventoryUpdateInfo> inventoryUpdateItems = new List<InventoryUpdateInfo>();
                
                foreach (SceExportItem sceExportItem in sceExportItems)
                {
                    //if (sceExportItem.PartNumber == "MF022048-14B_18")
                    //{

                    //}
                    
                    if (CheckSupplier(sceExportItem.Supplier, supplierItems))
                        continue;

                    foreach (FtpDataItem ftpDataItem in ftpDataItems)
                    {
                        //if ($"{ftpDataItem.ItStyleCode}-{ftpDataItem.ItSize.TrimEnd('0', '.')}" == "EBS3909MOP")
                        //{

                        //}

                        if (!string.IsNullOrEmpty(ftpDataItem.ItSize))
                        {
                            if (string.Equals(sceExportItem.PartNumber, $"{ftpDataItem.ItStyleCode}-{ftpDataItem.ItSize.TrimEnd('0', '.')}",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                double size = double.Parse(ftpDataItem.ItSize);

                                bool found = false;

                                foreach (var item in processingPeriod)
                                {
                                    if (ftpDataItem.ItVendorId == item.EdgeName)
                                    {
                                        found = true;

                                        for (double curSize = size - 2; curSize <= size + 2; curSize += 0.25)
                                        {
                                            inventoryUpdateItems.Add(new InventoryUpdateInfo
                                            {
                                                PartNumber = $"{ftpDataItem.ItStyleCode}-{curSize}",
                                                Supplier = item.SupplierName,
                                                Warehouse = item.SupplierName,
                                                Qty = 0
                                            });
                                        }
                                    }
                                    if (found)
                                        break;
                                }

                                foreach (var item in processingPeriod)
                                {
                                    if (ftpDataItem.ItVendorId == item.EdgeName)
                                    {
                                        //if (sceExportItem.PartNumber == "MF022048-14B_18")
                                        //{

                                        //}

                                        priceUpdateItems.Add(new PriceUpdateInfo
                                        {
                                            Action = "update",
                                            ProdId = sceExportItem.ProdId,
                                            ProductType = sceExportItem.ProductType,
                                            PartNumber = sceExportItem.PartNumber,
                                            Supplier = item.SupplierName,
                                            Warehouse = item.SupplierName,
                                            WebPrice = ftpDataItem.ItCurrentPrice,
                                            Jobber = ftpDataItem.ItCurrentPrice,
                                            MSRP = ftpDataItem.ItCurrentPrice,
                                            CostPrice = ftpDataItem.ItCost,
                                            Specification = sceExportItem.Specifications,
                                            PickupAvailable = "1",
                                            Featured = "1",
                                            ProcessingPeriod = "1"
                                        });
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (string.Equals(sceExportItem.PartNumber, ftpDataItem.ItStyleCode,
                                StringComparison.OrdinalIgnoreCase))
                            {
                                foreach (var item in processingPeriod)
                                {
                                    if (ftpDataItem.ItVendorId == item.EdgeName)
                                    {
                                        priceUpdateItems.Add(new PriceUpdateInfo
                                        {
                                            Action = "update",
                                            ProdId = sceExportItem.ProdId,
                                            ProductType = sceExportItem.ProductType,
                                            PartNumber = sceExportItem.PartNumber,
                                            Supplier = item.SupplierName,
                                            Warehouse = item.SupplierName,
                                            WebPrice = ftpDataItem.ItCurrentPrice,
                                            Jobber = ftpDataItem.ItCurrentPrice,
                                            MSRP = ftpDataItem.ItCurrentPrice,
                                            CostPrice = ftpDataItem.ItCost,
                                            Specification = sceExportItem.Specifications,
                                            PickupAvailable = "1",
                                            Featured = "1",
                                            ProcessingPeriod = "1"
                                        });

                                        inventoryUpdateItems.Add(new InventoryUpdateInfo
                                        {
                                            PartNumber = sceExportItem.PartNumber,
                                            Supplier = item.SupplierName,
                                            Warehouse = item.SupplierName,
                                            Qty = 0
                                        });
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                
                priceUpdateItems = priceUpdateItems.Distinct(new PriceUpdateInfo.PartNumberEqualityComparer()).ToList();
                foreach (var priceUpdateItem in priceUpdateItems)
                {
                    //if (priceUpdateItem.PartNumber == "MF022048-14B_18")
                    //{

                    //}

                    if (string.IsNullOrEmpty(priceUpdateItem.Specification))
                    {
                        priceUpdateItem.Specification = SpecHead + pickupEmptySpec;
                    }
                    else if (priceUpdateItem.Specification.Contains(SpecHead))
                    {
                        string specs = priceUpdateItem.Specification.Replace(SpecHead, string.Empty);
                        var specsArray = specs.Split('^');

                        bool pickupSpecFound = false;
                        foreach (string s in specsArray)
                        {
                            if (s.Contains(pickupEmptySpec))
                            {
                                pickupSpecFound = true;
                            }
                        }

                        if (!pickupSpecFound)
                        {
                            try
                            {
                                if (specs.Contains(pickupClassSpec))
                                {
                                    var splitedSpec = priceUpdateItem.Specification.Split('^');
                                    if (splitedSpec.Count() > 1)
                                    {
                                        priceUpdateItem.Specification = priceUpdateItem.Specification.Substring(0, priceUpdateItem.Specification.LastIndexOf("^", StringComparison.Ordinal)) + pickupSpec;
                                    }
                                }
                                else
                                    priceUpdateItem.Specification = SpecHead + specs + pickupSpec;
                            }
                            catch (Exception e)
                            {
                                MessagePrinter.PrintMessage($"Some problem in spec - ProdId:{priceUpdateItem.ProdId} - Spec:{priceUpdateItem.Specification}", ImportanceLevel.High);
                            }

                        }
                    }
                }
                
                List<string> notExistSuppliers = new List<string>();

                foreach (var sceExportItem in sceExportItems)
                {
                    var priceUpdateItem = priceUpdateItems.FirstOrDefault(i => i.PartNumber == sceExportItem.PartNumber && i.ProdId == sceExportItem.ProdId);
                    if (priceUpdateItem != null)
                        continue;

                    //if (sceExportItem.PartNumber == "MF022048-14B_18")
                    //{

                    //}

                    if (CheckSupplier(sceExportItem.Supplier, supplierItems))
                        continue;

                    if (notExistSuppliers.Contains(sceExportItem.Supplier))
                        continue;

                    bool supplierFound = false;

                    foreach (var item in processingPeriod)
                    {
                        if (sceExportItem.Supplier == item.SupplierName)
                        {
                            supplierFound = true;

                            string processingTime = item.SupplierDeliveryTime.Replace("Days", string.Empty).Trim();
                            string spec = sceExportItem.Specifications.Replace(pickupEmptySpec, "").Replace("^^", "^")
                                .Replace("~~", "~").Replace("Specifications##^", SpecHead);

                            if (string.IsNullOrEmpty(spec))
                                spec = SpecHead + pickupNotAvailableSpec;
                            else if (spec.Contains(pickupClassSpec))
                                spec = sceExportItem.Specifications;
                            else
                                spec += pickupNotAvailableSpec;

                            spec = spec.Replace(pickupEmptySpec, "").Replace("^^", "^")
                                .Replace("~~", "~").Replace("Specifications##^", SpecHead);

                            string pickup = "0";

                            if (pickup != sceExportItem.PickupAvailable || spec != sceExportItem.Specifications ||
                                processingTime != sceExportItem.ProcessingPeriod)
                            {
                                priceUpdateItems.Add(new PriceUpdateInfo
                                {
                                    ProdId = sceExportItem.ProdId,
                                    PartNumber = sceExportItem.PartNumber,
                                    PickupAvailable = pickup,
                                    Specification = spec,
                                    ProductType = sceExportItem.ProductType,
                                    MSRP = sceExportItem.MSRP,
                                    Jobber = sceExportItem.Jobber,
                                    CostPrice = sceExportItem.CostPrice,
                                    WebPrice = sceExportItem.WebPrice,
                                    Supplier = sceExportItem.Supplier,
                                    Warehouse = sceExportItem.Warehouse,
                                    Featured = "0",
                                    Action = "update",
                                    ProcessingPeriod = processingTime
                                });
                            }
                            else
                            {

                            }
                            break;
                        }
                    }

                    if (!supplierFound)
                    {
                        MessagePrinter.PrintMessage($"supplier '{sceExportItem.Supplier}' not found in supplier file", ImportanceLevel.High); ;
                        notExistSuppliers.Add(sceExportItem.Supplier);
                    }
                }

                foreach (var item in priceUpdateItems)
                {
                    //if (item.PartNumber == "MF022048-14B_18")
                    //{

                    //}

                    if (item.Specification.Contains("Available In Store"))
                    {
                        item.CustomHtmlBelowPrice = $"<p class=\"wrap-appoint-btn\"><img src=\"/files//marks_2/appointment_button2.png\" data-appointlet-organization=\"marks-jewelers\" data-appointlet-field-sku={item.PartNumber} class=\"appointment_button\"></p>";
                    }
                }

                var inventoryUpdateInfos = inventoryUpdateItems.GroupBy(i => i.PartNumber).Select(ob => new InventoryUpdateInfo(ob)).ToList();

                List<InventoryUpdateInfo> update = new List<InventoryUpdateInfo>();

                foreach (SceExportItem exportItem in sceExportItems)
                {
                    if (CheckSupplier(exportItem.Supplier, supplierItems))
                        continue;

                    bool found = false;

                    //if (exportItem.PartNumber == "MF022048-14B_18")
                    //{

                    //}

                    foreach (var inventoryUpdateInfo in inventoryUpdateInfos)
                    {
                        if (exportItem.PartNumber == inventoryUpdateInfo.PartNumber)
                        {
                            if (CheckSupplier(exportItem.Supplier, supplierZeroItems))
                            {
                                inventoryUpdateInfo.Qty = 1;
                                found = true;
                                update.Add(inventoryUpdateInfo);
                                break;
                            }
                            else
                            {
                                found = true;
                                update.Add(inventoryUpdateInfo);
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        if (CheckSupplier(exportItem.Supplier, supplierZeroItems))
                        {
                            update.Add(new InventoryUpdateInfo
                            {
                                PartNumber = exportItem.PartNumber,
                                Supplier = exportItem.Supplier,
                                Warehouse = exportItem.Supplier,
                                Qty = 0
                            });
                        }
                        else
                        {
                            update.Add(new InventoryUpdateInfo
                            {
                                PartNumber = exportItem.PartNumber,
                                Supplier = exportItem.Supplier,
                                Warehouse = exportItem.Supplier,
                                Qty = 99
                            });
                        }

                    }
                }

                foreach (var item in update)
                {
                    if (item.Qty > 0 && item.Qty != 99)
                    {
                        foreach (var priceUpdate in priceUpdateItems)
                        {
                            if (item.PartNumber == priceUpdate.PartNumber && item.Supplier == priceUpdate.Supplier)
                            {
                                priceUpdate.ShippingCarrier2 = "USPS";
                                priceUpdate.pickupAvailable = "1";
                            }
                        }
                    }
                }

                if (priceUpdateItems.Count > 0)
                {
                    MessagePrinter.PrintMessage($"Create local price file");
                    string filePath = FileHelper.CreatePriceUpdateFile(FileHelper.GetSettingsPath("EdgePriceUpdate.csv"), priceUpdateItems);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        MessagePrinter.PrintMessage("Upload price batch file to FTP");
                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, "EdgePriceUpdate.csv", filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            if (extSett.DoBatch)
                            {
                                int batchId = SceApiHelper.BatchUpdate(urlForBatch, Settings);
                                MessagePrinter.PrintMessage($"File Batched. BatchId - {batchId}");
                            }
                        }
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (update.Count > 0)
                {
                    MessagePrinter.PrintMessage($"Create local inventory file");
                    string filePath = FileHelper.CreateInventoryUpdateFile(FileHelper.GetSettingsPath("EdgeInventoryUpdate.csv"), update);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        MessagePrinter.PrintMessage("Upload inventory file to FTP");
                        string url = FtpHelper.UploadFileToFtp(Settings.FtpAddress, Settings.FtpUsername, Settings.FtpPassword, "EdgeInventoryUpdate.csv", filePath, true);
                        if (!string.IsNullOrEmpty(url))
                        {
                            string urlForBatch = url.Replace("ftp://efilestorage.com", "http://efilestorage.com/scefiles");
                            MessagePrinter.PrintMessage($"Invntory file uploaded - {urlForBatch}");
                        }
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message + e.StackTrace, ImportanceLevel.Critical);
            }

            StartOrPushPropertiesThread();
        }

        private bool CheckSupplier(string supplier, List<SupplierItems> exceptionList)
        {
            foreach (SupplierItems exceptionItem in exceptionList)
            {
                //if (supplier == "RDI Trading")
                //{

                //}
                if (string.Equals(supplier, exceptionItem.SupplierName,
                    StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
