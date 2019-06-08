namespace Turn14Connector.DataItems.SCE
{
    class InventoryUpdateInfo
    {

        #region Constructors

        public InventoryUpdateInfo(ExtWareInfo ware)
        {
            Brand = ware.Brand;
            ProdId = ware.ProdId;
            PartNumber = ware.ScePartNumber;
            ManufacturerPartNumber = ware.ManufacturerNumber;
            Stock = ware.Stock;
            ManufacturerStock = ware.ManufacturerStock;
        }


        #endregion

        #region Properties

        public string ProdId { get; set; }
        public string PartNumber { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
        public int Stock { get; set; }
        public int ManufacturerStock { get; set; }

        #endregion

    }
}
