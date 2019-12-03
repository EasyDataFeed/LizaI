using ScraperApiTurn14.DataItems;
using ScraperApiTurn14.DataItems.Turn14;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Databox.Libs.ScraperApiTurn14
{
    public class ExtSettings
    {

        #region Constructors

        public ExtSettings()
        {
            DateFrom = DateTime.Today;
            DateTo = DateTime.Today;
            TransferInfoItems = new List<TransferInfoItem>();
            Turn14Brands = new BrandsJson();
            BrandsForScraping = new BrandsJson();
            VehicleInfo = new List<VehicleInfoForFitments>();
        }

        #endregion

        #region Connector Setting

        [XmlIgnore]
        public DateTime DateFrom { get; set; }
        [XmlIgnore]
        public DateTime DateTo { get; set; }
        public string VehicleInfoForFitments { get; set; }

        [XmlIgnore]
        public List<VehicleInfoForFitments> VehicleInfo { get; set; }

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
