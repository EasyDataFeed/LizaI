using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSchumacher.Extensions
{
    public static class StringExtension
    {
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
    }
}
