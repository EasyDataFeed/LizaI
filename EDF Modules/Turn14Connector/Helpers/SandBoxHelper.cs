using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turn14Connector.DataItems.Turn14;

namespace Turn14Connector.Helpers
{
    class SandBoxHelper
    {
        public TrackingNumbersJson GetTrackingNumbersJson()
        {
            TrackingNumbersJson trackingNumbersJson = new TrackingNumbersJson();

            dataTrackingNumbers dataTrackingNumbers = new dataTrackingNumbers();

            dataTrackingNumbers.type = "Tracking";
            dataTrackingNumbers.id = "123456";
            attributesTrackingNumbers attributes = new attributesTrackingNumbers();

            attributes.tracking_number = "1Z4XR1234567891235";
            attributes.service = "UPS Next Day Air";

            referencesTrackingNumbers referencesTrackingNumbers = new referencesTrackingNumbers();

            referencesTrackingNumbers.type = "Invoice";
            referencesTrackingNumbers.id = "123456";
            referencesTrackingNumbers.number = "789012";
            referencesTrackingNumbers.purchase_order_number = "90085";

            dataTrackingNumbers.attributes = attributes;
            dataTrackingNumbers.attributes.references = new List<referencesTrackingNumbers>();
            dataTrackingNumbers.attributes.references.Add(referencesTrackingNumbers);
            trackingNumbersJson.data = new List<dataTrackingNumbers>();
            trackingNumbersJson.data.Add(dataTrackingNumbers);
            return trackingNumbersJson;
        }

        public Turn14Order GeTurn14Order()
        {
            Turn14Order turn14Order = new Turn14Order();
            DataTurn14Order data = new DataTurn14Order();
            data.environment = "testing";
            data.po_number = "90085";
            data.order_notes = "An order note";

            data.locations = new List<LocationTurn14Order>();
            LocationTurn14Order location = new LocationTurn14Order();

            location.combine_in_out_stock = true;
            location.location = "59";
            location.items = new List<ItemTurn14Order>();

            ItemTurn14Order item = new ItemTurn14Order();
            item.item_identifier = $"afe54-11478";
            item.item_identifier_type = "part_number";
            item.quantity = 1;
            location.items.Add(item);

            ShippingTurn14Order shipping = new ShippingTurn14Order();
            shipping.shipping_code = 1;//UPS Next Day Air
            shipping.saturday_delivery = false;
            shipping.signature_required = false;
            location.shipping = shipping;

            data.locations.Add(location);

            data.recipient = new RecipientTurn14Order
            {
                name = $"Test Test",
                phone_number = "267-468-0350",
                zip = "98001",
                address = "1",
                address_2 = "1",
                city = "Auburn",
                state = "WA",
                country = "US",
                email_address = "tetstest@mail.com",
                is_shop_address = false
            };

            turn14Order.data = data;

            return turn14Order;
        }

        public Turn14OrderQuote GetTurn14OrderQuote()
        {
            Turn14OrderQuote turn14OrderQuote = new Turn14OrderQuote();

            dataTurn14OrderQuote data = new dataTurn14OrderQuote();
            data.environment = "testing";
            data.po_number = "90085";
            data.order_notes = "An order note";

            data.locations = new List<locationsTurn14OrderQuote>();
            locationsTurn14OrderQuote location = new locationsTurn14OrderQuote();

            location.combine_in_out_stock = true;
            location.location = "default";
            location.items = new List<itemsTurn14OrderQuote>();

            itemsTurn14OrderQuote item = new itemsTurn14OrderQuote();
            item.item_identifier = $"afe54-11478";
            item.item_identifier_type = "part_number";
            item.quantity = 1;
            location.items.Add(item);

            data.locations.Add(location);

            data.recipient = new recipientTurn14OrderQuote
            {
                name = $"Test Test",
                phone_number = "267-468-0350",
                zip = "98001",
                address = "1",
                address_2 = "1",
                city = "Auburn",
                state = "WA",
                country = "US",
                email_address = "tetstest@mail.com",
                is_shop_address = false
            };

            turn14OrderQuote.data = data;

            return turn14OrderQuote;
        }
    }
}
