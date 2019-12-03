using System.Collections.Generic;

namespace ScraperApiTurn14.DataItems.Turn14
{
    public class OrderQuoteJson
    {
        public DataOrderQuote data { get; set; }
    }

    public class DataOrderQuote
    {
        public string id { get; set; }
        public string type { get; set; }
        public AttributesOrderQuote attributes { get; set; }
    }

    public class AttributesOrderQuote
    {
        public string po_number { get; set; }
        public string order_notes { get; set; }
        public List<ShipmentOrderQuote> shipment { get; set; }
        public RecipientOrderQuote recipient { get; set; }
        public double total { get; set; }
    }

    public class RecipientOrderQuote
    {
        public string name { get; set; }
        public string address { get; set; }
        public string address_two { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone_number { get; set; }
        public string is_shop_address { get; set; }
    }

    public class ShipmentOrderQuote
    {
        public string location { get; set; }
        public string type { get; set; }
        public List<ItemOrderQuote> items { get; set; }
        public List<ShippingOrderQuote> shipping { get; set; }
    }

    public class ItemOrderQuote
    {
        public string item_id { get; set; }
        public string part_number { get; set; }
        public int quantity { get; set; }
        public double unit_price { get; set; }
        public int core_charge { get; set; }
        public double line_total { get; set; }
    }

    public class ShippingOrderQuote
    {
        public int shipping_quote_id { get; set; }
        public string shipping_code { get; set; }
        public double cost { get; set; }
        public int days_in_transit { get; set; }
        public bool saturday_delivery { get; set; }
        public bool signature_required { get; set; }
    }
}
