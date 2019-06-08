using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn14_EDF.DateItems
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
