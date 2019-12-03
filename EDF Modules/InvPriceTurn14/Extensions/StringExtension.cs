using System.Text.RegularExpressions;

namespace InvPriceTurn14.Extensions
{
    public static class StringExtension
    {
        public static string RemoveDirtyData(this string p)
        {
            return
                p.RemoveUnicode()
                    .Replace("--", "-")
                    .Replace("/", "")
                    .Replace("\"", "")
                    .Replace("*", "")
                    .Replace("€", " ")
                    .Replace("œ", " ")
                    .Replace("â", " ")
                    .Replace("¢", " ")
                    .Replace("Â", " ")
                    .Replace("®", " ")
                    .Replace("„", " ")
                    .Replace("  ", " ")
                    .Replace("[", " ")
                    .Replace("]", " ")
                    .Replace("{", " ")
                    .Replace("}", "")
                    .Replace("~~", "")
                    .Replace("varchar", "")
                    .Replace("sp_", "")
                    .Replace("xp_", "")
                    .Replace("™", "")
                    .Replace("“", "")
                    .Replace("Ã‚â€•", "")
                    .Replace("Ã¢â", "")
                    .Replace("¬â„¢s", "")
                    .Replace("€•", "")
                    .Replace("â€™", "")
                    .Replace("¢", "")
                    .Replace("@@", "")
                    .Replace("â€œ", "")
                    .Replace("â€”", "")
                    .Replace("insert into", "")
                    .Replace("/script", "")
                    .Replace("delete from", "")
                    .Replace("drop table", "")
                    .Replace("exec(", "")
                    .Replace("declare()*@", "")
                    .Replace("cast(", "")
                    .Replace("<strong>", "")
                    .Replace("</strong>", "")
                    .Replace("\r\n", "")
                    .Replace("\r", "")
                    .Replace("\n", "");
        }

        public static string ReplaceTextForAnchorText(this string p)
        {
            return p.RemoveDirtyData().Replace("&", " and ");
        }

        public static string RemoveUnicode(this string input)
        {
            var s = input;
            s = Regex.Replace(s, @"[^\u0000-\u007F]", string.Empty);
            return s;
        }
    }
}
