using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerapeakVehicleMatching.DataItems
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:ebay:apis:eBLBaseComponents", IsNullable = false)]
    public partial class GetSingleItemResponse
    {

        private System.DateTime timestampField;

        private string ackField;

        private string buildField;

        private int versionField;

        private GetSingleItemResponseItem itemField;

        private Errors errorsFields;

        /// <remarks/>
        public System.DateTime Timestamp
        {
            get
            {
                return this.timestampField;
            }
            set
            {
                this.timestampField = value;
            }
        }

        /// <remarks/>
        public string Ack
        {
            get
            {
                return this.ackField;
            }
            set
            {
                this.ackField = value;
            }
        }

        /// <remarks/>
        public string Build
        {
            get
            {
                return this.buildField;
            }
            set
            {
                this.buildField = value;
            }
        }

        /// <remarks/>
        public int Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItem Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public Errors Errors
        {
            get
            {
                return this.errorsFields;
            }
            set
            {
                this.errorsFields = value;
            }
        }
    }

    /// <remarks/>

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItem
    {

        private bool bestOfferEnabledField;

        private string descriptionField;

        private ulong itemIDField;

        private System.DateTime endTimeField;

        private System.DateTime startTimeField;

        private string viewItemURLForNaturalSearchField;

        private string listingTypeField;

        private string locationField;

        private string paymentMethodsField;

        private string galleryURLField;

        private string[] pictureURLField;

        private int primaryCategoryIDField;

        private string primaryCategoryNameField;

        private int quantityField;

        private GetSingleItemResponseItemSeller sellerField;

        private int bidCountField;

        private GetSingleItemResponseItemConvertedCurrentPrice convertedCurrentPriceField;

        private GetSingleItemResponseItemCurrentPrice currentPriceField;

        private string listingStatusField;

        private int quantitySoldField;

        private string[] shipToLocationsField;

        private string siteField;

        private string timeLeftField;

        private string titleField;

        private GetSingleItemResponseItemShippingCostSummary shippingCostSummaryField;

        private GetSingleItemResponseItemNameValueList[] itemSpecificsField;

        private int hitCountField;

        private string primaryCategoryIDPathField;

        private GetSingleItemResponseItemStorefront storefrontField;

        private string countryField;

        private GetSingleItemResponseItemReturnPolicy returnPolicyField;

        private bool autoPayField;

        private bool integratedMerchantCreditCardEnabledField;

        private int handlingTimeField;

        private int conditionIDField;

        private string conditionDisplayNameField;

        private string quantityAvailableHintField;

        private int quantityThresholdField;

        private string[] excludeShipToLocationField;

        private bool topRatedListingField;

        private bool globalShippingField;

        private int itemCompatibilityCountField;

        private GetSingleItemResponseItemDiscountPriceInfo discountPriceInfoField;

        private GetSingleItemResponseItemCompatibility[] itemCompatibilityListField;

        private int quantitySoldByPickupInStoreField;

        private string sKUField;

        private bool newBestOfferField;

        /// <remarks/>
        public bool BestOfferEnabled
        {
            get
            {
                return this.bestOfferEnabledField;
            }
            set
            {
                this.bestOfferEnabledField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public ulong ItemID
        {
            get
            {
                return this.itemIDField;
            }
            set
            {
                this.itemIDField = value;
            }
        }

        /// <remarks/>
        public System.DateTime EndTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime StartTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public string ViewItemURLForNaturalSearch
        {
            get
            {
                return this.viewItemURLForNaturalSearchField;
            }
            set
            {
                this.viewItemURLForNaturalSearchField = value;
            }
        }

        /// <remarks/>
        public string ListingType
        {
            get
            {
                return this.listingTypeField;
            }
            set
            {
                this.listingTypeField = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string PaymentMethods
        {
            get
            {
                return this.paymentMethodsField;
            }
            set
            {
                this.paymentMethodsField = value;
            }
        }

        /// <remarks/>
        public string GalleryURL
        {
            get
            {
                return this.galleryURLField;
            }
            set
            {
                this.galleryURLField = value;
            }
        }

        public GetSingleItemResponseItemDiscountPriceInfo DiscountPriceInfo
        {
            get
            {
                return this.discountPriceInfoField;
            }
            set
            {
                this.discountPriceInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PictureURL")]
        public string[] PictureURL
        {
            get
            {
                return this.pictureURLField;
            }
            set
            {
                this.pictureURLField = value;
            }
        }

        /// <remarks/>
        public int PrimaryCategoryID
        {
            get
            {
                return this.primaryCategoryIDField;
            }
            set
            {
                this.primaryCategoryIDField = value;
            }
        }

        /// <remarks/>
        public string PrimaryCategoryName
        {
            get
            {
                return this.primaryCategoryNameField;
            }
            set
            {
                this.primaryCategoryNameField = value;
            }
        }

        /// <remarks/>
        public int Quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItemSeller Seller
        {
            get
            {
                return this.sellerField;
            }
            set
            {
                this.sellerField = value;
            }
        }

        /// <remarks/>
        public int BidCount
        {
            get
            {
                return this.bidCountField;
            }
            set
            {
                this.bidCountField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItemConvertedCurrentPrice ConvertedCurrentPrice
        {
            get
            {
                return this.convertedCurrentPriceField;
            }
            set
            {
                this.convertedCurrentPriceField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItemCurrentPrice CurrentPrice
        {
            get
            {
                return this.currentPriceField;
            }
            set
            {
                this.currentPriceField = value;
            }
        }

        /// <remarks/>
        public string ListingStatus
        {
            get
            {
                return this.listingStatusField;
            }
            set
            {
                this.listingStatusField = value;
            }
        }

        /// <remarks/>
        public int QuantitySold
        {
            get
            {
                return this.quantitySoldField;
            }
            set
            {
                this.quantitySoldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ShipToLocations")]
        public string[] ShipToLocations
        {
            get
            {
                return this.shipToLocationsField;
            }
            set
            {
                this.shipToLocationsField = value;
            }
        }

        /// <remarks/>
        public string Site
        {
            get
            {
                return this.siteField;
            }
            set
            {
                this.siteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string TimeLeft
        {
            get
            {
                return this.timeLeftField;
            }
            set
            {
                this.timeLeftField = value;
            }
        }

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItemShippingCostSummary ShippingCostSummary
        {
            get
            {
                return this.shippingCostSummaryField;
            }
            set
            {
                this.shippingCostSummaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NameValueList", IsNullable = false)]
        public GetSingleItemResponseItemNameValueList[] ItemSpecifics
        {
            get
            {
                return this.itemSpecificsField;
            }
            set
            {
                this.itemSpecificsField = value;
            }
        }

        /// <remarks/>
        public int HitCount
        {
            get
            {
                return this.hitCountField;
            }
            set
            {
                this.hitCountField = value;
            }
        }

        /// <remarks/>
        public string PrimaryCategoryIDPath
        {
            get
            {
                return this.primaryCategoryIDPathField;
            }
            set
            {
                this.primaryCategoryIDPathField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItemStorefront Storefront
        {
            get
            {
                return this.storefrontField;
            }
            set
            {
                this.storefrontField = value;
            }
        }

        /// <remarks/>
        public string Country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItemReturnPolicy ReturnPolicy
        {
            get
            {
                return this.returnPolicyField;
            }
            set
            {
                this.returnPolicyField = value;
            }
        }

        /// <remarks/>
        public bool AutoPay
        {
            get
            {
                return this.autoPayField;
            }
            set
            {
                this.autoPayField = value;
            }
        }

        /// <remarks/>
        public bool IntegratedMerchantCreditCardEnabled
        {
            get
            {
                return this.integratedMerchantCreditCardEnabledField;
            }
            set
            {
                this.integratedMerchantCreditCardEnabledField = value;
            }
        }

        /// <remarks/>
        public int HandlingTime
        {
            get
            {
                return this.handlingTimeField;
            }
            set
            {
                this.handlingTimeField = value;
            }
        }

        /// <remarks/>
        public int ConditionID
        {
            get
            {
                return this.conditionIDField;
            }
            set
            {
                this.conditionIDField = value;
            }
        }

        /// <remarks/>
        public string ConditionDisplayName
        {
            get
            {
                return this.conditionDisplayNameField;
            }
            set
            {
                this.conditionDisplayNameField = value;
            }
        }

        /// <remarks/>
        //public string QuantityAvailableHint
        //{
        //    get
        //    {
        //        return this.quantityAvailableHintField;
        //    }
        //    set
        //    {
        //        this.quantityAvailableHintField = value;
        //    }
        //}

        ///// <remarks/>
        //public int QuantityThreshold
        //{
        //    get
        //    {
        //        return this.quantityThresholdField;
        //    }
        //    set
        //    {
        //        this.quantityThresholdField = value;
        //    }
        //}

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExcludeShipToLocation")]
        public string[] ExcludeShipToLocation
        {
            get
            {
                return this.excludeShipToLocationField;
            }
            set
            {
                this.excludeShipToLocationField = value;
            }
        }

        /// <remarks/>
        public bool TopRatedListing
        {
            get
            {
                return this.topRatedListingField;
            }
            set
            {
                this.topRatedListingField = value;
            }
        }

        /// <remarks/>
        public bool GlobalShipping
        {
            get
            {
                return this.globalShippingField;
            }
            set
            {
                this.globalShippingField = value;
            }
        }

        /// <remarks/>
        public int ItemCompatibilityCount
        {
            get
            {
                return this.itemCompatibilityCountField;
            }
            set
            {
                this.itemCompatibilityCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Compatibility", IsNullable = false)]
        public GetSingleItemResponseItemCompatibility[] ItemCompatibilityList
        {
            get
            {
                return this.itemCompatibilityListField;
            }
            set
            {
                this.itemCompatibilityListField = value;
            }
        }

        /// <remarks/>
        public int QuantitySoldByPickupInStore
        {
            get
            {
                return this.quantitySoldByPickupInStoreField;
            }
            set
            {
                this.quantitySoldByPickupInStoreField = value;
            }
        }

        /// <remarks/>
        public string SKU
        {
            get
            {
                return this.sKUField;
            }
            set
            {
                this.sKUField = value;
            }
        }

        /// <remarks/>
        public bool NewBestOffer
        {
            get
            {
                return this.newBestOfferField;
            }
            set
            {
                this.newBestOfferField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemSeller
    {

        private string userIDField;

        private string feedbackRatingStarField;

        private uint feedbackScoreField;

        private decimal positiveFeedbackPercentField;

        private bool topRatedSellerField;

        /// <remarks/>
        public string UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        public string FeedbackRatingStar
        {
            get
            {
                return this.feedbackRatingStarField;
            }
            set
            {
                this.feedbackRatingStarField = value;
            }
        }

        /// <remarks/>
        public uint FeedbackScore
        {
            get
            {
                return this.feedbackScoreField;
            }
            set
            {
                this.feedbackScoreField = value;
            }
        }

        /// <remarks/>
        public decimal PositiveFeedbackPercent
        {
            get
            {
                return this.positiveFeedbackPercentField;
            }
            set
            {
                this.positiveFeedbackPercentField = value;
            }
        }

        /// <remarks/>
        public bool TopRatedSeller
        {
            get
            {
                return this.topRatedSellerField;
            }
            set
            {
                this.topRatedSellerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemConvertedCurrentPrice
    {

        private string currencyIDField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyID
        {
            get
            {
                return this.currencyIDField;
            }
            set
            {
                this.currencyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemCurrentPrice
    {

        private string currencyIDField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyID
        {
            get
            {
                return this.currencyIDField;
            }
            set
            {
                this.currencyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemShippingCostSummary
    {

        private GetSingleItemResponseItemShippingCostSummaryShippingServiceCost shippingServiceCostField;

        private string shippingTypeField;

        private GetSingleItemResponseItemShippingCostSummaryListedShippingServiceCost listedShippingServiceCostField;

        /// <remarks/>
        public GetSingleItemResponseItemShippingCostSummaryShippingServiceCost ShippingServiceCost
        {
            get
            {
                return this.shippingServiceCostField;
            }
            set
            {
                this.shippingServiceCostField = value;
            }
        }

        /// <remarks/>
        public string ShippingType
        {
            get
            {
                return this.shippingTypeField;
            }
            set
            {
                this.shippingTypeField = value;
            }
        }

        /// <remarks/>
        public GetSingleItemResponseItemShippingCostSummaryListedShippingServiceCost ListedShippingServiceCost
        {
            get
            {
                return this.listedShippingServiceCostField;
            }
            set
            {
                this.listedShippingServiceCostField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemShippingCostSummaryShippingServiceCost
    {

        private string currencyIDField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyID
        {
            get
            {
                return this.currencyIDField;
            }
            set
            {
                this.currencyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemShippingCostSummaryListedShippingServiceCost
    {

        private string currencyIDField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyID
        {
            get
            {
                return this.currencyIDField;
            }
            set
            {
                this.currencyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemNameValueList
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemStorefront
    {

        private string storeURLField;

        private string storeNameField;

        /// <remarks/>
        public string StoreURL
        {
            get
            {
                return this.storeURLField;
            }
            set
            {
                this.storeURLField = value;
            }
        }

        /// <remarks/>
        public string StoreName
        {
            get
            {
                return this.storeNameField;
            }
            set
            {
                this.storeNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemReturnPolicy
    {

        private string refundField;

        private string returnsWithinField;

        private string returnsAcceptedField;

        private string descriptionField;

        private string shippingCostPaidByField;

        /// <remarks/>
        public string Refund
        {
            get
            {
                return this.refundField;
            }
            set
            {
                this.refundField = value;
            }
        }

        /// <remarks/>
        public string ReturnsWithin
        {
            get
            {
                return this.returnsWithinField;
            }
            set
            {
                this.returnsWithinField = value;
            }
        }

        /// <remarks/>
        public string ReturnsAccepted
        {
            get
            {
                return this.returnsAcceptedField;
            }
            set
            {
                this.returnsAcceptedField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string ShippingCostPaidBy
        {
            get
            {
                return this.shippingCostPaidByField;
            }
            set
            {
                this.shippingCostPaidByField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemCompatibility
    {

        private GetSingleItemResponseItemCompatibilityNameValueList[] nameValueListField;

        private object compatibilityNotesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("NameValueList")]
        public GetSingleItemResponseItemCompatibilityNameValueList[] NameValueList
        {
            get
            {
                return this.nameValueListField;
            }
            set
            {
                this.nameValueListField = value;
            }
        }

        /// <remarks/>
        public object CompatibilityNotes
        {
            get
            {
                return this.compatibilityNotesField;
            }
            set
            {
                this.compatibilityNotesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemCompatibilityNameValueList
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemDiscountPriceInfo
    {

        private GetSingleItemResponseItemDiscountPriceInfoOriginalRetailPrice originalRetailPriceField;

        private string pricingTreatmentField;

        private bool soldOneBayField;

        private bool soldOffeBayField;

        /// <remarks/>
        public GetSingleItemResponseItemDiscountPriceInfoOriginalRetailPrice OriginalRetailPrice
        {
            get
            {
                return this.originalRetailPriceField;
            }
            set
            {
                this.originalRetailPriceField = value;
            }
        }

        /// <remarks/>
        public string PricingTreatment
        {
            get
            {
                return this.pricingTreatmentField;
            }
            set
            {
                this.pricingTreatmentField = value;
            }
        }

        /// <remarks/>
        public bool SoldOneBay
        {
            get
            {
                return this.soldOneBayField;
            }
            set
            {
                this.soldOneBayField = value;
            }
        }

        /// <remarks/>
        public bool SoldOffeBay
        {
            get
            {
                return this.soldOffeBayField;
            }
            set
            {
                this.soldOffeBayField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemDiscountPriceInfoOriginalRetailPrice
    {

        private string currencyIDField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currencyID
        {
            get
            {
                return this.currencyIDField;
            }
            set
            {
                this.currencyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        } 
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class Errors
    {

        private string shortMessageField;

        private string longMessageField;

        private string errorCodeField;

        private string severityCodeField;

        //private string errorClassificationField;
        public string ErrorClassification { get; set; }

        /// <remarks/>
        public string ShortMessage
        {
            get
            {
                return this.shortMessageField;
            }
            set
            {
                this.shortMessageField = value;
            }
        }

        /// <remarks/>
        public string LongMessage
        {
            get
            {
                return this.longMessageField;
            }
            set
            {
                this.longMessageField = value;
            }
        }

        /// <remarks/>
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }

        /// <remarks/>
        public string SeverityCode
        {
            get => this.severityCodeField;
            set => this.severityCodeField = value;
        }

        ///// <remarks/>
        //public string ErrorClassification
        //{
        //    get
        //    {
        //        return this.errorClassificationField;
        //    }
        //    set
        //    {
        //        this.errorClassificationField = value;
        //    }
        //}
    }
}

