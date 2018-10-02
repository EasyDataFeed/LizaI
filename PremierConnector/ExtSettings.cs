using PremierConnector;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Databox.Libs.PremierConnector
{
    public class ExtSettings
    {
        public bool UpdateInventory { get; set; }
        public bool UpdatePrices { get; set; }
        public bool SubmitOrders { get; set; }
        public bool UpdateTrackingNumbers { get; set; }
        public string InvFtpAddress { get; set; }
        public string InvFtpLogin { get; set; }
        public string InvFtpPassword { get; set; }
        public string PremierAPIKey { get; set; }
        public string DefaultPhoneNumber { get; set; }
        [XmlIgnore]
        public string PremierAPIURL { get; set; }
        [XmlIgnore]
        public List<InventoryItem> InventoryItems { get; set; }
        [XmlIgnore]
        public List<ExtSalesOrder> SalesOrders { get; set; }
        [XmlIgnore]
        public DateTime DateFrom { get; set; }
        [XmlIgnore]
        public DateTime TrackingStartDate { get; set; }
        [XmlIgnore]
        public DateTime DateTo { get; set; }
        public List<string> Emails { get; set; }
        public bool CheckPremierExistingInventory { get; set; }
        public ExtSettings()
        {
            SalesOrders = new List<ExtSalesOrder>();
            InventoryItems = new List<InventoryItem>();
            Emails = new List<string>();
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
        }
    }
}
