﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fabricut.DataItems
{
    public class PriceUpdate
    {
        public string Action { get; set; }
        public string ProdId { get; set; }
        public string ProductType { get; set; }
        public string PartNumber { get; set; }
        public double MSRP { get; set; }
        public double Jobber { get; set; }
        public double WebPrice { get; set; }
        public double CostPrice { get; set; }
    }
}
