using Databox.Libs.MarksJewelersSuppliersData;
using MarksJewelersSuppliersData.DataItems;
using MarksJewelersSuppliersData.Enums;
using MarksJewelersSuppliersData.SupplierDataBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarksJewelersSuppliersData.Helpers
{
    public class SceBatchHelper
    {
        public static string BuildMainCategory<T>(ProductType type, SupplierType supplierType, T item)
        {
            string mainCategory = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    mainCategory = SDBJohnHardy.BuildMainCategory(type, item);
                    break;
                case SupplierType.TagHeuer:
                    mainCategory = SDBTagHeuer.BuildMainCategory(type, item);
                    break;
            }

            return mainCategory;
        }


        public static string BuildSubCategory<T>(ProductType type, SupplierType supplierType, T item)
        {
            string subCategory = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    subCategory = SDBJohnHardy.BuildSubCategory(type, item);
                    break;
                case SupplierType.TagHeuer:
                    subCategory = SDBTagHeuer.BuildSubCategory(type, item);
                    break;
            }

            return subCategory;
        }

        public static string BuildSectionCategory<T>(ProductType type, SupplierType supplierType, T item)
        {
            string sectionCategory = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    sectionCategory = SDBJohnHardy.BuildSectionCategory(type, item);
                    break;
                case SupplierType.TagHeuer:
                    sectionCategory = SDBTagHeuer.BuildSectionCategory(type, item);
                    break;
            }

            return sectionCategory;
        }

        public static string BuildCrossSellMainCat1<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat1 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat1 = SDBJohnHardy.BuildCrossSellMainCat1(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat1 = SDBTagHeuer.BuildCrossSellMainCat1(type, item);
                    break;
            }

            return crossSellMainCat1;
        }

        public static string BuildCrossSellSubCat1<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat1 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat1 = SDBJohnHardy.BuildCrossSellSubCat1(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat1 = SDBTagHeuer.BuildCrossSellSubCat1(type, item);
                    break;
            }

            return crossSellSubCat1;
        }

        public static string BuildCrossSellSecCat1<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat1 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat1 = SDBJohnHardy.BuildCrossSellSecCat1(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat1 = SDBTagHeuer.BuildCrossSellSecCat1(type, item);
                    break;
            }

            return crossSellSecCat1;
        }

        public static string BuildCrossSellMainCat2<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat2 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat2 = SDBJohnHardy.BuildCrossSellMainCat2(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat2 = SDBTagHeuer.BuildCrossSellMainCat2(type, item);
                    break;
            }

            return crossSellMainCat2;
        }

        public static string BuildCrossSellSubCat2<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat2 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat2 = SDBJohnHardy.BuildCrossSellSubCat2(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat2 = SDBTagHeuer.BuildCrossSellSubCat2(type, item);
                    break;
            }

            return crossSellSubCat2;
        }

        public static string BuildAnchorText<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string anchorText = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    anchorText = SDBJohnHardy.BuildAnchorText(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    anchorText = SDBTagHeuer.BuildAnchorText(type, item, batchItem);
                    break;
            }

            return anchorText;
        }

        public static string BuildProductTitle<T>(ProductType type, SupplierType supplierType, T item)
        {
            string productTitle = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    productTitle = SDBJohnHardy.BuildProductTitle(type, item);
                    break;
                case SupplierType.TagHeuer:
                    productTitle = SDBTagHeuer.BuildProductTitle(type, item);
                    break;
            }

            return productTitle;
        }

        public static string BuildSpiderURL<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string spiderURL = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    spiderURL = SDBJohnHardy.BuildSpiderURL(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    spiderURL = SDBTagHeuer.BuildSpiderURL(type, item, batchItem);
                    break;
            }

            return spiderURL;
        }

        public static string BuildBrand<T>(ProductType type, SupplierType supplierType, T item)
        {
            string brand = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    brand = SDBJohnHardy.BuildBrand(type, item);
                    break;
                case SupplierType.TagHeuer:
                    brand = SDBTagHeuer.BuildBrand(type, item);
                    break;
            }

            return brand;
        }

        public static string BuildDescription<T>(ProductType type, SupplierType supplierType, T item)
        {
            string description = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    description = SDBJohnHardy.BuildDescription(type, item);
                    break;
                case SupplierType.TagHeuer:
                    description = SDBTagHeuer.BuildDescription(type, item);
                    break;
            }

            return description;
        }

        public static string BuildMETADescription<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string mETADescription = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    mETADescription = SDBJohnHardy.BuildMETADescription(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    mETADescription = SDBTagHeuer.BuildMETADescription(type, item, batchItem);
                    break;
            }

            return mETADescription;
        }

        public static string BuildMETAKeywords<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string mETAKeywords = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    mETAKeywords = SDBJohnHardy.BuildMETAKeywords(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    mETAKeywords = SDBTagHeuer.BuildMETAKeywords(type, item, batchItem);
                    break;
            }

            return mETAKeywords;
        }

        public static string BuildGeneralImage<T>(ProductType type, SupplierType supplierType, T item, ExtSettings extSett)
        {
            string generalImage = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    generalImage = SDBJohnHardy.BuildGeneralImage(type, item);
                    break;
                case SupplierType.TagHeuer:
                    generalImage = SDBTagHeuer.BuildGeneralImage(type, item, extSett);
                    break;
            }

            return generalImage;
        }

        public static string BuildPartNumber<T>(ProductType type, SupplierType supplierType, T item)
        {
            string partNumber = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    partNumber = SDBJohnHardy.BuildPartNumber(type, item);
                    break;
                case SupplierType.TagHeuer:
                    partNumber = SDBTagHeuer.BuildPartNumber(type, item);
                    break;
            }

            return partNumber;
        }

        public static double BuildMSRP<T>(ProductType type, SupplierType supplierType, T item)
        {
            double mSRP = 0;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    mSRP = SDBJohnHardy.BuildMSRP(type, item);
                    break;
                case SupplierType.TagHeuer:
                    mSRP = SDBTagHeuer.BuildMSRP(type, item);
                    break;
            }

            return mSRP;
        }

        public static double BuildJobber<T>(ProductType type, SupplierType supplierType, T item)
        {
            double jobber = 0;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    jobber = SDBJohnHardy.BuildJobber(type, item);
                    break;
                case SupplierType.TagHeuer:
                    jobber = SDBTagHeuer.BuildJobber(type, item);
                    break;
            }

            return jobber;
        }

        public static double BuildWebPrice<T>(ProductType type, SupplierType supplierType, T item)
        {
            double webPrice = 0;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    webPrice = SDBJohnHardy.BuildWebPrice(type, item);
                    break;
                case SupplierType.TagHeuer:
                    webPrice = SDBTagHeuer.BuildWebPrice(type, item);
                    break;
            }

            return webPrice;
        }

        public static double BuildCostPrice<T>(ProductType type, SupplierType supplierType, T item)
        {
            double costPrice = 0;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    costPrice = SDBJohnHardy.BuildCostPrice(type, item);
                    break;
                case SupplierType.TagHeuer:
                    costPrice = SDBTagHeuer.BuildCostPrice(type, item);
                    break;
            }

            return costPrice;
        }

        public static string BuildCrossSellSecCat2<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSecCat2 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat2 = SDBJohnHardy.BuildCrossSellSecCat2(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat2 = SDBTagHeuer.BuildCrossSellSecCat2(type, item);
                    break;
            }

            return crossSellSecCat2;
        }

        public static string BuildCrossSellMainCat3<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat3 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat3 = SDBJohnHardy.BuildCrossSellMainCat3(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat3 = SDBTagHeuer.BuildCrossSellMainCat3(type, item);
                    break;
            }

            return crossSellMainCat3;
        }

        public static string BuildCrossSellSubCat3<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat3 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat3 = SDBJohnHardy.BuildCrossSellSubCat3(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat3 = SDBTagHeuer.BuildCrossSellSubCat3(type, item);
                    break;
            }

            return crossSellSubCat3;
        }

        public static string BuildCrossSellSecCat3<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSecCat3 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat3 = SDBJohnHardy.BuildCrossSellSecCat3(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat3 = SDBTagHeuer.BuildCrossSellSecCat3(type, item);
                    break;
            }

            return crossSellSecCat3;
        }

        public static string BuildCrossSellMainCat4<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat4 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat4 = SDBJohnHardy.BuildCrossSellMainCat4(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat4 = SDBTagHeuer.BuildCrossSellMainCat4(type, item);
                    break;
            }

            return crossSellMainCat4;
        }
        public static string BuildCrossSellSubCat4<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat4 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat4 = SDBJohnHardy.BuildCrossSellSubCat4(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat4 = SDBTagHeuer.BuildCrossSellSubCat4(type, item);
                    break;
            }

            return crossSellSubCat4;
        }

        public static string BuildCrossSellSecCat4<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSecCat4 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat4 = SDBJohnHardy.BuildCrossSellSecCat4(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat4 = SDBTagHeuer.BuildCrossSellSecCat4(type, item);
                    break;
            }

            return crossSellSecCat4;
        }

        public static string BuildCrossSellMainCat5<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat5 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat5 = SDBJohnHardy.BuildCrossSellMainCat5(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat5 = SDBTagHeuer.BuildCrossSellMainCat5(type, item);
                    break;
            }

            return crossSellMainCat5;
        }

        public static string BuildCrossSellSubCat5<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat5 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat5 = SDBJohnHardy.BuildCrossSellSubCat5(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat5 = SDBTagHeuer.BuildCrossSellSubCat5(type, item);
                    break;
            }

            return crossSellSubCat5;
        }

        public static string BuildCrossSellSecCat5<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSecCat5 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat5 = SDBJohnHardy.BuildCrossSellSecCat5(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat5 = SDBTagHeuer.BuildCrossSellSecCat5(type, item);
                    break;
            }

            return crossSellSecCat5;
        }

        public static string BuildCrossSellMainCat6<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat6 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat6 = SDBJohnHardy.BuildCrossSellMainCat6(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat6 = SDBTagHeuer.BuildCrossSellMainCat6(type, item);
                    break;
            }

            return crossSellMainCat6;
        }

        public static string BuildCrossSellSubCat6<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat6 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat6 = SDBJohnHardy.BuildCrossSellSubCat6(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat6 = SDBTagHeuer.BuildCrossSellSubCat6(type, item);
                    break;
            }

            return crossSellSubCat6;
        }

        public static string BuildCrossSellSecCat6<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSecCat6 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat6 = SDBJohnHardy.BuildCrossSellSecCat6(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat6 = SDBTagHeuer.BuildCrossSellSecCat6(type, item);
                    break;
            }

            return crossSellSecCat6;
        }

        public static string BuildCrossSellMainCat7<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat7 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat7 = SDBJohnHardy.BuildCrossSellMainCat7(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat7 = SDBTagHeuer.BuildCrossSellMainCat7(type, item, batchItem);
                    break;
            }

            return crossSellMainCat7;
        }

        public static string BuildCrossSellSubCat7<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat7 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat7 = SDBJohnHardy.BuildCrossSellSubCat7(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat7 = SDBTagHeuer.BuildCrossSellSubCat7(type, item, batchItem);
                    break;
            }

            return crossSellSubCat7;
        }

        public static string BuildCrossSellSecCat7<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSecCat7 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat7 = SDBJohnHardy.BuildCrossSellSecCat7(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat7 = SDBTagHeuer.BuildCrossSellSecCat7(type, item);
                    break;
            }

            return crossSellSecCat7;
        }

        public static string BuildCrossSellMainCat8<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat8 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat8 = SDBJohnHardy.BuildCrossSellMainCat8(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat8 = SDBTagHeuer.BuildCrossSellMainCat8(type, item, batchItem);
                    break;
            }

            return crossSellMainCat8;
        }

        public static string BuildCrossSellSubCat8<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat8 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat8 = SDBJohnHardy.BuildCrossSellSubCat8(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat8 = SDBTagHeuer.BuildCrossSellSubCat8(type, item, batchItem);
                    break;
            }

            return crossSellSubCat8;
        }

        public static string BuildCrossSellSecCat8<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSecCat8 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat8 = SDBJohnHardy.BuildCrossSellSecCat8(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat8 = SDBTagHeuer.BuildCrossSellSecCat8(type, item);
                    break;
            }

            return crossSellSecCat8;
        }

        public static string BuildCrossSellMainCat9<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat9 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat9 = SDBJohnHardy.BuildCrossSellMainCat9(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat9 = SDBTagHeuer.BuildCrossSellMainCat9(type, item, batchItem);
                    break;
            }

            return crossSellMainCat9;
        }

        public static string BuildCrossSellSubCat9<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat9 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat9 = SDBJohnHardy.BuildCrossSellSubCat9(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat9 = SDBTagHeuer.BuildCrossSellSubCat9(type, item, batchItem);
                    break;
            }

            return crossSellSubCat9;
        }

        public static string BuildCrossSellSecCat9<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat9 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat9 = SDBJohnHardy.BuildCrossSellSecCat9(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat9 = SDBTagHeuer.BuildCrossSellSecCat9(type, item);
                    break;
            }

            return crossSellSecCat9;
        }

        public static string BuildCrossSellMainCat10<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat10 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat10 = SDBJohnHardy.BuildCrossSellMainCat10(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat10 = SDBTagHeuer.BuildCrossSellMainCat10(type, item, batchItem);
                    break;
            }

            return crossSellMainCat10;
        }

        public static string BuildCrossSellSubCat10<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat10 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat10 = SDBJohnHardy.BuildCrossSellSubCat10(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat10 = SDBTagHeuer.BuildCrossSellSubCat10(type, item, batchItem);
                    break;
            }

            return crossSellSubCat10;
        }

        public static string BuildCrossSellSecCat10<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat10 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat10 = SDBJohnHardy.BuildCrossSellSecCat10(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat10 = SDBTagHeuer.BuildCrossSellSecCat10(type, item);
                    break;
            }

            return crossSellSecCat10;
        }

        public static string BuildCrossSellMainCat11<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat11 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat11 = SDBJohnHardy.BuildCrossSellMainCat11(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat11 = SDBTagHeuer.BuildCrossSellMainCat11(type, item, batchItem);
                    break;
            }

            return crossSellMainCat11;
        }

        public static string BuildCrossSellSubCat11<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat11 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat11 = SDBJohnHardy.BuildCrossSellSubCat11(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat11 = SDBTagHeuer.BuildCrossSellSubCat11(type, item, batchItem);
                    break;
            }

            return crossSellSubCat11;
        }

        public static string BuildCrossSellSecCat11<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat11 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat11 = SDBJohnHardy.BuildCrossSellSecCat11(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat11 = SDBTagHeuer.BuildCrossSellSecCat11(type, item);
                    break;
            }

            return crossSellSecCat11;
        }

        public static string BuildCrossSellMainCat12<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat12 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat12 = SDBJohnHardy.BuildCrossSellMainCat12(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat12 = SDBTagHeuer.BuildCrossSellMainCat12(type, item, batchItem);
                    break;
            }

            return crossSellMainCat12;
        }

        public static string BuildCrossSellSubCat12<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat12 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat12 = SDBJohnHardy.BuildCrossSellSubCat12(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat12 = SDBTagHeuer.BuildCrossSellSubCat12(type, item, batchItem);
                    break;
            }

            return crossSellSubCat12;
        }

        public static string BuildCrossSellSecCat12<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat12 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat12 = SDBJohnHardy.BuildCrossSellSecCat12(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat12 = SDBTagHeuer.BuildCrossSellSecCat12(type, item);
                    break;
            }

            return crossSellSecCat12;
        }

        public static string BuildCrossSellMainCat13<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat13 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat13 = SDBJohnHardy.BuildCrossSellMainCat13(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat13 = SDBTagHeuer.BuildCrossSellMainCat13(type, item, batchItem);
                    break;
            }

            return crossSellMainCat13;
        }

        public static string BuildCrossSellSubCat13<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat13 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat13 = SDBJohnHardy.BuildCrossSellSubCat13(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat13 = SDBTagHeuer.BuildCrossSellSubCat13(type, item, batchItem);
                    break;
            }

            return crossSellSubCat13;
        }

        public static string BuildCrossSellSecCat13<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat13 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat13 = SDBJohnHardy.BuildCrossSellSecCat13(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat13 = SDBTagHeuer.BuildCrossSellSecCat13(type, item);
                    break;
            }

            return crossSellSecCat13;
        }

        public static string BuildCrossSellMainCat14<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat14 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat14 = SDBJohnHardy.BuildCrossSellMainCat14(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat14 = SDBTagHeuer.BuildCrossSellMainCat14(type, item, batchItem);
                    break;
            }

            return crossSellMainCat14;
        }

        public static string BuildCrossSellSubCat14<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat14 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat14 = SDBJohnHardy.BuildCrossSellSubCat14(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat14 = SDBTagHeuer.BuildCrossSellSubCat14(type, item, batchItem);
                    break;
            }

            return crossSellSubCat14;
        }

        public static string BuildCrossSellSecCat14<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat14 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat14 = SDBJohnHardy.BuildCrossSellSecCat14(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat14 = SDBTagHeuer.BuildCrossSellSecCat14(type, item);
                    break;
            }

            return crossSellSecCat14;
        }

        public static string BuildCrossSellMainCat15<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellMainCat15 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat15 = SDBJohnHardy.BuildCrossSellMainCat15(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat15 = SDBTagHeuer.BuildCrossSellMainCat15(type, item, batchItem);
                    break;
            }

            return crossSellMainCat15;
        }

        public static string BuildCrossSellSubCat15<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string crossSellSubCat15 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat15 = SDBJohnHardy.BuildCrossSellSubCat15(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat15 = SDBTagHeuer.BuildCrossSellSubCat15(type, item, batchItem);
                    break;
            }

            return crossSellSubCat15;
        }

        public static string BuildCrossSellSecCat15<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat15 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat15 = SDBJohnHardy.BuildCrossSellSecCat15(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat15 = SDBTagHeuer.BuildCrossSellSecCat15(type, item);
                    break;
            }

            return crossSellSecCat15;
        }

        public static string BuildCrossSellMainCat16<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat16 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat16 = SDBJohnHardy.BuildCrossSellMainCat16(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat16 = SDBTagHeuer.BuildCrossSellMainCat16(type, item);
                    break;
            }

            return crossSellMainCat16;
        }

        public static string BuildCrossSellSubCat16<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat16 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat16 = SDBJohnHardy.BuildCrossSellSubCat16(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat16 = SDBTagHeuer.BuildCrossSellSubCat16(type, item);
                    break;
            }

            return crossSellSubCat16;
        }

        public static string BuildCrossSellSecCat16<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat16 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat16 = SDBJohnHardy.BuildCrossSellSecCat16(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat16 = SDBTagHeuer.BuildCrossSellSecCat16(type, item);
                    break;
            }

            return crossSellSecCat16;
        }

        public static string BuildCrossSellMainCat17<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat17 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat17 = SDBJohnHardy.BuildCrossSellMainCat17(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat17 = SDBTagHeuer.BuildCrossSellMainCat17(type, item);
                    break;
            }

            return crossSellMainCat17;
        }

        public static string BuildCrossSellSubCat17<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat17 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat17 = SDBJohnHardy.BuildCrossSellSubCat17(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat17 = SDBTagHeuer.BuildCrossSellSubCat17(type, item);
                    break;
            }

            return crossSellSubCat17;
        }

        public static string BuildCrossSellSecCat17<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat17 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat17 = SDBJohnHardy.BuildCrossSellSecCat17(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat17 = SDBTagHeuer.BuildCrossSellSecCat17(type, item);
                    break;
            }

            return crossSellSecCat17;
        }

        public static string BuildCrossSellMainCat18<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat18 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat18 = SDBJohnHardy.BuildCrossSellMainCat18(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat18 = SDBTagHeuer.BuildCrossSellMainCat18(type, item);
                    break;
            }

            return crossSellMainCat18;
        }

        public static string BuildCrossSellSubCat18<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat18 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat18 = SDBJohnHardy.BuildCrossSellSubCat18(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat18 = SDBTagHeuer.BuildCrossSellSubCat18(type, item);
                    break;
            }

            return crossSellSubCat18;
        }

        public static string BuildCrossSellSecCat18<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat18 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat18 = SDBJohnHardy.BuildCrossSellSecCat18(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat18 = SDBTagHeuer.BuildCrossSellSecCat18(type, item);
                    break;
            }

            return crossSellSecCat18;
        }

        public static string BuildCrossSellMainCat19<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat19 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat19 = SDBJohnHardy.BuildCrossSellMainCat19(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat19 = SDBTagHeuer.BuildCrossSellMainCat19(type, item);
                    break;
            }

            return crossSellMainCat19;
        }

        public static string BuildCrossSellSubCat19<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat19 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat19 = SDBJohnHardy.BuildCrossSellSubCat19(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat19 = SDBTagHeuer.BuildCrossSellSubCat19(type, item);
                    break;
            }

            return crossSellSubCat19;
        }

        public static string BuildCrossSellSecCat19<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat19 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat19 = SDBJohnHardy.BuildCrossSellSecCat19(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat19 = SDBTagHeuer.BuildCrossSellSecCat19(type, item);
                    break;
            }

            return crossSellSecCat19;
        }

        public static string BuildCrossSellMainCat20<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat20 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellMainCat20 = SDBJohnHardy.BuildCrossSellMainCat20(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat20 = SDBTagHeuer.BuildCrossSellMainCat20(type, item);
                    break;
            }

            return crossSellMainCat20;
        }

        public static string BuildCrossSellSubCat20<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat20 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSubCat20 = SDBJohnHardy.BuildCrossSellSubCat20(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat20 = SDBTagHeuer.BuildCrossSellSubCat20(type, item);
                    break;
            }

            return crossSellSubCat20;
        }

        public static string BuildCrossSellSecCat20<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat20 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    crossSellSecCat20 = SDBJohnHardy.BuildCrossSellSecCat20(type, item);
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat20 = SDBTagHeuer.BuildCrossSellSecCat20(type, item);
                    break;
            }

            return crossSellSecCat20;
        }

        public static string BuildCrossSellMainCat21<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat21 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    
                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat21 = SDBTagHeuer.BuildCrossSellMainCat21(type, item);
                    break;
            }

            return crossSellMainCat21;
        }

        public static string BuildCrossSellSubCat21<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat21 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    
                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat21 = SDBTagHeuer.BuildCrossSellSubCat21(type, item);
                    break;
            }

            return crossSellSubCat21;
        }

        public static string BuildCrossSellSecCat21<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat21 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    
                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat21 = SDBTagHeuer.BuildCrossSellSecCat21(type, item);
                    break;
            }

            return crossSellSecCat21;
        }

        public static string BuildCrossSellMainCat22<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat22 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat22 = SDBTagHeuer.BuildCrossSellMainCat22(type, item);
                    break;
            }

            return crossSellMainCat22;
        }

        public static string BuildCrossSellSubCat22<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat22 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat22 = SDBTagHeuer.BuildCrossSellSubCat22(type, item);
                    break;
            }

            return crossSellSubCat22;
        }

        public static string BuildCrossSellSecCat22<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat22 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat22 = SDBTagHeuer.BuildCrossSellSecCat22(type, item);
                    break;
            }

            return crossSellSecCat22;
        }

        public static string BuildCrossSellMainCat23<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat23 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat23 = SDBTagHeuer.BuildCrossSellMainCat23(type, item);
                    break;
            }

            return crossSellMainCat23;
        }

        public static string BuildCrossSellSubCat23<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat23 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat23 = SDBTagHeuer.BuildCrossSellSubCat23(type, item);
                    break;
            }

            return crossSellSubCat23;
        }

        public static string BuildCrossSellSecCat23<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat23 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat23 = SDBTagHeuer.BuildCrossSellSecCat23(type, item);
                    break;
            }

            return crossSellSecCat23;
        }

        public static string BuildCrossSellMainCat24<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat24 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat24 = SDBTagHeuer.BuildCrossSellMainCat24(type, item);
                    break;
            }

            return crossSellMainCat24;
        }

        public static string BuildCrossSellSubCat24<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat24 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat24 = SDBTagHeuer.BuildCrossSellSubCat24(type, item);
                    break;
            }

            return crossSellSubCat24;
        }

        public static string BuildCrossSellSecCat24<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat24 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat24 = SDBTagHeuer.BuildCrossSellSecCat24(type, item);
                    break;
            }

            return crossSellSecCat24;
        }

        public static string BuildCrossSellMainCat25<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat25 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat25 = SDBTagHeuer.BuildCrossSellMainCat25(type, item);
                    break;
            }

            return crossSellMainCat25;
        }

        public static string BuildCrossSellSubCat25<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat25 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat25 = SDBTagHeuer.BuildCrossSellSubCat25(type, item);
                    break;
            }

            return crossSellSubCat25;
        }

        public static string BuildCrossSellSecCat25<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat25 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat25 = SDBTagHeuer.BuildCrossSellSecCat25(type, item);
                    break;
            }

            return crossSellSecCat25;
        }

        public static string BuildCrossSellMainCat26<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat26 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat26 = SDBTagHeuer.BuildCrossSellMainCat26(type, item);
                    break;
            }

            return crossSellMainCat26;
        }

        public static string BuildCrossSellSubCat26<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat26 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat26 = SDBTagHeuer.BuildCrossSellSubCat26(type, item);
                    break;
            }

            return crossSellSubCat26;
        }

        public static string BuildCrossSellSecCat26<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat26 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat26 = SDBTagHeuer.BuildCrossSellSecCat26(type, item);
                    break;
            }

            return crossSellSecCat26;
        }

        public static string BuildCrossSellMainCat27<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat27 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat27 = SDBTagHeuer.BuildCrossSellMainCat27(type, item);
                    break;
            }

            return crossSellMainCat27;
        }

        public static string BuildCrossSellSubCat27<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat27 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat27 = SDBTagHeuer.BuildCrossSellSubCat27(type, item);
                    break;
            }

            return crossSellSubCat27;
        }

        public static string BuildCrossSellSecCat27<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat27 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat27 = SDBTagHeuer.BuildCrossSellSecCat27(type, item);
                    break;
            }

            return crossSellSecCat27;
        }

        public static string BuildCrossSellMainCat28<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat28 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat28 = SDBTagHeuer.BuildCrossSellMainCat28(type, item);
                    break;
            }

            return crossSellMainCat28;
        }

        public static string BuildCrossSellSubCat28<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat28 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat28 = SDBTagHeuer.BuildCrossSellSubCat28(type, item);
                    break;
            }

            return crossSellSubCat28;
        }

        public static string BuildCrossSellSecCat28<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat28 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat28 = SDBTagHeuer.BuildCrossSellSecCat28(type, item);
                    break;
            }

            return crossSellSecCat28;
        }

        public static string BuildCrossSellMainCat29<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat29 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat29 = SDBTagHeuer.BuildCrossSellMainCat29(type, item);
                    break;
            }

            return crossSellMainCat29;
        }

        public static string BuildCrossSellSubCat29<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat29 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat29 = SDBTagHeuer.BuildCrossSellSubCat29(type, item);
                    break;
            }

            return crossSellSubCat29;
        }

        public static string BuildCrossSellSecCat29<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat29 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat29 = SDBTagHeuer.BuildCrossSellSecCat29(type, item);
                    break;
            }

            return crossSellSecCat29;
        }

        public static string BuildCrossSellMainCat30<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat30 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat30 = SDBTagHeuer.BuildCrossSellMainCat30(type, item);
                    break;
            }

            return crossSellMainCat30;
        }

        public static string BuildCrossSellSubCat30<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat30 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat30 = SDBTagHeuer.BuildCrossSellSubCat30(type, item);
                    break;
            }

            return crossSellSubCat30;
        }

        public static string BuildCrossSellSecCat30<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat30 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat30 = SDBTagHeuer.BuildCrossSellSecCat30(type, item);
                    break;
            }

            return crossSellSecCat30;
        }

        public static string BuildCrossSellMainCat31<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat31 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat31 = SDBTagHeuer.BuildCrossSellMainCat31(type, item);
                    break;
            }

            return crossSellMainCat31;
        }

        public static string BuildCrossSellSubCat31<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat31 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat31 = SDBTagHeuer.BuildCrossSellSubCat31(type, item);
                    break;
            }

            return crossSellSubCat31;
        }

        public static string BuildCrossSellSecCat31<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat31 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat31 = SDBTagHeuer.BuildCrossSellSecCat31(type, item);
                    break;
            }

            return crossSellSecCat31;
        }

        public static string BuildCrossSellMainCat32<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat32 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat32 = SDBTagHeuer.BuildCrossSellMainCat32(type, item);
                    break;
            }

            return crossSellMainCat32;
        }

        public static string BuildCrossSellSubCat32<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat32 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat32 = SDBTagHeuer.BuildCrossSellSubCat32(type, item);
                    break;
            }

            return crossSellSubCat32;
        }

        public static string BuildCrossSellSecCat32<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat32 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat32 = SDBTagHeuer.BuildCrossSellSecCat32(type, item);
                    break;
            }

            return crossSellSecCat32;
        }

        public static string BuildCrossSellMainCat33<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat33 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat33 = SDBTagHeuer.BuildCrossSellMainCat33(type, item);
                    break;
            }

            return crossSellMainCat33;
        }

        public static string BuildCrossSellSubCat33<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat33 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat33 = SDBTagHeuer.BuildCrossSellSubCat33(type, item);
                    break;
            }

            return crossSellSubCat33;
        }

        public static string BuildCrossSellSecCat33<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat33 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat33 = SDBTagHeuer.BuildCrossSellSecCat33(type, item);
                    break;
            }

            return crossSellSecCat33;
        }

        public static string BuildCrossSellMainCat34<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellMainCat34 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellMainCat34 = SDBTagHeuer.BuildCrossSellMainCat34(type, item);
                    break;
            }

            return crossSellMainCat34;
        }

        public static string BuildCrossSellSubCat34<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSubCat34 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSubCat34 = SDBTagHeuer.BuildCrossSellSubCat34(type, item);
                    break;
            }

            return crossSellSubCat34;
        }

        public static string BuildCrossSellSecCat34<T>(ProductType type, SupplierType supplierType, T item)
        {
            string crossSellSecCat34 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:

                    break;
                case SupplierType.TagHeuer:
                    crossSellSecCat34 = SDBTagHeuer.BuildCrossSellSecCat34(type, item);
                    break;
            }

            return crossSellSecCat34;
        }

        public static string BuildCustomHtmlAboveQty<T>(ProductType type, SupplierType supplierType, T item)
        {
            string customHtmlAboveQty = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    customHtmlAboveQty = SDBJohnHardy.BuildCustomHtmlAboveQty(type, item);
                    break;
                case SupplierType.TagHeuer:

                    break;
            }

            return customHtmlAboveQty;
        }

        public static string BuildSpecifications<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string specifications = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    specifications = SDBJohnHardy.BuildSpecifications(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    specifications = SDBTagHeuer.BuildSpecifications(type, item, batchItem);
                    break;
            }

            return specifications;
        }

        public static string BuildSubTitle<T>(ProductType type, SupplierType supplierType, T item, SceBatchItem batchItem)
        {
            string subTitle = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    subTitle = SDBJohnHardy.BuildSubTitle(type, item, batchItem);
                    break;
                case SupplierType.TagHeuer:
                    subTitle = SDBTagHeuer.BuildSubTitle(type, item, batchItem);
                    break;
            }

            return subTitle;
        }

        public static string BuildSupplier<T>(ProductType type, SupplierType supplierType, T item)
        {
            string supplier = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    supplier = SDBJohnHardy.BuildSupplier(type, item);
                    break;
                case SupplierType.TagHeuer:
                    supplier = SDBTagHeuer.BuildSupplier(type, item);
                    break;
            }

            return supplier;
        }

        public static string BuildReOrderSupplier<T>(ProductType type, SupplierType supplierType, T item)
        {
            string reOrderSupplier = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    reOrderSupplier = SDBJohnHardy.BuildReOrderSupplier(type, item);
                    break;
                case SupplierType.TagHeuer:
                    reOrderSupplier = SDBTagHeuer.BuildReOrderSupplier(type, item);
                    break;
            }

            return reOrderSupplier;
        }

        public static string BuildWarehouse<T>(ProductType type, SupplierType supplierType, T item)
        {
            string warehouse = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    warehouse = SDBJohnHardy.BuildWarehouse(type, item);
                    break;
                case SupplierType.TagHeuer:
                    warehouse = SDBTagHeuer.BuildWarehouse(type, item);
                    break;
            }

            return warehouse;
        }

        public static string BuildProcessingTime<T>(ProductType type, SupplierType supplierType, T item)
        {
            string processingTime = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    processingTime = SDBJohnHardy.BuildProcessingTime(type, item);
                    break;
                case SupplierType.TagHeuer:
                    processingTime = SDBTagHeuer.BuildProcessingTime(type, item);
                    break;
            }

            return processingTime;
        }

        public static string BuildShippingType<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingType = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingType = SDBJohnHardy.BuildShippingType(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingType = SDBTagHeuer.BuildShippingType(type, item);
                    break;
            }

            return shippingType;
        }

        public static string BuildShippingCarrier1<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingCarrier1 = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingCarrier1 = SDBJohnHardy.BuildShippingCarrier1(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingCarrier1 = SDBTagHeuer.BuildShippingCarrier1(type, item);
                    break;
            }

            return shippingCarrier1;
        }

        public static string BuildAllowground<T>(ProductType type, SupplierType supplierType, T item)
        {
            string allowground = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    allowground = SDBJohnHardy.BuildAllowground(type, item);
                    break;
                case SupplierType.TagHeuer:
                    allowground = SDBTagHeuer.BuildAllowground(type, item);
                    break;
            }

            return allowground;
        }

        public static string BuildAllow3day<T>(ProductType type, SupplierType supplierType, T item)
        {
            string allow3day = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    allow3day = SDBJohnHardy.BuildAllow3day(type, item);
                    break;
                case SupplierType.TagHeuer:
                    allow3day = SDBTagHeuer.BuildAllow3day(type, item);
                    break;
            }

            return allow3day;
        }

        public static string BuildAllow2day<T>(ProductType type, SupplierType supplierType, T item)
        {
            string allow2day = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    allow2day = SDBJohnHardy.BuildAllow2day(type, item);
                    break;
                case SupplierType.TagHeuer:
                    allow2day = SDBTagHeuer.BuildAllow2day(type, item);
                    break;
            }

            return allow2day;
        }

        public static string BuildAllownextday<T>(ProductType type, SupplierType supplierType, T item)
        {
            string allownextday = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    allownextday = SDBJohnHardy.BuildAllownextday(type, item);
                    break;
                case SupplierType.TagHeuer:
                    allownextday = SDBTagHeuer.BuildAllownextday(type, item);
                    break;
            }

            return allownextday;
        }

        public static string BuildShippingGroundRate<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingGroundRate = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingGroundRate = SDBJohnHardy.BuildShippingGroundRate(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingGroundRate = SDBTagHeuer.BuildShippingGroundRate(type, item);
                    break;
            }

            return shippingGroundRate;
        }

        public static string BuildShippingNextDayAirRate<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingNextDayAirRate = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingNextDayAirRate = SDBJohnHardy.BuildShippingNextDayAirRate(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingNextDayAirRate = SDBTagHeuer.BuildShippingNextDayAirRate(type, item);
                    break;
            }

            return shippingNextDayAirRate;
        }

        public static string BuildItemWeight<T>(ProductType type, SupplierType supplierType, T item)
        {
            string itemWeight = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    itemWeight = SDBJohnHardy.BuildItemWeight(type, item);
                    break;
                case SupplierType.TagHeuer:
                    itemWeight = SDBTagHeuer.BuildItemWeight(type, item);
                    break;
            }

            return itemWeight;
        }

        public static string BuildItemHeight<T>(ProductType type, SupplierType supplierType, T item)
        {
            string itemHeight = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    itemHeight = SDBJohnHardy.BuildItemHeight(type, item);
                    break;
                case SupplierType.TagHeuer:
                    itemHeight = SDBTagHeuer.BuildItemHeight(type, item);
                    break;
            }

            return itemHeight;
        }

        public static string BuildItemWidth<T>(ProductType type, SupplierType supplierType, T item)
        {
            string itemWidth = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    itemWidth = SDBJohnHardy.BuildItemWidth(type, item);
                    break;
                case SupplierType.TagHeuer:
                    itemWidth = SDBTagHeuer.BuildItemWidth(type, item);
                    break;
            }

            return itemWidth;
        }

        public static string BuildItemLength<T>(ProductType type, SupplierType supplierType, T item)
        {
            string itemLength = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    itemLength = SDBJohnHardy.BuildItemLength(type, item);
                    break;
                case SupplierType.TagHeuer:
                    itemLength = SDBTagHeuer.BuildItemLength(type, item);
                    break;
            }

            return itemLength;
        }

        public static string BuildShippingWeight<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingWeight = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingWeight = SDBJohnHardy.BuildShippingWeight(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingWeight = SDBTagHeuer.BuildShippingWeight(type, item);
                    break;
            }

            return shippingWeight;
        }

        public static string BuildShippingHeight<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingHeight = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingHeight = SDBJohnHardy.BuildShippingHeight(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingHeight = SDBTagHeuer.BuildShippingHeight(type, item);
                    break;
            }

            return shippingHeight;
        }

        public static string BuildShippingWidth<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingWidth = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingWidth = SDBJohnHardy.BuildShippingWidth(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingWidth = SDBTagHeuer.BuildShippingWidth(type, item);
                    break;
            }

            return shippingWidth;
        }

        public static string BuildShippingLength<T>(ProductType type, SupplierType supplierType, T item)
        {
            string shippingLength = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    shippingLength = SDBJohnHardy.BuildShippingLength(type, item);
                    break;
                case SupplierType.TagHeuer:
                    shippingLength = SDBTagHeuer.BuildShippingLength(type, item);
                    break;
            }

            return shippingLength;
        }

        public static string BuildAllowNewCategory<T>(ProductType type, SupplierType supplierType, T item)
        {
            string allowNewCategory = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    allowNewCategory = SDBJohnHardy.BuildAllowNewCategory(type, item);
                    break;
                case SupplierType.TagHeuer:
                    allowNewCategory = SDBTagHeuer.BuildAllowNewCategory(type, item);
                    break;
            }

            return allowNewCategory;
        }

        public static string BuildAllowNewBrand<T>(ProductType type, SupplierType supplierType, T item)
        {
            string allowNewBrand = string.Empty;

            switch (supplierType)
            {
                case SupplierType.JohnHardy:
                    allowNewBrand = SDBJohnHardy.BuildAllowNewBrand(type, item);
                    break;
                case SupplierType.TagHeuer:
                    allowNewBrand = SDBTagHeuer.BuildAllowNewBrand(type, item);
                    break;
            }

            return allowNewBrand;
        }
    }
}
