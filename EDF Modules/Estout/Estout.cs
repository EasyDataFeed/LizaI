#region using

using System;
using System.Collections.Generic;
using System.Web;
using HtmlAgilityPack;
using Estout;
using Databox.Libs.Estout;
using Estout.Enums;
using Estout.Extensions;

#endregion

namespace WheelsScraper
{
    public class Estout : BaseScraper
    {
        private const string MAIN_PAGE = "https://www.estout.com";

        public Estout()
        {
            Name = "Estout";
            Url = "https://www.estout.com/search/results?page=1";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;
            PageRetriever.AfterSetHeaders += PageRetriever_AfterSetHeaders;
            SpecialSettings = new ExtSettings();
        }

        private void PageRetriever_AfterSetHeaders(System.Net.HttpWebRequest arg1, System.Net.CookieContainer arg2)
        {
            arg1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36 OPR/48.0.2685.35";
        }

        #region Standart Methods

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

        //public override System.Windows.Forms.Control SettingsTab
        //{
        //	get
        //	{
        //		var frm = new ucExtSettings();
        //		frm.Sett = Settings;
        //		return frm;
        //	}
        //}

        public override WareInfo WareInfoType
        {
            get
            {
                return new ExtWareInfo();
            }
        }

        protected override bool Login()
        {
            var login = GetLoginInfo();
            if (login == null)
            {
                string message = "No valid login found";
                MessagePrinter.PrintMessage(message);
                throw new Exception(message);
            }

            string data = $"un={login.Login}&pw={login.Password}";
            string actionUrl = "https://www.estout.com/checklogin?ref=";

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var html = PageRetriever.WriteToServer(actionUrl, data, true);
            var doc = CreateDoc(html);
            doc.LoadHtml(html);

            string logout = doc.DocumentNode.SelectSingleNode("//a[contains(text(),'LOGOUT')]").InnerTextOrNull();
            if (string.IsNullOrEmpty(logout))
            {
                return false;
            }

            return true;
        }

        #endregion    

