using System.Collections.Generic;

namespace Turn14Connector.DataItems.Turn14
{
    class AllLocationsJson
    {
        public AllLocationsJson()
        {
            data = new List<dataAllLocations>();
        }

        public List<dataAllLocations> data { get; set; }
    }

    public class dataAllLocations
    {
        public string id { get; set; }
        public string type { get; set; }
        public attributesAllLocations attributes { get; set; }
    }

    public class attributesAllLocations
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
