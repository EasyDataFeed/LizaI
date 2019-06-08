using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Fabricut.DataItems;

namespace Fabricut.Helpers
{
    public class RequestHelper
    {
        private const string GET_PRODUCT_REQUEST = "https://api.fabricut.com/v1E/ce07ff40cc6d417d8577084fcf092fbc/product/";
        private const string STOCK_INFO = "/stock+";

        public static JsonStockInfoItem GetStockItem(string productId)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = HttpWebRequest.Create($"{GET_PRODUCT_REQUEST}{productId}{STOCK_INFO}") as HttpWebRequest;
                request.Method = "GET";

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        var jsondata = sr.ReadToEnd();

                        if (jsondata != null)
                        {
                            var jsonInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonStockInfoItem>(jsondata);
                            return jsonInfo;
                        }
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
