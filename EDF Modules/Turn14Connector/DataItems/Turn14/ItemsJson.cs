using System;
using System.Collections.Generic;

namespace Turn14Connector.DataItems.Turn14
{
    class ItemsJson
    {
        public List<dataItems> data { get; set; }
        public metaItems meta { get; set; }
        public linksItems links { get; set; }
    }

    public class dataItems
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesItems attributes { get; set; }
    }

    public class attributesItems
    {
        public string product_name { get; set; }
        public string part_number { get; set; }
        public string mfr_part_number { get; set; }
        public string part_description { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public List<dimensionsItems> dimensions { get; set; }
        public string brand_id { get; set; }
        public string brand { get; set; }
        public string price_group_id { get; set; }
        public string price_group { get; set; }
        public bool active { get; set; }
        public bool regular_stock { get; set; }
        public string dropship_controller_id { get; set; }
        public bool air_freight_prohibited { get; set; }
        public bool not_carb_approved { get; set; }
        public string prop_65 { get; set; }
        public List<warehouse_availability> warehouse_availability { get; set; }
        public string thumbnail { get; set; }
        public string barcode { get; set; }
    }

    public class dimensionsItems
    {
        public string box_number { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
    }

    public class warehouse_availability
    {
        public string location_id { get; set; }
        public bool can_place_order { get; set; }
    }

    public class metaItems
    {
        public string total_pages { get; set; }
    }

    public class linksItems
    {
        public string self { get; set; }
        public string last { get; set; }
        public string next { get; set; }
    }

    public class SingleItemJson
    {
        public dataSingleItem data { get; set; }

    }

    public class dataSingleItem
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesSingleItem attributes { get; set; }
    }

    public class attributesSingleItem
    {
        public string product_name { get; set; }
        public string part_number { get; set; }
        public string mfr_part_number { get; set; }
        public string part_description { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public List<dimensionsSingleItem> dimensions { get; set; }
        public string brand_id { get; set; }
        public string brand { get; set; }
        public string price_group_id { get; set; }
        public string price_group { get; set; }
        public bool active { get; set; }
        public bool regular_stock { get; set; }
        public string dropship_controller_id { get; set; }
        public bool air_freight_prohibited { get; set; }
        public bool not_carb_approved { get; set; }
        public string prop_65 { get; set; }
        public List<warehouse_availabilitySingleItem> warehouse_availability { get; set; }
        public string thumbnail { get; set; }
    }

    public class dimensionsSingleItem
    {
        public string box_number { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
    }

    public class warehouse_availabilitySingleItem
    {
        public string location_id { get; set; }
        public bool can_place_order { get; set; }
    }

    public class SingleItemDataJson
    {
        public List<dataSingleItemData> data { get; set; }
    }

    public class dataSingleItemData
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<filesSingleItem> files { get; set; }
        public List<vehicle_fitments> vehicle_fitments { get; set; }
    }

    public class filesSingleItem
    {
        public string id { get; set; }
        public string type { get; set; }
        public string file_extension { get; set; }
        public string media_content { get; set; }
        public List<linksSingleItem> links { get; set; }
    }

    public class linksSingleItem
    {
        public string url { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public string size { get; set; }
    }

    public class vehicle_fitments
    {
        public string vehicle_id { get; set; }
    }

    public class SingleItemPricingJson
    {
        public dataSingleItemPricing data { get; set; }
    }

    public class dataSingleItemPricing
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesSingleItemPricing attributes { get; set; }
    }

    public class attributesSingleItemPricing
    {
        public bool has_map { get; set; }
        public bool can_purchase { get; set; }
        public List<pricelistsSingleItemPricing> pricelists { get; set; }
        public double purchase_cost { get; set; }
    }

    public class pricelistsSingleItemPricing
    {
        public string name { get; set; }
        public double price { get; set; }
    }

    public class InventorySingleItemJson
    {
        public List<dataInventorySingleItem> data { get; set; }
    }

    public class dataInventorySingleItem
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesInventorySingleItem attributes { get; set; }
        public relationshipsInventorySingleItem relationships { get; set; }
    }

    public class attributesInventorySingleItem
    {
        public Dictionary<String, string> inventory { get; set; }
        public manufacturer manufacturer { get; set; }
    }

    public class manufacturer
    {
        public string stock { get; set; }
        public DateTime esd { get; set; }
    }

    public class relationshipsInventorySingleItem
    {
        public itemInventorySingleItem item { get; set; }
    }

    public class itemInventorySingleItem
    {
        public string links { get; set; }
    }
}
