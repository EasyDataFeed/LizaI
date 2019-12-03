namespace ScraperEbayApi.DataItems
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:ebay:apis:eBLBaseComponents", IsNullable = false)]
    public partial class GetSingleItemResponse
    {
        private System.DateTime timestampField;
        private string ackField;
        private GetSingleItemResponseItem itemField;
        private Errors errorsFields;
        
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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItem
    {
        private string descriptionField;
        private ulong itemIDField;
        private string[] pictureURLField;
        private string primaryCategoryNameField;
        private int quantityField;
        private GetSingleItemResponseItemSeller sellerField;
        private GetSingleItemResponseItemConvertedCurrentPrice convertedCurrentPriceField;
        private string listingStatusField;
        private int quantitySoldField;
        private string titleField;
        private GetSingleItemResponseItemShippingCostSummary shippingCostSummaryField;
        private GetSingleItemResponseItemNameValueList[] itemSpecificsField;
        private int hitCountField;
        private GetSingleItemResponseItemStorefront storefrontField;
        private int itemCompatibilityCountField;
        private GetSingleItemResponseItemDiscountPriceInfo discountPriceInfoField;
        private GetSingleItemResponseItemCompatibility[] itemCompatibilityListField;
        private string sKUField;
        
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
    }
    
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemSeller
    {
        private string userIDField;

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
    }
    
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemConvertedCurrentPrice
    {
        private decimal valueField;
        
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
    public partial class GetSingleItemResponseItemCurrentPrice
    {
        private string currencyIDField;
        private decimal valueField;
    }
    
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemShippingCostSummary
    {
        private GetSingleItemResponseItemShippingCostSummaryShippingServiceCost shippingServiceCostField;
        private string shippingTypeField;
        private GetSingleItemResponseItemShippingCostSummaryListedShippingServiceCost listedShippingServiceCostField;
        
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
    
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemShippingCostSummaryShippingServiceCost
    {
        private decimal valueField;
        
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
    public partial class GetSingleItemResponseItemShippingCostSummaryListedShippingServiceCost
    {
        private decimal valueField;
        
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
    public partial class GetSingleItemResponseItemNameValueList
    {

        private string nameField;

        private string valueField;
        
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
    
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemStorefront
    {
        private string storeNameField;
        
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
    }
    
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemCompatibility
    {

        private GetSingleItemResponseItemCompatibilityNameValueList[] nameValueListField;

        private object compatibilityNotesField;
        
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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemCompatibilityNameValueList
    {

        private string nameField;

        private string valueField;
        
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
    
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemDiscountPriceInfo
    {
        private GetSingleItemResponseItemDiscountPriceInfoOriginalRetailPrice originalRetailPriceField;

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
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public partial class GetSingleItemResponseItemDiscountPriceInfoOriginalRetailPrice
    {
        private decimal valueField;

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
        private string longMessageField;
        
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
    }
}
