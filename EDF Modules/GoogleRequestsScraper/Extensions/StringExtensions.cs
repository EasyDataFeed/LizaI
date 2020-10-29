using System.Text.RegularExpressions;
using WheelsScraper;

namespace GoogleRequestsScraper.Extensions
{
   public static class StringExtensions
    {
        public static string FillProfile(this string s, ProxyInfo proxy)
        {
            s = s.Replace("%ProxIp%", proxy.Address.Split(':')[0])
                .Replace("%ProxyPort%", proxy.Address.Split(':')[1])
                .Replace("%ProxyUserName%", proxy.Login)
                .Replace("%ProxyUserPassword%", proxy.Password);

            return s;
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string RemoveUnicode(this string input)
        {
            var s = input;
            s = Regex.Replace(s, @"[^\u0000-\u007F]", string.Empty);
            return s;
        }

        public static string RemoveDirtyData(this string p)
        {
            return
                p.RemoveUnicode()
                    .Replace("--", "-")
                    .Replace(" - ", "-")
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
                    .Replace("@", "")
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
                    .Replace("\r\n", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ");
        }
    }
}
