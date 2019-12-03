using ScraperApiTurn14.DataItems.Turn14;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace ScraperApiTurn14.Helpers
{
    class Turn14ApiHelper
    {

        #region Constructors

        public static void SetAuthInfo(string clientId, string clientSecret)
        {
            _authInfo = new AuthJson
            {
                client_id = clientId,
                client_secret = clientSecret,
                grant_type = "client_credentials"
            };
        }

        #endregion

        #region Variables and Properties

        private static AuthJson _authInfo;
        public static TokenJson TokenInfo { get; set; }


        #endregion

        #region Token

        public static void GetToken(bool production = true)
        {
            TokenJson resp = new TokenJson();
            string requestUrl = production ? "https://api.turn14.com/v1/token" : "https://apitest.turn14.com/v1/token";
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            string dataString = $"{{\"grant_type\":\"{_authInfo.grant_type}\",\"client_id\":\"{_authInfo.client_id}\",\"client_secret\":\"{_authInfo.client_secret}\"}}";
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

                    var jss = new JavaScriptSerializer();
                    resp = jss.Deserialize<TokenJson>(jsonString);
                    resp.ExpiredTime = DateTime.Now.AddSeconds(resp.expires_in);
                    TokenInfo = resp;
                    //return resp;
                }
            }
        }

        private static void RefreshToken(bool production = true)
        {
            if (!production)
                GetToken(false);
            else if (DateTime.Now > TokenInfo.ExpiredTime)
                GetToken();
        }

        #endregion

        #region Items

        public static ItemsJson GetItemsByBrand(dataBrands dataBrands, int page)
        {
            RefreshToken();
            ItemsJson responce = new ItemsJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/items/brand/{dataBrands.id}?page={page}") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<ItemsJson>(jsonString);

                    return responce;
                }
            }
        }

        public static SingleItemDataJson GetSingleItemData(dataItems dataItems)
        {
            RefreshToken();
            SingleItemDataJson responce = new SingleItemDataJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/items/data/{dataItems.id}") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<SingleItemDataJson>(jsonString);

                    return responce;
                }
            }
        }

        #endregion

        #region Brands

        public static BrandsJson GetBrands()
        {
            RefreshToken();
            BrandsJson responce = new BrandsJson();

            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/brands") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<BrandsJson>(jsonString);

                    return responce;
                }
            }
        }

        #endregion

        #region Pricing

        public static SingleItemPricingJson GetSingleItemPricing(dataItems dataItems)
        {
            RefreshToken();
            SingleItemPricingJson responce = new SingleItemPricingJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/pricing/{dataItems.id}") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<SingleItemPricingJson>(jsonString);

                    return responce;
                }
            }
        }

        #endregion

        #region Inventory

        public static InventorySingleItemJson GetInventorySingleItem(dataItems dataItems)
        {
            RefreshToken();
            try
            {
                InventorySingleItemJson responce = new InventorySingleItemJson();
                HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/inventory/{dataItems.id}") as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        var jsonString = sr.ReadToEnd();

                        var jss = new JavaScriptSerializer();
                        responce = jss.Deserialize<InventorySingleItemJson>(jsonString);
                        //var abc = JsonConvert.DeserializeObject<InventorySingleItemJson>(jsonString);
                        //var inventoryInfo = abc.data[0].attributes.inventory.ToObject<InventoryInfo>();

                        return responce;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public static AllLocationsJson GetAllLocations()
        {
            RefreshToken();
            AllLocationsJson responce = new AllLocationsJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/locations") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<AllLocationsJson>(jsonString);

                    return responce;
                }
            }
        }


        #endregion

    }
}
