using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinterest.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > maxLength)
                str = str.Substring(0, maxLength).Trim();

            return str;
        }

        public static string RemoveParametrs(this string str)
        {
            if (str.Contains("?"))
                str = str.Substring(0, str.LastIndexOf("?")).Trim();

            return str;
        }
    }
}
