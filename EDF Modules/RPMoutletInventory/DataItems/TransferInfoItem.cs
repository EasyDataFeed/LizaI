using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPMoutletInventory.DataItems
{
    public class TransferInfoItem
    {
        #region SCE

        public int ProdIdSce { get; set; }
        public int ProdTypeSce { get; set; }

        public string PartNumberSce { get; set; }
        public string ManufacturerPartNumberSce { get; set; }
        public string BrandSce { get; set; }


        public string Msrp { get; set; }
        public string CostPrice { get; set; }
        public string WebPrice { get; set; }
        public string Jobber { get; set; }

        #endregion

        #region Premier Data

        public string PartNumberPremier { get; set; }
        public string ManufacturerPartNumberPremier { get; set; }
        public string BrandPremier { get; set; }

        public string QuantityPremier { get; set; }
        public string PremierUtahWarehouse { get; set; }
        public string PremierKentuckyWarehouse { get; set; }
        public string PremierTexasWarehouse { get; set; }
        public string PremierCaliforniaWarehouse { get; set; }
        public string PremierCalgaryABWarehouse { get; set; }
        public string PremierWashingtonWarehouse { get; set; }
        public string PremierColoradoWarehouse { get; set; }
        public string PremierPonokaABWarehouse { get; set; }

        #endregion

        #region Turn14

        public string PartNumberTurn14 { get; set; }
        public string ManufacturerPartNumberTurn14 { get; set; }
        public string BrandTurn14 { get; set; }
        public string QuantityTurn14 { get; set; }
        public string Turn14WarehouseEastStock { get; set; }
        public string Turn14WarehouseWesStock { get; set; }
        public string Turn14MfrStock { get; set; }

        #endregion

        #region Meyer

        public string PartNumberMeyer { get; set; }
        public string ManufacturerPartNumberMeyer { get; set; }
        public string BrandMeyerLong { get; set; }
        public string BrandMeyerShort { get; set; }
        public string QuantityMeyer { get; set; }
        public string MeyerWarehouse { get; set; } = "Warehouse";

        #endregion

        #region Ebay
        public string EbayId { get; set; }
        public string BrandInEbay { get; set; }
        public string EbayPrice { get; set; }
        public int EbayQuantity { get; set; }
        public bool FoundOnEbay { get; set; }
        #endregion

    }
}
