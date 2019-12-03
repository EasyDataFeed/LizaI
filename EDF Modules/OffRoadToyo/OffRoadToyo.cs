using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using OffRoadToyo;
using Databox.Libs.OffRoadToyo;
using OffRoadToyo.Helpers;
using OffRoadToyo.DataItems;

namespace WheelsScraper
{
    public class OffRoadToyo : BaseScraper
    {
        public OffRoadToyo()
        {
            Name = "OffRoadToyo";
            Url = "https://www.OffRoadToyo.com/";
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
                List<FileItems> fileItems = new List<FileItems>();
                var tiresJson = FileApiHelper.GetTiresList(extSett.TiresFilePath, out string errorTires);

                foreach (var tire in tiresJson.tires)
                {
                    var tirepatternJson = FileApiHelper.GetTirepattern(tire.sizes_url, out string errorTirepattern);

                    if (tirepatternJson != null)
                    {
                        foreach (var tirepattern in tirepatternJson.tirepattern)
                        {
                            #region const

                            string spec = "Specifications##";
                            string approvedRimWidth = string.Empty;
                            string diameter = string.Empty;
                            string dualMaxLoad = string.Empty;
                            string dualPsi = string.Empty;
                            string loadID = string.Empty;
                            string loadIndex = string.Empty;
                            string maxLoadSingleDual = string.Empty;
                            string overallDiameter = string.Empty;
                            string overallWidth = string.Empty;
                            string plyRating = string.Empty;
                            string productWarranty = string.Empty;
                            string ratio = string.Empty;
                            string width = string.Empty;
                            string revsPerMile = string.Empty;
                            string sidewall = string.Empty;
                            string speedRating = string.Empty;
                            string treadDepth = string.Empty;
                            string uTQG = string.Empty;
                            string approvedRimWidthRange = string.Empty;
                            string construction = string.Empty;
                            string loadWidth = string.Empty;
                            string maxLoadSingle = string.Empty;
                            string maxPSISingle = string.Empty;
                            string mPH = string.Empty;
                            string sidewallConstruction = string.Empty;
                            string staticLoadRadius = string.Empty;
                            string staticLoadWidth = string.Empty;
                            string tireSize = string.Empty;
                            string treadWidth = string.Empty;

                            #endregion

                            #region Specifications

                            if (!string.IsNullOrEmpty(tirepattern.tire.RimWidth))
                            {
                                approvedRimWidth = $"Approved Rim Width (in)~{tirepattern.tire.RimWidth}^";
                            }

                            if (!string.IsNullOrEmpty(tirepattern.tire.ApprovedRimWidthRangeinch))
                            {
                                diameter = $"Diameter~{tirepattern.tire.ApprovedRimWidthRangeinch}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.MaxLoadDual))
                            {
                                dualMaxLoad = $"Dual Max Load~{tirepattern.tire.MaxLoadDual}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.MaxPSIDual))
                            {
                                dualPsi = $"Dual Psi~{tirepattern.tire.MaxPSIDual}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.LoadRange))
                            {
                                loadID = $"Load ID~{tirepattern.tire.LoadRange}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.LoadIndexSS))
                            {
                                string loadIndexChar = string.Empty;
                                foreach (var item in tirepattern.tire.LoadIndexSS)
                                {
                                    if (char.IsNumber(item) || char.IsPunctuation(item))
                                    {
                                        loadIndexChar += item;
                                    }
                                }

                                loadIndex = $"Load Index~{ loadIndexChar}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.MaxLoadDual) && !string.IsNullOrEmpty(tirepattern.tire.MaxLoadSingle))
                            {
                                maxLoadSingleDual = $"Max Load Single / Dual (lbs)~{tirepattern.tire.MaxLoadDual} / {tirepattern.tire.MaxLoadSingle}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.OverallDiameterinch))
                            {
                                overallDiameter = $"Overall Diameter (in)~{tirepattern.tire.OverallDiameterinch}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.OverallWidthinch))
                            {
                                overallWidth = $"Overall Width (in)~{tirepattern.tire.OverallWidthinch}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.PlyRating))
                            {
                                plyRating = $"Ply Rating~{tirepattern.tire.PlyRating}^";
                            }
                            if (!string.IsNullOrEmpty(tire.warranty))
                            {
                                productWarranty = $"Product Warranty~{tire.warranty}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.TireSize))
                            {
                                if (tirepattern.tire.TireSize.ToLower().Contains("r"))
                                {
                                    string ratioChar = string.Empty;
                                    string widthChar = string.Empty;
                                    var tireSizeSplit = tirepattern.tire.TireSize.ToLower().Split('r');
                                    var ratioSplit = tireSizeSplit.Length > 0 ? tireSizeSplit[0] : string.Empty;
                                    foreach (var item in ratioSplit)
                                    {
                                        if (char.IsDigit(item))
                                        {
                                            ratioChar += item;
                                        }
                                    }

                                    var widthSplit = tireSizeSplit.Length > 0 ? tireSizeSplit[1] : string.Empty;
                                    foreach (var item in widthSplit)
                                    {
                                        if (char.IsDigit(item))
                                        {
                                            widthChar += item;
                                        }
                                        else if (char.IsLetter(item))
                                            break;
                                    }

                                    if (!string.IsNullOrEmpty(ratioChar))
                                    {
                                        ratio = $"Ratio~{ParseDouble(ratioChar)}^";
                                    }
                                    if (!string.IsNullOrEmpty(widthChar))
                                    {
                                        width = $"Width~{ParseDouble(widthChar)}^";
                                    }
                                }
                                if (tirepattern.tire.TireSize.ToLower().Contains("x"))
                                {
                                    string ratioChar = string.Empty;
                                    string widthChar = string.Empty;
                                    var tireSizeSplit = tirepattern.tire.TireSize.ToLower().Split('x');
                                    var ratioSplit = tireSizeSplit.Length > 0 ? tireSizeSplit[0] : string.Empty;
                                    foreach (var item in ratioSplit)
                                    {
                                        if (char.IsDigit(item))
                                        {
                                            ratioChar += item;
                                        }
                                    }

                                    var widthSplit = tireSizeSplit.Length > 0 ? tireSizeSplit[1] : string.Empty;
                                    foreach (var item in widthSplit)
                                    {
                                        if (char.IsDigit(item) || char.IsPunctuation(item))
                                        {
                                            widthChar += item;
                                        }
                                        else if (char.IsLetter(item))
                                            break;
                                    }

                                    if (!string.IsNullOrEmpty(ratioChar))
                                    {
                                        ratio = $"Ratio~{ParseDouble(ratioChar)}^";
                                    }
                                    if (!string.IsNullOrEmpty(widthChar))
                                    {
                                        width = $"Width~{ParseDouble(widthChar)}^";
                                    }
                                }
                                if (tirepattern.tire.TireSize.Contains("/"))
                                {
                                    string ratioChar = string.Empty;
                                    string widthChar = string.Empty;
                                    var tireSizeSplit = tirepattern.tire.TireSize.Split('/');
                                    var ratioSplit = tireSizeSplit.Length > 0 ? tireSizeSplit[0] : string.Empty;
                                    foreach (var item in ratioSplit)
                                    {
                                        if (char.IsDigit(item))
                                        {
                                            ratioChar += item;
                                        }
                                    }

                                    var widthSplit = tireSizeSplit.Length > 0 ? tireSizeSplit[1] : string.Empty;
                                    foreach (var item in widthSplit)
                                    {
                                        if (char.IsDigit(item))
                                        {
                                            widthChar += item;
                                        }
                                        else if (char.IsLetter(item))
                                            break;
                                    }

                                    if (!string.IsNullOrEmpty(ratioChar))
                                    {
                                        ratio = $"Ratio~{ParseDouble(ratioChar)}^";
                                    }
                                    if (!string.IsNullOrEmpty(widthChar))
                                    {
                                        width = $"Width~{ParseDouble(widthChar)}^";
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.quotRevsMile))
                            {
                                revsPerMile = $"Revs Per Mile~{tirepattern.tire.quotRevsMile}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.Sidewall))
                            {
                                sidewall = $"Side-wall~{tirepattern.tire.Sidewall}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.LoadIndexSS))
                            {
                                string speedRatingChar = string.Empty;
                                foreach (var item in tirepattern.tire.LoadIndexSS)
                                {
                                    if (char.IsLetter(item))
                                    {
                                        speedRatingChar += item;
                                    }
                                }

                                speedRating = $"Speed Rating~{speedRatingChar}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.TreadDepth))
                            {
                                treadDepth = $"Tread Depth~{tirepattern.tire.TreadDepth}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.UTQG))
                            {
                                uTQG = $"U.T.Q.G.~{tirepattern.tire.UTQG}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.ApprovedRimWidthRangeinch))
                            {
                                approvedRimWidthRange = $"Approved Rim Width Range (inch)~{tirepattern.tire.ApprovedRimWidthRangeinch}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.Construction))
                            {
                                construction = $"Construction~{tirepattern.tire.Construction}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.LoadWidth))
                            {
                                loadWidth = $"Load Width~{tirepattern.tire.LoadWidth}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.MaxLoadSingle))
                            {
                                maxLoadSingle = $"Max Load Single~{tirepattern.tire.MaxLoadSingle}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.MaxPSISingle))
                            {
                                maxPSISingle = $"Max PSI Single~{tirepattern.tire.MaxPSISingle}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.MPH))
                            {
                                mPH = $"MPH~{tirepattern.tire.MPH}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.SidewallConstruction))
                            {
                                sidewallConstruction = $"Sidewall Construction~{tirepattern.tire.SidewallConstruction}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.StaticLoadRadius))
                            {
                                staticLoadRadius = $"Static Load Radius~{tirepattern.tire.StaticLoadRadius}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.StaticLoadWidth))
                            {
                                staticLoadWidth = $"Static Load Width~{tirepattern.tire.StaticLoadWidth}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.TireSize))
                            {
                                tireSize = $"Tire Size~{tirepattern.tire.TireSize}^";
                            }
                            if (!string.IsNullOrEmpty(tirepattern.tire.TreadWidth))
                            {
                                treadWidth = $"Tread Width~{tirepattern.tire.TreadWidth}^";
                            }

                            #endregion

                            fileItems.Add(new FileItems()
                            {
                                Action = "add",
                                ProductType = "1",
                                ProductTitle = $"{tire.title} {tirepattern.tire.TireSize} {tirepattern.tire.ProductCode}".Replace("&", "and")
                                                                                                                              .Replace("0x", "0 x")
                                                                                                                              .Replace("x0", "x 0")
                                                                                                                              .Replace("X0", "X 0")
                                                                                                                              .Replace("0X", "0 X")
                                                                                                                              .Replace("--", "-"),

                                AnchorText = $"{tire.title} {tirepattern.tire.TireSize} {tirepattern.tire.ProductCode}".Replace("&", "and")
                                                                                                                              .Replace("0x", "0 x")
                                                                                                                              .Replace("x0", "x 0")
                                                                                                                              .Replace("X0", "X 0")
                                                                                                                              .Replace("0X", "0 X")
                                                                                                                              .Replace("--", "-"),

                                SpiderURL = $"{tire.title} {tirepattern.tire.TireSize} {tirepattern.tire.ProductCode}".Replace("&", "and")
                                                                                                                              .Replace("0x", "0 x")
                                                                                                                              .Replace("x0", "x 0")
                                                                                                                              .Replace("X0", "X 0")
                                                                                                                              .Replace("0X", "0 X")
                                                                                                                              .Replace("--", "-"),

                                AllowNewBrand = "1",
                                Brand = "Toyo Tiers",
                                Description = tire.description,
                                METADescription = $"{tire.title} {tirepattern.tire.TireSize} {tirepattern.tire.ProductCode}".Replace("&", "and")
                                                                                                                              .Replace("0x", "0 x")
                                                                                                                              .Replace("x0", "x 0")
                                                                                                                              .Replace("X0", "X 0")
                                                                                                                              .Replace("0X", "0 X")
                                                                                                                              .Replace("--", "-"),

                                METAKeywords = $"{tire.title} {tirepattern.tire.TireSize} {tirepattern.tire.ProductCode}".Replace("&", "and")
                                                                                                                              .Replace("0x", "0 x")
                                                                                                                              .Replace("x0", "x 0")
                                                                                                                              .Replace("X0", "X 0")
                                                                                                                              .Replace("0X", "0 X")
                                                                                                                              .Replace("--", "-"),
                                GeneralImage = tire.image.Trim(','),
                                AllowNewCategory = "1",
                                MainCategory = "Wheels & Tires",
                                SubCategory = "Tires",
                                SectionCategory = "",
                                Specifications = $"{spec}{approvedRimWidth}{diameter}{dualMaxLoad}{dualPsi}{loadID}{loadIndex}{maxLoadSingleDual}{overallDiameter}{overallWidth}{plyRating}{productWarranty}{ratio}{width}{revsPerMile}" +
                                $"{sidewall}{speedRating}{treadDepth}{uTQG}{approvedRimWidthRange}{construction}{loadWidth}{maxLoadSingle}{maxPSISingle}{mPH}{sidewallConstruction}{staticLoadRadius}{staticLoadWidth}{tireSize}{treadWidth}".Trim('^'),
                                Supplier = "Offroad Warehouse",
                                ReOrderSupplier = "Offroad Warehouse",
                                Warehouse = "ORW-Warehouse",
                                ProcessingTime = "1",
                                Combinable = "0",
                                Consolidatable = "0",
                                PickupAvailable = "",
                                ShippingType = "dynamic",
                                ShippingCarrier1 = "USPS",
                                ShippingCarrier2 = "UPS",
                                ShippingCarrier3 = "FedEX",
                                ShippingCarrier4 = "",
                                ShippingCarrier5 = "",
                                Allowground = "1",
                                Allow3day = "1",
                                Allow2day = "1",
                                Allownextday = "1",
                                Allowinternational = "1",
                                ShippingGroundRate = "",
                                Shipping3DayAirRate = "",
                                Shipping2DayAirRate = "",
                                ShippingNextDayAirRate = "",
                                HandlingSurcharge = "20",
                                FileAttachments = "",
                                Videos = "",
                                SearchTags = "",
                                PartNumber = $"@{tirepattern.tire.ProductCode}",
                                ManufacturerPartNumber = $"@{tirepattern.tire.ProductCode}",
                                EAN = $"@{tirepattern.tire.EAN}",
                                UPC = "",
                                WarningNote = "",
                                MSRP = "",
                                Jobber = "",
                                WebPrice = "",
                                CostPrice = "",
                                ItemWeight = tirepattern.tire.TireWeightlbs,
                                ItemHeight = tirepattern.tire.ApprovedRimWidthRangeinch,
                                ItemWidth = tirepattern.tire.OverallWidthinch,
                                ItemLength = tirepattern.tire.ApprovedRimWidthRangeinch,
                                ShippingWeight = tirepattern.tire.TireWeightlbs,
                                ShippingHeight = tirepattern.tire.ApprovedRimWidthRangeinch,
                                ShippingWidth = tirepattern.tire.OverallWidthinch,
                                ShippingLength = tirepattern.tire.ApprovedRimWidthRangeinch
                            });
                        }

                        MessagePrinter.PrintMessage($"{tire.title} processed");
                    }
                    else
                    {
                        MessagePrinter.PrintMessage($"Error in {tire.title}", ImportanceLevel.Mid);
                    }
                }

                foreach (var item in fileItems)
                {
                    if (!string.IsNullOrEmpty(item.EAN))
                    {
                        if (item.EAN.Contains(","))
                        {
                            var eanSplit = item.EAN.Split(',');
                            var eanOne = eanSplit.Length > 0 ? eanSplit[0] : string.Empty;
                            item.EAN = eanOne;
                        }
                    }
                }

                fileItems = fileItems.Distinct(new FileItems.FileItemsEqualityComparer()).ToList();
                MessagePrinter.PrintMessage($"Create local file");
                string filePath = FileHelper.CreateFile(FileHelper.GetSettingsPath("OffRoadToyo.csv"), fileItems);
                if (!string.IsNullOrEmpty(filePath))
                {
                    MessagePrinter.PrintMessage($"Local file created - {filePath}");
                }
                else
                {
                    MessagePrinter.PrintMessage($"Can't create local scrape file", ImportanceLevel.Critical);
                }
            }
            catch (Exception e)
            {
                
            }

            StartOrPushPropertiesThread();
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
