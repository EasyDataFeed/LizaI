using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using MarksJewelersSuppliersData;
using Databox.Libs.MarksJewelersSuppliersData;
using MarksJewelersSuppliersData.DataItems;
using MarksJewelersSuppliersData.Helpers;
using MarksJewelersSuppliersData.Extensions;
using MarksJewelersSuppliersData.Enums;
using Scraper.Lib.Main;
using System.IO;

namespace WheelsScraper
{
    public class MarksJewelersSuppliersData : BaseScraper
    {
        private const int DescriptionLength = 100;
        List<string> existingFilesOnS3 = new List<string>();

        public MarksJewelersSuppliersData()
        {
            Name = "MarksJewelersSuppliersData";
            Url = "https://www.MarksJewelersSuppliersData.com/";
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

        public string UploadImageToS3(string filePath)
        {
            var s3 = new S3Uploader();

            //if (existingFilesOnS3.Count() == 0)
            //existingFilesOnS3 = existingFilesOnS3.GetAmazonFoolderFiles(Settings.S3Settings);

            var imagePath = filePath.Split(',');

            foreach (var itemImage in imagePath)
            {
                foreach (var itemimageS3 in existingFilesOnS3)
                {
                    if (Path.GetFileNameWithoutExtension(itemimageS3) != Path.GetFileNameWithoutExtension(itemImage))
                    {
                        string imageUrl = s3.UploadToS3(itemImage, Settings.S3Settings);
                    }
                    else
                    {
                        //string imageUrl = existingFilesOnS3.(itemImage, Settings.S3Settings);
                    }
                }
            }

            //filePath -> Split(",") -> загрузить каждую картинку на S2 (вернёт ссылку) -> ссылку записать в existingFilesOnS3
            return existingFilesOnS3.ToString();
        }

        protected override void RealStartProcess()
        {
            try
            {
                if (extSett.JohnHardyCheck)
                {
                    //string johnHardyDataItem = $"D:/Leprizoriy/Work SCE/EDF/EDF Modules/MarksJewelersSuppliersData/EDF/MJ.csv";
                    List<JohnHardyDataItem> johnHardyCollection = FileHelper.ReadSupplierJohnHardyFile(extSett.JohnHardyFilePath);
                    MessagePrinter.PrintMessage($"John Hardy file readed.");

                    List<SceBatchItem> batchCollection = new List<SceBatchItem>();
                    List<SceBatchItem> notValidBatchCollection = new List<SceBatchItem>();
                    List<SceBatchItem> indexItems = new List<SceBatchItem>();

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
                    List<SceExportItem> sceExportItems = new List<SceExportItem>();
                    //var sceFile1 = @"D:\Altaresh\Work\My Work\Rebmbrand\EDGE Connector\EDGE\newSceExport1.csv";
                    //sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile1));
                    foreach (string sceFile in sceFiles)
                    {
                        //считать sce export
                        sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile));
                    }

                    MessagePrinter.PrintMessage("SCE export readed");
                    MessagePrinter.PrintMessage("John Hardy collection start process");

                    int counter = 0;
                    int index = 0;

                    foreach (var fileItem in johnHardyCollection)
                    {
                        if (fileItem.StyleNo == "RBS659641AFRBBLSX7")
                        {

                        }

                        SceBatchItem item = JHFillSceBatchItem(fileItem, SupplierType.JohnHardy);
                        bool isValidItem = IsValid.ValidateItem(item);
                        bool flag = false;

                        foreach (var sceItem in sceExportItems)
                        {
                            if (sceItem.Brand == item.Brand && sceItem.PartNumber == item.PartNumber)
                            {
                                item.Index = index;
                                index++;
                                indexItems.Add(item);

                                //flag = true;
                                //break;
                            }
                        }

                        //if (flag == false)
                        //{
                        if (isValidItem)
                        {
                            batchCollection.Add(item);
                            counter++;
                        }
                        else
                        {
                            notValidBatchCollection.Add(item);
                            counter++;

                            MessagePrinter.PrintMessage($"Item {item.Brand} {item.PartNumber} has not valid data", ImportanceLevel.Mid);
                        }
                        //}
                    }

                    List<SceBatchItem> resultValues = new List<SceBatchItem>();

                    foreach (SceBatchItem s1 in indexItems)
                    {
                        foreach (SceBatchItem s2 in indexItems)
                        {
                            if (s1.SpiderURL == s2.SpiderURL && s1.Index != s2.Index)
                            {
                                resultValues.Add(s1);
                                notValidBatchCollection.Add(s1);
                            }
                        }
                    }

                    foreach (SceBatchItem resultValue in resultValues)
                    {
                        batchCollection.Remove(resultValue);
                    }

                    MessagePrinter.PrintMessage("John Hardy collection processed");
                    MessagePrinter.PrintMessage($"{counter} - processed.");

                    string filePath = FileHelper.CreateJHFile(FileHelper.GetSettingsPath($"JHSuppliersData.csv"), batchCollection);
                    MessagePrinter.PrintMessage($"File path - {filePath}");

                    string filePath1 = FileHelper.CreateNotValidJHFile(FileHelper.GetSettingsPath($"JHSuppliersNotValidData.csv"), notValidBatchCollection);
                    MessagePrinter.PrintMessage($"File path for not valid products - {filePath1}");
                }
                else
                {
                    MessagePrinter.PrintMessage("Don't found John Hardy file.");
                }

                if (extSett.TagHeuerCheck)
                {
                    //string johnHardyDataItem = $"D:/Leprizoriy/Work SCE/EDF/EDF Modules/MarksJewelersSuppliersData/EDF/MJ.csv";
                    List<TagHeuerDataItem> tagHeuerCollection = FileHelper.ReadSupplierTagHeuerFile(extSett.TagHeuerFilePath);
                    MessagePrinter.PrintMessage($"Tag Heuer file readed.");

                    List<SceBatchItem> batchCollection = new List<SceBatchItem>();
                    List<SceBatchItem> notValidBatchCollection = new List<SceBatchItem>();

                    //List<string> sceFiles = SceApiHelper.LoadProductsExport(Settings);
                    List<string> sceFiles = new List<string>() { @"E:\Leprizoriy\Work SCE\EDF\WheelsScraper 2\WheelsScraper\bin\Debug\newSceExport1.csv" };

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
                    List<SceExportItem> sceExportItems = new List<SceExportItem>();
                    //var sceFile1 = @"D:\Altaresh\Work\My Work\Rebmbrand\EDGE Connector\EDGE\newSceExport1.csv";
                    //sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile1));
                    foreach (string sceFile in sceFiles)
                    {
                        //считать sce export
                        sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile));
                    }

