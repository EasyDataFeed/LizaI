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

namespace WheelsScraper
{
    public class Milwaukeetool : BaseScraper
    {
        private const int PageWaitSleep = 6000;
        private const int PageScrollSleep = 1500;
        private const int PageSendKeysSleep = 1500;
        private List<string> NotProductPages { get; set; }
        private List<string> ExceptionPages { get; set; }
        private List<string> Categories { get; set; }
        private List<string> Products { get; set; }
        public Milwaukeetool()
        {
            Name = "Milwaukeetool";
            Url = "https://www.Milwaukeetool.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
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

            //try found "Answers Collapsed"
            HtmlNode collapsedAnwersNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//span[contains(text(),'Answers Collapsed')]");
            if (collapsedAnwersNode != null)
            {
                //load collapsed anwers
                string id = collapsedAnwersNode.AttributeOrNull("id");
                IWebElement collapsedAnswersElement = driver.FindElement(By.Id(id));

                if (collapsedAnswersElement.Displayed)
                    collapsedAnswersElement.Click();

                //подождать загрузки коментариев
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

                    //подождать загрузки коментариев
                    Thread.Sleep(PageWaitSleep);
                }
                else
                {
                    //ERROR EXCEPTION 
                    string endPageError = "END PAGE ERROR!";
                }
            }
        }

        private List<string> LoadLinksSite()
        {
            List<string> links = new List<string>();

            using (IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory)) // передаем путь до chromedriver.exe
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

                            var products = htmlDoc.DocumentNode.SelectNodes("//div[@class='coveo-result-list-container coveo-list-layout-container']/div/a");
                            if (products != null)
                            {
                                foreach (var product in products)
                                {
                                    string link;
                                    link = "https://milwaukeetool.com" + product.AttributeOrNull("href");

                                    links.Add(link);
                                }
                            }

                            IWebElement nextPage = driver.FindElement(By.XPath("//li[@class='coveo-pager-next coveo-pager-anchor coveo-pager-list-item']"));
                            actions.MoveToElement(nextPage);
                            actions.Click();
                            actions.Perform();
                            Thread.Sleep(PageWaitSleep);
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

            foreach (string link in links)
            {
                if (!link.Contains("Products"))
                {
                    continue;
                }

                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem()
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
                //pqi.URL = "https://milwaukeetool.com/Products/Hand-Tools/Layout-and-Marking/Marking/48-22-3915";
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
                    wi.URL = pqi.URL;
                    wi.PartNumber = sku;

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
                        wi.ProductTitle = $"Milwaukeetool {sku} {productTitle.InnerTextOrNull()}";
                        wi.AnchorText = $"{sku} {productTitle.InnerTextOrNull()}";
                    }

                    var productDescription =
                        htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'product-info__overview readmore']/p");
                    if (productDescription != null)
                    {
                        wi.ProductDescription = productDescription.InnerTextOrNull();
                    }

                    string metaDescription = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']")
                        .AttributeOrNull("content");
                    if (!string.IsNullOrEmpty(metaDescription))
                    {
                        wi.METADescription = metaDescription;
                    }

                    string metaKeywords = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='keywords']")
                        .AttributeOrNull("content");
                    if (!string.IsNullOrEmpty(metaDescription))
                    {
                        wi.METAKeywords = metaKeywords;
                    }

                    string includes = string.Empty;
                    var includesNodeCollection =
                        htmlDoc.DocumentNode.SelectNodes("//span[@class = 'product-include__title']");
                    if (includesNodeCollection != null)
                    {
                        foreach (var item in includesNodeCollection)
                        {
                            includes += item.InnerTextOrNull() + ",";
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
                            prodFeatures += item.InnerTextOrNull() + "~!~";
                        }
                    }

                    if (!string.IsNullOrEmpty(prodFeatures))
                        wi.BulletPoint = prodFeatures.Trim('~', '!');

                    string specifications = "Specification##";
                    var specCollectionNodeCollection =
                        htmlDoc.DocumentNode.SelectNodes("//div[@class='product-specs__col']/div");
                    if (specCollectionNodeCollection != null)
                    {
                        foreach (var specNode in specCollectionNodeCollection)
                        {
                            string specName = specNode.SelectSingleNode(".//span[1]").InnerTextOrNull();
                            string specValue = specNode.SelectSingleNode(".//span[2]").InnerTextOrNull();

                            specifications += $"{specName}~{specValue}^";
                        }
                    }

                    if (specifications != "Specification##")
                        wi.Specifications = specifications.Trim('^');

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

                    MessagePrinter.PrintMessage("Product page processed");

                    AddWareInfo(wi);
                    OnItemLoaded(wi);
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
