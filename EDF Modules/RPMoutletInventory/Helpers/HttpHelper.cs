#region using

using System;
using System.IO;
using System.Net;
using System.Text;

#endregion

namespace RPMoutletInventory.Helpers
{
   public static class HttpHelper
    {
        private const String CONTENT_BOUNDARY = "-----------------------------BcLtEsToOlBoUnDaRy";

        public static string Post(string token, string file)

        {
            string ret = ""; //return string
            string data = File.ReadAllText(file);
            string url = "https://bulksell.ebay.com/ws/eBayISAPI.dll?FileExchangeUpload";
            var req = (HttpWebRequest)HttpWebRequest.Create(url);
            int timeout = 1000*60*2; //2 minutes
            req.Timeout = timeout;
            req.ContentType = "multipart/form-data; boundary=" + CONTENT_BOUNDARY;
            req.UserAgent = "FileExchange Tool/1.0";
            req.Accept =
                "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, application/pdf, application/x-comet, */*";
            req.KeepAlive = true;
            req.Method = "POST";
            if ((token == null) || (token.Length <= 0))
                return ret;
            string filename = file.Substring(file.LastIndexOf("\\") + 1);
            String request = "--" +
                             CONTENT_BOUNDARY +
                             "\r\n" +
                             "Content-Disposition: form-data; name=\"token\"\r\n\r\n" +
                             token +
                             "\r\n" +
                             "--" +
                             CONTENT_BOUNDARY +
                             "\r\n" +
                             "Content-Disposition: form-data; name=\"file\"; filename=\"" + filename + "\"" +
                             "\r\nContent-Type: text/plain\r\n\r\n" +
                             data +
                             "\r\n" +
                             "--" +
                             CONTENT_BOUNDARY +
                             "\r\n";


            byte[] bytes = Encoding.ASCII.GetBytes(request);

            req.ContentLength = bytes.Length;

            Stream os = req.GetRequestStream();

            os.Write(bytes, 0, bytes.Length);

            os.Close();

            WebResponse resp = req.GetResponse();

            if (resp == null)

                return ret;

            StreamReader sr = new StreamReader(resp.GetResponseStream());

            string respStr = sr.ReadToEnd().Trim();

            return respStr;
        }
    }
}