                    MessagePrinter.PrintMessage("SCE export readed");
                    MessagePrinter.PrintMessage("John Hardy collection start process");

                    int counter = 0;

                    foreach (var fileItem in tagHeuerCollection)
                    {
                        if (fileItem.MPN == "CAR201AA.BA0714")
                        {

                        }

                        SceBatchItem item = THFillSceBatchItem(fileItem, SupplierType.TagHeuer);
                        bool isValidItem = IsValid.ValidateItem(item);
                        //bool flag = false;

                        foreach (var sceItem in sceExportItems)
                        {
                            if (sceItem.Brand == item.Brand && sceItem.PartNumber == item.PartNumber)
                            {
                                //flag = true;
                                //break;
                            }
                        }

                        //if (flag == false)
                        //{
                        if (isValidItem)
                        {
                            item.GeneralImage = UploadImageToS3(item.GeneralImage);

                            batchCollection.Add(item);
                            counter++;
                        }
                        else
                        {
                            notValidBatchCollection.Add(item);
                            counter++;

                            MessagePrinter.PrintMessage($"Item {item.Brand} {item.PartNumber} has not valid data", ImportanceLevel.Mid);
                        }
                        //}
                    }

                    //notValidBatchCollection = batchCollection.Distinct(new SceBatchItem.SceBatchItemProductTitleEqualityComparer()).ToList();

                    MessagePrinter.PrintMessage("Tag Heuer collection processed");
                    MessagePrinter.PrintMessage($"{counter} - processed.");

                    string filePath = FileHelper.CreateTHFile(FileHelper.GetSettingsPath($"THSuppliersData.csv"), batchCollection);
                    MessagePrinter.PrintMessage($"File path - {filePath}");

