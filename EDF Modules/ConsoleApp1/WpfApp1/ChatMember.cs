using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class ChatMember
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string ChatName { get; set; }
        public string UserName { get; set; }
        public double LastOnline { get; set; }
        public TdLib.TdApi.Message[] LastMessages { get; set; }


        public override string ToString()
        {
            return $"{Id},{ChatId},{ChatName},{UserName},{UnixTimeStampToString(LastOnline)},{(LastMessages.Length > 0 ? UnixTimeStampToString(LastMessages[0].Date) : "")},{(LastMessages.Length > 1 ? UnixTimeStampToString(LastMessages[1].Date) : "")},{(LastMessages.Length > 2 ? UnixTimeStampToString(LastMessages[2].Date) : "")}";
        }
        public static string CSVHeder()
        {
            return "UserId,ChatId,ChatName,UserName,LastOnLine,LastMessage1,LastMessage2,LastMessage3";
        }

        public static string UnixTimeStampToString(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime.ToString("yyyyMMdd");
        }
    }
}
