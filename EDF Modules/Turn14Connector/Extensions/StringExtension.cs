namespace Turn14Connector.Extensions
{
    public static class StringExtension
    {
        public static string RemoveDoubleSpace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("  ", " ").Trim();
            }

            return str;
        }
    }
}
