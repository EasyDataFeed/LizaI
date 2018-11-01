using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class BotInfo
    {
        public string PhoneNumber { get; set; }
        public string GroupName { get; set; }
        public string ProxyIp { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword {get;set;}

        public static bool TryParse(string s, out BotInfo botInfo)
        {
            botInfo = new BotInfo();

            try
            {
                var list = s.Split(',');
                if (list.Length >= 2)
                {
                    if (!int.TryParse(list[3], out int proxyPort))
                    {
                        return false;
                    }
                    
                    botInfo.PhoneNumber = list[0];
                    botInfo.GroupName = list[1];
                    botInfo.ProxyIp = list[2];
                    botInfo.ProxyPort = proxyPort;
                    botInfo.ProxyLogin = list[4];
                    botInfo.ProxyPassword = list[5];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
