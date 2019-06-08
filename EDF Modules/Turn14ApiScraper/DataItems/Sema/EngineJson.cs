using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turn14ApiScraper.DataItems.Sema
{
    public class EngineJson
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Engines> Engines { get; set; }
    }

    public class Engines
    {
        public string VehicleID { get; set; }
        public string Liter { get; set; }
        public string CC { get; set; }
        public string CID { get; set; }
        public string Cylinders { get; set; }
        public string BlockType { get; set; }
        public string EngBoreMetric { get; set; }
        public string EngStrokeIn { get; set; }
        public string EngStrokeMetric { get; set; }
        public string ValvesPerEngine { get; set; }
        public string AspirationName { get; set; }
        public string CylinderHeadTypeName { get; set; }
        public string FuelTypeName { get; set; }
        public string IgnitionSystemTypeName { get; set; }
        public string MfrName { get; set; }
        public string HorsePower { get; set; }
        public string KilowattPower { get; set; }
    }
}
