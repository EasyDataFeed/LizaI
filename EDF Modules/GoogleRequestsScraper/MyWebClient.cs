using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleRequestsScraper
{
    internal class MyWebClient : WebClient
    {
        private TimeSpan timeout;
        public MyWebClient(TimeSpan timeout)
        {
            this.timeout = timeout;
        }
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = (int)timeout.TotalMilliseconds;
            return w;
        }
    }
}
