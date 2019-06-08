using System.Collections.Generic;
using System.Linq;
using Turn14Connector.SCEapi;

namespace Turn14Connector.DataItems.Turn14
{
    class Turn14OrderQuote
    {
        public Turn14OrderQuote() { }

        public Turn14OrderQuote(OrderSync sceOrder, bool useProduction = false)
        {
            data = new dataTurn14OrderQuote();

            data.environment = useProduction ? "production" : "testing";
            data.po_number = sceOrder.OrderId.ToString();
            data.order_notes = "An order note";

            data.locations = new List<locationsTurn14OrderQuote>();
            locationsTurn14OrderQuote location = new locationsTurn14OrderQuote();

            location.combine_in_out_stock = true;
            location.location = "default";
            location.items = new List<itemsTurn14OrderQuote>();

            foreach (var sceItem in sceOrder.SceOrder.OrderItems)
            {
                itemsTurn14OrderQuote item = new itemsTurn14OrderQuote();
                item.item_identifier = $"{sceItem.PartNo}";
                //item.item_identifier_type = "item_id";
                //item.item_identifier_type = "mfr_part_number";
                item.item_identifier_type = "part_number";
                item.quantity = sceItem.Qty;
                location.items.Add(item);
            }

            data.locations.Add(location);

            var primaryContId = sceOrder.SceOrder.Account.PrimaryContactID;
            var primaryCont = sceOrder.SceOrder.Account.Contacts.FirstOrDefault(a => a.ID == primaryContId) ??
                              sceOrder.SceOrder.Account.Contacts.FirstOrDefault();

            data.recipient = new recipientTurn14OrderQuote
            {
                name = $"{primaryCont?.FirstName} {primaryCont?.MiddleName} {primaryCont?.LastName}",
                phone_number = sceOrder.SceOrder.ShippingAddress.Phone,
                zip = sceOrder.SceOrder.ShippingAddress?.Zip,
                address = sceOrder.SceOrder.ShippingAddress?.Address1,
                address_2 = sceOrder.SceOrder.ShippingAddress?.Address2,
                city = sceOrder.SceOrder.ShippingAddress?.City,
                state = sceOrder.SceOrder.ShippingAddress?.StateAbbr,
                country = sceOrder.SceOrder.ShippingAddress?.CountryCode,
                email_address = primaryCont?.Email,
                is_shop_address = false
            };

        }

        public dataTurn14OrderQuote data { get; set; }
    }

    public class dataTurn14OrderQuote
    {
        public string environment { get; set; }
        public string po_number { get; set; }
        public string order_notes { get; set; }

        public List<locationsTurn14OrderQuote> locations { get; set; }
        public recipientTurn14OrderQuote recipient { get; set; }

    }

    public class locationsTurn14OrderQuote
    {
        public string location { get; set; }
        public bool combine_in_out_stock { get; set; }
        public List<itemsTurn14OrderQuote> items { get; set; }
    }

    public class itemsTurn14OrderQuote
    {
        public string item_identifier { get; set; }
        public string item_identifier_type { get; set; }
        public int quantity { get; set; }
    }

    public class recipientTurn14OrderQuote
    {
        public string name { get; set; }
        public string address { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string email_address { get; set; }
        public string phone_number { get; set; }
        public bool is_shop_address { get; set; }
    }

}
