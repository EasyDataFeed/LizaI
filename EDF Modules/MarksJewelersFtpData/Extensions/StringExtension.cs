using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarksJewelersFtpData.Extensions
{
    public static class StringExtension
    {
        public static string FormatImageNumber(this string s)
        {
            switch (s.Length)
            {
                case 1:
                    return $"00{s}";
                case 2:
                    return $"0{s}";
                default:
                    return s;
            }
        }
    }
}
