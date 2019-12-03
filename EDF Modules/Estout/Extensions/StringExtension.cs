namespace Estout.Extensions
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
