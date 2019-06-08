#region using 

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Turn14ApiScraper.DataItems.Turn14;

#endregion

namespace Turn14ApiScraper.Helpers
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
        //public static string ClientId { get; set; }
        //public static string ClientSecret { get; set; }


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

        public static void GetToken(AuthJson authJson, bool production = true)
        {
            TokenJson resp = new TokenJson();

            string requestUrl = production ? "https://api.turn14.com/v1/token" : "https://apitest.turn14.com/v1/token";
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            var jss = new JavaScriptSerializer();
            string dataString = jss.Serialize(authJson);
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
                    resp = jssResponce.Deserialize<TokenJson>(jsonString);
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

        public static ItemsJson GetItems(int page)
        {
            RefreshToken();
            ItemsJson responce = new ItemsJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/items?page={page}") as HttpWebRequest;
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

        public static SingleItemJson GetSingleItem(dataItems dataItems)
        {
            RefreshToken();
            SingleItemJson responce = new SingleItemJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/items/{dataItems.id}") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<SingleItemJson>(jsonString);

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

        #region Orders

        public static InvoicesJson GetInvoices()
        {
            RefreshToken();
            InvoicesJson responce = new InvoicesJson();

            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/invoices") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<InvoicesJson>(jsonString);

                    return responce;
                }
            }
        }

        public static SingleInvoiceJson GetSingleInvoice(dataInvoice dataInvoice)
        {
            RefreshToken();
            SingleInvoiceJson responce = new SingleInvoiceJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/invoices/{dataInvoice.id}") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<SingleInvoiceJson>(jsonString);

                    return responce;
                }
            }
        }

        public static TrackingNumbersJson GetTrackingNumbers(DateTime from, DateTime to, bool production)
        {
            RefreshToken(production);
            TrackingNumbersJson responce = new TrackingNumbersJson();

            string requestUrl = production ? "https://api.turn14.com/v1/tracking" : "https://apitest.turn14.com/v1/tracking";
            HttpWebRequest request = WebRequest.Create($"{requestUrl}?start_date={from.ToString("yyyy-MM-dd")}&end_date={to.ToString("yyyy-MM-dd")}") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<TrackingNumbersJson>(jsonString);

                    return responce;
                }
            }
        }

        public static ShippingOptionsJson GetShippingOptions(bool production)
        {
            RefreshToken(production);
            ShippingOptionsJson responce = new ShippingOptionsJson();

            string requestUrl = production ? "https://api.turn14.com/v1/shipping" : "https://apitest.turn14.com/v1/shipping";

            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<ShippingOptionsJson>(jsonString);

                    return responce;
                }
            }
        }

        public static OrderQuoteJson RequestOrderQuote(Turn14OrderQuote order, bool production, out string error)
        {
            RefreshToken(production);
            OrderQuoteJson resp = new OrderQuoteJson();
            error = string.Empty;
            try
            {
                string requestUrl = production ? "https://api.turn14.com/v1/quote" : "https://apitest.turn14.com/v1/quote";

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                var jss = new JavaScriptSerializer();
                string dataString = jss.Serialize(order);
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
                        resp = jssResponce.Deserialize<OrderQuoteJson>(jsonString);
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
        }

        public static Turn14OrderJson ImportOrder(Turn14Order order, bool production, out string error)
        {
            RefreshToken(production);
            Turn14OrderJson resp = new Turn14OrderJson();
            error = string.Empty;
            try
            {
                string requestUrl = production ? "https://api.turn14.com/v1/order" : "https://apitest.turn14.com/v1/order";

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                var jss = new JavaScriptSerializer();
                string dataString = jss.Serialize(order);
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
                        resp = jssResponce.Deserialize<Turn14OrderJson>(jsonString);
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

        public static SingleBrandJson GetSingleBrand(dataBrands dataBrands)
        {
            RefreshToken();
            SingleBrandJson responce = new SingleBrandJson();
            HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/brands/{dataBrands.id}") as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

            using (var resp = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    var jsonString = sr.ReadToEnd();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<SingleBrandJson>(jsonString);

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

        public static InventoryJson GetInventory(int page)
        {
            RefreshToken();
            try
            {
                InventoryJson responce = new InventoryJson();
                HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/inventory?page={page}") as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        var jsonString = sr.ReadToEnd();

                        var jss = new JavaScriptSerializer();
                        responce = jss.Deserialize<InventoryJson>(jsonString);
                        //var abc = JsonConvert.DeserializeObject<InventoryJson>(jsonString);
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

        public static InventorySingleBrandJson GetInventorySingleBrand(int page, dataBrands dataBrands)
        {
            RefreshToken();
            try
            {
                InventorySingleBrandJson responce = new InventorySingleBrandJson();
                HttpWebRequest request = WebRequest.Create($"https://api.turn14.com/v1/inventory/brand/{dataBrands.id}?page={page}") as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", $"{TokenInfo.token_type} {TokenInfo.access_token}");

                using (var resp = request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        var jsonString = sr.ReadToEnd();

                        var jss = new JavaScriptSerializer();
                        responce = jss.Deserialize<InventorySingleBrandJson>(jsonString);
                        //var abc = JsonConvert.DeserializeObject<InventorySingleBrandJson>(jsonString);
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
