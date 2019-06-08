using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Dalessuperstore
{
    public class ExtWareInfo : WareInfo
    {
        public ExtWareInfo() { }

        public ExtWareInfo(ExtWareInfo extWareInfo)
        {
            this.GeneralImage = extWareInfo.GeneralImage;
            this.ProductTitle = extWareInfo.ProductTitle;
            this.PartNumber = extWareInfo.PartNumber;
            this.SubTitle = extWareInfo.SubTitle;
            this.Price = extWareInfo.Price;
            this.FitmentDetails = extWareInfo.FitmentDetails;
            this.FitmentCategories = extWareInfo.FitmentCategories;
            this.ItemWeight = extWareInfo.ItemWeight;
            this.PDFManual = extWareInfo.PDFManual;
            this.Bullets = extWareInfo.Bullets;
            this.URL = extWareInfo.URL;
            this.UpsellTitle = extWareInfo.UpsellTitle;
            this.HiddenUpsell = extWareInfo.HiddenUpsell;
            this.Description = extWareInfo.Description;
            this.DescriptionFeatures = extWareInfo.DescriptionFeatures;
            this.ShippingDimensions = extWareInfo.ShippingDimensions;
            this.ProductVideo = extWareInfo.ProductVideo;
            this.HiddenUpsellOptions = extWareInfo.HiddenUpsellOptions;
            this.Year = extWareInfo.Year;
            this.Make = extWareInfo.Make;
            this.Model = extWareInfo.Model;
            this.Engine = extWareInfo.Engine;
            this.StartYear = extWareInfo.StartYear;
            this.EndYear = extWareInfo.EndYear;
        }

        public string GeneralImage { get; set; }
        public string ProductTitle { get; set; }
        public string SubTitle { get; set; }
        public string Price { get; set; }
        public string FitmentDetails { get; set; }
        public string FitmentCategories { get; set; }
        public string ItemWeight { get; set; }
        public string PDFManual { get; set; }
        public string Bullets { get; set; }
        public string UpsellTitle { get; set; }
        public string HiddenUpsell { get; set; }
        public string DescriptionFeatures { get; set; }
        public string ShippingDimensions { get; set; }
        public string ProductVideo { get; set; }
        public int Year { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string HiddenUpsellOptions { get; set; }
    }
}
