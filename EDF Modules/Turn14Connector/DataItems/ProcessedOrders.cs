using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turn14Connector.SCEapi;

namespace Turn14Connector.DataItems.Turn14
{
    class ProcessedOrder : Turn14OrderJson
    {
        public ProcessedOrder() { }
        public ProcessedOrder(Turn14OrderJson turn14Order, DateTime processedDate, OrderSync orderSync = null)
        {
            data = turn14Order.data;
            ProcessedDate = processedDate;

            if (orderSync != null)
            {
                Code = orderSync.SceOrder.ShippingService.Code;
                SceCode = orderSync.SceOrder.ShippingService.SceCode;
                CarrierID = orderSync.SceOrder.ShippingService.CarrierID;
            }
            else
            {
                Code = "43";
                SceCode = eOrderShipType.NextDay;
                CarrierID = eShippingProvider.UPS;
            }
        }

        public DateTime ProcessedDate { get; set; }
        public DateTime TrackNumberDate { get; set; }
        public string TrackNumber { get; set; }

        public string Code { get; set; }
        public eOrderShipType SceCode { get; set; }
        public eShippingProvider CarrierID { get; set; }
    }
}
