using Databox.Libs.MarksJewelersSuppliersData;
using MarksJewelersSuppliersData.DataItems;
using MarksJewelersSuppliersData.Enums;
using MarksJewelersSuppliersData.Extensions;
using MarksJewelersSuppliersData.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MarksJewelersSuppliersData.SupplierDataBuilders
{
    public class SDBTagHeuer
    {
        private const int DescriptionLength = 100;

        public static string BuildProductTitle<T>(ProductType type, T item)
        {
            string productTitle = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    productTitle = $"{itemData.Gender} {itemData.ProductTitle} {itemData.CaseDiameter} with {itemData.TypeOfBand}";
                }
            }

            return productTitle;
        }

        public static string BuildAnchorText<T>(ProductType type, T item, SceBatchItem batchItem)
        {
            string anchorText = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    anchorText = batchItem.ProductTitle.Truncate(DescriptionLength);
                }
            }

            return anchorText;
        }

        public static string BuildSpiderURL<T>(ProductType type, T item, SceBatchItem batchItem)
        {
            string spiderURL = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    spiderURL = batchItem.AnchorText.Truncate(DescriptionLength);
                }
            }

            return spiderURL;
        }

        public static string BuildSubTitle<T>(ProductType type, T item, SceBatchItem batchItem)
        {
            string subTitle = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    subTitle = itemData.WebId;
                }
            }

            return subTitle;
        }

        public static string BuildBrand<T>(ProductType type, T item)
        {
            string brand = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    brand = itemData.Designer;
                }
            }

            return brand;
        }

        public static string BuildDescription<T>(ProductType type, T item)
        {
            string description = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    description = $"{itemData.ProductDescription}<br>Warranty: {itemData.WarrantyInfo}";
                }
            }

            return description;
        }

        public static string BuildMETADescription<T>(ProductType type, T item, SceBatchItem batchItem)
        {
            string mETADescription = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    mETADescription = batchItem.ProductTitle;
                }
            }

            return mETADescription;
        }

        public static string BuildMETAKeywords<T>(ProductType type, T item, SceBatchItem batchItem)
        {
            string mETAKeywords = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    mETAKeywords = batchItem.ProductTitle;
                }
            }

            return mETAKeywords;
        }

        public static string BuildGeneralImage<T>(ProductType type, T item, ExtSettings extSett)
        {
            string generalImage = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    foreach (var image in extSett.GeneralImageList)
                    {
                        if (Path.GetFileNameWithoutExtension(image) == itemData.MPN)
                        {
                            generalImage += $"{image},";
                        }
                    }

                    generalImage = generalImage.TrimEnd(',');
                }
            }

            return generalImage;
        }

        public static string BuildPartNumber<T>(ProductType type, T item)
        {
            string partNumber = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    partNumber = itemData.MPN;
                }
            }

            return partNumber;
        }

        public static double BuildMSRP<T>(ProductType type, T item)
        {
            double mSRP = 0;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    mSRP = itemData.WebPrice;
                }
            }

            return mSRP;
        }

        public static double BuildJobber<T>(ProductType type, T item)
        {
            double jobber = 0;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    jobber = itemData.WebPrice;
                }
            }

            return jobber;
        }

        public static double BuildWebPrice<T>(ProductType type, T item)
        {
            double webPrice = 0;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    webPrice = itemData.WebPrice;
                }
            }

            return webPrice;
        }

        public static double BuildCostPrice<T>(ProductType type, T item)
        {
            double costPrice = 0;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    costPrice = itemData.Cost;
                }
            }

            return costPrice;
        }

        public static string BuildSupplier<T>(ProductType type, T item)
        {
            string supplier = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    supplier = "Tag Heuer";
                }
            }

            return supplier;
        }

        public static string BuildReOrderSupplier<T>(ProductType type, T item)
        {
            string reOrderSupplier = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    reOrderSupplier = "Tag Heuer";
                }
            }

            return reOrderSupplier;
        }

        public static string BuildWarehouse<T>(ProductType type, T item)
        {
            string warehouse = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    warehouse = "Tag Heuer";
                }
            }

            return warehouse;
        }

        public static string BuildProcessingTime<T>(ProductType type, T item)
        {
            string processingTime = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    processingTime = "10";
                }
            }

            return processingTime;
        }

        public static string BuildShippingType<T>(ProductType type, T item)
        {
            string shippingType = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingType = "dynamic";
                }
            }

            return shippingType;
        }

        public static string BuildShippingCarrier1<T>(ProductType type, T item)
        {
            string shippingCarrier1 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingCarrier1 = "FedEX";
                }
            }

            return shippingCarrier1;
        }

        public static string BuildAllowground<T>(ProductType type, T item)
        {
            string allowground = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    allowground = "1";
                }
            }

            return allowground;
        }

        public static string BuildAllow3day<T>(ProductType type, T item)
        {
            string allow3day = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    allow3day = "1";
                }
            }

            return allow3day;
        }

        public static string BuildAllow2day<T>(ProductType type, T item)
        {
            string allow2day = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    allow2day = "1";
                }
            }

            return allow2day;
        }

        public static string BuildAllownextday<T>(ProductType type, T item)
        {
            string allownextday = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    allownextday = "-1";
                }
            }

            return allownextday;
        }

        public static string BuildShippingGroundRate<T>(ProductType type, T item)
        {
            string shippingGroundRate = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingGroundRate = "free";
                }
            }

            return shippingGroundRate;
        }

        public static string BuildShippingNextDayAirRate<T>(ProductType type, T item)
        {
            string shippingNextDayAirRate = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingNextDayAirRate = "free";
                }
            }

            return shippingNextDayAirRate;
        }

        public static string BuildItemWeight<T>(ProductType type, T item)
        {
            string itemWeight = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    itemWeight = "1";
                }
            }

            return itemWeight;
        }

        public static string BuildItemHeight<T>(ProductType type, T item)
        {
            string itemHeight = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    itemHeight = "1";
                }
            }

            return itemHeight;
        }

        public static string BuildItemWidth<T>(ProductType type, T item)
        {
            string itemWidth = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    itemWidth = "1";
                }
            }

            return itemWidth;
        }

        public static string BuildItemLength<T>(ProductType type, T item)
        {
            string itemLength = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    itemLength = "1";
                }
            }

            return itemLength;
        }

        public static string BuildShippingWeight<T>(ProductType type, T item)
        {
            string shippingWeight = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingWeight = "1";
                }
            }

            return shippingWeight;
        }

        public static string BuildShippingHeight<T>(ProductType type, T item)
        {
            string shippingHeight = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingHeight = "1";
                }
            }

            return shippingHeight;
        }

        public static string BuildShippingWidth<T>(ProductType type, T item)
        {
            string shippingWidth = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingWidth = "1";
                }
            }

            return shippingWidth;
        }

        public static string BuildShippingLength<T>(ProductType type, T item)
        {
            string shippingLength = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    shippingLength = "1";
                }
            }

            return shippingLength;
        }

        public static string BuildAllowNewCategory<T>(ProductType type, T item)
        {
            string allowNewCategory = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    allowNewCategory = "1";
                }
            }

            return allowNewCategory;
        }

        public static string BuildAllowNewBrand<T>(ProductType type, T item)
        {
            string allowNewBrand = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    allowNewBrand = "1";
                }
            }

            return allowNewBrand;
        }

        public static string BuildMainCategory<T>(ProductType type, T item)
        {
            string mainCategory = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    mainCategory = "Timepieces";
                }
            }

            return mainCategory;
        }

        public static string BuildSubCategory<T>(ProductType type, T item)
        {
            string subCategory = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    subCategory = "Shop By Designer";
                }
            }

            return subCategory;
        }

        public static string BuildSectionCategory<T>(ProductType type, T item)
        {
            string sectionCategory = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    sectionCategory = "Tag Heuer";
                }
            }

            return sectionCategory;
        }

        public static string BuildCrossSellMainCat1<T>(ProductType type, T item)
        {
            string crossSellMainCat1 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat1 = "Timepieces";
                }
            }

            return crossSellMainCat1;
        }

        public static string BuildCrossSellSubCat1<T>(ProductType type, T item)
        {
            string crossSellSubCat1 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat1 = "Shop By Gender";
                }
            }

            return crossSellSubCat1;
        }

        public static string BuildCrossSellSecCat1<T>(ProductType type, T item)
        {
            string crossSellSecCat1 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Gender == "Male")
                    {
                        crossSellSecCat1 = "Men's";
                    }
                    else if (itemData.Gender == "Female")
                    {
                        crossSellSecCat1 = "Ladies";
                    }
                }
            }

            return crossSellSecCat1;
        }

        public static string BuildCrossSellMainCat2<T>(ProductType type, T item)
        {
            string crossSellMainCat2 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat2 = "Timepieces";
                }
            }

            return crossSellMainCat2;
        }

        public static string BuildCrossSellSubCat2<T>(ProductType type, T item)
        {
            string crossSellSubCat2 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat2 = "Shop By Shape";
                }
            }

            return crossSellSubCat2;
        }

        public static string BuildCrossSellSecCat2<T>(ProductType type, T item)
        {
            string crossSellSecCat2 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.CaseShape == "round")
                    {
                        crossSellSecCat2 = "Round";
                    }
                    else if (itemData.CaseShape == "square")
                    {
                        crossSellSecCat2 = "Square";
                    }
                }
            }

            return crossSellSecCat2;
        }

        public static string BuildCrossSellMainCat3<T>(ProductType type, T item)
        {
            string crossSellMainCat3 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat3 = "Timepieces";
                }
            }

            return crossSellMainCat3;
        }

        public static string BuildCrossSellSubCat3<T>(ProductType type, T item)
        {
            string crossSellSubCat3 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat3 = "Shop By Band";
                }
            }

            return crossSellSubCat3;
        }

        public static string BuildCrossSellSecCat3<T>(ProductType type, T item)
        {
            string crossSellSecCat3 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.TypeOfBand.Contains("bracelet") || itemData.TypeOfBand.Contains("ceramic") || itemData.TypeOfBand.Contains("steel"))
                    {
                        crossSellSecCat3 = "Bracelet";
                    }
                    else if (itemData.TypeOfBand.Contains("strap") || itemData.TypeOfBand.Contains("rubber"))
                    {
                        crossSellSecCat3 = "Strap";
                    }
                }
            }

            return crossSellSecCat3;
        }

        public static string BuildCrossSellMainCat4<T>(ProductType type, T item)
        {
            string crossSellMainCat4 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat4 = "Tag Heuer";
                }
            }

            return crossSellMainCat4;
        }

        public static string BuildCrossSellSubCat4<T>(ProductType type, T item)
        {
            string crossSellSubCat4 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat4 = "Shop By Band";
                }
            }

            return crossSellSubCat4;
        }

        public static string BuildCrossSellSecCat4<T>(ProductType type, T item)
        {
            string crossSellSecCat4 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.TypeOfBand.Contains("bracelet") || itemData.TypeOfBand.Contains("ceramic") || itemData.TypeOfBand.Contains("steel"))
                    {
                        crossSellSecCat4 = "Bracelet";
                    }
                    else if (itemData.TypeOfBand.Contains("strap") || itemData.TypeOfBand.Contains("rubber"))
                    {
                        crossSellSecCat4 = "Strap";
                    }
                }
            }

            return crossSellSecCat4;
        }

        public static string BuildCrossSellMainCat5<T>(ProductType type, T item)
        {
            string crossSellMainCat5 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat5 = "Timepieces";
                }
            }

            return crossSellMainCat5;
        }

        public static string BuildCrossSellSubCat5<T>(ProductType type, T item)
        {
            string crossSellSubCat5 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat5 = "Shop By Size";
                }
            }

            return crossSellSubCat5;
        }

        public static string BuildCrossSellSecCat5<T>(ProductType type, T item)
        {
            string crossSellSecCat5 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.CaseDiameter < 32.00)
                    {
                        crossSellSecCat5 = "Small";
                    }
                    else if (itemData.CaseDiameter >= 32.00 && itemData.CaseDiameter <= 42.00)
                    {
                        crossSellSecCat5 = "Medium";
                    }
                    else if (itemData.CaseDiameter > 42.00)
                    {
                        crossSellSecCat5 = "Large";
                    }
                }
            }

            return crossSellSecCat5;
        }

        public static string BuildCrossSellMainCat6<T>(ProductType type, T item)
        {
            string crossSellMainCat6 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat6 = "Tag Heuer";
                }
            }

            return crossSellMainCat6;
        }

        public static string BuildCrossSellSubCat6<T>(ProductType type, T item)
        {
            string crossSellSubCat6 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat6 = "Shop By Size";
                }
            }

            return crossSellSubCat6;
        }

        public static string BuildCrossSellSecCat6<T>(ProductType type, T item)
        {
            string crossSellSecCat6 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.CaseDiameter < 32.00)
                    {
                        crossSellSecCat6 = "Small";
                    }
                    else if (itemData.CaseDiameter >= 32.00 && itemData.CaseDiameter <= 42.00)
                    {
                        crossSellSecCat6 = "Medium";
                    }
                    else if (itemData.CaseDiameter > 42.00)
                    {
                        crossSellSecCat6 = "Large";
                    }
                }
            }

            return crossSellSecCat6;
        }

        public static string BuildCrossSellSecCat7<T>(ProductType type, T item)
        {
            string crossSellSecCat7 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.StoneId == "Diamonds")
                    {
                        crossSellSecCat7 = "Diamond";
                    }
                    else
                    {
                        crossSellSecCat7 = "";
                    }
                }
            }

            return crossSellSecCat7;
        }

        public static string BuildCrossSellMainCat7<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat7 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat7))
                    {
                        crossSellMainCat7 = "Tag Heuer";
                    }
                }
            }

            return crossSellMainCat7;
        }

        public static string BuildCrossSellSubCat7<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat7 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat7))
                    {
                        crossSellSubCat7 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat7;
        }

        public static string BuildCrossSellSecCat8<T>(ProductType type, T item)
        {
            string crossSellSecCat8 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.StoneId == "Diamonds")
                    {
                        crossSellSecCat8 = "Fashion";
                    }
                    else
                    {
                        crossSellSecCat8 = "";
                    }
                }
            }

            return crossSellSecCat8;
        }

        public static string BuildCrossSellMainCat8<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat8 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat8))
                    {
                        crossSellMainCat8 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat8;
        }

        public static string BuildCrossSellSubCat8<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat8 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat8))
                    {
                        crossSellSubCat8 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat8;
        }

        public static string BuildCrossSellSecCat9<T>(ProductType type, T item)
        {
            string crossSellSecCat9 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.StoneId == "Diamonds")
                    {
                        crossSellSecCat9 = "Diamond";
                    }
                    else
                    {
                        crossSellSecCat9 = "";
                    }
                }
            }

            return crossSellSecCat9;
        }

        public static string BuildCrossSellMainCat9<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat9 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat9))
                    {
                        crossSellMainCat9 = "Tag Heuer";
                    }
                }
            }

            return crossSellMainCat9;
        }

        public static string BuildCrossSellSubCat9<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat9 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat9))
                    {
                        crossSellSubCat9 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat9;
        }

        public static string BuildCrossSellSecCat10<T>(ProductType type, T item)
        {
            string crossSellSecCat10 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.StoneId == "Diamonds")
                    {
                        crossSellSecCat10 = "Fashion";
                    }
                    else
                    {
                        crossSellSecCat10 = "";
                    }
                }
            }

            return crossSellSecCat10;
        }

        public static string BuildCrossSellMainCat10<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat10 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat10))
                    {
                        crossSellMainCat10 = "Tag Heuer";
                    }
                }
            }

            return crossSellMainCat10;
        }

        public static string BuildCrossSellSubCat10<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat10 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat10))
                    {
                        crossSellSubCat10 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat10;
        }

        public static string BuildCrossSellSecCat11<T>(ProductType type, T item)
        {
            string crossSellSecCat11 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.StoneId == "Diamonds")
                    {
                        crossSellSecCat11 = "Diamond";
                    }
                    else
                    {
                        crossSellSecCat11 = "";
                    }
                }
            }

            return crossSellSecCat11;
        }

        public static string BuildCrossSellMainCat11<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat11 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat11))
                    {
                        crossSellMainCat11 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat11;
        }

        public static string BuildCrossSellSubCat11<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat11 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat11))
                    {
                        crossSellSubCat11 = "Shop Automatics";
                    }
                }
            }

            return crossSellSubCat11;
        }

        public static string BuildCrossSellSecCat12<T>(ProductType type, T item)
        {
            string crossSellSecCat12 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.StoneId == "Diamonds")
                    {
                        crossSellSecCat12 = "Fashion";
                    }
                    else
                    {
                        crossSellSecCat12 = "";
                    }
                }
            }

            return crossSellSecCat12;
        }

        public static string BuildCrossSellMainCat12<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat12 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat12))
                    {
                        crossSellMainCat12 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat12;
        }

        public static string BuildCrossSellSubCat12<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat12 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat12))
                    {
                        crossSellSubCat12 = "Shop Automatics";
                    }
                }
            }

            return crossSellSubCat12;
        }

        public static string BuildCrossSellSecCat13<T>(ProductType type, T item)
        {
            string crossSellSecCat13 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.WatchType.Contains("Chronograph") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                    {
                        crossSellSecCat13 = "Chronograph";
                    }
                    else
                    {
                        crossSellSecCat13 = "";
                    }
                }
            }

            return crossSellSecCat13;
        }

        public static string BuildCrossSellMainCat13<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat13 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat13))
                    {
                        crossSellMainCat13 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat13;
        }

        public static string BuildCrossSellSubCat13<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat13 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat13))
                    {
                        crossSellSubCat13 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat13;
        }

        public static string BuildCrossSellSecCat14<T>(ProductType type, T item)
        {
            string crossSellSecCat14 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.WatchType.Contains("Chronograph") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                    {
                        crossSellSecCat14 = "Chronograph";
                    }
                    else
                    {
                        crossSellSecCat14 = "";
                    }
                }
            }

            return crossSellSecCat14;
        }

        public static string BuildCrossSellMainCat14<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat14 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat14))
                    {
                        crossSellMainCat14 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat14;
        }

        public static string BuildCrossSellSubCat14<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat14 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat14))
                    {
                        crossSellSubCat14 = "Shop Automatics";
                    }
                }
            }

            return crossSellSubCat14;
        }

        public static string BuildCrossSellSecCat15<T>(ProductType type, T item)
        {
            string crossSellSecCat15 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.WatchType.Contains("Chronograph") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                    {
                        crossSellSecCat15 = "Chronograph";
                    }
                    else
                    {
                        crossSellSecCat15 = "";
                    }
                }
            }

            return crossSellSecCat15;
        }

        public static string BuildCrossSellMainCat15<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellMainCat15 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat15))
                    {
                        crossSellMainCat15 = "Tag Heuer";
                    }
                }
            }

            return crossSellMainCat15;
        }

        public static string BuildCrossSellSubCat15<T>(ProductType type, T item, SceBatchItem sceBatchItem)
        {
            string crossSellSubCat15 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (!string.IsNullOrEmpty(sceBatchItem.CrossSellSecCat15))
                    {
                        crossSellSubCat15 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat15;
        }

        public static string BuildCrossSellMainCat16<T>(ProductType type, T item)
        {
            string crossSellMainCat16 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat16 = "Tag Heuer";
                }
            }

            return crossSellMainCat16;
        }

        public static string BuildCrossSellSubCat16<T>(ProductType type, T item)
        {
            string crossSellSubCat16 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat16 = "Tag Heuer Collections";
                }
            }

            return crossSellSubCat16;
        }

        public static string BuildCrossSellSecCat16<T>(ProductType type, T item)
        {
            string crossSellSecCat16 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSecCat16 = itemData.Collection;
                }
            }

            return crossSellSecCat16;
        }

        public static string BuildCrossSellMainCat17<T>(ProductType type, T item)
        {
            string crossSellMainCat17 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Monaco" || itemData.Collection == "Autavia" || itemData.Collection == "Monza")
                    {
                        crossSellMainCat17 = "Timepieces";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                        {
                            crossSellMainCat17 = "Timepieces";
                        }
                    }
                }
            }

            return crossSellMainCat17;
        }

        public static string BuildCrossSellSubCat17<T>(ProductType type, T item)
        {
            string crossSellSubCat17 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Monaco" || itemData.Collection == "Autavia" || itemData.Collection == "Monza")
                    {
                        crossSellSubCat17 = "Shop By Style";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                        {
                            crossSellSubCat17 = "Shop By Style";
                        }
                    }
                }
            }

            return crossSellSubCat17;
        }

        public static string BuildCrossSellSecCat17<T>(ProductType type, T item)
        {
            string crossSellSecCat17 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Monaco" || itemData.Collection == "Autavia" || itemData.Collection == "Monza")
                    {
                        crossSellSecCat17 = "Sport";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                        {
                            crossSellSecCat17 = "Sport";
                        }
                    }
                }
            }

            return crossSellSecCat17;
        }

        public static string BuildCrossSellMainCat18<T>(ProductType type, T item)
        {
            string crossSellMainCat18 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        crossSellMainCat18 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat18;
        }

        public static string BuildCrossSellSubCat18<T>(ProductType type, T item)
        {
            string crossSellSubCat18 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        crossSellSubCat18 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat18;
        }

        public static string BuildCrossSellSecCat18<T>(ProductType type, T item)
        {
            string crossSellSecCat18 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        crossSellSecCat18 = "Dive";
                    }
                }
            }

            return crossSellSecCat18;
        }

        public static string BuildCrossSellMainCat19<T>(ProductType type, T item)
        {
            string crossSellMainCat19 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Link")
                    {
                        crossSellMainCat19 = "Timepieces";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9") || itemData.WatchType.Contains("Quartz Watch"))
                        {
                            crossSellMainCat19 = "Timepieces";
                        }
                    }
                }
            }

            return crossSellMainCat19;
        }

        public static string BuildCrossSellSubCat19<T>(ProductType type, T item)
        {
            string crossSellSubCat19 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Link")
                    {
                        crossSellSubCat19 = "Shop By Style";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9") || itemData.WatchType.Contains("Quartz Watch"))
                        {
                            crossSellSubCat19 = "Shop By Style";
                        }
                    }
                }
            }

            return crossSellSubCat19;
        }

        public static string BuildCrossSellSecCat19<T>(ProductType type, T item)
        {
            string crossSellSecCat19 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Link")
                    {
                        crossSellSecCat19 = "Dress";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9") || itemData.WatchType.Contains("Quartz Watch"))
                        {
                            crossSellSecCat19 = "Dress";
                        }
                    }
                }
            }

            return crossSellSecCat19;
        }

        public static string BuildCrossSellMainCat20<T>(ProductType type, T item)
        {
            string crossSellMainCat20 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellMainCat20 = "Timepieces";
                    }
                    else if (itemData.Collection == "Aquaracer" || itemData.Collection == "Formula 1")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellMainCat20 = "Timepieces";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T"))
                        {
                            crossSellMainCat20 = "Timepieces";
                        }
                    }
                }
            }

            return crossSellMainCat20;
        }

        public static string BuildCrossSellSubCat20<T>(ProductType type, T item)
        {
            string crossSellSubCat20 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellSubCat20 = "Shop Automatics";
                    }
                    else if (itemData.Collection == "Aquaracer" || itemData.Collection == "Formula 1")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSubCat20 = "Shop Automatics";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T"))
                        {
                            crossSellSubCat20 = "Shop Automatics";
                        }
                    }
                }
            }

            return crossSellSubCat20;
        }

        public static string BuildCrossSellSecCat20<T>(ProductType type, T item)
        {
            string crossSellSecCat20 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellSecCat20 = "Sport";
                    }
                    else if (itemData.Collection == "Aquaracer" || itemData.Collection == "Formula 1")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSecCat20 = "Sport";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T"))
                        {
                            crossSellSecCat20 = "Sport";
                        }
                    }
                }
            }

            return crossSellSecCat20;
        }

        public static string BuildCrossSellMainCat21<T>(ProductType type, T item)
        {
            string crossSellMainCat21 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco")
                    {
                        crossSellMainCat21 = "Timepieces";
                    }
                    else if (itemData.Collection == "Link")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellMainCat21 = "Timepieces";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9"))
                        {
                            crossSellMainCat21 = "Timepieces";
                        }
                    }
                }
            }

            return crossSellMainCat21;
        }

        public static string BuildCrossSellSubCat21<T>(ProductType type, T item)
        {
            string crossSellSubCat21 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco")
                    {
                        crossSellSubCat21 = "Shop Automatics";
                    }
                    else if (itemData.Collection == "Link")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSubCat21 = "Shop Automatics";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9"))
                        {
                            crossSellSubCat21 = "Shop Automatics";
                        }
                    }
                }
            }

            return crossSellSubCat21;
        }

        public static string BuildCrossSellSecCat21<T>(ProductType type, T item)
        {
            string crossSellSecCat21 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco")
                    {
                        crossSellSecCat21 = "Dive";
                    }
                    else if (itemData.Collection == "Link")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSecCat21 = "Dive";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9"))
                        {
                            crossSellSecCat21 = "Dive";
                        }
                    }
                }
            }

            return crossSellSecCat21;
        }

        public static string BuildCrossSellMainCat22<T>(ProductType type, T item)
        {
            string crossSellMainCat22 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellMainCat22 = "Timepieces";
                        }
                    }
                }
            }

            return crossSellMainCat22;
        }

        public static string BuildCrossSellSubCat22<T>(ProductType type, T item)
        {
            string crossSellSubCat22 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSubCat22 = "Shop Automatics";
                        }
                    }
                }
            }

            return crossSellSubCat22;
        }

        public static string BuildCrossSellSecCat22<T>(ProductType type, T item)
        {
            string crossSellSecCat22 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSecCat22 = "Dress";
                        }
                    }
                }
            }

            return crossSellSecCat22;
        }

        public static string BuildCrossSellMainCat23<T>(ProductType type, T item)
        {
            string crossSellMainCat23 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellMainCat23 = "Tag Heuer";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                        {
                            crossSellMainCat23 = "Tag Heuer";
                        }
                    }
                }
            }

            return crossSellMainCat23;
        }

        public static string BuildCrossSellSubCat23<T>(ProductType type, T item)
        {
            string crossSellSubCat23 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellSubCat23 = "Shop By Style";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                        {
                            crossSellSubCat23 = "Shop By Style";
                        }
                    }
                }
            }

            return crossSellSubCat23;
        }

        public static string BuildCrossSellSecCat23<T>(ProductType type, T item)
        {
            string crossSellSecCat23 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellSecCat23 = "Sport";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("PANAMERICANA SPECIAL EDITION"))
                        {
                            crossSellSecCat23 = "Sport";
                        }
                    }
                }
            }

            return crossSellSecCat23;
        }

        public static string BuildCrossSellMainCat24<T>(ProductType type, T item)
        {
            string crossSellMainCat24 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellMainCat24 = "Tag Heuer";
                    }
                    else if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Link")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellMainCat24 = "Tag Heuer";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9"))
                        {
                            crossSellMainCat24 = "Tag Heuer";
                        }
                    }
                }
            }

            return crossSellMainCat24;
        }

        public static string BuildCrossSellSubCat24<T>(ProductType type, T item)
        {
            string crossSellSubCat24 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellSubCat24 = "Shop By Style";
                    }
                    else if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Link")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSubCat24 = "Shop By Style";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9"))
                        {
                            crossSellSubCat24 = "Shop By Style";
                        }
                    }
                }
            }

            return crossSellSubCat24;
        }

        public static string BuildCrossSellSecCat24<T>(ProductType type, T item)
        {
            string crossSellSecCat24 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Monaco" || itemData.Collection == "Autavia")
                    {
                        crossSellSecCat24 = "Automatic";
                    }
                    else if (itemData.Collection == "Formula 1" || itemData.Collection == "Aquaracer" || itemData.Collection == "Link")
                    {
                        if (itemData.WatchType.Contains("Automatic"))
                        {
                            crossSellSecCat24 = "Automatic";
                        }
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Heuer 01") || itemData.WatchType.Contains("Heuer 02") || itemData.WatchType.Contains("Heuer 02T") || itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9"))
                        {
                            crossSellSecCat24 = "Automatic";
                        }
                    }
                }
            }

            return crossSellSecCat24;
        }

        public static string BuildCrossSellMainCat25<T>(ProductType type, T item)
        {
            string crossSellMainCat25 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        crossSellMainCat25 = "Tag Heuer";
                    }
                }
            }

            return crossSellMainCat25;
        }

        public static string BuildCrossSellSubCat25<T>(ProductType type, T item)
        {
            string crossSellSubCat25 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        crossSellSubCat25 = "Shop By Style";
                    }
                }
            }

            return crossSellSubCat25;
        }

        public static string BuildCrossSellSecCat25<T>(ProductType type, T item)
        {
            string crossSellSecCat25 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Aquaracer")
                    {
                        crossSellSecCat25 = "Dive";
                    }
                }
            }

            return crossSellSecCat25;
        }

        public static string BuildCrossSellMainCat26<T>(ProductType type, T item)
        {
            string crossSellMainCat26 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Link" || itemData.Collection == "Monaco")
                    {
                        crossSellMainCat26 = "Tag Heuer";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9") || itemData.WatchType.Contains("Quartz Watch"))
                        {
                            crossSellMainCat26 = "Tag Heuer";
                        }
                    }
                }
            }

            return crossSellMainCat26;
        }

        public static string BuildCrossSellSubCat26<T>(ProductType type, T item)
        {
            string crossSellSubCat26 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Link" || itemData.Collection == "Monaco")
                    {
                        crossSellSubCat26 = "Shop By Style";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9") || itemData.WatchType.Contains("Quartz Watch"))
                        {
                            crossSellSubCat26 = "Shop By Style";
                        }
                    }
                }
            }

            return crossSellSubCat26;
        }

        public static string BuildCrossSellSecCat26<T>(ProductType type, T item)
        {
            string crossSellSecCat26 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Collection == "Link" || itemData.Collection == "Monaco")
                    {
                        crossSellSecCat26 = "Dress";
                    }
                    else if (itemData.Collection == "Carrera")
                    {
                        if (itemData.WatchType.Contains("Calibre 16") || itemData.WatchType.Contains("Calibre 5") || itemData.WatchType.Contains("Calibre 9") || itemData.WatchType.Contains("Quartz Watch"))
                        {
                            crossSellSecCat26 = "Dress";
                        }
                    }
                }
            }

            return crossSellSecCat26;
        }

        public static string BuildCrossSellMainCat27<T>(ProductType type, T item)
        {
            string crossSellMainCat27 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("anthracite") || itemData.DialColor.Contains("carbon") || itemData.DialColor.Contains("grey"))
                    {
                        crossSellMainCat27 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat27;
        }

        public static string BuildCrossSellSubCat27<T>(ProductType type, T item)
        {
            string crossSellSubCat27 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("anthracite") || itemData.DialColor.Contains("carbon") || itemData.DialColor.Contains("grey"))
                    {
                        crossSellSubCat27 = "Shop By Color";
                    }
                }
            }

            return crossSellSubCat27;
        }

        public static string BuildCrossSellSecCat27<T>(ProductType type, T item)
        {
            string crossSellSecCat27 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("anthracite") || itemData.DialColor.Contains("carbon") || itemData.DialColor.Contains("grey"))
                    {
                        crossSellSecCat27 = "Grey";
                    }
                }
            }

            return crossSellSecCat27;
        }

        public static string BuildCrossSellMainCat28<T>(ProductType type, T item)
        {
            string crossSellMainCat28 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("black"))
                    {
                        crossSellMainCat28 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat28;
        }

        public static string BuildCrossSellSubCat28<T>(ProductType type, T item)
        {
            string crossSellSubCat28 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("black"))
                    {
                        crossSellSubCat28 = "Shop By Color";
                    }
                }
            }

            return crossSellSubCat28;
        }

        public static string BuildCrossSellSecCat28<T>(ProductType type, T item)
        {
            string crossSellSecCat28 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("black"))
                    {
                        crossSellSecCat28 = "Black";
                    }
                }
            }

            return crossSellSecCat28;
        }

        public static string BuildCrossSellMainCat29<T>(ProductType type, T item)
        {
            string crossSellMainCat29 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("blue"))
                    {
                        crossSellMainCat29 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat29;
        }

        public static string BuildCrossSellSubCat29<T>(ProductType type, T item)
        {
            string crossSellSubCat29 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("blue"))
                    {
                        crossSellSubCat29 = "Shop By Color";
                    }
                }
            }

            return crossSellSubCat29;
        }

        public static string BuildCrossSellSecCat29<T>(ProductType type, T item)
        {
            string crossSellSecCat29 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("blue"))
                    {
                        crossSellSecCat29 = "Blue";
                    }
                }
            }

            return crossSellSecCat29;
        }

        public static string BuildCrossSellMainCat30<T>(ProductType type, T item)
        {
            string crossSellMainCat30 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("brown") || itemData.DialColor.Contains("khaki"))
                    {
                        crossSellMainCat30 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat30;
        }

        public static string BuildCrossSellSubCat30<T>(ProductType type, T item)
        {
            string crossSellSubCat30 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("brown") || itemData.DialColor.Contains("khaki"))
                    {
                        crossSellSubCat30 = "Shop By Color";
                    }
                }
            }

            return crossSellSubCat30;
        }

        public static string BuildCrossSellSecCat30<T>(ProductType type, T item)
        {
            string crossSellSecCat30 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("brown") || itemData.DialColor.Contains("khaki"))
                    {
                        crossSellSecCat30 = "Brown";
                    }
                }
            }

            return crossSellSecCat30;
        }

        public static string BuildCrossSellMainCat31<T>(ProductType type, T item)
        {
            string crossSellMainCat31 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("burgundy") || itemData.DialColor.Contains("red"))
                    {
                        crossSellMainCat31 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat31;
        }

        public static string BuildCrossSellSubCat31<T>(ProductType type, T item)
        {
            string crossSellSubCat31 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("burgundy") || itemData.DialColor.Contains("red"))
                    {
                        crossSellSubCat31 = "Shop By Color";
                    }
                }
            }

            return crossSellSubCat31;
        }

        public static string BuildCrossSellSecCat31<T>(ProductType type, T item)
        {
            string crossSellSecCat31 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("burgundy") || itemData.DialColor.Contains("red"))
                    {
                        crossSellSecCat31 = "Red";
                    }
                }
            }

            return crossSellSecCat31;
        }

        public static string BuildCrossSellMainCat32<T>(ProductType type, T item)
        {
            string crossSellMainCat32 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("pink"))
                    {
                        crossSellMainCat32 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat32;
        }

        public static string BuildCrossSellSubCat32<T>(ProductType type, T item)
        {
            string crossSellSubCat32 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("pink"))
                    {
                        crossSellSubCat32 = "Shop By Color";
                    }
                }
            }

            return crossSellSubCat32;
        }

        public static string BuildCrossSellSecCat32<T>(ProductType type, T item)
        {
            string crossSellSecCat32 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("pink"))
                    {
                        crossSellSecCat32 = "Pink";
                    }
                }
            }

            return crossSellSecCat32;
        }

        public static string BuildCrossSellMainCat33<T>(ProductType type, T item)
        {
            string crossSellMainCat33 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("silver") || itemData.DialColor.Contains("white"))
                    {
                        crossSellMainCat33 = "Timepieces";
                    }
                }
            }

            return crossSellMainCat33;
        }

        public static string BuildCrossSellSubCat33<T>(ProductType type, T item)
        {
            string crossSellSubCat33 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("silver") || itemData.DialColor.Contains("white"))
                    {
                        crossSellSubCat33 = "Shop By Color";
                    }
                }
            }

            return crossSellSubCat33;
        }

        public static string BuildCrossSellSecCat33<T>(ProductType type, T item)
        {
            string crossSellSecCat33 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.DialColor.Contains("silver") || itemData.DialColor.Contains("white"))
                    {
                        crossSellSecCat33 = "White";
                    }
                }
            }

            return crossSellSecCat33;
        }

        public static string BuildCrossSellMainCat34<T>(ProductType type, T item)
        {
            string crossSellMainCat34 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellMainCat34 = "Tag Heuer ";
                }
            }

            return crossSellMainCat34;
        }

        public static string BuildCrossSellSubCat34<T>(ProductType type, T item)
        {
            string crossSellSubCat34 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    crossSellSubCat34 = "Shop By Gender";
                }
            }

            return crossSellSubCat34;
        }

        public static string BuildCrossSellSecCat34<T>(ProductType type, T item)
        {
            string crossSellSecCat34 = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    if (itemData.Gender == "Male")
                    {
                        crossSellSecCat34 = "Men's";
                    }
                    else if (itemData.Gender == "Female")
                    {
                        crossSellSecCat34 = "Ladies";
                    }
                }
            }

            return crossSellSecCat34;
        }

        public static string BuildSpecifications<T>(ProductType type, T item, SceBatchItem batchItem)
        {
            string specifications = string.Empty;
            var itemData = item as TagHeuerDataItem;

            if (itemData != null)
            {
                if (type == ProductType.Timepieces)
                {
                    string designer = "";
                    string collection = "";
                    string caseShape = "";
                    string caseDiameter = "";
                    string gender = "";
                    string movementType = "";
                    string bezel = "";
                    string bezelGemstoneInformation = "";
                    string crystal = "";
                    string dial = "";
                    string dialInformation = "";
                    string waterResistancy = "";
                    string typeOfBand = "";
                    string braceletStrapMaterial = "";
                    string claspType = "";
                    string countryOfOrigin = "";
                    string stoneID = "";
                    string totalCaratWeight = "";
                    string color01 = "";
                    string color02 = "";
                    string color03 = "";
                    string color04 = "";
                    string color05 = "";
                    string color06 = "";
                    string color07 = "";
                    string color1 = "";
                    string color2 = "";
                    string color3 = "";
                    string color4 = "";
                    string color5 = "";
                    string color6 = "";
                    string color7 = "";
                    string color = "";
                    string spec = "";
                    int counter = 0;

                    designer = $"Designer~{itemData.Designer}^";
                    collection = $"Collection~{itemData.Collection}^";
                    caseShape = $"Case Shape~{itemData.CaseShape}^";
                    caseDiameter = $"Case Diameter (MM)~{itemData.CaseDiameter}mm^";
                    gender = $"Gender~{itemData.Gender}^";

                    if (!string.IsNullOrEmpty(itemData.MovementName))
                    {
                        movementType = $"Movement Type~{itemData.MovementType} {itemData.MovementName}^";
                    }
                    else
                    {
                        movementType = $"Movement Type~{itemData.MovementType}^";
                    }

                    bezel = $"Bezel~{itemData.Bezel}^";
                    bezelGemstoneInformation = $"Bezel Gemstone Information~{itemData.BezelGemstoneInformation}^";
                    crystal = $"Crystal~{itemData.Crystal}^";
                    dial = $"Dial~{itemData.Dial}^";
                    dialInformation = $"Dial Information~{itemData.DialInformation}^";
                    waterResistancy = $"Water Resistancy (M)~{itemData.WaterResistancy}^";
                    typeOfBand = $"Type of Band~{itemData.TypeOfBand}^";
                    braceletStrapMaterial = $"Bracelet/Strap Material~{itemData.BraceletStrapMaterial}^";
                    claspType = $"Clasp Type~{itemData.ClaspType}^";
                    countryOfOrigin = $"Country of Origin~{itemData.CountryOfOrigin}^";
                    stoneID = $"Stone ID~{itemData.StoneId}^";
                    totalCaratWeight = $"Total carat weight~{itemData.TotalCaratWeight}^";

                    if (batchItem.CrossSellSecCat27 == "Grey")
                    {
                        color1 = "Grey^";
                        counter++;
                    }

                    if (batchItem.CrossSellSecCat28 == "Black")
                    {
                        color2 = "Black^";
                        counter++;
                    }

                    if (batchItem.CrossSellSecCat29 == "Blue")
                    {
                        color3 = "Blue^";
                        counter++;
                    }

                    if (batchItem.CrossSellSecCat30 == "Brown")
                    {
                        color4 = "Brown^";
                        counter++;
                    }

                    if (batchItem.CrossSellSecCat31 == "Red")
                    {
                        color5 = "Red^";
                        counter++;
                    }

                    if (batchItem.CrossSellSecCat32 == "Pink")
                    {
                        color6 = "Pink^";
                        counter++;
                    }

                    if (batchItem.CrossSellSecCat33 == "White")
                    {
                        color7 = "White^";
                        counter++;
                    }

                    if (counter == 1)
                    {
                        color = $"Color~{color1}{color2}{color3}{color4}{color5}{color6}{color7}";
                    }
                    else if (counter == 2)
                    {
                        if (!string.IsNullOrEmpty(color1))
                        {
                            color01 = $"Color 1~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2))
                        {
                            color01 = $"Color 1~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3))
                        {
                            color01 = $"Color 1~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4))
                        {
                            color01 = $"Color 1~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5))
                        {
                            color01 = $"Color 1~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6))
                        {
                            color01 = $"Color 1~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7))
                        {
                            color01 = $"Color 1~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}"))
                        {
                            color02 = $"Color 2~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}"))
                        {
                            color02 = $"Color 2~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}"))
                        {
                            color02 = $"Color 2~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}"))
                        {
                            color02 = $"Color 2~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}"))
                        {
                            color02 = $"Color 2~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}"))
                        {
                            color02 = $"Color 2~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}"))
                        {
                            color02 = $"Color 2~{color7}";
                        }

                        color = color01 + color02;
                    }
                    else if (counter == 3)
                    {
                        if (!string.IsNullOrEmpty(color1))
                        {
                            color01 = $"Color 1~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2))
                        {
                            color01 = $"Color 1~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3))
                        {
                            color01 = $"Color 1~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4))
                        {
                            color01 = $"Color 1~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5))
                        {
                            color01 = $"Color 1~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6))
                        {
                            color01 = $"Color 1~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7))
                        {
                            color01 = $"Color 1~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}"))
                        {
                            color02 = $"Color 2~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}"))
                        {
                            color02 = $"Color 2~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}"))
                        {
                            color02 = $"Color 2~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}"))
                        {
                            color02 = $"Color 2~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}"))
                        {
                            color02 = $"Color 2~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}"))
                        {
                            color02 = $"Color 2~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}"))
                        {
                            color02 = $"Color 2~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}"))
                        {
                            color03 = $"Color 3~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}"))
                        {
                            color03 = $"Color 3~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}"))
                        {
                            color03 = $"Color 3~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}"))
                        {
                            color03 = $"Color 3~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}"))
                        {
                            color03 = $"Color 3~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}"))
                        {
                            color03 = $"Color 3~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}"))
                        {
                            color03 = $"Color 3~{color7}";
                        }

                        color = color01 + color02 + color03;
                    }
                    else if (counter == 4)
                    {
                        if (!string.IsNullOrEmpty(color1))
                        {
                            color01 = $"Color 1~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2))
                        {
                            color01 = $"Color 1~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3))
                        {
                            color01 = $"Color 1~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4))
                        {
                            color01 = $"Color 1~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5))
                        {
                            color01 = $"Color 1~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6))
                        {
                            color01 = $"Color 1~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7))
                        {
                            color01 = $"Color 1~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}"))
                        {
                            color02 = $"Color 2~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}"))
                        {
                            color02 = $"Color 2~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}"))
                        {
                            color02 = $"Color 2~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}"))
                        {
                            color02 = $"Color 2~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}"))
                        {
                            color02 = $"Color 2~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}"))
                        {
                            color02 = $"Color 2~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}"))
                        {
                            color02 = $"Color 2~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}"))
                        {
                            color03 = $"Color 3~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}"))
                        {
                            color03 = $"Color 3~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}"))
                        {
                            color03 = $"Color 3~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}"))
                        {
                            color03 = $"Color 3~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}"))
                        {
                            color03 = $"Color 3~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}"))
                        {
                            color03 = $"Color 3~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}"))
                        {
                            color03 = $"Color 3~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}"))
                        {
                            color04 = $"Color 4~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}"))
                        {
                            color04 = $"Color 4~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}"))
                        {
                            color04 = $"Color 4~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}"))
                        {
                            color04 = $"Color 4~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}"))
                        {
                            color04 = $"Color 4~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}"))
                        {
                            color04 = $"Color 4~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}"))
                        {
                            color04 = $"Color 4~{color7}";
                        }

                        color = color01 + color02 + color03 + color04;
                    }
                    else if (counter == 5)
                    {
                        if (!string.IsNullOrEmpty(color1))
                        {
                            color01 = $"Color 1~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2))
                        {
                            color01 = $"Color 1~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3))
                        {
                            color01 = $"Color 1~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4))
                        {
                            color01 = $"Color 1~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5))
                        {
                            color01 = $"Color 1~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6))
                        {
                            color01 = $"Color 1~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7))
                        {
                            color01 = $"Color 1~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}"))
                        {
                            color02 = $"Color 2~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}"))
                        {
                            color02 = $"Color 2~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}"))
                        {
                            color02 = $"Color 2~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}"))
                        {
                            color02 = $"Color 2~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}"))
                        {
                            color02 = $"Color 2~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}"))
                        {
                            color02 = $"Color 2~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}"))
                        {
                            color02 = $"Color 2~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}"))
                        {
                            color03 = $"Color 3~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}"))
                        {
                            color03 = $"Color 3~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}"))
                        {
                            color03 = $"Color 3~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}"))
                        {
                            color03 = $"Color 3~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}"))
                        {
                            color03 = $"Color 3~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}"))
                        {
                            color03 = $"Color 3~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}"))
                        {
                            color03 = $"Color 3~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}"))
                        {
                            color04 = $"Color 4~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}"))
                        {
                            color04 = $"Color 4~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}"))
                        {
                            color04 = $"Color 4~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}"))
                        {
                            color04 = $"Color 4~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}"))
                        {
                            color04 = $"Color 4~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}"))
                        {
                            color04 = $"Color 4~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}"))
                        {
                            color04 = $"Color 4~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}") && !color04.Contains($"{color1}"))
                        {
                            color05 = $"Color 5~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}") && !color04.Contains($"{color2}"))
                        {
                            color05 = $"Color 5~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}") && !color04.Contains($"{color3}"))
                        {
                            color05 = $"Color 5~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}") && !color04.Contains($"{color4}"))
                        {
                            color05 = $"Color 5~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}") && !color04.Contains($"{color5}"))
                        {
                            color05 = $"Color 5~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}") && !color04.Contains($"{color6}"))
                        {
                            color05 = $"Color 5~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}") && !color04.Contains($"{color7}"))
                        {
                            color05 = $"Color 5~{color7}";
                        }

                        color = color01 + color02 + color03 + color04 + color05;
                    }
                    else if (counter == 6)
                    {
                        if (!string.IsNullOrEmpty(color1))
                        {
                            color01 = $"Color 1~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2))
                        {
                            color01 = $"Color 1~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3))
                        {
                            color01 = $"Color 1~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4))
                        {
                            color01 = $"Color 1~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5))
                        {
                            color01 = $"Color 1~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6))
                        {
                            color01 = $"Color 1~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7))
                        {
                            color01 = $"Color 1~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}"))
                        {
                            color02 = $"Color 2~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}"))
                        {
                            color02 = $"Color 2~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}"))
                        {
                            color02 = $"Color 2~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}"))
                        {
                            color02 = $"Color 2~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}"))
                        {
                            color02 = $"Color 2~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}"))
                        {
                            color02 = $"Color 2~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}"))
                        {
                            color02 = $"Color 2~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}"))
                        {
                            color03 = $"Color 3~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}"))
                        {
                            color03 = $"Color 3~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}"))
                        {
                            color03 = $"Color 3~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}"))
                        {
                            color03 = $"Color 3~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}"))
                        {
                            color03 = $"Color 3~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}"))
                        {
                            color03 = $"Color 3~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}"))
                        {
                            color03 = $"Color 3~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}"))
                        {
                            color04 = $"Color 4~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}"))
                        {
                            color04 = $"Color 4~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}"))
                        {
                            color04 = $"Color 4~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}"))
                        {
                            color04 = $"Color 4~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}"))
                        {
                            color04 = $"Color 4~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}"))
                        {
                            color04 = $"Color 4~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}"))
                        {
                            color04 = $"Color 4~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}") && !color04.Contains($"{color1}"))
                        {
                            color05 = $"Color 5~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}") && !color04.Contains($"{color2}"))
                        {
                            color05 = $"Color 5~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}") && !color04.Contains($"{color3}"))
                        {
                            color05 = $"Color 5~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}") && !color04.Contains($"{color4}"))
                        {
                            color05 = $"Color 5~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}") && !color04.Contains($"{color5}"))
                        {
                            color05 = $"Color 5~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}") && !color04.Contains($"{color6}"))
                        {
                            color05 = $"Color 5~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}") && !color04.Contains($"{color7}"))
                        {
                            color05 = $"Color 5~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}") && !color04.Contains($"{color1}") && !color05.Contains($"{color1}"))
                        {
                            color06 = $"Color 6~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}") && !color04.Contains($"{color2}") && !color05.Contains($"{color2}"))
                        {
                            color06 = $"Color 6~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}") && !color04.Contains($"{color3}") && !color05.Contains($"{color3}"))
                        {
                            color06 = $"Color 6~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}") && !color04.Contains($"{color4}") && !color05.Contains($"{color4}"))
                        {
                            color06 = $"Color 6~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}") && !color04.Contains($"{color5}") && !color05.Contains($"{color5}"))
                        {
                            color06 = $"Color 6~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}") && !color04.Contains($"{color6}") && !color05.Contains($"{color6}"))
                        {
                            color06 = $"Color 6~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}") && !color04.Contains($"{color7}") && !color05.Contains($"{color7}"))
                        {
                            color06 = $"Color 6~{color7}";
                        }

                        color = color01 + color02 + color03 + color04 + color05 + color06;
                    }
                    else if (counter == 7)
                    {
                        if (!string.IsNullOrEmpty(color1))
                        {
                            color01 = $"Color 1~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2))
                        {
                            color01 = $"Color 1~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3))
                        {
                            color01 = $"Color 1~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4))
                        {
                            color01 = $"Color 1~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5))
                        {
                            color01 = $"Color 1~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6))
                        {
                            color01 = $"Color 1~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7))
                        {
                            color01 = $"Color 1~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}"))
                        {
                            color02 = $"Color 2~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}"))
                        {
                            color02 = $"Color 2~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}"))
                        {
                            color02 = $"Color 2~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}"))
                        {
                            color02 = $"Color 2~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}"))
                        {
                            color02 = $"Color 2~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}"))
                        {
                            color02 = $"Color 2~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}"))
                        {
                            color02 = $"Color 2~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}"))
                        {
                            color03 = $"Color 3~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}"))
                        {
                            color03 = $"Color 3~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}"))
                        {
                            color03 = $"Color 3~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}"))
                        {
                            color03 = $"Color 3~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}"))
                        {
                            color03 = $"Color 3~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}"))
                        {
                            color03 = $"Color 3~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}"))
                        {
                            color03 = $"Color 3~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}"))
                        {
                            color04 = $"Color 4~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}"))
                        {
                            color04 = $"Color 4~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}"))
                        {
                            color04 = $"Color 4~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}"))
                        {
                            color04 = $"Color 4~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}"))
                        {
                            color04 = $"Color 4~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}"))
                        {
                            color04 = $"Color 4~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}"))
                        {
                            color04 = $"Color 4~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}") && !color04.Contains($"{color1}"))
                        {
                            color05 = $"Color 5~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}") && !color04.Contains($"{color2}"))
                        {
                            color05 = $"Color 5~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}") && !color04.Contains($"{color3}"))
                        {
                            color05 = $"Color 5~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}") && !color04.Contains($"{color4}"))
                        {
                            color05 = $"Color 5~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}") && !color04.Contains($"{color5}"))
                        {
                            color05 = $"Color 5~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}") && !color04.Contains($"{color6}"))
                        {
                            color05 = $"Color 5~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}") && !color04.Contains($"{color7}"))
                        {
                            color05 = $"Color 5~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}") && !color04.Contains($"{color1}") && !color05.Contains($"{color1}"))
                        {
                            color06 = $"Color 6~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}") && !color04.Contains($"{color2}") && !color05.Contains($"{color2}"))
                        {
                            color06 = $"Color 6~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}") && !color04.Contains($"{color3}") && !color05.Contains($"{color3}"))
                        {
                            color06 = $"Color 6~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}") && !color04.Contains($"{color4}") && !color05.Contains($"{color4}"))
                        {
                            color06 = $"Color 6~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}") && !color04.Contains($"{color5}") && !color05.Contains($"{color5}"))
                        {
                            color06 = $"Color 6~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}") && !color04.Contains($"{color6}") && !color05.Contains($"{color6}"))
                        {
                            color06 = $"Color 6~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}") && !color04.Contains($"{color7}") && !color05.Contains($"{color7}"))
                        {
                            color06 = $"Color 6~{color7}";
                        }

                        if (!string.IsNullOrEmpty(color1) && !color01.Contains($"{color1}") && !color02.Contains($"{color1}") && !color03.Contains($"{color1}") && !color04.Contains($"{color1}") && !color05.Contains($"{color1}") && !color06.Contains($"{color1}"))
                        {
                            color07 = $"Color 7~{color1}";
                        }
                        else if (!string.IsNullOrEmpty(color2) && !color01.Contains($"{color2}") && !color02.Contains($"{color2}") && !color03.Contains($"{color2}") && !color04.Contains($"{color2}") && !color05.Contains($"{color2}") && !color06.Contains($"{color2}"))
                        {
                            color07 = $"Color 7~{color2}";
                        }
                        else if (!string.IsNullOrEmpty(color3) && !color01.Contains($"{color3}") && !color02.Contains($"{color3}") && !color03.Contains($"{color3}") && !color04.Contains($"{color3}") && !color05.Contains($"{color3}") && !color06.Contains($"{color3}"))
                        {
                            color07 = $"Color 7~{color3}";
                        }
                        else if (!string.IsNullOrEmpty(color4) && !color01.Contains($"{color4}") && !color02.Contains($"{color4}") && !color03.Contains($"{color4}") && !color04.Contains($"{color4}") && !color05.Contains($"{color4}") && !color06.Contains($"{color4}"))
                        {
                            color07 = $"Color 7~{color4}";
                        }
                        else if (!string.IsNullOrEmpty(color5) && !color01.Contains($"{color5}") && !color02.Contains($"{color5}") && !color03.Contains($"{color5}") && !color04.Contains($"{color5}") && !color05.Contains($"{color5}") && !color06.Contains($"{color5}"))
                        {
                            color07 = $"Color 7~{color5}";
                        }
                        else if (!string.IsNullOrEmpty(color6) && !color01.Contains($"{color6}") && !color02.Contains($"{color6}") && !color03.Contains($"{color6}") && !color04.Contains($"{color6}") && !color05.Contains($"{color6}") && !color06.Contains($"{color6}"))
                        {
                            color07 = $"Color 7~{color6}";
                        }
                        else if (!string.IsNullOrEmpty(color7) && !color01.Contains($"{color7}") && !color02.Contains($"{color7}") && !color03.Contains($"{color7}") && !color04.Contains($"{color7}") && !color05.Contains($"{color7}") && !color06.Contains($"{color7}"))
                        {
                            color07 = $"Color 7~{color7}";
                        }

                        color = color01 + color02 + color03 + color04 + color05 + color06 + color07;
                    }

                    spec = $"Specifications##{designer}{collection}{caseShape}{caseDiameter}{gender}{movementType}{bezel}{bezelGemstoneInformation}{crystal}{dial}{dialInformation}{waterResistancy}{typeOfBand}{braceletStrapMaterial}{claspType}{countryOfOrigin}{stoneID}{totalCaratWeight}{color}";
                    specifications = spec.Trim('^');
                }
            }

            return specifications;
        }
    }
}
