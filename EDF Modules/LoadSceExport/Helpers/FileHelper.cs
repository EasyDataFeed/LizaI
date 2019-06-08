using LoadSceExport.DataItems;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoadSceExport.Helpers
{
    public class FileHelper
    {
        private const string Separator = ",";
        private const char ComaSeparator = ',';
        private const int CsvReaderBufferSize = 1024 * 1024 * 100;

        public static List<ExtWareInfo> ReadSceExportFile(string filePath)
        {
            List<ExtWareInfo> ftpItems = new List<ExtWareInfo>();

            using (StreamReader sr = File.OpenText(filePath))
            {
                using (CsvReader csv = new CsvReader(sr, true, ComaSeparator, CsvReaderBufferSize))
                {
                    while (csv.ReadNextRecord())
                    {
                        double.TryParse(csv["MSRP"], out double msrp);
                        double.TryParse(csv["Jobber"], out double jobber);
                        double.TryParse(csv["Web Price"], out double webPrice);
                        double.TryParse(csv["Cost Price"], out double costPrice);
                        string partNumber = "";
                        string manufacturerPartNumber = "";
                        string uPC = "";

                        ExtWareInfo item = new ExtWareInfo
                        {
                            Action = csv["Action"],
                            ProdId = csv["Prodid"],
                            ProductType = csv["Product Type"],
                            MerchandiseType = csv["Merchandise Type"],
                            ProductTitle = csv["Product Title"],
                            AnchorText = csv["Anchor Text"],
                            SpiderURL = csv["Spider URL"],
                            AmazonTitle = csv["Amazon Title"],
                            AmazonDescription = csv["Amazon Description"],
                            EBayTitle = csv["eBay Title"],
                            EBayDescription = csv["eBay Description"],
                            SubTitle = csv["Sub Title"],
                            ScreenTitle = csv["Screen Title"],
                            AllowNewBrand = csv["allow new brand"],
                            Brand = csv["Brand"],
                            Description = csv["Description"],
                            AdditionalDescription = csv["Additional Description"],
                            AdditionalDescription2 = csv["Additional Description 2"],
                            AdditionalDescription3 = csv["Additional Description 3"],
                            AdditionalDescription4 = csv["Additional Description 4"],
                            BulletPoint = csv["Bullet Point"],
                            PrimaryKeyword = csv["primary keyword"],
                            SecondaryKeyword = csv["secondary keyword"],
                            TertiaryKeyword = csv["tertiary keyword"],
                            METADescription = csv["META Description"],
                            METAKeywords = csv["META Keywords"],
                            SpiderRule = csv["Spider Rule"],
                            GeneralImageTags = csv["General Image Tags"],
                            GeneralImage = csv["General Image"],
                            AllowNewCategory = csv["allow new category"],
                            MainCategory = csv["Main Category"],
                            MainCategoryID = csv["Main Category ID"],
                            MainCategoryMetaTitle = csv["Main Category Meta Title"],
                            MainCategoryMetaKeywords = csv["Main Category Meta Keywords"],
                            MainCategoryMetaDescription = csv["Main Category Meta Description"],
                            MainCategorySpiderURL = csv["Main Category Spider URL"],
                            MainCategoryRedirectURL = csv["Main Category Redirect URL"],
                            MainCategoryImageURL = csv["Main Category Image URL"],
                            MainCategoryAmazonCategory = csv["Main Category Amazon Category"],
                            MainCategoryAmazonCategoryID = csv["Main Category Amazon Category ID"],
                            SubCategory = csv["Sub Category"],
                            SubCategoryID = csv["Sub Category ID"],
                            SubCategoryMetaTitle = csv["Sub Category Meta Title"],
                            SubCategoryMetaKeywords = csv["Sub Category Meta Keywords"],
                            SubCategoryMetaDescription = csv["Sub Category Meta Description"],
                            SubCategorySpiderURL = csv["Sub Category Spider URL"],
                            SubCategoryRedirectURL = csv["Sub Category Redirect URL"],
                            SubCategoryImageURL = csv["Sub Category Image URL"],
                            SubCategoryAmazonCategory = csv["Sub Category Amazon Category"],
                            SubCategoryAmazonCategoryID = csv["Sub Category Amazon Category ID"],
                            SubCategoryEbayCategory = csv["Sub Category Ebay Category"],
                            SectionCategory = csv["Section Category"],
                            SectionCategoryID = csv["Section Category ID"],
                            SectionCategoryMetaTitle = csv["Section Category Meta Title"],
                            SectionCategoryMetaKeywords = csv["Section Category Meta Keywords"],
                            SectionCategoryMetaDescription = csv["Section Category Meta Description"],
                            SectionCategorySpiderURL = csv["Section Category Spider URL"],
                            SectionCategoryRedirectURL = csv["Section Category Redirect URL"],
                            SectionCategoryImageURL = csv["Section Category Image URL"],
                            SectionCategoryAmazonCategory = csv["Section Category Amazon Category"],
                            SectionCategoryAmazonCategoryID = csv["Section Category Amazon Category ID"],
                            SectionCategoryEbayCategory = csv["Section Category Ebay Category"],
                            CrossSellMainCat1 = csv["Cross sell main cat 1"],
                            CrossSellSubCat1 = csv["Cross sell sub cat 1"],
                            CrossSellSecCat1 = csv["Cross sell sec cat 1"],
                            CrossSellMainCat2 = csv["Cross sell main cat 2"],
                            CrossSellSubCat2 = csv["Cross sell sub cat 2"],
                            CrossSellSecCat2 = csv["Cross sell sec cat 2"],
                            CrossSellMainCat3 = csv["Cross sell main cat 3"],
                            CrossSellSubCat3 = csv["Cross sell sub cat 3"],
                            CrossSellSecCat3 = csv["Cross sell sec cat 3"],
                            CrossSellMainCat4 = csv["Cross sell main cat 4"],
                            CrossSellSubCat4 = csv["Cross sell sub cat 4"],
                            CrossSellSecCat4 = csv["Cross sell sec cat 4"],
                            CrossSellMainCat5 = csv["Cross sell main cat 5"],
                            CrossSellSubCat5 = csv["Cross sell sub cat 5"],
                            CrossSellSecCat5 = csv["Cross sell sec cat 5"],
                            Specifications = csv["Specifications"],
                            Featured = csv["Featured"],
                            Supplier = csv["Supplier"],
                            ReOrderSupplier = csv["Re-Order Supplier"],
                            Warehouse = csv["Warehouse"],
                            Workflow = csv["Workflow"],
                            Redirect301 = csv["301 redirect"],
                            Hidden = csv["Hidden"],
                            Internaluse = csv["internaluse"],
                            ProcessingTime = csv["Processing Time"],
                            OutOfStockProcessingTime = csv["OutOfStock Processing Time"],
                            Combinable = csv["Combinable"],
                            Consolidatable = csv["Consolidatable"],
                            ShippingType = csv["Shipping Type"],
                            ShippingCarrier1 = csv["Shipping Carrier 1"],
                            ShippingCarrier2 = csv["Shipping Carrier 2"],
                            ShippingCarrier3 = csv["Shipping Carrier 3"],
                            ShippingCarrier4 = csv["Shipping Carrier 4"],
                            ShippingCarrier5 = csv["Shipping Carrier 5"],
                            Allowground = csv["allowground"],
                            Allow3day = csv["allow3day"],
                            Allow2day = csv["allow2day"],
                            Allownextday = csv["allownextday"],
                            Allowinternational = csv["allowinternational"],
                            ShippingGroundRate = csv["Shipping Ground Rate"],
                            Shipping3DayAirRate = csv["Shipping 3-Day Air Rate"],
                            Shipping2DayAirRate = csv["Shipping 2-Day Air Rate"],
                            ShippingNextDayAirRate = csv["Shipping Next-Day Air Rate"],
                            ShippingInterNorthAmerica = csv["Shipping Inter. North America"],
                            ShippingInterSouthAmerica = csv["Shipping Inter. South America"],
                            ShippingInterEurope = csv["Shipping Inter. Europe"],
                            ShippingInterAfrica = csv["Shipping Inter. Africa"],
                            ShippingInterAsia = csv["Shipping Inter. Asia"],
                            ShippingInterOceania = csv["Shipping Inter. Oceania"],
                            UpsellProdIDs = csv["upsellProdIDs"],
                            UpsellProdNames = csv["upsellProdNames"],
                            ForceUpsell = csv["Force Upsell"],
                            UpsellProdQty = csv["upsellProdQty"],
                            RelatedProdIDs = csv["relatedProdIDs"],
                            RelatedProdNames = csv["relatedProdNames"],
                            UpsellWizard = csv["Upsell Wizard"],
                            CompareProducts = csv["Compare Products"],
                            AccessoryProducts = csv["Accessory Products"],
                            FileAttachments = csv["File Attachments"],
                            Videos = csv["Videos"],
                            SearchTags = csv["Search Tags"],
                            NegativeSearchTags = csv["Negative Search Tags"],
                            LiveInventory = csv["Live Inventory"],
                            MinQty = csv["Min Qty"],
                            MaxQty = csv["Max Qty"],
                            QtyIncrements = csv["Qty Increments"],
                            PredefinedQty = csv["Predefined Qty"],
                            QtyDiscounts = csv["Qty Discounts"],
                            SortPriority = csv["Sort Priority"],
                            OutOfStockRule = csv["Out Of Stock Rule"],
                            CheckoutRule = csv["Checkout rule"],
                            ShowPricesToPublic = csv["Show Prices To Public"],
                            Expedited = csv["Expedited"],
                            AmazonAffiliateAsin = csv["amazon affiliate asin"],
                            WholesaleLevel = csv["wholesale level"],
                            CustomHtmlAboveQty = csv["custom html above qty"],
                            CustomHtmlBelowThumbnail = csv["custom html below thumbnail"],
                            CustomHtmlAboveUpsells = csv["custom html above upsells"],
                            CustomHtmlAbovePrice = csv["custom html above price"],
                            CustomHtmlBelowPrice = csv["custom html below price"],
                            CustomHtmlAboveImage = csv["custom html above image"],
                            CustomHtmlBelowProduct = csv["custom html below product"],
                            CustomHtmlBelowImage = csv["custom html below image"],
                            AutoResponder = csv["auto responder"],
                            AllowSample = csv["allow Sample"],
                            SamplePrice = csv["sample Price"],
                            SampleSufix = csv["sample Sufix"],
                            SampleGroundShipping = csv["sample Ground Shipping"],
                            SampleInterShipping = csv["sample Inter Shipping"],
                            SupportTicket = csv["support ticket"],
                            PickupAvailable = csv["pickup available"],
                            CreateDate = csv["create date"],
                            UpdateDate = csv["update date"],
                            DiscontinueDate = csv["discontinue date"],
                            DiscontinueWarningDate = csv["discontinue warning date"],
                            DisclaimerWarning = csv["Disclaimer Warning"],
                            Taxable = csv["taxable"],
                            ApplicationURL = csv["Application URL"],
                            PartNumberDependancies = csv["Part Number Dependancies"],


                            PartNumber = csv["Part Number"],
                            

                            ManufacturerPartNumber = csv["Manufacturer Part Number"],


                            UPC = csv["UPC"],


                            EAN = csv["EAN"],
                            ItemCondition = csv["ItemCondition"],
                            ApplicationSpecificImage = csv["Application Specific Image"],
                            ApplicationSpecifications = csv["Application Specifications"],
                            WarningNote = csv["Warning Note"],
                            ApplicationQtyDiscounts = csv["Application Qty Discounts"],
                            MSRP = msrp,
                            Jobber = jobber.ToString(),
                            WebPrice = webPrice,
                            CostPrice = costPrice,
                            ItemWeight = csv["Item Weight"],
                            ItemHeight = csv["Item Height"],
                            ItemWidth = csv["Item Width"],
                            ItemLength = csv["Item Length"],
                            ShippingWeight = csv["Shipping Weight"],
                            ShippingHeight = csv["Shipping Height"],
                            ShippingWidth = csv["Shipping Width"],
                            ShippingLength = csv["Shipping Length"],
                            PrimaryOptionStyle = csv["Primary Option Style"],
                            PrimaryOptionTitle = csv["Primary Option Title"],
                            PrimaryChoice = csv["Primary Choice"],
                            PrimaryOptionPosition = csv["Primary Option Position"],
                            PrimaryOptionParent = csv["Primary Option Parent"],
                            PrimaryOptionType = csv["Primary Option Type"],
                            PrimaryOptionID = csv["Primary Option ID"],
                            SecondOptionStyle = csv["Second Option Style"],
                            SecondOptionType = csv["Second option type"],
                            SecondOptionTitle = csv["Second Option Title"],
                            SecondOptionChoice = csv["Second Option Choice"],
                            SecondOptionChoiceParent = csv["Second Option Choice Parent"],
                            SecondOptionPartNumberExt = csv["Second Option Part Number Ext"],
                            ApplicationSpecificImage2 = csv["Application Specific Image 2"],
                            ApplicationSpecificImage2Tags = csv["application specific image 2 tags"],
                            ThirdOptionStyle = csv["Third Option Style"],
                            ThirdOptionTitle = csv["Third Option Title"],
                            ThirdOptionChoice = csv["Third Option Choice"],
                            ThirdOptionChoiceParent = csv["Third Option Choice Parent"],
                            ThirdOptionPartNumberExt = csv["Third Option Part Number Ext"],
                            ApplicationSpecificImage3 = csv["Application Specific Image 3"],
                            ApplicationSpecificImage3Tags = csv["application specific image 3 tags"],
                            FourthOptionStyle = csv["Fourth Option Style"],
                            FourthOptionTitle = csv["Fourth Option Title"],
                            FourthOptionChoice = csv["Fourth Option Choice"],
                            FourthOptionChoiceParent = csv["Fourth Option Choice Parent"],
                            FourthOptionPartNumberExt = csv["Fourth Option Part Number Ext"],
                            ApplicationSpecificImage4 = csv["Application Specific Image 4"],
                            ApplicationSpecificImage4Tags = csv["application specific image 4 tags"],
                            FifthOptionStyle = csv["Fifth Option Style"],
                            FifthOptionTitle = csv["Fifth Option Title"],
                            FifthOptionChoice = csv["Fifth Option Choice"],
                            FifthOptionChoiceParent = csv["Fifth Option Choice Parent"],
                            FifthOptionPartNumberExt = csv["Fifth Option Part Number Ext"],
                            ApplicationSpecificImage5 = csv["Application Specific Image 5"],
                            ApplicationSpecificImage5Tags = csv["application specific image 5 tags"],
                            VehicleMake = csv["Vehicle Make"],
                            VehicleModel = csv["Vehicle Model"],
                            VehicleSubModel = csv["Vehicle Sub Model"],
                            StartYear = csv["Start Year"],
                            EndYear = csv["End Year"],
                            Engine = csv["Engine"],
                            EngineVersion = csv["Engine Version"],
                            Aspiration = csv["Aspiration"],
                            CylinderType = csv["Cylinder Type"],
                            DriveType = csv["Drive Type"],
                            Brakes = csv["Brakes"],
                            ABS = csv["ABS"],
                            Transmission = csv["Transmission"],
                            Doors = csv["Doors"],
                            BodyType = csv["Body Type"],
                            BedType = csv["Bed Type"],
                            Navigation = csv["Navigation"],
                            ClimateControl = csv["Climate Control"],
                            Radio = csv["Radio"],
                            HeatedSeats = csv["Heated Seats"],
                            Sunroof = csv["Sunroof"],
                            VehicleEngineOptions = csv["Vehicle Engine Options"],
                            VehicleInteriorOptions = csv["Vehicle Interior Options"],
                            VehicleExteriorOptions = csv["Vehicle Exterior Options"],
                            VehicleWarningNote = csv["Vehicle Warning Note"],
                            VehicleDescription = csv["Vehicle Description"],
                            FitmentID = csv["Fitment ID"],
                            WheelDiameter = csv["Wheel Diameter"],
                            WheelWidth = csv["Wheel Width"],
                            WheelFinish = csv["Wheel Finish"],
                            WheelBoltPattern = csv["Wheel BoltPattern"],
                            WheelOffset = csv["Wheel Offset"],
                            WheelHubBore = csv["Wheel HubBore"],
                            SectionWidth = csv["Section Width"],
                            AspectRatio = csv["Aspect Ratio"],
                            Diameter = csv["Diameter"],
                            SpeedRating = csv["Speed Rating"],
                            LoadIndex = csv["Load Index"],
                            TireType = csv["Tire Type"],
                            LoadRange = csv["Load Range"],
                            SideWall = csv["Side Wall"],
                            XFactor = csv["xFactor"]
                        };

                        if (!string.IsNullOrEmpty(item.PartNumber))
                        {
                            if (!item.PartNumber.Contains("@"))
                            {
                                partNumber = $"@{item.PartNumber}";
                                item.PartNumber = partNumber;
                            }
                        }
                        
                        if (!string.IsNullOrEmpty(item.ManufacturerPartNumber))
                        {
                            if (!item.ManufacturerPartNumber.Contains("@"))
                            {
                                manufacturerPartNumber = $"@{item.ManufacturerPartNumber}";
                                item.ManufacturerPartNumber = manufacturerPartNumber;
                            }  
                        }
                        
                        if (!string.IsNullOrEmpty(item.UPC))
                        {
                            if (!item.UPC.Contains("@"))
                            {
                                uPC = $"@{item.UPC}";
                                item.UPC = uPC;
                            }
                        }

                        ftpItems.Add(item);
                    }
                }

                return ftpItems;
            }
        }

        private static string StringToCSVCell(string str)
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
    }
}
