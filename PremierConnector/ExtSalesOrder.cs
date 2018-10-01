using PremierConnector.JsonTypes;
using PremierConnector.sceAPI;
using System;
using System.Web.Script.Serialization;

namespace PremierConnector
{
    public class ExtSalesOrder : SalesOrder
    {
        [ScriptIgnore]
        public eOrderShipType SceCode { get; set; }
        [ScriptIgnore]
        public eShippingProvider ShipCarrierTag { get; set; }
        [ScriptIgnore]
        public string ShippingCode { get; set; }
        [ScriptIgnore]
        public bool Submit { get; set; }
        [ScriptIgnore]
        public int SceOrderID { get; set; }
        [ScriptIgnore]
        public string Message { get; set; }
        [ScriptIgnore]
        public DateTime Date { get; set; }
    }
}
