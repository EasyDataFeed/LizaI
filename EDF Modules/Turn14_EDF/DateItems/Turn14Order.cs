using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turn14_EDF.SCEapi;

namespace Turn14_EDF.DateItems
{
    class Turn14Order
    {
        public int OrderSceID { get; set; }
        public string Date { get; set; }
        public string Code { get; set; }
        public eOrderShipType SCECode { get; set; }
        public eShippingProvider CarrierID { get; set; }
        public List<Shipment> Shipments { get; set; }
        public bool TrackingNoUpdated { get; set; }

        public Turn14Order(int orderID, string date, List<Shipment> shipments, string code, eOrderShipType sceCode, eShippingProvider carrierID)
        {
            OrderSceID = orderID;
            Date = date;
            Shipments = shipments;
            TrackingNoUpdated = false;
            Code = code;
            SCECode = sceCode;
            CarrierID = carrierID;
        }
    }
}
