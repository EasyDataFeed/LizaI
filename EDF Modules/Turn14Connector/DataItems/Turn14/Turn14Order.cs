using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turn14Connector.SCEapi;

namespace Turn14Connector.DataItems.Turn14
{
    class Turn14Order
    {
        public Turn14Order() { }

        public Turn14Order(OrderSync sceOrder, OrderQuoteJson orderQuote, ShippingOptionsJson shippingOptions, bool useProduction = false)
        {
            data = new DataTurn14Order();

            data.environment = useProduction ? "production" : "testing";
            data.po_number = sceOrder.OrderId.ToString();
            data.order_notes = "An order note";

            data.locations = new List<LocationTurn14Order>();
            LocationTurn14Order location = new LocationTurn14Order();

            location.combine_in_out_stock = true;
            location.location = orderQuote.data.attributes.shipment.FirstOrDefault()?.location.ToString();
            location.items = new List<ItemTurn14Order>();

            foreach (var sceItem in sceOrder.SceOrder.OrderItems)
            {
                ItemTurn14Order item = new ItemTurn14Order();
                item.item_identifier = $"{sceItem.PartNo}";
                //item.item_identifier_type = "item_id";
                //item.item_identifier_type = "mfr_part_number";
                item.item_identifier_type = "part_number";
                item.quantity = sceItem.Qty;
                location.items.Add(item);
            }

            ShippingTurn14Order shipping = new ShippingTurn14Order();
            shipping.shipping_code = GetShippingCode(sceOrder, shippingOptions);
            shipping.saturday_delivery = false;
            shipping.signature_required = false;
            location.shipping = shipping;

            data.locations.Add(location);

            var primaryContId = sceOrder.SceOrder.Account.PrimaryContactID;
            var primaryCont = sceOrder.SceOrder.Account.Contacts.FirstOrDefault(a => a.ID == primaryContId) ??
                              sceOrder.SceOrder.Account.Contacts.FirstOrDefault();

            data.recipient = new RecipientTurn14Order
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

        private int GetShippingCode(OrderSync sceOrder, ShippingOptionsJson shippingOptions)
        {
            string turn14ServiceStr = string.Empty;
            switch (sceOrder.SceOrder.ShippingService.SceCode)
            {
                case eOrderShipType.Ground:
                    if (sceOrder.SceOrder.ShippingService.CarrierTag == "UPS")
                    {
                        turn14ServiceStr = shippingOptions.data.FirstOrDefault(i => i.attributes.carrier_name == "UPS" && i.attributes.transportation_name == "UPS Ground")?.id;
                    }

                    int.TryParse(turn14ServiceStr, out int turn14GroundService);

                    return turn14GroundService;
                case eOrderShipType.NextDay:
                    if (sceOrder.SceOrder.ShippingService.CarrierTag == "UPS")
                    {
                        turn14ServiceStr = shippingOptions.data.FirstOrDefault(i => i.attributes.carrier_name == "UPS" && i.attributes.transportation_name == "UPS Next Day Air")?.id;
                    }

                    int.TryParse(turn14ServiceStr, out int turn14NextDayService);

                    return turn14NextDayService;
                case eOrderShipType.TwoDay:
                    if (sceOrder.SceOrder.ShippingService.CarrierTag == "UPS")
                    {
                        turn14ServiceStr = shippingOptions.data.FirstOrDefault(i => i.attributes.carrier_name == "UPS" && i.attributes.transportation_name == "UPS Second Day Air")?.id;
                    }

                    int.TryParse(turn14ServiceStr, out int turn14TwoDayService);

                    return turn14TwoDayService;
                case eOrderShipType.ThreeDay:
                    if (sceOrder.SceOrder.ShippingService.CarrierTag == "USPS")
                    {
                        turn14ServiceStr = shippingOptions.data.FirstOrDefault(i => i.attributes.carrier_name == "USPS" && i.attributes.transportation_name == "USPS Priority Mail")?.id;
                    }

                    int.TryParse(turn14ServiceStr, out int turn14ThreeDayService);

                    return turn14ThreeDayService;
                default:
                    return 0;
            }
        }

        public DataTurn14Order data { get; set; }
    }

    public class DataTurn14Order
    {
        public string environment { get; set; }
        public string po_number { get; set; }
        public string order_notes { get; set; }
        public List<LocationTurn14Order> locations { get; set; }
        public List<Expedited_LogisticsTurn14Order> expedited_logistics { get; set; }
        public bool acknowledge_prop_65 { get; set; }
        public RecipientTurn14Order recipient { get; set; }
    }

    public class RecipientTurn14Order
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

    public class LocationTurn14Order
    {
        public string location { get; set; }
        public bool combine_in_out_stock { get; set; }
        public List<ItemTurn14Order> items { get; set; }
        public ShippingTurn14Order shipping { get; set; }
    }

    public class ShippingTurn14Order
    {
        public int shipping_code { get; set; }
        public bool saturday_delivery { get; set; }
        public bool signature_required { get; set; }
    }

    public class ItemTurn14Order
    {
        public string item_identifier { get; set; }
        public string item_identifier_type { get; set; }
        public int quantity { get; set; }
    }

    public class Expedited_LogisticsTurn14Order
    {
        public int dropship_controller_id { get; set; }
        public string days { get; set; }
        public string location { get; set; }
    }

}

