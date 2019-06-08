using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acumotors.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > maxLength)
                str = str.Substring(0, maxLength).Trim();

            return str;
        }
    }
}
