namespace RPMoutletInventory.DataItems.Meyer
{
    public class JsonMeyerItem
    {
        public string ItemNumber { get; set; }
        public string ManufacturerID { get; set; }
        public string ItemDescription { get; set; }
        public string PartStatus { get; set; }
        public string Kit { get; set; }
        public string KitOnly { get; set; }
        public string UPC { get; set; }
        public string LTLRequired { get; set; }
        //public float? Length { get; set; }
        //public float? Width { get; set; }
        //public float? Height { get; set; }
        //public float? Weight { get; set; }
        public string ManufacturerName { get; set; }
        public int? QtyAvailable { get; set; }
        //public float? SuggestedRetailPrice { get; set; }
        //public float? JobberPrice { get; set; }
        //public object MinAdvertisedPrice { get; set; }
        //public float? CustomerPrice { get; set; }
    }

}
