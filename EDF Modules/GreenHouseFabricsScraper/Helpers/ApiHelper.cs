using GreenHouseFabricsScraper.DataItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace GreenHouseFabricsScraper.Helpers
{
    public class ApiHelper
    {
        public static HtmlJson GetHtml(int start)
        {
            try
            {
                HtmlJson responce = new HtmlJson();
                HttpWebRequest request = WebRequest.Create($"https://www.greenhousefabrics.com/fabrics/ajax?start={start}") as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        var jsonString = sr.ReadToEnd();

                        var jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = 50000000;
                        responce = jss.Deserialize<HtmlJson>(jsonString);

                        return responce;
                    }
                }
            }
            catch (WebException exc)
            {
                string error = "";
                if (exc.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)exc.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