        protected override void RealStartProcess()
        {
            lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = (int)ItemType.ProcessingGroupPage });
            StartOrPushPropertiesThread();
        }

        protected void ProcessGroupPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            try
            {
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                HtmlNodeCollection products = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'col-xs-6 col-md-4 col-xl-4']/div/a");
                if (products != null)
                {
                    foreach (var item in products)
                    {
                        string productLink = item.AttributeOrNull("href").TrimStart('.');
                        string link = $"https://www.estout.com{productLink}";

                        var pqi1 = new ProcessQueueItem();
                        pqi1.URL = link;
                        pqi1.ItemType = (int)ItemType.ProcessingProductPage;
                        lock (this)
                        {
                            lstProcessQueue.Add(pqi1);
                        }
                    }
                }

                if (pqi.URL == "https://www.estout.com/search/results?page=1")
                {
                    var nextPage = htmlDoc.DocumentNode.SelectSingleNode("//html[@lang = 'en']/head/title");
                    if (nextPage != null)
                    {
                        var countPages = nextPage.InnerTextOrNull();
                        var allPages = countPages.Split(' ');
                        var firstPage = allPages.Length > 2 ? allPages[3] : string.Empty;
                        var lastPage = allPages.Length > 4 ? allPages[5] : string.Empty;
                        int first = Int32.Parse(firstPage);
                        int last = Int32.Parse(lastPage);
                        List<string> nextLink = new List<string>();

                        for (int page = first + 1; page <= last; page++)
                        {
                            nextLink.Add(pqi.URL.Replace($"{first}", $"{page}"));
                        }

                        foreach (var link in nextLink)
                        {
                            var pqi1 = new ProcessQueueItem();
                            pqi1.URL = link;
                            pqi1.ItemType = (int)ItemType.ProcessingGroupPage;
                            lock (this)
                            {
                                lstProcessQueue.Add(pqi1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this URL: {pqi.URL}", ImportanceLevel.Critical);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("All Brands found");
            StartOrPushPropertiesThread();
        }

        private void ProcessProductPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                //pqi.URL = "https://www.estout.com/details?name=Augusto-2-Charcoal&sku=AUGU-2";

                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                ExtWareInfo wi = new ExtWareInfo();

                string productTitle = htmlDoc.DocumentNode.SelectSingleNode("//p[@class='desc']/strong").InnerTextOrNull();
                if (!string.IsNullOrEmpty(productTitle))
                    wi.ProductTitle = productTitle;
                else
                    productTitle = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='desc']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(productTitle))
                    wi.ProductTitle = productTitle.Substring(0, productTitle.IndexOf(" "));

                string colorName = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='color-name']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(colorName))
                    wi.ColorName = colorName;
                else if (!string.IsNullOrEmpty(productTitle))
                {
                    colorName = productTitle.Substring(productTitle.IndexOf(" "));
                    if (!string.IsNullOrEmpty(colorName))
                        wi.ColorName = colorName;
                }

                string itemNumber = htmlDoc.DocumentNode.SelectSingleNode("//p[contains(text(),'ITEM')]").InnerTextOrNull();
                if (!string.IsNullOrEmpty(itemNumber))
                    wi.ItemNumber = itemNumber.Replace("ITEM #:", string.Empty).Trim();

                string quantityStr = htmlDoc.DocumentNode.SelectSingleNode("//div[@style='font-size:1.3em']/span").InnerTextOrNull();
                if (!string.IsNullOrEmpty(quantityStr))
                {
                    quantityStr = quantityStr.Replace("Yards", string.Empty).Trim();
                    double quontity = ParseDouble(quantityStr);
                    wi.Quantity = (int)quontity;
                }

                string price = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='showprice']").AttributeOrNull("p");
                if (!string.IsNullOrEmpty(price))
                    wi.Price = ParseDouble(price);

                string status = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='text-success']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(status))
                    wi.Status = status;

                string generalImage = htmlDoc.DocumentNode.SelectSingleNode("//img[@id='main_img']").AttributeOrNull("lg");
                if (!string.IsNullOrEmpty(generalImage))
                    wi.GeneralImage = MAIN_PAGE + generalImage;

                wi.URL = pqi.URL;

                string contentHorizontalRepeat = string.Empty;
                string contentVerticalRepeat = string.Empty;

                HtmlNodeCollection specCollection = htmlDoc.DocumentNode.SelectNodes("//table[@class = 'table table-striped specs']/tbody/tr"/*"//div[@id='specs']/div"*/);
                if (specCollection != null)
                {
                    foreach (HtmlNode node in specCollection)
                    {

                        #region Collection

                        // var collection =htmlDoc.DocumentNode.SelectSingleNode("//div[@class='spec-tag' and contains(text(),'Collection')]");
                        //var collectionHeader = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(),'Collection')]");
                        var collectionHeader = node.SelectSingleNode(".//td[1][contains(text(),'Collection')]"/*".//div[contains(text(),'Collection')]"*/);
                        if (collectionHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']/div/a");
                            if (contentBodyCollection != null)
                            {
                                string contentCollection = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentCollection += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentCollection))
                                    wi.Collection = contentCollection.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Overal Width

                        var overalWidthHeader = node.SelectSingleNode(".//td[1][contains(text(),'Overall Width')]"/*".//div[contains(text(),'Overall Width')]"*/);
                        if (overalWidthHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']");
                            if (contentBodyCollection != null)
                            {
                                string contentOverallWidth = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentOverallWidth += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentOverallWidth))
                                    wi.OverallWidth = contentOverallWidth.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region OtherColor

                        var otherColor = node.SelectSingleNode(".//td[1][contains(text(),'Other Colors')]"/*".//div[contains(text(),'Other Colors')]"*/);
                        if (otherColor != null)
                        {
                            HtmlNode contentBody = node.SelectSingleNode(".//td[2][@class='value']/div/a");
                            if (contentBody != null)
                            {
                                string colorUrl = MAIN_PAGE + "/" + contentBody.AttributeOrNull("href");

                                var html1 = PageRetriever.ReadFromServer(colorUrl, true);
                                var htmlDoc1 = new HtmlDocument();
                                html1 = HttpUtility.HtmlDecode(html1);
                                htmlDoc1.LoadHtml(html1);

                                HtmlNodeCollection colorCollection = htmlDoc1.DocumentNode.SelectNodes("//div[@class='r-desc']");
                                if (colorCollection != null)
                                {
                                    int counterColor = 1;
                                    foreach (HtmlNode htmlNode in colorCollection)
                                    {
                                        if (counterColor == 1)
                                        {
                                            counterColor++;
                                            wi.Color1 = htmlNode.InnerTextOrNull();
                                        }
                                        else if (counterColor == 2)
                                        {
                                            counterColor++;
                                            wi.Color2 = htmlNode.InnerTextOrNull();
                                        }
                                        else if (counterColor == 3)
                                        {
                                            counterColor++;
                                            wi.Color3 = htmlNode.InnerTextOrNull();
                                        }
                                        else if (counterColor == 4)
                                        {
                                            counterColor++;
                                            wi.Color4 = htmlNode.InnerTextOrNull();
                                        }
                                        else if (counterColor == 5)
                                        {
                                            counterColor++;
                                            wi.Color5 = htmlNode.InnerTextOrNull();
                                        }
                                    }
                                }
                                continue;
                            }
                        }

                        #endregion

                        #region Usable Width

                        var usableWidthHeader = node.SelectSingleNode(".//td[1][contains(text(),'Usable Width')]");
                        if (usableWidthHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']");
                            if (contentBodyCollection != null)
                            {
                                string contentUsableWidth = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentUsableWidth += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentUsableWidth))
                                    wi.UsableWidth = contentUsableWidth.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Railroaded

                        var railroadedHeader = node.SelectSingleNode(".//td[1][contains(text(),'Railroaded')]");
                        if (railroadedHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']");
                            if (contentBodyCollection != null)
                            {
                                string contentRailroaded = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentRailroaded += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentRailroaded))
                                    wi.Railroaded = contentRailroaded.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Repeat

                        var repeatHeader = node.SelectSingleNode(".//td[1][contains(text(),'Repeat')]");
                        if (repeatHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']");
                            if (contentBodyCollection != null)
                            {
                                if (repeatHeader.InnerTextOrNull() == "Horizontal Repeat")
                                {
                                    foreach (HtmlNode htmlNode in contentBodyCollection)
                                    {
                                        contentHorizontalRepeat += $"{htmlNode.InnerTextOrNull()} (h)";
                                    }
                                }

                                if (repeatHeader.InnerTextOrNull() == "Vertical Repeat")
                                {
                                    foreach (HtmlNode htmlNode in contentBodyCollection)
                                    {
                                        contentVerticalRepeat += $"{htmlNode.InnerTextOrNull()} (v)";
                                    }
                                }

                                if (!string.IsNullOrEmpty(contentHorizontalRepeat) || !string.IsNullOrEmpty(contentVerticalRepeat))
                                    wi.Repeat = $"{contentVerticalRepeat} / {contentHorizontalRepeat}";

                                continue;
                            }
                        }

                        #endregion

                        #region Content

                        var contentHeader = node.SelectSingleNode(".//td[1][contains(text(),'Content')]");
                        if (contentHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']/div");
                            if (contentBodyCollection != null)
                            {
                                string contentBody = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentBody += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentBody))
                                    wi.Content = contentBody.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Tests

                        var testsHeader = node.SelectSingleNode(".//td[1][contains(text(),'Tests')]");
                        if (testsHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']/div");
                            if (contentBodyCollection != null)
                            {
                                string contentTests = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentTests += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentTests))
                                    wi.Tests = contentTests.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Clean

                        var cleanHeader = node.SelectSingleNode(".//td[1][contains(text(),'Clean')]");
                        if (cleanHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']/div");
                            if (contentBodyCollection != null)
                            {
                                string contentClean = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentClean += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentClean))
                                    wi.Clean = contentClean.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Finish

                        var finishHeader = node.SelectSingleNode(".//td[1][contains(text(),'Finish')]");
                        if (finishHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']/div");
                            if (contentBodyCollection != null)
                            {
                                string contentFinish = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentFinish += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentFinish))
                                    wi.Finish = contentFinish.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Origin

                        var originHeader = node.SelectSingleNode(".//td[1][contains(text(),'Origin')]");
                        if (originHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']");
                            if (contentBodyCollection != null)
                            {
                                string contentOrigin = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentOrigin += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentOrigin))
                                    wi.Origin = contentOrigin.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Brand

                        var brandHeader = node.SelectSingleNode(".//td[1][contains(text(),'Brand')]");
                        if (brandHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']");
                            if (contentBodyCollection != null)
                            {
                                string contentBrand = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentBrand += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentBrand))
                                    wi.Brand = contentBrand.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                        #region Backorder Lead Time

                        var backorderLeadTimeHeader = node.SelectSingleNode(".//td[1][contains(text(),'Backorder Lead')]");
                        if (backorderLeadTimeHeader != null)
                        {
                            HtmlNodeCollection contentBodyCollection = node.SelectNodes(".//td[2][@class='value']");
                            if (contentBodyCollection != null)
                            {
                                string contentBackorderLeadTime = string.Empty;
                                foreach (HtmlNode htmlNode in contentBodyCollection)
                                {
                                    contentBackorderLeadTime += htmlNode.InnerTextOrNull() + ",";
                                }
                                if (!string.IsNullOrEmpty(contentBackorderLeadTime))
                                    wi.BackorderLeadTime = contentBackorderLeadTime.Trim(',');

                                continue;
                            }
                        }

                        #endregion

                    }
                }

                AddWareInfo(wi);
                OnItemLoaded(wi);
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this URL: {pqi.URL}", ImportanceLevel.High);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Product processed");
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case (int)ItemType.ProcessingGroupPage:
                    act = ProcessGroupPage;
                    break;
                //case (int)ItemType.ProcessingBrand:
                //    act = ProcessBrand;
                //    break;
                //case (int)ItemType.ProcessingGridPage:
                //    act = ProcessGridPage;
                //    break;
                case (int)ItemType.ProcessingProductPage:
                    act = ProcessProductPage;
                    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }
    }
}
