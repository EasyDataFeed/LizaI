using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using FSchumacher;
using Databox.Libs.FSchumacher;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using FSchumacher.DataItems;
using FSchumacher.Enums;
using FSchumacher.Extensions;
using FSchumacher.Helpers;
using Newtonsoft.Json;

namespace WheelsScraper
{
    public class FSchumacher : BaseScraper
    {

        System.Net.CookieContainer cc { get; set; }
        public FSchumacher()
        {
            Name = "FSchumacher";
            Url = "https://www.FSchumacher.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
        }

        private ExtSettings extSett
        {
            get
            {
                return (ExtSettings)Settings.SpecialSettings;
            }
        }

        public override Type[] GetTypesForXmlSerialization()
        {
            return new Type[] { typeof(ExtSettings) };
        }

        public override System.Windows.Forms.Control SettingsTab
        {
            get
            {
                var frm = new ucExtSettings();
                frm.Sett = Settings;
                return frm;
            }
        }

        public override WareInfo WareInfoType
        {
            get
            {
                return new ExtWareInfo();
            }
        }


        #region Old

        //protected override bool Login()
        //{
        //    try
        //    {
        //        var login = GetLoginInfo();
        //        if (login == null)
        //        {
        //            string message = "No valid login found";
        //            MessagePrinter.PrintMessage(message);
        //            throw new Exception(message);
        //        }

        //        string data = "redirectUrl=https%3A%2F%2Ffschumacher.com%2F%3Fshow%3Dlogin&User=carolyn%40mahoneswallpapershop.com&Password=scvcok9va";
        //        PageRetriever.Headers.Clear();
        //        PageRetriever.Headers.Add("Cookie", extSett.Cookies);

        //        var html1 = PageRetriever.ReadFromServer("https://fschumacher.com/item/176502", false);

        //        if (html1.Contains("MY SCHUMACHER"))
        //        {
        //            MessagePrinter.PrintMessage("http://fschumacher.com - logged in success.");
        //            cc = PageRetriever.cc;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessagePrinter.PrintMessage(e.ToString(), ImportanceLevel.Critical);
        //        return false;
        //    }
        //}

        #endregion


        protected override bool Login()
        {
            try
            {
                var login = GetLoginInfo();
                if (login == null)
                {
                    string message = "No valid login found";
                    MessagePrinter.PrintMessage(message);
                    throw new Exception(message);
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string data = $"redirectUrl=https%3A%2F%2Fwww.fschumacher.com%2F&User={HttpUtility.UrlEncode(login.Login)}&Password={HttpUtility.UrlEncode(login.Password/*.Replace("!", "%21")*/)}".Replace("!", "%21");
                //string data = $"redirectUrl=&User={HttpUtility.UrlEncode(login.Login)}&Password={HttpUtility.UrlEncode(login.Password)}";
                // "redirectUrl=https%3A%2F%2Ffschumacher.com%2F%3Fshow%3Dlogin&User=carolyn%40mahoneswallpapershop.com&Password=scvcok9va";
                //PageRetriever.Headers.Clear();    
                //PageRetriever.Headers.Add("Cookie", extSett.Cookies);

                var pageRes = PageRetriever.WriteToServer("http://fschumacher.com/TradeAccount/Login", data, true);

                //var html1 = PageRetriever.ReadFromServer("https://fschumacher.com/item/176502", true);

                if (!pageRes.Contains("/Assets/images/HomePage/Icon_Profile.svg"))
                {
                    MessagePrinter.PrintMessage("http://fschumacher.com - logged in success.");
                    cc = PageRetriever.cc;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.ToString(), ImportanceLevel.Critical);
                return false;
            }
        }

        private void PartzillaHeaders(System.Net.HttpWebRequest arg1, System.Net.CookieContainer arg2)
        {
            arg1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299";
        }


        protected override void RealStartProcess()
        {
            List<string> categoriesList = new List<string>();

            string category1 = "http://fschumacher.com/catalog/Fabrics";
            string category2 = "http://fschumacher.com/catalog/Wallcoverings";
            string category3 = "http://fschumacher.com/catalog/Trim";

            categoriesList.Add(category1);
            categoriesList.Add(category2);
            categoriesList.Add(category3);

            foreach (string category in categoriesList)
            {
                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem { URL = category, ItemType = (int)ItemTypes.ProcessingCategoryPage });
                }
            }
            StartOrPushPropertiesThread();
        }

