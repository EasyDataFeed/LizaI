using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn14ApiScraper.DataItems
{
    public class TransferInfoItem
    {

        #region SCE

        public string ProdIdSce { get; set; }
        public string ProdTypeSce { get; set; }

        public string PartNumberSce { get; set; }
        public string ManufacturerPartNumberSce { get; set; }
        public string BrandSce { get; set; }


        public double Msrp { get; set; }
        public double CostPrice { get; set; }
        public double WebPrice { get; set; }
        public double Jobber { get; set; }

        #endregion

        #region Turn14

        public string PartNumberTurn14 { get; set; }
        public string ManufacturerPartNumberTurn14 { get; set; }
        public string BrandTurn14 { get; set; }
        public string QuantityTurn14 { get; set; }
        public string Turn14WarehouseEastStock { get; set; }
        public string Turn14WarehouseWesStock { get; set; }
        public string Turn14WarehouseCentralStock { get; set; }

        #endregion

    }
}
