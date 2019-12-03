using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OffRoadToyo.DataItems
{
    public class Rootobject
    {
        public Tirepattern[] tirepattern { get; set; }
    }

    public class Tirepattern
    {
        public Tire tire { get; set; }
    }

    public class Tire
    {
        [JsonProperty(PropertyName = "Product Code")]
        public string ProductCode { get; set; }

        [JsonProperty(PropertyName = "Product Name")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "Tire Size")]
        public string TireSize { get; set; }

        [JsonProperty(PropertyName = "Load Index SS")]
        public string LoadIndexSS { get; set; }

        [JsonProperty(PropertyName = "Load Range")]
        public string LoadRange { get; set; }

        [JsonProperty(PropertyName = "Max Load Dual")]
        public string MaxLoadDual { get; set; }

        [JsonProperty(PropertyName = "Max Load Single")]
        public string MaxLoadSingle { get; set; }

        [JsonProperty(PropertyName = "Static Load Radius")]
        public string StaticLoadRadius { get; set; }

        [JsonProperty(PropertyName = "Static Load Width")]
        public string StaticLoadWidth { get; set; }

        [JsonProperty(PropertyName = "Overall Diameter (inch)")]
        public string OverallDiameterinch { get; set; }

        [JsonProperty(PropertyName = "Overall Width (inch)")]
        public string OverallWidthinch { get; set; }

        [JsonProperty(PropertyName = "Approved Rim Width Range (inch)")]
        public string ApprovedRimWidthRangeinch { get; set; }

        [JsonProperty(PropertyName = "Rim Width")]
        public string RimWidth { get; set; }

        [JsonProperty(PropertyName = "Construction")]
        public string Construction { get; set; }

        [JsonProperty(PropertyName = "Max PSI Dual")]
        public string MaxPSIDual { get; set; }

        [JsonProperty(PropertyName = "Max PSI Single")]
        public string MaxPSISingle { get; set; }

        [JsonProperty(PropertyName = "Pattern Id")]
        public string PatternId { get; set; }

        [JsonProperty(PropertyName = "Ply Rating")]
        public string PlyRating { get; set; }

        [JsonProperty(PropertyName = "&quot;Revs/Mile")]
        public string quotRevsMile { get; set; }

        [JsonProperty(PropertyName = "Sidewall")]
        public string Sidewall { get; set; }

        [JsonProperty(PropertyName = "Sidewall Construction")]
        public string SidewallConstruction { get; set; }

        [JsonProperty(PropertyName = "Load Width")]
        public string LoadWidth { get; set; }

        [JsonProperty(PropertyName = "UTQG")]
        public string UTQG { get; set; }

        [JsonProperty(PropertyName = "Tire Weight (lbs.)")]
        public string TireWeightlbs { get; set; }

        [JsonProperty(PropertyName = "Load Index/Speed Symbol")]
        public string LoadIndexSpeedSymbol { get; set; }

        [JsonProperty(PropertyName = "Tread Width")]
        public string TreadWidth { get; set; }

        [JsonProperty(PropertyName = "For Sale")]
        public string ForSale { get; set; }

        [JsonProperty(PropertyName = "Tread Depth")]
        public string TreadDepth { get; set; }

        [JsonProperty(PropertyName = "MPH")]
        public string MPH { get; set; }

        [JsonProperty(PropertyName = "EAN")]
        public string EAN { get; set; }
    }
}
