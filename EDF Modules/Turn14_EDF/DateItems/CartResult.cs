using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn14_EDF.DateItems
{
    class CartResult
    {
        public bool DetectCartChange { get; set; }
        public bool Validation { get; set; }
        public string FailedValidationMessage { get; set; }
        public string Result { get; set; }
        public string LastUpdate { get; set; }
    }
}
