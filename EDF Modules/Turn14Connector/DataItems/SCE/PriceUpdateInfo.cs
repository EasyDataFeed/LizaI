using System;

namespace Turn14Connector.DataItems.SCE
{
    class PriceUpdateInfo
    {
        #region Constructors

        public PriceUpdateInfo(ExtWareInfo ware, bool considerMapPrice, double percentageOfCost, double surchargePerLb, double minSurcharge, bool doPricePerPound)
        {
            double webPrice = GetPriceRules(ware.CostPrice, percentageOfCost);

            if (considerMapPrice)
            {
                if (ware.WebPrice != 0)
                {
                    if (ware.WebPrice > webPrice)
                    {
                        ware.Msrp = ware.WebPrice;
                        webPrice = ware.WebPrice;
                    }
                    else
                    {
                        ware.Msrp = webPrice;
                    }
                }
                else
                    ware.Msrp = webPrice;
            }
            else
            {
                ware.Msrp = webPrice;
            }

            //if (double.Parse(costPrice) > double.Parse(msrp))
            //    msrp = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

            //if (double.Parse(costPrice) >= double.Parse(webPrice))
            //    webPrice = ((double.Parse(costPrice) + 11) + ((double.Parse(costPrice) + 11) * 0.08)).ToString();

            //if (double.Parse(msrp) < double.Parse(webPrice))
            //    msrp = webPrice;

            ware.Jober = webPrice;
            double currentPrice = 0;

            if (doPricePerPound)
            {
                double.TryParse(ware.Weight, out double doubleWeight);
                doubleWeight = Math.Round(doubleWeight);
                currentPrice = minSurcharge + (surchargePerLb * doubleWeight);
            }

            Action = "update";
            Brand = ware.Brand;
            ProdId = ware.ProdId;
            ProductType = ware.ProductType;
            PartNumber = ware.ScePartNumber;
            MSRP = ware.Msrp + currentPrice;
            WebPrice = webPrice + currentPrice;
            Jobber = ware.Jober + currentPrice;
            CostPrice = ware.CostPrice;
        }

        #endregion

        #region Properties

        public string Action { get; set; }
        public string ProdId { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
        public string PartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public double MSRP { get; set; }
        public double CostPrice { get; set; }
        public double WebPrice { get; set; }
        public double Jobber { get; set; }
        public double CurrentPrice { get; set; }

        #endregion

        private double GetPriceRules(double price, double percentageOfCost)
        {
            price = price + (price * percentageOfCost / 100);

            return Math.Round(price, 2);
        }

    }
}
