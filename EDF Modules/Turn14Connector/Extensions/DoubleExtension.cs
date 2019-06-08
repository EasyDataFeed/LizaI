using System;

namespace Turn14Connector.Extensions
{
    public static class DoubleExtension
    {
        public static double Round(this double val)
        {
            return Math.Round(val, 2);
        }
    }
}
