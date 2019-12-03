using System;
using System.Collections.Generic;

namespace ScraperApiTurn14.DataItems.Turn14
{
    public class BrandsJson
    {
        public BrandsJson()
        {
            data = new List<dataBrands>();
        }

        public List<dataBrands> data { get; set; }
    }


    public class dataBrands
    {
        public long id { get; set; }
        public string type { get; set; }
        public attributesBrands attributes { get; set; }

        public override string ToString()
        {
            return attributes.name ?? String.Empty;
        }
    }
    public class attributesBrands
    {
        public string name { get; set; }
    }

    class SingleBrandJson
    {
        public dataSingleBrand data { get; set; }
    }

    public class dataSingleBrand
    {
        public long id { get; set; }
        public string type { get; set; }
        public attributesSingleBrand attributes { get; set; }
    }

    public class attributesSingleBrand
    {
        public string name { get; set; }
        public List<pricegroupsSingleBrand> pricegroups { get; set; }
    }

    public class pricegroupsSingleBrand
    {
        public string pricegroup_id { get; set; }
        public string pricegroup_name { get; set; }
    }

    class InventorySingleBrandJson
    {
        public List<dataInventorySingleBrand> data { get; set; }
    }

    public class dataInventorySingleBrand
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesInventorySingleBrand attributes { get; set; }
        public relationshipsInventorySingleBrand relationships { get; set; }
    }

    public class attributesInventorySingleBrand
    {
        public Dictionary<String, string> inventory { get; set; }
        public manufacturerInventerySingleBrand manufacturerInventerySingleBrand { get; set; }
    }

    public class InventorySingleBrandInfo
    {
        public Dictionary<String, string> inventory { get; set; }
    }

    public class manufacturerInventerySingleBrand
    {
        public string stock { get; set; }
        public DateTime esd { get; set; }
    }

    public class relationshipsInventorySingleBrand
    {
        public itemInventorySingleBrand item { get; set; }
    }

    public class itemInventorySingleBrand
    {
        public string links { get; set; }
    }
}
