using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FSchumacher.Helpers
{
    public static class PostHelper
    {
        private const string postUri = "http://fschumacher.com/Catalog/GetProducts";

        public static string GetPage(int pageNumber, string departmentname, CookieContainer cc)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(postUri) as HttpWebRequest;
                request.Headers.Clear();
                request.Method = "POST";
                request.Accept = "*/*";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cc;

                ASCIIEncoding ascii = new ASCIIEncoding();

                string strFormat =
                    $"pagenumber={pageNumber}&departmentname={departmentname}&rowperpage=30&sortby=Popularity&type=Prints&pricefrom=0&priceto=300";

                strFormat =$"pagenumber=2&departmentname=Fabrics&rowperpage=30&sortby=Popularity&type=Prints&pricefrom=0&priceto=300";

                byte[] postBytes = ascii.GetBytes(strFormat);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postBytes, 0, postBytes.Length);
                    stream.Flush();
                }

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        List<string> result = new List<string>();
                        var readerStr = sr.ReadToEnd();

                        return readerStr;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ModelInfoGetterError: {ex.Message}");
            }
            return null;
        }
    }
}
