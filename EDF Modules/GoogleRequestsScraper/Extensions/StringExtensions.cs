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
    }
}
