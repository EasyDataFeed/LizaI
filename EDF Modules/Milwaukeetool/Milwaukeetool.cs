using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Milwaukeetool;
using Databox.Libs.Milwaukeetool;
using System.Net;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Interactions;
using Milwaukeetool.Extensions;

namespace WheelsScraper
{
    public class Milwaukeetool : BaseScraper
    {
        private const int PageWaitSleep = 6000;
        private const int PageScrollSleep = 1500;
        private const int PageSendKeysSleep = 1500;
        private const int PartNumberLength = 50;
        private const int AnchorTextLength = 100;
        private const int BulletPointsLength = 1000;
        private List<string> NotProductPages { get; set; }
        private List<string> ExceptionPages { get; set; }
        private List<string> Categories { get; set; }
        private List<string> Products { get; set; }
        private List<string> Links { get; set; }
        public Milwaukeetool()
        {
            Name = "Milwaukeetool";
            Url = "https://www.Milwaukeetool.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Links = new List<string>();
            Wares.Clear();
            BrandItemType = 2;

            Complete += Milwaukeetool_Complete;

            SpecialSettings = new ExtSettings();
        }

        private void Milwaukeetool_Complete(object sender, EventArgs e)
        {
            if (Wares.Count > 0)
            {
                List<ExtWareInfo> newWares = new List<ExtWareInfo>();

                foreach (var ware in Wares)
                {
                    newWares.Add((ExtWareInfo)ware);
                }

                newWares = newWares.Distinct(ExtWareInfo.MilwaukeetoolPartNumberComparer).ToList();

                Wares.Clear();

                foreach (var newWare in newWares)
                {
                    Wares.Add(newWare);
                }
            }
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
            return true;
        }

        public static void CheckServerError(IWebDriver driver)
        {
            var answerHtmlDoc = GetHtmlDocument(driver);
            var servErrorNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'Server Error')]");
            if (servErrorNode != null)
            {
                string error = servErrorNode.InnerText;
                if (error == "Server Error")
                {

                }
            }

