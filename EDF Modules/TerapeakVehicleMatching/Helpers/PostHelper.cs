using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using TerapeakVehicleMatching.DataItems;

namespace TerapeakVehicleMatching.Helpers
{
    public static class PostHelper
    {
        public static GetSingleItemResponse GetPage(string url, out string exceptionStr)
        {
            exceptionStr = string.Empty;
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Headers.Clear();
                request.Method = "GET";

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        //var stt = sr.ReadToEnd();

                        XmlSerializer serializer = new XmlSerializer(typeof(GetSingleItemResponse));

                        GetSingleItemResponse ebayItem = (GetSingleItemResponse)serializer.Deserialize(sr);

                        return ebayItem;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ModelInfoGetterError: {ex.Message}");
                exceptionStr = ex.ToString();
            }
            return null;
        }
    }
}
