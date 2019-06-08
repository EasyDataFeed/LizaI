using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn14ApiScraper.Extensions
{
    public static class DoubleExtension
    {
        public static double Round(this double val)
        {
            return Math.Round(val, 2);
        }
    }
}