            var connectionErrorNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'There is no Internet connection')]");
            if (connectionErrorNode != null)
            {
                string error = connectionErrorNode.InnerText;
                if (error == "There is no Internet connection")
                {

                }
            }
        }

        private static bool LengthChanged(long oldLength, long newLength)
        {
            if (oldLength == newLength)
                return false;

            return true;
        }

        private static void ScrollHtmlDocument(IWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("javascript:window.scrollBy(0,10000)");
            Thread.Sleep(PageScrollSleep);
            js.ExecuteScript("javascript:window.scrollBy(0,3000)");
            Thread.Sleep(PageScrollSleep);
            js.ExecuteScript("javascript:window.scrollBy(0,3000)");
            Thread.Sleep(PageScrollSleep);

            CheckCollapsed(driver);
        }

        private static HtmlAgilityPack.HtmlDocument GetHtmlDocument(IWebDriver driver)
        {
            var answerHtml = driver.PageSource;
            var answerHtmlDoc = new HtmlAgilityPack.HtmlDocument();
            answerHtml = HttpUtility.HtmlDecode(answerHtml);
            answerHtmlDoc.LoadHtml(answerHtml);

            return answerHtmlDoc;
        }

        private static void CheckCollapsed(IWebDriver driver)
        {
            HtmlAgilityPack.HtmlDocument answerHtmlDoc = GetHtmlDocument(driver);
            
            HtmlNode collapsedAnwersNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//span[contains(text(),'Answers Collapsed')]");
            if (collapsedAnwersNode != null)
            {
                string id = collapsedAnwersNode.AttributeOrNull("id");
                IWebElement collapsedAnswersElement = driver.FindElement(By.Id(id));

                if (collapsedAnswersElement.Displayed)
                    collapsedAnswersElement.Click();
                
                Thread.Sleep(PageWaitSleep);
            }
            else
            {
                HtmlNode viewMoreNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//div[contains(text(),'View More')]");
                if (viewMoreNode != null)
                {
                    string id = viewMoreNode.AttributeOrNull("id");
                    IWebElement viewMoreElement = driver.FindElement(By.Id(id));

                    if (viewMoreElement.Displayed)
                        viewMoreElement.Click();
                    
                    Thread.Sleep(PageWaitSleep);
                }
                else
                {
                    string endPageError = "END PAGE ERROR!";
                }
            }
        }

        private List<string> LoadLinksSite()
        {
            List<string> links = new List<string>();

            using (IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
            {
                foreach (var categoryLink in Categories)
                {
                    try
                    {
                        bool endPage = false;

                        var productLink = categoryLink;
                        Thread.Sleep(PageWaitSleep);
                        driver.Navigate().GoToUrl(productLink);
                        Thread.Sleep(PageWaitSleep);

                        while (!endPage)
                        {
                            Actions actions = new Actions(driver);
                            var htmlDoc = GetHtmlDocument(driver);

                            var products = htmlDoc.DocumentNode.SelectNodes("//div[@class='coveo-result-list-container coveo-card-layout-container']/div/div/a");
                            if (products != null)
                            {
                                foreach (var product in products)
                                {
                                    string link;
                                    link = product.AttributeOrNull("href");

                                    links.Add(link);
                                }
                            }

                            var nextClick = htmlDoc.DocumentNode.SelectSingleNode("//li[@class='coveo-pager-next coveo-pager-anchor coveo-pager-list-item coveo-accessible-button']");
                            if (nextClick != null)
                            {
                                IWebElement nextPage = driver.FindElement(By.XPath(nextClick.XPath));
                                nextPage.Click();
                                Thread.Sleep(PageWaitSleep);
                                endPage = false;
                            }
                            else
                            {
                                endPage = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessagePrinter.PrintMessage("Category processed.");
                    }

                }

                driver.Quit();
                Thread.Sleep(3000);
            }

            return links;
        }

        private List<string> LoadLinksSiteMap()
        {
            List<string> linksSiteMap = new List<string>();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var html = PageRetriever.ReadFromServer("https://milwaukeetool.com/sitemap.xml", true);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(html);

            var xmlLink = xmlDoc.GetElementsByTagName("loc");
            foreach (XmlNode o in xmlLink)
            {
                linksSiteMap.Add(o.InnerText);
            }

            return linksSiteMap;
        }

        protected override void RealStartProcess()
        {
            NotProductPages = new List<string>();
            ExceptionPages = new List<string>();
            Categories = new List<string>();

            string html = PageRetriever.ReadFromServer("https://milwaukeetool.com", true);
            html = HttpUtility.HtmlDecode(html);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var collectionCategories = htmlDoc.DocumentNode.SelectNodes("//div[@id='products-subnav']/div[1]/div/a");
            if (collectionCategories != null)
            {
                foreach (var item in collectionCategories)
                {
                    string categoryLink;
                    categoryLink = "https://milwaukeetool.com" + item.AttributeOrNull("href");

                    if (categoryLink.Contains("/Products"))
                    {
                        Categories.Add(categoryLink);
                    }
                }
            }

            var links = LoadLinksSite();

            //links.Add("https://milwaukeetool.com/Products/Safety-Solutions/Personal-Protective-Equipment/Hard-Hats/Front%20Brim%20Hard%20Hat%20with%20BOLT%20Accessory%20System");
            //links.Add("https://milwaukeetool.com/Products/Safety-Solutions/Personal-Protective-Equipment/High-Visibility-Safety-Vests/High%20Visibility%20Performance%20Safety%20Vests");
            //links.Add("https://milwaukeetool.com/Products/Safety-Solutions/Tool-Lanyards/48-22-8855");

            foreach (string link in links)
            {
                if (!link.Contains("Products"))
                {
                    continue;
                }

                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItemExt()
                    {
                        ItemType = 10,
                        URL = link
                    });
                }
            }

            var linksSiteMap = LoadLinksSiteMap();

            foreach (string linkSiteMap in linksSiteMap)
            {
                if (!linkSiteMap.Contains("Products"))
                {
                    continue;
                }

                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem()
                    {
                        ItemType = 10,
                        URL = linkSiteMap,
                        Name = "siteMap"
                    });
                }
            }

            StartOrPushPropertiesThread();
        }

        protected void ProcessBrandsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            try
            {
                //pqi.URL = "https://milwaukeetool.com/Products/Safety-Solutions/Personal-Protective-Equipment/Hard-Hats/Front%20Brim%20Hard%20Hat%20with%20BOLT%20Accessory%20System";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var wi = new ExtWareInfo();
                string sku = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='product-info__ratings']").AttributeOrNull("data-bv-product-id");
                if (!string.IsNullOrEmpty(sku))
                {
                    var linksOp = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'variant-filter-data hidden']/script");
                    if (linksOp != null)
                    {
                        string title = string.Empty;
                        var productTitle = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class = 'product-info__title']");
                        if (productTitle != null)
                        {
                            title = productTitle.InnerTextOrNull();
                        }

                        var linkSpl = linksOp.InnerTextOrNull().Split(',');
                        string products = string.Empty;

                        foreach (var link in linkSpl)
                        {
                            string options = string.Empty;
                            string linkProduct = string.Empty;

                            if (link.Contains(":"))
                            {
                                var linkSpl2 = link.Split(':');

                                if (linkSpl2.Length == 2)
                                {
                                    string optionsName = linkSpl2.Length > 0 ? linkSpl2[0].Replace("{", "")
                                                                                       .Replace("\\", "")
                                                                                       .Replace("\"", "")
                                                                                       .Replace("}", "")
                                                                                       .Replace(";", "")
                                                                                       .Replace(")", "")
                                                                                       .Replace("\n", "") : string.Empty;

                                    string optionsValue = linkSpl2.Length > 0 ? linkSpl2[1].Replace("{", "")
                                                                                       .Replace("\\", "")
                                                                                       .Replace("\"", "")
                                                                                       .Replace("}", "")
                                                                                       .Replace(";", "")
                                                                                       .Replace(")", "")
                                                                                       .Replace("\n", "") : string.Empty;
                                    options = $"^{optionsName}~{optionsValue}";
                                }

                                if (linkSpl2.Length == 3)
                                {
                                    string optionsLink = linkSpl2.Length > 0 ? linkSpl2[2].Replace("{", "")
                                                                                       .Replace("\\", "")
                                                                                       .Replace("\"", "")
                                                                                       .Replace("}", "")
                                                                                       .Replace(";", "")
                                                                                       .Replace(")", "")
                                                                                       .Replace("\n", "") : string.Empty;

                                    linkProduct = $",https://milwaukeetool.com{optionsLink}#";
                                }

                                products += $"{linkProduct}{options}";
                            }
                        }
                        products = products.Trim(',').Trim(' ');
                        var prodSpl = products.Split(',');

                        foreach (var item in prodSpl)
                        {
                            var opSpl = item.Split('#');
                            var url = opSpl.Length > 0 ? opSpl[0] : string.Empty;
                            var option = opSpl.Length > 0 ? opSpl[1] : string.Empty;

                            if (url != null)
                            {
                                lock (this)
                                {
                                    lstProcessQueue.Add(new ProcessQueueItemExt()
                                    {
                                        ItemType = 10,
                                        URL = url,
                                        Item = option,
                                        Name = "from item with options",
                                        ProductUrl = pqi.URL
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        wi.URL = pqi.URL;
                        wi.PartNumber = sku.Truncate(PartNumberLength);
                        wi.Action = "add";
                        wi.ProductType = "1";
                        if (pqi.Name == "from item with options")
                            wi.ProductUrl = ((ProcessQueueItemExt)pqi).ProductUrl;

                        if (string.IsNullOrEmpty(pqi.Name))
                        {
                            var links = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'tabs']/a");
                            if (links != null)
                            {
                                foreach (var link in links)
                                {
                                    string link2 = "https://milwaukeetool.com" + link.AttributeOrNull("href");

                                    if (link2 != pqi.URL)
                                    {
                                        lock (this)
                                        {
                                            lstProcessQueue.Add(new ProcessQueueItem()
                                            {
                                                ItemType = 10,
                                                URL = link2,
                                                Name = "from item"
                                            });
                                        }
                                    }
                                }
                            }
                        }

                        var productTitle = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class = 'product-info__title']");
                        if (productTitle != null)
                        {
                            wi.ProductTitle = $"Milwaukeetool {sku} {productTitle.InnerTextOrNull().Replace("0x", "x0").Replace("0 x", "x 0").Replace("®", "В®").Replace("°", "В°").Replace("вЂќ", "\"").Replace("вЂ™", "'").Replace("Р'", "").Replace(" &", "and")}";
                            wi.AnchorText = $"{sku} {productTitle.InnerTextOrNull().Replace("0x", "x0").Replace("0 x", "x 0").Replace("&", "and").Replace("®", "В®").Replace("°", "В°").Replace("вЂќ", "\"").Replace("вЂ™", "'").Truncate(AnchorTextLength)}";
                        }

                        var productDescription =
                            htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'product-info__overview readmore']/p");
                        if (productDescription != null)
                        {
                            wi.ProductDescription = productDescription.InnerTextOrNull().Replace("0x", "x0")
                                                                                        .Replace("0 x", "x 0")
                                                                                        .Replace("в„ў", "&trade;")
                                                                                        .Replace("®", "&reg;")
                                                                                        .Replace("°", "&deg;")
                                                                                        .Replace("вЂќ", "\"")
                                                                                        .Replace("вЂ™", "'");
                        }

                        string metaDescription = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']")
                            .AttributeOrNull("content");
                        if (!string.IsNullOrEmpty(metaDescription))
                        {
                            wi.METADescription = metaDescription.Replace("0x", "x0").Replace("0 x", "x 0").Replace("вЂќ", "\"").Replace("вЂ™", "'");
                        }
                        else
                        {
                            wi.METADescription = wi.ProductTitle;
                        }

                        string metaKeywords = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='keywords']")
                            .AttributeOrNull("content");
                        if (!string.IsNullOrEmpty(metaKeywords))
                        {
                            wi.METAKeywords = metaKeywords.Replace("0x", "x0").Replace("0 x", "x 0").Replace("вЂќ", "\"").Replace("вЂ™", "'");
                        }
                        else
                        {
                            wi.METAKeywords = wi.ProductTitle;
                        }

                        string includes = string.Empty;
                        var includesNodeCollection =
                            htmlDoc.DocumentNode.SelectNodes("//span[@class = 'product-include__title']");
                        if (includesNodeCollection != null)
                        {
                            foreach (var item in includesNodeCollection)
                            {
                                includes += item.InnerTextOrNull().Replace("0x", "x0").Replace("0 x", "x 0").Replace("вЂќ", "\"").Replace("вЂ™", "'") + ",";
                            }
                        }

                        if (!string.IsNullOrEmpty(includes))
                            wi.Includes = includes.Trim(',');

                        string prodFeatures = string.Empty;
                        var productFeaturesNodeCollection =
                            htmlDoc.DocumentNode.SelectNodes("//div[@class = 'product-features']/ul/li");
                        if (productFeaturesNodeCollection != null)
                        {
                            foreach (var item in productFeaturesNodeCollection)
                            {
                                prodFeatures += item.InnerTextOrNull().Replace("0x", "x0").Replace("0 x", "x 0").Replace("вЂќ", "\"").Replace("вЂ™", "'") + "~!~";
                            }
                        }

                        if (!string.IsNullOrEmpty(prodFeatures))
                        {
                            var bulletPoint = prodFeatures.Replace("0x", "x0")
                                                          .Replace("0 x", "x 0")
                                                          .Replace("в„ў", "&trade;")
                                                          .Replace("®", "&reg;")
                                                          .Replace("°", "&deg;")
                                                          .Replace("вЂќ", "\"")
                                                          .Replace("вЂ™", "'").Trim('~', '!').Truncate(BulletPointsLength);
                            if (bulletPoint.Contains('!'))
                            {
                                wi.BulletPoint = bulletPoint.Substring(0, bulletPoint.LastIndexOf('!')).Trim('~', '!');
                            }
                        }   

                        string specifications = "Specification##";
                        var specCollectionNodeCollection =
                            htmlDoc.DocumentNode.SelectNodes("//div[@class='product-specs__col']/div");
                        if (specCollectionNodeCollection != null)
                        {
                            foreach (var specNode in specCollectionNodeCollection)
                            {
                                string specName = specNode.SelectSingleNode(".//span[1]").InnerTextOrNull();
                                string specValue = specNode.SelectSingleNode(".//span[2]").InnerTextOrNull();

                                if (!string.IsNullOrEmpty(specName) && !string.IsNullOrEmpty(specValue)) 
                                    specifications += $"{specName}~{specValue}^";
                            }
                        }

                        if (specifications != "Specification##")
                            wi.Specifications = specifications.Trim('^').Replace("0x", "x0")
                                                                        .Replace("0 x", "x 0")
                                                                        .Replace("в„ў", "&trade;")
                                                                        .Replace("®", "&reg;")
                                                                        .Replace("°", "&deg;")
                                                                        .Replace("вЂќ", "\"")
                                                                        .Replace("вЂ™", "'");

                        string generalImage1 = string.Empty;
                        string generalImage2 = string.Empty;
                        string generalImageResult = string.Empty;
                        var generalImageNodeCollection = htmlDoc.DocumentNode.SelectNodes("//div[@class='media-gallery__thumb']");
                        var image = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='media-gallery__img']/img");
                        if (generalImageNodeCollection != null)
                        {
                            List<String> imageList = new List<string>();
                            foreach (var generalImageNode in generalImageNodeCollection)
                            {
                                imageList.Add("https://milwaukeetool.com" + generalImageNode.AttributeOrNull("data-main"));
                            }
                            imageList = imageList.Distinct().ToList();
                            foreach (var generalImage in imageList)
                            {
                                generalImage1 += generalImage + ".jpg" + ",";
                            }
                        }
                        if (image != null)
                        {
                            generalImage2 = "https://milwaukeetool.com" + image.AttributeOrNull("src") + ".jpg" + ",";
                            generalImageResult = generalImage2 + generalImage1;
                        }
                        if (!string.IsNullOrEmpty(generalImageResult))
                            wi.GeneralImage = generalImageResult.Trim(',');

                        //if (pqi.Name == "from item with options")
                        //{
                        //    var options = pqi.Item.ToString().Trim('^').Split('^');
                        //    foreach (var option in options)
                        //    {
                        //        var oneOption = option.Split('~');
                        //        var title = oneOption.Length > 0 ? oneOption[0] : string.Empty;
                        //        var value = oneOption.Length > 0 ? oneOption[1] : string.Empty;

                        //        if (title != "ID")
                        //        {
                        //            if (string.IsNullOrEmpty(wi.PrimaryOptionTitle) && string.IsNullOrEmpty(wi.PrimaryChoice))
                        //            {
                        //                wi.PrimaryOptionTitle = title;
                        //                wi.PrimaryChoice = value;
                        //            }
                        //            else if (!string.IsNullOrEmpty(wi.PrimaryOptionTitle) && !string.IsNullOrEmpty(wi.PrimaryChoice) &&
                        //                      string.IsNullOrEmpty(wi.SecondOptionTitle) && string.IsNullOrEmpty(wi.SecondOptionChoice))
                        //            {
                        //                wi.SecondOptionTitle = title;
                        //                wi.SecondOptionChoice = value;
                        //            }
                        //            else if (!string.IsNullOrEmpty(wi.PrimaryOptionTitle) && !string.IsNullOrEmpty(wi.PrimaryChoice) &&
                        //                     !string.IsNullOrEmpty(wi.SecondOptionTitle) && !string.IsNullOrEmpty(wi.SecondOptionChoice) &&
                        //                      string.IsNullOrEmpty(wi.ThirdOptionTitle) && string.IsNullOrEmpty(wi.ThirdOptionChoice))
                        //            {
                        //                wi.ThirdOptionTitle = title;
                        //                wi.ThirdOptionChoice = value;
                        //            }
                        //            else if (!string.IsNullOrEmpty(wi.PrimaryOptionTitle) && !string.IsNullOrEmpty(wi.PrimaryChoice) &&
                        //                     !string.IsNullOrEmpty(wi.SecondOptionTitle) && !string.IsNullOrEmpty(wi.SecondOptionChoice) &&
                        //                     !string.IsNullOrEmpty(wi.ThirdOptionTitle) && !string.IsNullOrEmpty(wi.ThirdOptionChoice) &&
                        //                      string.IsNullOrEmpty(wi.FourthOptionTitle) && string.IsNullOrEmpty(wi.FourthOptionChoice))
                        //            {
                        //                wi.FourthOptionTitle = title;
                        //                wi.FourthOptionChoice = value;
                        //            }
                        //        }
                        //    }
                        //}

                        MessagePrinter.PrintMessage("Product page processed");

                        AddWareInfo(wi);
                        OnItemLoaded(wi);
                    }
                }
                else
                {
                    NotProductPages.Add(pqi.URL);
                    MessagePrinter.PrintMessage("It's not product page.");
                }
            }
            catch (Exception exc)
            {
                ExceptionPages.Add(pqi.URL);
                MessagePrinter.PrintMessage($"{exc.Message} on this page - {pqi.URL}", ImportanceLevel.Mid);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 10)
                act = ProcessBrandsListPage;
            else act = null;

            return act;
        }
    }
}
