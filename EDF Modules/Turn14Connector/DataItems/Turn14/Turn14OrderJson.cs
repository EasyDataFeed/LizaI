using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turn14Connector.DataItems.Turn14
{
    class Turn14OrderJson
    {
        public Turn14OrderJson() { }

        public DataTurn14OrderJson data { get; set; }
    }

    public class DataTurn14OrderJson
    {
        public int id { get; set; }
        public string type { get; set; }
        public AttributesTurn14OrderJson attributes { get; set; }
    }

    public class AttributesTurn14OrderJson
    {
        public string po_number { get; set; }
        public string order_notes { get; set; }
        public List<ShipmentTurn14OrderJson> shipment { get; set; }
        public RecipientTurn14OrderJson recipient { get; set; }
        public double total { get; set; }
    }

    public class RecipientTurn14OrderJson
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

    public class ShipmentTurn14OrderJson
    {
        public int location { get; set; }
        public string type { get; set; }
        public List<ItemTurn14OrderJson> items { get; set; }
        public ShippingTurn14OrderJson shipping { get; set; }
    }

    public class ShippingTurn14OrderJson
    {
        public int shipping_quote_id { get; set; }
        public int shipping_code { get; set; }
        public double cost { get; set; }
        public int days_in_transit { get; set; }
        public bool saturday_delivery { get; set; }
        public bool signature_required { get; set; }
    }

    public class ItemTurn14OrderJson
    {
        public string item_id { get; set; }
        public string part_number { get; set; }
        public int quantity { get; set; }
        public double unit_price { get; set; }
        public int core_charge { get; set; }
        public double line_total { get; set; }
    }
}