                    string filePath1 = FileHelper.CreateNotValidTHFile(FileHelper.GetSettingsPath($"THSuppliersNotValidData.csv"), notValidBatchCollection);
                    MessagePrinter.PrintMessage($"File path for not valid products - {filePath1}");
                }
                else
                {
                    MessagePrinter.PrintMessage("Don't found Tag Heuer file.");
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} {e.StackTrace}", ImportanceLevel.Critical);
            }

            StartOrPushPropertiesThread();
        }

        private SceBatchItem JHFillSceBatchItem<T>(T item, SupplierType supplierType)
        {
            SceBatchItem batchItem = new SceBatchItem();

            try
            {
                JohnHardyDataItem johnHardy = new JohnHardyDataItem();
                ProductType productType = GetType(item);

                batchItem.Action = "Add";
                batchItem.ProductType = "1";
                batchItem.PartNumber = SceBatchHelper.BuildPartNumber(productType, supplierType, item);
                batchItem.ProductTitle = SceBatchHelper.BuildProductTitle(productType, supplierType, item);
                batchItem.AnchorText = SceBatchHelper.BuildAnchorText(productType, supplierType, item, batchItem);
                batchItem.SpiderURL = SceBatchHelper.BuildSpiderURL(productType, supplierType, item, batchItem);
                batchItem.Brand = SceBatchHelper.BuildBrand(productType, supplierType, item);
                batchItem.Description = SceBatchHelper.BuildDescription(productType, supplierType, item);
                batchItem.METADescription = SceBatchHelper.BuildMETADescription(productType, supplierType, item, batchItem);
                batchItem.METAKeywords = SceBatchHelper.BuildMETAKeywords(productType, supplierType, item, batchItem);
                batchItem.GeneralImage = SceBatchHelper.BuildGeneralImage(productType, supplierType, item, extSett);
                batchItem.MSRP = SceBatchHelper.BuildMSRP(productType, supplierType, item);
                batchItem.Jobber = SceBatchHelper.BuildJobber(productType, supplierType, item);
                batchItem.WebPrice = SceBatchHelper.BuildWebPrice(productType, supplierType, item);
                batchItem.CostPrice = SceBatchHelper.BuildCostPrice(productType, supplierType, item);
                batchItem.MainCategory = SceBatchHelper.BuildMainCategory(productType, supplierType, item);
                batchItem.SubCategory = SceBatchHelper.BuildSubCategory(productType, supplierType, item);
                batchItem.SectionCategory = SceBatchHelper.BuildSectionCategory(productType, supplierType, item);
                batchItem.CrossSellSubCat1 = SceBatchHelper.BuildCrossSellSubCat1(productType, supplierType, item);
                batchItem.CrossSellSecCat1 = SceBatchHelper.BuildCrossSellSecCat1(productType, supplierType, item);
                batchItem.CrossSellMainCat1 = SceBatchHelper.BuildCrossSellMainCat1(productType, supplierType, item);
                batchItem.CrossSellMainCat2 = SceBatchHelper.BuildCrossSellMainCat2(productType, supplierType, item);
                batchItem.CrossSellSubCat2 = SceBatchHelper.BuildCrossSellSubCat2(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat2 = SceBatchHelper.BuildCrossSellSecCat2(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat3 = SceBatchHelper.BuildCrossSellMainCat3(productType, supplierType, item);
                batchItem.CrossSellSubCat3 = SceBatchHelper.BuildCrossSellSubCat3(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat3 = SceBatchHelper.BuildCrossSellSecCat3(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat4 = SceBatchHelper.BuildCrossSellMainCat4(productType, supplierType, item);
                batchItem.CrossSellSubCat4 = SceBatchHelper.BuildCrossSellSubCat4(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat4 = SceBatchHelper.BuildCrossSellSecCat4(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat5 = SceBatchHelper.BuildCrossSellMainCat5(productType, supplierType, item);
                batchItem.CrossSellSubCat5 = SceBatchHelper.BuildCrossSellSubCat5(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat5 = SceBatchHelper.BuildCrossSellSecCat5(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat6 = SceBatchHelper.BuildCrossSellMainCat6(productType, supplierType, item);
                batchItem.CrossSellSubCat6 = SceBatchHelper.BuildCrossSellSubCat6(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat6 = SceBatchHelper.BuildCrossSellSecCat6(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat7 = SceBatchHelper.BuildCrossSellSubCat7(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat7 = SceBatchHelper.BuildCrossSellSecCat7(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat7 = SceBatchHelper.BuildCrossSellMainCat7(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat8 = SceBatchHelper.BuildCrossSellSubCat8(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat8 = SceBatchHelper.BuildCrossSellSecCat8(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat8 = SceBatchHelper.BuildCrossSellMainCat8(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat9 = SceBatchHelper.BuildCrossSellSubCat9(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat9 = SceBatchHelper.BuildCrossSellSecCat9(productType, supplierType, item);
                batchItem.CrossSellMainCat9 = SceBatchHelper.BuildCrossSellMainCat9(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat10 = SceBatchHelper.BuildCrossSellMainCat10(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat10 = SceBatchHelper.BuildCrossSellSubCat10(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat10 = SceBatchHelper.BuildCrossSellSecCat10(productType, supplierType, item);
                batchItem.CrossSellMainCat11 = SceBatchHelper.BuildCrossSellMainCat11(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat11 = SceBatchHelper.BuildCrossSellSubCat11(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat11 = SceBatchHelper.BuildCrossSellSecCat11(productType, supplierType, item);
                batchItem.CrossSellMainCat12 = SceBatchHelper.BuildCrossSellMainCat12(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat12 = SceBatchHelper.BuildCrossSellSubCat12(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat12 = SceBatchHelper.BuildCrossSellSecCat12(productType, supplierType, item);
                batchItem.CrossSellMainCat13 = SceBatchHelper.BuildCrossSellMainCat13(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat13 = SceBatchHelper.BuildCrossSellSubCat13(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat13 = SceBatchHelper.BuildCrossSellSecCat13(productType, supplierType, item);
                batchItem.CrossSellMainCat14 = SceBatchHelper.BuildCrossSellMainCat14(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat14 = SceBatchHelper.BuildCrossSellSubCat14(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat14 = SceBatchHelper.BuildCrossSellSecCat14(productType, supplierType, item);
                batchItem.CrossSellMainCat15 = SceBatchHelper.BuildCrossSellMainCat15(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat15 = SceBatchHelper.BuildCrossSellSubCat15(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat15 = SceBatchHelper.BuildCrossSellSecCat15(productType, supplierType, item);
                batchItem.CrossSellMainCat16 = SceBatchHelper.BuildCrossSellMainCat16(productType, supplierType, item);
                batchItem.CrossSellSubCat16 = SceBatchHelper.BuildCrossSellSubCat16(productType, supplierType, item);
                batchItem.CrossSellSecCat16 = SceBatchHelper.BuildCrossSellSecCat16(productType, supplierType, item);
                batchItem.CrossSellMainCat17 = SceBatchHelper.BuildCrossSellMainCat17(productType, supplierType, item);
                batchItem.CrossSellSubCat17 = SceBatchHelper.BuildCrossSellSubCat17(productType, supplierType, item);
                batchItem.CrossSellSecCat17 = SceBatchHelper.BuildCrossSellSecCat17(productType, supplierType, item);
                batchItem.CrossSellMainCat18 = SceBatchHelper.BuildCrossSellMainCat18(productType, supplierType, item);
                batchItem.CrossSellSubCat18 = SceBatchHelper.BuildCrossSellSubCat18(productType, supplierType, item);
                batchItem.CrossSellSecCat18 = SceBatchHelper.BuildCrossSellSecCat18(productType, supplierType, item);
                batchItem.CrossSellMainCat19 = SceBatchHelper.BuildCrossSellMainCat19(productType, supplierType, item);
                batchItem.CrossSellSubCat19 = SceBatchHelper.BuildCrossSellSubCat19(productType, supplierType, item);
                batchItem.CrossSellSecCat19 = SceBatchHelper.BuildCrossSellSecCat19(productType, supplierType, item);
                batchItem.CrossSellMainCat20 = SceBatchHelper.BuildCrossSellMainCat20(productType, supplierType, item);
                batchItem.CrossSellSubCat20 = SceBatchHelper.BuildCrossSellSubCat20(productType, supplierType, item);
                batchItem.CrossSellSecCat20 = SceBatchHelper.BuildCrossSellSecCat20(productType, supplierType, item);
                batchItem.CustomHtmlAboveQty = SceBatchHelper.BuildCustomHtmlAboveQty(productType, supplierType, item);
                batchItem.Specifications = SceBatchHelper.BuildSpecifications(productType, supplierType, item, batchItem);
                batchItem.SubTitle = SceBatchHelper.BuildSubTitle(productType, supplierType, item, batchItem);
                batchItem.Supplier = SceBatchHelper.BuildSupplier(productType, supplierType, item);
                batchItem.ReOrderSupplier = SceBatchHelper.BuildReOrderSupplier(productType, supplierType, item);
                batchItem.Warehouse = SceBatchHelper.BuildWarehouse(productType, supplierType, item);
                batchItem.ProcessingTime = SceBatchHelper.BuildProcessingTime(productType, supplierType, item);
                batchItem.ShippingType = SceBatchHelper.BuildShippingType(productType, supplierType, item);
                batchItem.ShippingCarrier1 = SceBatchHelper.BuildShippingCarrier1(productType, supplierType, item);
                batchItem.Allowground = SceBatchHelper.BuildAllowground(productType, supplierType, item);
                batchItem.Allow3day = SceBatchHelper.BuildAllow3day(productType, supplierType, item);
                batchItem.Allow2day = SceBatchHelper.BuildAllow2day(productType, supplierType, item);
                batchItem.Allownextday = SceBatchHelper.BuildAllownextday(productType, supplierType, item);
                batchItem.ShippingGroundRate = SceBatchHelper.BuildShippingGroundRate(productType, supplierType, item);
                batchItem.ShippingNextDayAirRate = SceBatchHelper.BuildShippingNextDayAirRate(productType, supplierType, item);
                batchItem.ItemWeight = SceBatchHelper.BuildItemWeight(productType, supplierType, item);
                batchItem.ItemHeight = SceBatchHelper.BuildItemHeight(productType, supplierType, item);
                batchItem.ItemWidth = SceBatchHelper.BuildItemWidth(productType, supplierType, item);
                batchItem.ItemLength = SceBatchHelper.BuildItemLength(productType, supplierType, item);
                batchItem.ShippingWeight = SceBatchHelper.BuildShippingWeight(productType, supplierType, item);
                batchItem.ShippingHeight = SceBatchHelper.BuildShippingHeight(productType, supplierType, item);
                batchItem.ShippingWidth = SceBatchHelper.BuildShippingWidth(productType, supplierType, item);
                batchItem.ShippingLength = SceBatchHelper.BuildShippingLength(productType, supplierType, item);
                batchItem.AllowNewCategory = SceBatchHelper.BuildAllowNewCategory(productType, supplierType, item);
                batchItem.AllowNewBrand = SceBatchHelper.BuildAllowNewBrand(productType, supplierType, item);
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error in item [{batchItem.PartNumber}] - {e.Message} {e.StackTrace}", ImportanceLevel.High);
            }

            return batchItem;
        }

        private SceBatchItem THFillSceBatchItem<T>(T item, SupplierType supplierType)
        {
            SceBatchItem batchItem = new SceBatchItem();

            try
            {
                TagHeuerDataItem tagHeuer = new TagHeuerDataItem();
                ProductType productType = GetType(item);

                batchItem.Action = "Add";
                batchItem.ProductType = "1";
                batchItem.ProductTitle = SceBatchHelper.BuildProductTitle(productType, supplierType, item);
                batchItem.AnchorText = SceBatchHelper.BuildAnchorText(productType, supplierType, item, batchItem);
                batchItem.SpiderURL = SceBatchHelper.BuildSpiderURL(productType, supplierType, item, batchItem);
                batchItem.Brand = SceBatchHelper.BuildBrand(productType, supplierType, item);
                batchItem.Description = SceBatchHelper.BuildDescription(productType, supplierType, item);
                batchItem.METADescription = SceBatchHelper.BuildMETADescription(productType, supplierType, item, batchItem);
                batchItem.METAKeywords = SceBatchHelper.BuildMETAKeywords(productType, supplierType, item, batchItem);
                batchItem.GeneralImage = SceBatchHelper.BuildGeneralImage(productType, supplierType, item, extSett);
                batchItem.PartNumber = SceBatchHelper.BuildPartNumber(productType, supplierType, item);
                batchItem.MSRP = SceBatchHelper.BuildMSRP(productType, supplierType, item);
                batchItem.Jobber = SceBatchHelper.BuildJobber(productType, supplierType, item);
                batchItem.WebPrice = SceBatchHelper.BuildWebPrice(productType, supplierType, item);
                batchItem.CostPrice = SceBatchHelper.BuildCostPrice(productType, supplierType, item);
                batchItem.Supplier = SceBatchHelper.BuildSupplier(productType, supplierType, item);
                batchItem.ReOrderSupplier = SceBatchHelper.BuildReOrderSupplier(productType, supplierType, item);
                batchItem.Warehouse = SceBatchHelper.BuildWarehouse(productType, supplierType, item);
                batchItem.ProcessingTime = SceBatchHelper.BuildProcessingTime(productType, supplierType, item);
                batchItem.ShippingType = SceBatchHelper.BuildShippingType(productType, supplierType, item);
                batchItem.ShippingCarrier1 = SceBatchHelper.BuildShippingCarrier1(productType, supplierType, item);
                batchItem.Allowground = SceBatchHelper.BuildAllowground(productType, supplierType, item);
                batchItem.Allow3day = SceBatchHelper.BuildAllow3day(productType, supplierType, item);
                batchItem.Allow2day = SceBatchHelper.BuildAllow2day(productType, supplierType, item);
                batchItem.Allownextday = SceBatchHelper.BuildAllownextday(productType, supplierType, item);
                batchItem.ShippingGroundRate = SceBatchHelper.BuildShippingGroundRate(productType, supplierType, item);
                batchItem.ShippingNextDayAirRate = SceBatchHelper.BuildShippingNextDayAirRate(productType, supplierType, item);
                batchItem.ItemWeight = SceBatchHelper.BuildItemWeight(productType, supplierType, item);
                batchItem.ItemHeight = SceBatchHelper.BuildItemHeight(productType, supplierType, item);
                batchItem.ItemWidth = SceBatchHelper.BuildItemWidth(productType, supplierType, item);
                batchItem.ItemLength = SceBatchHelper.BuildItemLength(productType, supplierType, item);
                batchItem.ShippingWeight = SceBatchHelper.BuildShippingWeight(productType, supplierType, item);
                batchItem.ShippingHeight = SceBatchHelper.BuildShippingHeight(productType, supplierType, item);
                batchItem.ShippingWidth = SceBatchHelper.BuildShippingWidth(productType, supplierType, item);
                batchItem.ShippingLength = SceBatchHelper.BuildShippingLength(productType, supplierType, item);
                batchItem.AllowNewCategory = SceBatchHelper.BuildAllowNewCategory(productType, supplierType, item);
                batchItem.AllowNewBrand = SceBatchHelper.BuildAllowNewBrand(productType, supplierType, item);
                batchItem.MainCategory = SceBatchHelper.BuildMainCategory(productType, supplierType, item);
                batchItem.SubCategory = SceBatchHelper.BuildSubCategory(productType, supplierType, item);
                batchItem.SectionCategory = SceBatchHelper.BuildSectionCategory(productType, supplierType, item);
                batchItem.CrossSellMainCat1 = SceBatchHelper.BuildCrossSellMainCat1(productType, supplierType, item);
                batchItem.CrossSellSubCat1 = SceBatchHelper.BuildCrossSellSubCat1(productType, supplierType, item);
                batchItem.CrossSellSecCat1 = SceBatchHelper.BuildCrossSellSecCat1(productType, supplierType, item);
                batchItem.CrossSellMainCat2 = SceBatchHelper.BuildCrossSellMainCat2(productType, supplierType, item);
                batchItem.CrossSellSubCat2 = SceBatchHelper.BuildCrossSellSubCat2(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat2 = SceBatchHelper.BuildCrossSellSecCat2(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat3 = SceBatchHelper.BuildCrossSellMainCat3(productType, supplierType, item);
                batchItem.CrossSellSubCat3 = SceBatchHelper.BuildCrossSellSubCat3(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat3 = SceBatchHelper.BuildCrossSellSecCat3(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat4 = SceBatchHelper.BuildCrossSellMainCat4(productType, supplierType, item);
                batchItem.CrossSellSubCat4 = SceBatchHelper.BuildCrossSellSubCat4(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat4 = SceBatchHelper.BuildCrossSellSecCat4(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat5 = SceBatchHelper.BuildCrossSellMainCat5(productType, supplierType, item);
                batchItem.CrossSellSubCat5 = SceBatchHelper.BuildCrossSellSubCat5(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat5 = SceBatchHelper.BuildCrossSellSecCat5(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat6 = SceBatchHelper.BuildCrossSellMainCat6(productType, supplierType, item);
                batchItem.CrossSellSubCat6 = SceBatchHelper.BuildCrossSellSubCat6(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat6 = SceBatchHelper.BuildCrossSellSecCat6(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat7 = SceBatchHelper.BuildCrossSellSecCat7(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat7 = SceBatchHelper.BuildCrossSellMainCat7(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat7 = SceBatchHelper.BuildCrossSellSubCat7(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat8 = SceBatchHelper.BuildCrossSellSecCat8(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat8 = SceBatchHelper.BuildCrossSellMainCat8(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat8 = SceBatchHelper.BuildCrossSellSubCat8(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat9 = SceBatchHelper.BuildCrossSellSecCat9(productType, supplierType, item);
                batchItem.CrossSellMainCat9 = SceBatchHelper.BuildCrossSellMainCat9(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat9 = SceBatchHelper.BuildCrossSellSubCat9(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat10 = SceBatchHelper.BuildCrossSellSecCat10(productType, supplierType, item);
                batchItem.CrossSellMainCat10 = SceBatchHelper.BuildCrossSellMainCat10(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat10 = SceBatchHelper.BuildCrossSellSubCat10(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat11 = SceBatchHelper.BuildCrossSellSecCat11(productType, supplierType, item);
                batchItem.CrossSellMainCat11 = SceBatchHelper.BuildCrossSellMainCat11(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat11 = SceBatchHelper.BuildCrossSellSubCat11(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat12 = SceBatchHelper.BuildCrossSellSecCat12(productType, supplierType, item);
                batchItem.CrossSellMainCat12 = SceBatchHelper.BuildCrossSellMainCat12(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat12 = SceBatchHelper.BuildCrossSellSubCat12(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat13 = SceBatchHelper.BuildCrossSellSecCat13(productType, supplierType, item);
                batchItem.CrossSellMainCat13 = SceBatchHelper.BuildCrossSellMainCat13(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat13 = SceBatchHelper.BuildCrossSellSubCat13(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat14 = SceBatchHelper.BuildCrossSellSecCat14(productType, supplierType, item);
                batchItem.CrossSellMainCat14 = SceBatchHelper.BuildCrossSellMainCat14(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat14 = SceBatchHelper.BuildCrossSellSubCat14(productType, supplierType, item, batchItem);
                batchItem.CrossSellSecCat15 = SceBatchHelper.BuildCrossSellSecCat15(productType, supplierType, item);
                batchItem.CrossSellMainCat15 = SceBatchHelper.BuildCrossSellMainCat15(productType, supplierType, item, batchItem);
                batchItem.CrossSellSubCat15 = SceBatchHelper.BuildCrossSellSubCat15(productType, supplierType, item, batchItem);
                batchItem.CrossSellMainCat16 = SceBatchHelper.BuildCrossSellMainCat16(productType, supplierType, item);
                batchItem.CrossSellSubCat16 = SceBatchHelper.BuildCrossSellSubCat16(productType, supplierType, item);
                batchItem.CrossSellSecCat16 = SceBatchHelper.BuildCrossSellSecCat16(productType, supplierType, item);
                batchItem.CrossSellMainCat17 = SceBatchHelper.BuildCrossSellMainCat17(productType, supplierType, item);
                batchItem.CrossSellSubCat17 = SceBatchHelper.BuildCrossSellSubCat17(productType, supplierType, item);
                batchItem.CrossSellSecCat17 = SceBatchHelper.BuildCrossSellSecCat17(productType, supplierType, item);
                batchItem.CrossSellMainCat18 = SceBatchHelper.BuildCrossSellMainCat18(productType, supplierType, item);
                batchItem.CrossSellSubCat18 = SceBatchHelper.BuildCrossSellSubCat18(productType, supplierType, item);
                batchItem.CrossSellSecCat18 = SceBatchHelper.BuildCrossSellSecCat18(productType, supplierType, item);
                batchItem.CrossSellMainCat19 = SceBatchHelper.BuildCrossSellMainCat19(productType, supplierType, item);
                batchItem.CrossSellSubCat19 = SceBatchHelper.BuildCrossSellSubCat19(productType, supplierType, item);
                batchItem.CrossSellSecCat19 = SceBatchHelper.BuildCrossSellSecCat19(productType, supplierType, item);
                batchItem.CrossSellMainCat20 = SceBatchHelper.BuildCrossSellMainCat20(productType, supplierType, item);
                batchItem.CrossSellSubCat20 = SceBatchHelper.BuildCrossSellSubCat20(productType, supplierType, item);
                batchItem.CrossSellSecCat20 = SceBatchHelper.BuildCrossSellSecCat20(productType, supplierType, item);
                batchItem.CrossSellMainCat21 = SceBatchHelper.BuildCrossSellMainCat21(productType, supplierType, item);
                batchItem.CrossSellSubCat21 = SceBatchHelper.BuildCrossSellSubCat21(productType, supplierType, item);
                batchItem.CrossSellSecCat21 = SceBatchHelper.BuildCrossSellSecCat21(productType, supplierType, item);
                batchItem.CrossSellMainCat22 = SceBatchHelper.BuildCrossSellMainCat22(productType, supplierType, item);
                batchItem.CrossSellSubCat22 = SceBatchHelper.BuildCrossSellSubCat22(productType, supplierType, item);
                batchItem.CrossSellSecCat22 = SceBatchHelper.BuildCrossSellSecCat22(productType, supplierType, item);
                batchItem.CrossSellMainCat23 = SceBatchHelper.BuildCrossSellMainCat23(productType, supplierType, item);
                batchItem.CrossSellSubCat23 = SceBatchHelper.BuildCrossSellSubCat23(productType, supplierType, item);
                batchItem.CrossSellSecCat23 = SceBatchHelper.BuildCrossSellSecCat23(productType, supplierType, item);
                batchItem.CrossSellMainCat24 = SceBatchHelper.BuildCrossSellMainCat24(productType, supplierType, item);
                batchItem.CrossSellSubCat24 = SceBatchHelper.BuildCrossSellSubCat24(productType, supplierType, item);
                batchItem.CrossSellSecCat24 = SceBatchHelper.BuildCrossSellSecCat24(productType, supplierType, item);
                batchItem.CrossSellMainCat25 = SceBatchHelper.BuildCrossSellMainCat25(productType, supplierType, item);
                batchItem.CrossSellSubCat25 = SceBatchHelper.BuildCrossSellSubCat25(productType, supplierType, item);
                batchItem.CrossSellSecCat25 = SceBatchHelper.BuildCrossSellSecCat25(productType, supplierType, item);
                batchItem.CrossSellMainCat26 = SceBatchHelper.BuildCrossSellMainCat26(productType, supplierType, item);
                batchItem.CrossSellSubCat26 = SceBatchHelper.BuildCrossSellSubCat26(productType, supplierType, item);
                batchItem.CrossSellSecCat26 = SceBatchHelper.BuildCrossSellSecCat26(productType, supplierType, item);
                batchItem.CrossSellMainCat27 = SceBatchHelper.BuildCrossSellMainCat27(productType, supplierType, item);
                batchItem.CrossSellSubCat27 = SceBatchHelper.BuildCrossSellSubCat27(productType, supplierType, item);
                batchItem.CrossSellSecCat27 = SceBatchHelper.BuildCrossSellSecCat27(productType, supplierType, item);
                batchItem.CrossSellMainCat28 = SceBatchHelper.BuildCrossSellMainCat28(productType, supplierType, item);
                batchItem.CrossSellSubCat28 = SceBatchHelper.BuildCrossSellSubCat28(productType, supplierType, item);
                batchItem.CrossSellSecCat28 = SceBatchHelper.BuildCrossSellSecCat28(productType, supplierType, item);
                batchItem.CrossSellMainCat29 = SceBatchHelper.BuildCrossSellMainCat29(productType, supplierType, item);
                batchItem.CrossSellSubCat29 = SceBatchHelper.BuildCrossSellSubCat29(productType, supplierType, item);
                batchItem.CrossSellSecCat29 = SceBatchHelper.BuildCrossSellSecCat29(productType, supplierType, item);
                batchItem.CrossSellMainCat30 = SceBatchHelper.BuildCrossSellMainCat30(productType, supplierType, item);
                batchItem.CrossSellSubCat30 = SceBatchHelper.BuildCrossSellSubCat30(productType, supplierType, item);
                batchItem.CrossSellSecCat30 = SceBatchHelper.BuildCrossSellSecCat30(productType, supplierType, item);
                batchItem.CrossSellMainCat31 = SceBatchHelper.BuildCrossSellMainCat31(productType, supplierType, item);
                batchItem.CrossSellSubCat31 = SceBatchHelper.BuildCrossSellSubCat31(productType, supplierType, item);
                batchItem.CrossSellSecCat31 = SceBatchHelper.BuildCrossSellSecCat31(productType, supplierType, item);
                batchItem.CrossSellMainCat32 = SceBatchHelper.BuildCrossSellMainCat32(productType, supplierType, item);
                batchItem.CrossSellSubCat32 = SceBatchHelper.BuildCrossSellSubCat32(productType, supplierType, item);
                batchItem.CrossSellSecCat32 = SceBatchHelper.BuildCrossSellSecCat32(productType, supplierType, item);
                batchItem.CrossSellMainCat33 = SceBatchHelper.BuildCrossSellMainCat33(productType, supplierType, item);
                batchItem.CrossSellSubCat33 = SceBatchHelper.BuildCrossSellSubCat33(productType, supplierType, item);
                batchItem.CrossSellSecCat33 = SceBatchHelper.BuildCrossSellSecCat33(productType, supplierType, item);
                batchItem.CrossSellMainCat34 = SceBatchHelper.BuildCrossSellMainCat34(productType, supplierType, item);
                batchItem.CrossSellSubCat34 = SceBatchHelper.BuildCrossSellSubCat34(productType, supplierType, item);
                batchItem.CrossSellSecCat34 = SceBatchHelper.BuildCrossSellSecCat34(productType, supplierType, item);
                batchItem.Specifications = SceBatchHelper.BuildSpecifications(productType, supplierType, item, batchItem);
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error in item [{batchItem.PartNumber}] - {e.Message} {e.StackTrace}", ImportanceLevel.High);
            }

            return batchItem;
        }

        private ProductType GetType<T>(T item)
        {
            ProductType type = ProductType.Error;

            if (item is JohnHardyDataItem)
            {
                var localItem = item as JohnHardyDataItem;

                switch (localItem.ProductType)
                {
                    case "Bracelet":
                        type = ProductType.Bracelet;
                        break;
                    case "Earrings":
                        type = ProductType.Earrings;
                        break;
                    case "Necklace":
                        type = ProductType.Necklace;
                        break;
                    case "Cufflinks":
                        type = ProductType.Cufflinks;
                        break;
                    case "Ring":
                        type = ProductType.Ring;
                        break;
                    default:
                        type = ProductType.Error;
                        break;
                }
            }
            else if (item is TagHeuerDataItem)
            {
                type = ProductType.Timepieces;
            }

            return type;
        }
    }
}
