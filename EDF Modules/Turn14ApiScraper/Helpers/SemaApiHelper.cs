using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Turn14ApiScraper.DataItems.Sema;
using Turn14ApiScraper.DataItems.TEST;
using Turn14ApiScraper.DataItems.Turn14;

namespace Turn14ApiScraper.Helpers
{
    class SemaApiHelper
    {

        public static TokenSemaJson GetSemaToken(string login, string password, out string error)
        {
            error = "";

            try
            {
                //login = "OscarG";
                //password = "OscarGarzaSDC123!";
                TokenSemaJson responce = new TokenSemaJson();
                HttpWebRequest request = WebRequest.Create($"https://sdc.semadatacoop.org/sdcapi/token/get?userName={login}&password={password}") as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        var jsonString = sr.ReadToEnd();
                        //jsonString = "{\"token\":\"EAAAAHLj15HlxuR7GqcrmESBC / Fyom0FgYGyXwsjws9qhj7f\",\"success\":true,\"message\":\"\"}";

                        var jss = new JavaScriptSerializer();
                        responce = jss.Deserialize<TokenSemaJson>(jsonString);

                        return responce;
                    }
                }
            }
            catch (WebException exc)
            {
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
                error = e.ToString();

                return null;
            }
        }

        public static EngineJson GetEngine(string token)
        {
            try
            {
                EngineJson resp = new EngineJson();
                HttpWebRequest request = WebRequest.Create($"https://sdc.semadatacoop.org/sdcapi/export/piesexport") as HttpWebRequest;
                request.Method = "POST";
                request.Accept = "text/xml";
                request.ContentType = "application/x-www-form-urlencoded";
                string dataString = $"token={token}&AAIA_BrandId=BDKW&PIESVersion=6.7";
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                //request.Headers.Add("AAIA_BrandId", "BDKW");
                //request.Headers.Add("DatasetId", "81");
                //request.Headers.Add("Year", "2000");
                //request.Headers.Add("MakeID", "58");
                //request.Headers.Add("ModelID", "170");
                byte[] bufferBytes = asciiEncoding.GetBytes(dataString);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bufferBytes, 0, bufferBytes.Length);
                    stream.Flush();
                }

                using (WebResponse responce = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(responce.GetResponseStream()))
                    {
                        var jsonString = sr.ReadToEnd();
                        jsonString = jsonString.Replace("&lt;", "<").Replace("&gt;", ">");
                        File.WriteAllText(@"E:\path.txt", jsonString);

                        var jss = new JavaScriptSerializer();
                        resp = jss.Deserialize<EngineJson>(jsonString);

                        return resp;
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

        public static DigitalAssetsJson GetDigitalAssets(string token, string brandCode, out string error)
        {
            error = "";

            try
            {
                //brandCode = "CLSG";
                DigitalAssetsJson resp = new DigitalAssetsJson();
                HttpWebRequest request = WebRequest.Create($"https://sdc.semadatacoop.org/sdcapi/lookup/products") as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.Accept = "text/xml";
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                string dataString = $"token={token}&AAIA_BrandId={brandCode}&piesSegments=all";
                byte[] bufferBytes = asciiEncoding.GetBytes(dataString);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bufferBytes, 0, bufferBytes.Length);
                    stream.Flush();
                }

                using (WebResponse responce = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(responce.GetResponseStream()))
                    {
                        string jsonString = sr.ReadToEnd();

                        var jssResponce = new JavaScriptSerializer();
                        resp = jssResponce.Deserialize<DigitalAssetsJson>(jsonString);

                        return resp;
                    }
                }
            }
            catch (WebException exc)
            {
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
                error = e.ToString();

                return null;
            }
        }
    }
}
