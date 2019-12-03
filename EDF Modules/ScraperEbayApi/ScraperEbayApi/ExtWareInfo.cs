﻿using WheelsScraper;

namespace ScraperEbayApi
{
    public class ExtWareInfo : WareInfo
    {
        public string Timestamp { get; set; }
        public string UserId { get; set; }
        public double ConvertedCurrentPrice { get; set; }
        public string ListingStatus { get; set; }
        public string HitCount { get; set; }
        public string StoreName { get; set; }
        public string ItemCompatibilityCount { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Engine { get; set; }
        public string Notes { get; set; }
        public string QuantitySold { get; set; }
        public string ItemNumber { get; set; }
        public string ShippingServiceCost { get; set; }
        public string ShippingType { get; set; }
        public string ListedShippingServiceCost { get; set; }
        public string OriginalRetailPrice { get; set; }
        public string PartBrand { get; set; }
        public string Specifications { get; set; }
        public string ProductTitle { get; set; }
        public string UPC { get; set; }
        public string InterchangePartNumber { get; set; }
        public string OtherPartNumber { get; set; }
        public string Categories { get; set; }
        public string SubCategory { get; set; }
        public ExtWareInfo() { }

        public ExtWareInfo(ExtWareInfo wi)
        {
            Timestamp = wi.Timestamp;
            UserId = wi.UserId;
            Brand = wi.Brand;
            Description = wi.Description;
            ManufacturerNumber = wi.ManufacturerNumber;
            ConvertedCurrentPrice = wi.ConvertedCurrentPrice;
            ListingStatus = wi.ListingStatus;
            HitCount = wi.HitCount;
            StoreName = wi.StoreName;
            ItemCompatibilityCount = wi.ItemCompatibilityCount;
            Year = wi.Year;
            Make = wi.Make;
            Model = wi.Model;
            Trim = wi.Trim;
            Engine = wi.Engine;
            Notes = wi.Notes;
            wi.Quantity = wi.Quantity;
            QuantitySold = wi.QuantitySold;
            ItemNumber = wi.ItemNumber;
            ShippingServiceCost = wi.ShippingServiceCost;
            ShippingType = wi.ShippingType;
            ListedShippingServiceCost = wi.ListedShippingServiceCost;
            OriginalRetailPrice = wi.OriginalRetailPrice;
            PartBrand = wi.PartBrand;
            Specifications = wi.Specifications;
            ProductTitle = wi.ProductTitle;
            UPC = wi.UPC;
            PartNumber = wi.PartNumber;
            InterchangePartNumber = wi.InterchangePartNumber;
            OtherPartNumber = wi.OtherPartNumber;
            ImageUrl = wi.ImageUrl;
            Categories = wi.Categories;
            SubCategory = wi.SubCategory;
        }
    }
}
