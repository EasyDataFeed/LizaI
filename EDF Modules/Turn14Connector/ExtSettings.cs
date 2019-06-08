#region using

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Turn14Connector.DataItems;
using Turn14Connector.DataItems.Turn14;

#endregion

namespace Databox.Libs.Turn14Connector
{
    public class ExtSettings
    {

        #region Constructors

        public ExtSettings()
        {
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            OrdersToSync = new List<OrderSync>();
            BrandsAlignments = new List<BrandsAlignment>();
            TransferInfoItems = new List<TransferInfoItem>();
            Turn14Brands = new BrandsJson();
            BrandsForScraping = new BrandsJson();
            VehicleInfo = new List<VehicleInfoForFitments>();
        }

        #endregion

        #region Connector Setting

        public bool DoBatch { get; set; }
        public bool InventorySync { get; set; }
        public bool PriceSync { get; set; }
        public bool OrdersSync { get; set; }
        public bool ConsiderMapPrice { get; set; }

        public string BrandFilePath { get; set; }
        public int PercentageForMsrp { get; set; }

        [XmlIgnore]
        public DateTime DateFrom { get; set; }
        [XmlIgnore]
        public DateTime DateTo { get; set; }

        public double SurchargePerLb { get; set; }
        public double MinSurcharge { get; set; }
        public bool DoPricePerPound { get; set; }
        public string VehicleInfoForFitments { get; set; }

        [XmlIgnore]
        public List<OrderSync> OrdersToSync { get; set; }

        [XmlIgnore]
        public List<VehicleInfoForFitments> VehicleInfo { get; set; }

        [XmlIgnore]
        public List<BrandsAlignment> BrandsAlignments { get; set; }

        [XmlIgnore]
        public List<TransferInfoItem> TransferInfoItems { get; set; }
        #endregion

        #region Scraping Setting

        [XmlIgnore]
        public BrandsJson Turn14Brands { get; set; }
        [XmlIgnore]
        public BrandsJson BrandsForScraping { get; set; }

        #endregion

        #region Turn14 Setting

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        #endregion

    }
}
