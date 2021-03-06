﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace LoadSceExport
{
    public class ExtWareInfo : WareInfo
    {
        public string Action { get; set; }
        public string ProdId { get; set; }
        public string ProductType { get; set; }
        public string MerchandiseType { get; set; }
        public string ProductTitle { get; set; }
        public string AnchorText { get; set; }
        public string SpiderURL { get; set; }
        public string AmazonTitle { get; set; }
        public string AmazonDescription { get; set; }
        public string EBayTitle { get; set; }
        public string EBayDescription { get; set; }
        public string SubTitle { get; set; }
        public string ScreenTitle { get; set; }
        public string AllowNewBrand { get; set; }
        public string AdditionalDescription { get; set; }
        public string AdditionalDescription2 { get; set; }
        public string AdditionalDescription3 { get; set; }
        public string AdditionalDescription4 { get; set; }
        public string BulletPoint { get; set; }
        public string PrimaryKeyword { get; set; }
        public string SecondaryKeyword { get; set; }
        public string TertiaryKeyword { get; set; }
        public string METADescription { get; set; }
        public string METAKeywords { get; set; }
        public string SpiderRule { get; set; }
        public string GeneralImageTags { get; set; }
        public string GeneralImage { get; set; }
        public string AllowNewCategory { get; set; }
        public string MainCategory { get; set; }
        public string MainCategoryID { get; set; }
        public string MainCategoryMetaTitle { get; set; }
        public string MainCategoryMetaKeywords { get; set; }
        public string MainCategoryMetaDescription { get; set; }
        public string MainCategorySpiderURL { get; set; }
        public string MainCategoryRedirectURL { get; set; }
        public string MainCategoryImageURL { get; set; }
        public string MainCategoryAmazonCategory { get; set; }
        public string MainCategoryAmazonCategoryID { get; set; }
        public string SubCategory { get; set; }
        public string SubCategoryID { get; set; }
        public string SubCategoryMetaTitle { get; set; }
        public string SubCategoryMetaKeywords { get; set; }
        public string SubCategoryMetaDescription { get; set; }
        public string SubCategorySpiderURL { get; set; }
        public string SubCategoryRedirectURL { get; set; }
        public string SubCategoryImageURL { get; set; }
        public string SubCategoryAmazonCategory { get; set; }
        public string SubCategoryAmazonCategoryID { get; set; }
        public string SubCategoryEbayCategory { get; set; }
        public string SectionCategory { get; set; }
        public string SectionCategoryID { get; set; }
        public string SectionCategoryMetaTitle { get; set; }
        public string SectionCategoryMetaKeywords { get; set; }
        public string SectionCategoryMetaDescription { get; set; }
        public string SectionCategorySpiderURL { get; set; }
        public string SectionCategoryRedirectURL { get; set; }
        public string SectionCategoryImageURL { get; set; }
        public string SectionCategoryAmazonCategory { get; set; }
        public string SectionCategoryAmazonCategoryID { get; set; }
        public string SectionCategoryEbayCategory { get; set; }
        public string CrossSellMainCat1 { get; set; }
        public string CrossSellSubCat1 { get; set; }
        public string CrossSellSecCat1 { get; set; }
        public string CrossSellMainCat2 { get; set; }
        public string CrossSellSubCat2 { get; set; }
        public string CrossSellSecCat2 { get; set; }
        public string CrossSellMainCat3 { get; set; }
        public string CrossSellSubCat3 { get; set; }
        public string CrossSellSecCat3 { get; set; }
        public string CrossSellMainCat4 { get; set; }
        public string CrossSellSubCat4 { get; set; }
        public string CrossSellSecCat4 { get; set; }
        public string CrossSellMainCat5 { get; set; }
        public string CrossSellSubCat5 { get; set; }
        public string CrossSellSecCat5 { get; set; }
        public string Specifications { get; set; }
        public string Featured { get; set; }
        public string Supplier { get; set; }
        public string ReOrderSupplier { get; set; }
        public string Warehouse { get; set; }
        public string Workflow { get; set; }
        public string Redirect301 { get; set; }
        public string Hidden { get; set; }
        public string Internaluse { get; set; }
        public string ProcessingTime { get; set; }
        public string OutOfStockProcessingTime { get; set; }
        public string Combinable { get; set; }
        public string Consolidatable { get; set; }
        public string ShippingType { get; set; }
        public string ShippingCarrier1 { get; set; }
        public string ShippingCarrier2 { get; set; }
        public string ShippingCarrier3 { get; set; }
        public string ShippingCarrier4 { get; set; }
        public string ShippingCarrier5 { get; set; }
        public string Allowground { get; set; }
        public string Allow3day { get; set; }
        public string Allow2day { get; set; }
        public string Allownextday { get; set; }
        public string Allowinternational { get; set; }
        public string ShippingGroundRate { get; set; }
        public string Shipping3DayAirRate { get; set; }
        public string Shipping2DayAirRate { get; set; }
        public string ShippingNextDayAirRate { get; set; }
        public string ShippingInterNorthAmerica { get; set; }
        public string ShippingInterSouthAmerica { get; set; }
        public string ShippingInterEurope { get; set; }
        public string ShippingInterAfrica { get; set; }
        public string ShippingInterAsia { get; set; }
        public string ShippingInterOceania { get; set; }
        public string UpsellProdIDs { get; set; }
        public string UpsellProdNames { get; set; }
        public string ForceUpsell { get; set; }
        public string UpsellProdQty { get; set; }
        public string RelatedProdIDs { get; set; }
        public string RelatedProdNames { get; set; }
        public string UpsellWizard { get; set; }
        public string CompareProducts { get; set; }
        public string AccessoryProducts { get; set; }
        public string FileAttachments { get; set; }
        public string Videos { get; set; }
        public string SearchTags { get; set; }
        public string NegativeSearchTags { get; set; }
        public string LiveInventory { get; set; }
        public string MinQty { get; set; }
        public string MaxQty { get; set; }
        public string QtyIncrements { get; set; }
        public string PredefinedQty { get; set; }
        public string QtyDiscounts { get; set; }
        public string SortPriority { get; set; }
        public string OutOfStockRule { get; set; }
        public string CheckoutRule { get; set; }
        public string ShowPricesToPublic { get; set; }
        public string Expedited { get; set; }
        public string AmazonAffiliateAsin { get; set; }
        public string WholesaleLevel { get; set; }
        public string CustomHtmlAboveQty { get; set; }
        public string CustomHtmlBelowThumbnail { get; set; }
        public string CustomHtmlAboveUpsells { get; set; }
        public string CustomHtmlAbovePrice { get; set; }
        public string CustomHtmlBelowPrice { get; set; }
        public string CustomHtmlAboveImage { get; set; }
        public string CustomHtmlBelowProduct { get; set; }
        public string CustomHtmlBelowImage { get; set; }
        public string AutoResponder { get; set; }
        public string AllowSample { get; set; }
        public string SamplePrice { get; set; }
        public string SampleSufix { get; set; }
        public string SampleGroundShipping { get; set; }
        public string SampleInterShipping { get; set; }
        public string SupportTicket { get; set; }
        public string PickupAvailable { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string DiscontinueDate { get; set; }
        public string DiscontinueWarningDate { get; set; }
        public string DisclaimerWarning { get; set; }
        public string Taxable { get; set; }
        public string ApplicationURL { get; set; }
        public string PartNumberDependancies { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string UPC { get; set; }
        public string EAN { get; set; }
        public string ItemCondition { get; set; }
        public string ApplicationSpecificImage { get; set; }
        public string ApplicationSpecifications { get; set; }
        public string WarningNote { get; set; }
        public string ApplicationQtyDiscounts { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
        public string ItemWeight { get; set; }
        public string ItemHeight { get; set; }
        public string ItemWidth { get; set; }
        public string ItemLength { get; set; }
        public string ShippingWeight { get; set; }
        public string ShippingHeight { get; set; }
        public string ShippingWidth { get; set; }
        public string ShippingLength { get; set; }
        public string PrimaryOptionStyle { get; set; }
        public string PrimaryOptionTitle { get; set; }
        public string PrimaryChoice { get; set; }
        public string PrimaryOptionPosition { get; set; }
        public string PrimaryOptionParent { get; set; }
        public string PrimaryOptionType { get; set; }
        public string PrimaryOptionID { get; set; }
        public string SecondOptionStyle { get; set; }
        public string SecondOptionType { get; set; }
        public string SecondOptionTitle { get; set; }
        public string SecondOptionChoice { get; set; }
        public string SecondOptionChoiceParent { get; set; }
        public string SecondOptionPartNumberExt { get; set; }
        public string ApplicationSpecificImage2 { get; set; }
        public string ApplicationSpecificImage2Tags { get; set; }
        public string ThirdOptionStyle { get; set; }
        public string ThirdOptionTitle { get; set; }
        public string ThirdOptionChoice { get; set; }
        public string ThirdOptionChoiceParent { get; set; }
        public string ThirdOptionPartNumberExt { get; set; }
        public string ApplicationSpecificImage3 { get; set; }
        public string ApplicationSpecificImage3Tags { get; set; }
        public string FourthOptionStyle { get; set; }
        public string FourthOptionTitle { get; set; }
        public string FourthOptionChoice { get; set; }
        public string FourthOptionChoiceParent { get; set; }
        public string FourthOptionPartNumberExt { get; set; }
        public string ApplicationSpecificImage4 { get; set; }
        public string ApplicationSpecificImage4Tags { get; set; }
        public string FifthOptionStyle { get; set; }
        public string FifthOptionTitle { get; set; }
        public string FifthOptionChoice { get; set; }
        public string FifthOptionChoiceParent { get; set; }
        public string FifthOptionPartNumberExt { get; set; }
        public string ApplicationSpecificImage5 { get; set; }
        public string ApplicationSpecificImage5Tags { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleSubModel { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string Engine { get; set; }
        public string EngineVersion { get; set; }
        public string Aspiration { get; set; }
        public string CylinderType { get; set; }
        public string DriveType { get; set; }
        public string Brakes { get; set; }
        public string ABS { get; set; }
        public string Transmission { get; set; }
        public string Doors { get; set; }
        public string BodyType { get; set; }
        public string BedType { get; set; }
        public string Navigation { get; set; }
        public string ClimateControl { get; set; }
        public string Radio { get; set; }
        public string HeatedSeats { get; set; }
        public string Sunroof { get; set; }
        public string VehicleEngineOptions { get; set; }
        public string VehicleInteriorOptions { get; set; }
        public string VehicleExteriorOptions { get; set; }
        public string VehicleWarningNote { get; set; }
        public string VehicleDescription { get; set; }
        public string FitmentID { get; set; }
        public string WheelDiameter { get; set; }
        public string WheelWidth { get; set; }
        public string WheelFinish { get; set; }
        public string WheelBoltPattern { get; set; }
        public string WheelOffset { get; set; }
        public string WheelHubBore { get; set; }
        public string SectionWidth { get; set; }
        public string AspectRatio { get; set; }
        public string Diameter { get; set; }
        public string SpeedRating { get; set; }
        public string LoadIndex { get; set; }
        public string TireType { get; set; }
        public string LoadRange { get; set; }
        public string SideWall { get; set; }
        public string XFactor { get; set; }
    }
}