        protected void ProcessCaregoryPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                string departmentname = pqi.URL.Substring(pqi.URL.LastIndexOf('/') + 1);

                List<string> itemsList = new List<string>();

                //var items = htmlDoc.DocumentNode.SelectNodes("//div[@class='product-thumb']/a");
                var items = htmlDoc.DocumentNode.SelectNodes("//div[@class='product-info']");
                foreach (var element in items)
                {
                    ExtWareInfo wi = new ExtWareInfo();

                    //itemsList.Add("http://fschumacher.com" + element.AttributeOrNull("href"));
                    string productLink = "http://fschumacher.com" + element.SelectSingleNode(".//div/a").AttributeOrNull("href");

                    string productType = element.SelectSingleNode(".//div[@class='product-type']").InnerTextOrNull();
                    if (!string.IsNullOrEmpty(productType))
                        wi.Type = productType;
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem
                        {
                            URL = productLink,
                            ItemType = (int)ItemTypes.ProcessingProductPage,
                            Item = wi,
                        });
                    }
                }

                int totalPages = 0;
                string scriptWithPages = htmlDoc.DocumentNode.SelectSingleNode("//script[contains(text(),'totalPage')]").InnerTextOrNull();

                if (scriptWithPages != null)
                {
                    string countPages = scriptWithPages.Substring(scriptWithPages.IndexOf("totalPage"));
                    countPages = countPages.Substring(0, countPages.IndexOf(";")).Trim();
                    countPages = countPages.Substring(countPages.IndexOf("=") + 1).Trim();
                    if (countPages.IsNumeric())
                    {
                        totalPages = int.Parse(countPages);
                    }
                }

                MessagePrinter.PrintMessage($"Processing {totalPages} pages in group - {departmentname} ");

                for (int pageNumber = 2; pageNumber <= totalPages; pageNumber++)
                {
                    ItemForPost itemForPost = new ItemForPost
                    {
                        CookieContainer = PageRetriever.cc,
                        Departmentname = departmentname,
                        Page = pageNumber,
                    };

                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem
                        {
                            ItemType = (int)ItemTypes.ProcessingPostRequest,
                            Item = itemForPost
                        });
                    }
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message}  in this Category - {pqi.URL}");
            }

            MessagePrinter.PrintMessage($"Category Processed!");
            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        private void ProcessPostRequest(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            var item = (ItemForPost)pqi.Item;

            try
            {
                //var htmlPost = PostHelper.GetPage(item.Page, item.Departmentname, PageRetriever.cc);
                string strFormat =
                    $"pagenumber={item.Page}&departmentname={item.Departmentname}&rowperpage=30&sortby=Popularity&pricefrom=0&priceto=300";

                Thread.Sleep(100);
                var htmlPost = PageRetriever.WriteToServer("http://fschumacher.com/Catalog/GetProducts", strFormat, true);

                var htmlDoc1 = new HtmlDocument();
                htmlPost = HttpUtility.HtmlDecode(htmlPost);
                htmlDoc1.LoadHtml(htmlPost);

                var items1 = htmlDoc1.DocumentNode.SelectNodes("//div[@class='product-info']");

                if (items1 != null)
                {
                    foreach (var element in items1)
                    {
                        ExtWareInfo wi = new ExtWareInfo();

                        string productLink = "http://fschumacher.com" + element.SelectSingleNode(".//div/a").AttributeOrNull("href");

                        string productType = element.SelectSingleNode(".//div[@class='product-type']").InnerTextOrNull();
                        if (!string.IsNullOrEmpty(productType))
                            wi.Type = productType;

                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem
                            {
                                URL = productLink,
                                ItemType = (int)ItemTypes.ProcessingProductPage,
                                Item = wi,
                            });
                        }
                    }
                    pqi.Processed = true;
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} when try recieve page info");
                pqi.Processed = false;
                Thread.Sleep(500);
            }

            MessagePrinter.PrintMessage($"Page with products processed");
            StartOrPushPropertiesThread();
        }

        private void ProcessProductPage(ProcessQueueItem pqi)
        {
            int abc = 0;

            if (cancel)
                return;

            //pqi.URL = "https://fschumacher.com/item/5005880";
            //pqi.URL = "http://fschumacher.com/item/C9000-20";

            //if (abc == 2)
            try
            {
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                ExtWareInfo wi = (ExtWareInfo)pqi.Item;

                wi.URL = pqi.URL;

                HtmlNodeCollection imagesCollection = htmlDoc.DocumentNode.SelectNodes("//a[@data-zoom-image]");
                if (imagesCollection != null)
                {
                    string images = string.Empty;
                    foreach (HtmlNode htmlNode in imagesCollection)
                    {
                        images += $"{htmlNode.AttributeOrNull("data-zoom-image").Trim()},";
                    }

                    if (!string.IsNullOrEmpty(images))
                        wi.Images = images.Trim(',');
                }

                string minimumOrderInfo = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(),'has a minimum order')]").InnerTextOrNull();
                if (!string.IsNullOrEmpty(minimumOrderInfo))
                {
                    wi.MinimumOrderInfo = minimumOrderInfo;
                }

                HtmlNodeCollection categories = htmlDoc.DocumentNode.SelectNodes("//i[@class='fa fa-chevron-right']");
                if (categories != null)
                {
                    if (categories.Count == 3)
                    {
                        try
                        {
                            wi.Collection = categories[2].PreviousSibling.InnerTextOrNull();
                        }
                        catch { }
                    }
                }

                string sku = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='sku']").AttributeOrNull("value");
                if (!string.IsNullOrEmpty(sku))
                    wi.SKU = $"SCH-{sku}";

                string productTitle = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='large-12 columns font-title-28 fontHandle']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(productTitle))
                    wi.ProductTitle = productTitle;

                try
                {
                    var color = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(text(),'COLOR:')]").NextSibling.InnerTextOrNull();
                    if (!string.IsNullOrEmpty(color))
                        wi.Color = color;
                }
                catch (Exception e) { }


                if (sku != null)
                    wi.ImageUrl = $"http://fschumacher.com/s3/schumacher-webassets/{sku}.jpg";

                #region Item Details

                //ItemDetails
                //var itemsDetails = htmlDoc.DocumentNode.SelectNodes("//div[@class='div-itemdetails']/div");
                //*[@id="productDetails"]/div[4]/div[1]/div[2]/div[2]/div
                string horisontalRepeat = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[4]/div[1]/div[3]/div[1]/div/text()").InnerTextOrNull();
                if (!string.IsNullOrEmpty(horisontalRepeat))
                    wi.HorizontalRepeat = horisontalRepeat;

                string match = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[4]/div[1]/div[5]/div[1]/div/text()").InnerTextOrNull();
                if (!string.IsNullOrEmpty(match))
                    wi.Match = match;

                string verticalRepeat = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[4]/div[1]/div[4]/div[1]/div/text()").InnerTextOrNull();
                if (!string.IsNullOrEmpty(verticalRepeat))
                    wi.VerticelRepeat = verticalRepeat;

                string content = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[4]/div[1]/div[2]/div[2]/div/text()").InnerTextOrNull();
                if (!string.IsNullOrEmpty(content))
                    wi.Content = content;

                string width = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[4]/div[1]/div[2]/div[1]/div/text()").InnerTextOrNull();
                if (!string.IsNullOrEmpty(width))
                    wi.Width = width;
                //*[@id="productDetails"]/div[4]/div[1]/div[5]/div[2]/div
                string performance = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[4]/div[1]/div[3]/div[2]/div/text()").InnerTextOrNull();
                if (!string.IsNullOrEmpty(performance))
                    wi.Performance = performance;

                string countryOfFinish = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[4]/div[1]/div[5]/div[2]/div/text()").InnerTextOrNull();
                if (!string.IsNullOrEmpty(countryOfFinish))
                    wi.CountryOfFinish = countryOfFinish;
                //*[@id="productDetails"]/div[3]/div[3]/div[5]/div/div[1]/div[1]/div/div/text()[2]
                string cutAndSoldByThe = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='productDetails']/div[3]/div[3]/div[5]/div/div[1]/div[1]/div/div/text()[2]").InnerTextOrNull();
                if (!string.IsNullOrEmpty(cutAndSoldByThe))
                    wi.CutAndSoldByThe = cutAndSoldByThe;

                var cost = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(),'USD')]").InnerTextOrNull();
                if (!string.IsNullOrEmpty(cost))
                {
                    if (cost.Contains(" USD"))
                    {
                        string subCost = cost.Substring(0, cost.IndexOf("\n"));
                        wi.PricedByThe = cost.Substring(cost.IndexOf("USD") + 3, cost.IndexOf("\n"));
                        wi.CostInfo = subCost;
                    }
                }

                #endregion

                #region Item Quantity Info

                string dyelotInfo = "";

                //var stockInfo = htmlDoc.DocumentNode.SelectNodes("//span[contains(text(),'DYELOT')]");
                //if (stockInfo != null)
                //{
                //    foreach (HtmlNode htmlNode in stockInfo)
                //    {
                //        dyelotInfo.Add(htmlNode.InnerTextOrNull());
                //    }
                //}

                try
                {
                    var html1 = PageRetriever.ReadFromServer($"http://fschumacher.com/Inventory/QuickInventoryView?sku={sku}", true);
                    var htmlDoc1 = new HtmlDocument();
                    html1 = HttpUtility.HtmlDecode(html1);
                    htmlDoc1.LoadHtml(html1);

                    //var stockInfo = htmlDoc1.DocumentNode.SelectSingleNode("//div[contains(text(),'DYELOT')]");
                    //if (stockInfo != null)
                    //{
                    //    dyelotInfo = stockInfo.InnerTextOrNull();
                    //    double quantityAvailable = 0;
                    //    string quantityAvailableStr = dyelotInfo.Substring(0, dyelotInfo.IndexOf(" "));

                    //    if (quantityAvailableStr.IsNumeric())
                    //    {
                    //        quantityAvailable += ParseDouble(quantityAvailableStr);
                    //    }

                    //    if (quantityAvailable != 0)
                    //        wi.QuantityAvailable = (int)quantityAvailable;
                    //}

                    var stockInfos = htmlDoc1.DocumentNode.SelectNodes("//div[@class = 'large-5 medium-5 small-5 columns dyelottile']");
                    if (stockInfos != null)
                    {
                        double quantityAvailable = 0;
                        foreach (var stockInfo in stockInfos)
                        {
                            var stock = stockInfo.InnerTextOrNull();
                            var spStock = stock.Split();
                            var yards = spStock[0];

                            if (yards.IsNumeric())
                            {
                                quantityAvailable += ParseDouble(yards);
                            }
                        }

                        if (quantityAvailable != 0)
                            wi.QuantityAvailable = (int)quantityAvailable;
                    }
                }
                catch (Exception e)
                {

                }

                #endregion

                #region Product Additional Colors


                //UrlForPost http://fschumacher.com/item/itemcontentcolors?itemid=164827

                var itemIdForAdditionalColors = htmlDoc.DocumentNode.SelectNodes("//a[@class='GAdata_ProductDetail' and @data-current-tag='Colorway_Select']");
                if (itemIdForAdditionalColors != null)
                {
                    List<string> urlsList = new List<string>();
                    foreach (HtmlNode node in itemIdForAdditionalColors)
                    {
                        urlsList.Add(node.AttributeOrNull("href").Replace("/item/", "SCH-"));
                    }

                    if (urlsList.Count > 0)
                    {
                        urlsList = urlsList.Distinct().ToList();
                    }


                    for (int i = 0; i < urlsList.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                {
                                    wi.Add1 = urlsList[i];
                                    break;
                                }
                            case 1:
                                {
                                    wi.Add2 = urlsList[i];
                                    break;
                                }
                            case 2:
                                {
                                    wi.Add3 = urlsList[i];
                                    break;
                                }
                            case 3:
                                {
                                    wi.Add4 = urlsList[i];
                                    break;
                                }
                            case 4:
                                {
                                    wi.Add5 = urlsList[i];
                                    break;
                                }
                            case 5:
                                {
                                    wi.Add6 = urlsList[i];
                                    break;
                                }
                            case 6:
                                {
                                    wi.Add7 = urlsList[i];
                                    break;
                                }
                        }
                    }
                }

                #endregion

                AddWareInfo((wi));
                OnItemLoaded(wi);
            }
            catch (Exception e)
            {
                //MessagePrinter.PrintMessage(e.Message, ImportanceLevel.High);
                MessagePrinter.PrintMessage($"{e.Message} on this Url - {pqi.URL}", ImportanceLevel.High);
            }

            MessagePrinter.PrintMessage($"product processed!");
            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case (int)ItemTypes.ProcessingCategoryPage:
                    act = ProcessCaregoryPage;
                    break;
                case (int)ItemTypes.ProcessingPostRequest:
                    act = ProcessPostRequest;
                    break;
                case (int)ItemTypes.ProcessingProductPage:
                    act = ProcessProductPage;
                    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }

        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
    }
}
