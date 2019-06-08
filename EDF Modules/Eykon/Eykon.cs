using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Eykon;
using Databox.Libs.Eykon;
using System.Text.RegularExpressions;
using Eykon.DataItems;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace WheelsScraper
{
    public class Eykon : BaseScraper
    {
        private const int PageWaitSleep = 6000;
        private const int PageScrollSleep = 1500;
        private const int PageSendKeysSleep = 1500;

        public List<String> ColorsPN { get; set; }

        public Eykon()
        {
            ColorsPN = new List<String>();
            Name = "Eykon";
            Url = "http://eykon.net/wallcovering";
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

        protected override void RealStartProcess()
        {
            lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = ProcessType.ProcessingMainPage });
            StartOrPushPropertiesThread();
        }

        public static void CheckServerError(IWebDriver driver)
        {
            var answerHtmlDoc = GetHtmlDocument(driver);
            //Если "Server Error". подождать и перезагрузить страницу
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
            //небольшой слип для ожидания скрола
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

        public bool ProcessColor(string productLink, string color)
        {
            List<ReportItem> reportItems = new List<ReportItem>();

            try
            {
                using (IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory)) // передаем путь до chromedriver.exe
                {
                    //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));                    
                    Thread.Sleep(PageWaitSleep);
                    driver.Navigate().GoToUrl(productLink);
                    //driver.Navigate().GoToUrl("http://ip.xss.myip.ru");
                    Thread.Sleep(PageWaitSleep);
                    CheckServerError(driver);
                    try
                    {
                        var htmlDoc = GetHtmlDocument(driver);
                        long oldLength = driver.PageSource.Length;
                        bool endPage = false;

                        var colorPartNumber = htmlDoc.DocumentNode.SelectNodes("//div[@class='hits']/div/div/div/a/div/small[1]");
                        if (colorPartNumber != null)
                        {
                            foreach (var item in colorPartNumber)
                            {
                                var colorPNumber = item.InnerTextOrNull();

                                ColorsPN.Add($"{colorPNumber}^{color}");
                            }
                        }

                        try
                        {
                            bool endPageFound = false;
                            do
                            {
                                Actions actions = new Actions(driver);

                                IWebElement quoraEmail1 = driver.FindElement(By.XPath("//a[@class = 'ais-pagination--link' and @aria-label = 'Next']"));
                                actions.MoveToElement(quoraEmail1);
                                Thread.Sleep(PageWaitSleep);
                                actions.Click();
                                actions.Perform();

                                Thread.Sleep(PageWaitSleep);

                                var htmlNextPageDoc = GetHtmlDocument(driver);
                                var colorPartNumber1 = htmlNextPageDoc.DocumentNode.SelectNodes("//div[@class='hits']/div/div/div/a/div/small[1]");
                                if (colorPartNumber1 != null)
                                {
                                    foreach (var item in colorPartNumber1)
                                    {
                                        var colorPNumber = item.InnerTextOrNull();

                                        ColorsPN.Add($"{colorPNumber}^{color}");
                                    }
                                }

                            } while (!endPageFound);
                        }
                        catch (Exception e)
                        {
                            return true;
                        }
                    }
                    catch
                    {

                    }

                    driver.Quit();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception e)
            {

            }

            return true;
        }

        public void ProcessMainPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            string html = PageRetriever.ReadFromServer("http://eykon.net/wallcovering", true);
            html = HttpUtility.HtmlDecode(html);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var colorSpecification = htmlDoc.DocumentNode.SelectNodes("//div[@class='category-list block-grid-12']/div/a");
            if (colorSpecification != null)
            {
                foreach (var item in colorSpecification)
                {
                    string productLink;
                    productLink = "http://eykon.net" + item.AttributeOrNull("href");
                    string color = item.AttributeOrNull("title").Replace(" Wallcovering", "");

                    ProcessColor(productLink, color);
                }
            }

            var collectionCategories = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'category-list block-grid-7']/div/a");
            if (collectionCategories != null)
            {
                foreach (var item in collectionCategories)
                {
                    string productLink;
                    productLink = item.AttributeOrNull("href");

                    var pqi1 = new ProcessQueueItem();
                    pqi1.URL = productLink;
                    pqi1.ItemType = ProcessType.ProcessingCategoryPage;
                    lock (this)
                    {
                        lstProcessQueue.Add(pqi1);
                    }
                }
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Main page processed");
            StartOrPushPropertiesThread();
        }

        public void ProcessCategoryPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                if (!string.IsNullOrEmpty(pqi.URL))
                {
                    var collectionCategories = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'block block--product see-more']/h2/a");
                    if (collectionCategories != null)
                    {
                        foreach (var item in collectionCategories)
                        {
                            string productLink;
                            productLink = item.AttributeOrNull("href");

                            var pqi1 = new ProcessQueueItem();
                            pqi1.URL = productLink;
                            pqi1.ItemType = ProcessType.ProcessingProductCategory;
                            lock (this)
                            {
                                lstProcessQueue.Add(pqi1);
                            }
                        }
                    }
                    else
                    {
                        string productLink;
                        productLink = pqi.URL;

                        var pqi1 = new ProcessQueueItem();
                        pqi1.URL = productLink;
                        pqi1.ItemType = ProcessType.ProcessingProductCategory;
                        lock (this)
                        {
                            lstProcessQueue.Add(pqi1);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Category page processed");
            StartOrPushPropertiesThread();
        }

        public void ProcessProductCategory(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                //var colorSpecification = htmlDoc.DocumentNode.SelectNodes("//div[@class='category-list block-grid-12']/div/a");
                var collectionCategories = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'block block--product']/a");
                if (/*colorSpecification*/ collectionCategories != null)
                {
                    foreach (var item in /*colorSpecification*/ collectionCategories)
                    {
                        string productLink;
                        productLink = item.AttributeOrNull("href");

                        var pqi1 = new ProcessQueueItem();
                        pqi1.URL = productLink;
                        pqi1.ItemType = ProcessType.ProcessingProduct;
                        lock (this)
                        {
                            lstProcessQueue.Add(pqi1);
                        }
                    }
                    var nextPage = htmlDoc.DocumentNode.SelectSingleNode("//a[@class = 'next i-next']");
                    if (nextPage != null)
                    {
                        var productLink = nextPage.AttributeOrNull("href");

                        var pqi1 = new ProcessQueueItem();
                        pqi1.URL = productLink;
                        pqi1.ItemType = ProcessType.ProcessingProductCategory;
                        lock (this)
                        {
                            lstProcessQueue.Add(pqi1);
                        }
                    }
                }
            }
            catch (Exception e)
            {


            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Product category processed");
            StartOrPushPropertiesThread();
        }

        public void ProcessProduct(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                //pqi.URL = "http://eykon.net/wallcovering/lanark/lanark/myriad";
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var script = htmlDoc.DocumentNode.SelectSingleNode("//script[contains(text(),'option_labels')]").InnerTextOrNull();
                if (script != null)
                {
                    var scriptGroupValue = Regex.Match(script, @"\{(.)*\}");
                    ProductId responce = new ProductId();

                    var jss = new JavaScriptSerializer();
                    responce = jss.Deserialize<ProductId>(scriptGroupValue.Value.Replace("\\\"", "\""));

                    List<string> images = new List<string>();

                    foreach (var product in responce.base_image)
                    {
                        var id = product.Key;

                        images.Add(product.Value.Replace("\\", ""));

                        string requestUrl = "http://eykon.net/productdetails/index/optiondetails/";
                        HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                        string dataString = "id=" + id;
                        byte[] bufferBytes = asciiEncoding.GetBytes(dataString);

                        using (Stream stream = request.GetRequestStream())
                        {
                            stream.Write(bufferBytes, 0, bufferBytes.Length);
                            stream.Flush();
                        }

                        using (WebResponse responce1 = request.GetResponse())
                        {
                            using (StreamReader sr = new StreamReader(responce1.GetResponseStream()))
                            {
                                string responceString = sr.ReadToEnd();

                                var options = htmlDoc.DocumentNode.SelectNodes("//ul[@id='configurable_swatch_web_colorway_name']/li");
                                if (options != null)
                                {
                                    foreach (var option in options)
                                    {
                                        var optionId = option.AttributeOrNull("id");
                                        var oneOption = option.SelectSingleNode("./a/div/div/div[1]").InnerTextOrNull();
                                        var sku = option.SelectSingleNode("./a/div/div/div[2]").InnerTextOrNull();

                                        try
                                        {
                                            if (responceString.Contains(oneOption))
                                            {
                                                ExtWareInfo wi = new ExtWareInfo();

                                                var generalImage = product.Value.Replace("\\", "");

                                                var productLink = responceString.Substring(responceString.IndexOf($"http://eykon.net/{oneOption.ToLower()}"));
                                                productLink = productLink.Substring(0, productLink.IndexOf("&"));

                                                wi.URL = productLink;

                                                var brand = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='breadcrumbs']/ul/li[3]/a");
                                                if (brand != null)
                                                {
                                                    wi.Brand = brand.InnerTextOrNull();
                                                }

                                                var subBrand = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='breadcrumbs']/ul/li[4]/a");
                                                if (subBrand != null)
                                                {
                                                    wi.SubBrand = subBrand.InnerTextOrNull();
                                                }

                                                var subTitle = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='product-name']/h1");
                                                if (subTitle != null)
                                                {
                                                    wi.SubTitle = subTitle.InnerTextOrNull();
                                                }

                                                if (!string.IsNullOrEmpty(wi.SubTitle))
                                                {
                                                    wi.Title = $"{wi.SubBrand} {wi.SubTitle} {oneOption} {sku}";
                                                }
                                                else
                                                {
                                                    wi.Title = $"{wi.SubTitle} {oneOption} {sku}";
                                                }

                                                var grabDocuments = htmlDoc.DocumentNode.SelectSingleNode("//ul[@class='document-list']/li/a");
                                                if (grabDocuments != null)
                                                {
                                                    wi.GrabDocuments = grabDocuments.AttributeOrNull("href");
                                                }

                                                var descriptionName = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='specifications']/table/tbody/tr[1]/th").InnerTextOrNull();
                                                if (descriptionName != null && descriptionName == "Notes")
                                                {
                                                    var description = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='specifications']/table/tbody/tr[1]/td");
                                                    if (description != null)
                                                    {
                                                        wi.Description = description.InnerTextOrNull();

                                                        if (!string.IsNullOrEmpty(wi.Description))
                                                        {
                                                            if (wi.Description.Contains("Custom colors, weights, and microventing available."))
                                                            {
                                                                wi.Description = wi.Description.Replace("Custom colors, weights, and microventing available.", "");

                                                                if (!string.IsNullOrEmpty(wi.Description))
                                                                {
                                                                    wi.Description = wi.Description;
                                                                }
                                                                else
                                                                {
                                                                    wi.Description = wi.Title;
                                                                }
                                                            }
                                                            else if (!wi.Description.Contains("Custom colors, weights, and microventing available."))
                                                            {
                                                                if (!string.IsNullOrEmpty(wi.Description))
                                                                {
                                                                    wi.Description = wi.Description;
                                                                }
                                                                else
                                                                {
                                                                    wi.Description = wi.Title;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            wi.Description = wi.Title;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    wi.Description = wi.Title;
                                                }

                                                var productType = htmlDoc.DocumentNode.SelectNodes("//div[@class='specifications']/table/tbody/tr");
                                                if (productType != null)
                                                {
                                                    string types = "";
                                                    string typeName = "";
                                                    string typeValue = "";

                                                    foreach (var item in productType)
                                                    {
                                                        types = item.SelectSingleNode("./th").InnerTextOrNull();

                                                        if (types == "Type")
                                                        {
                                                            typeName = item.SelectSingleNode("./th").InnerTextOrNull();
                                                            typeValue = item.SelectSingleNode("./td").InnerTextOrNull();
                                                        }
                                                    }

                                                    wi.ProductType = $"{typeName} {typeValue}";
                                                    wi.ProductTypeSpec = $"{typeName}~{typeValue}^";
                                                }

                                                var specifications = htmlDoc.DocumentNode.SelectNodes("//table[@class = 'data-table']/tbody/tr");
                                                if (specifications != null)
                                                {
                                                    string spec = "";
                                                    string fullSpec = "";

                                                    foreach (var item in specifications)
                                                    {
                                                        var specific = "Specification##";

                                                        var specName = item.SelectSingleNode("./th").InnerTextOrNull();
                                                        var specValue = item.SelectSingleNode("./td").InnerTextOrNull();

                                                        if (specName != "Type")
                                                        {

                                                            if (!string.IsNullOrEmpty(oneOption) && !string.IsNullOrEmpty(wi.ProductTypeSpec))
                                                            {
                                                                spec = $"{specific}Color~{oneOption}^{wi.ProductTypeSpec}";
                                                                fullSpec += $"{specName}~{specValue}^";
                                                            }
                                                            else if (string.IsNullOrEmpty(oneOption) && !string.IsNullOrEmpty(wi.ProductTypeSpec))
                                                            {
                                                                spec = $"{specific}{wi.ProductTypeSpec}";
                                                                fullSpec += $"{specName}~{specValue}^";
                                                            }
                                                            else if (!string.IsNullOrEmpty(oneOption) && string.IsNullOrEmpty(wi.ProductTypeSpec))
                                                            {
                                                                spec = $"{specific}Color~{oneOption}^";
                                                                fullSpec += $"{specName}~{specValue}^";
                                                            }
                                                            else if (string.IsNullOrEmpty(oneOption) && string.IsNullOrEmpty(wi.ProductTypeSpec))
                                                            {
                                                                spec = $"{specific}";
                                                                fullSpec += $"{specName}~{specValue}^";
                                                            }
                                                        }
                                                    }

                                                    wi.Specifications = spec + fullSpec.Trim('^');
                                                }

                                                if (wi.ProductType == "Type 1")
                                                {
                                                    wi.CrossSellSubCategory1 = wi.ProductType;
                                                }

                                                if (wi.ProductType == "Type 2")
                                                {
                                                    wi.CrossSellSubCategory2 = wi.ProductType;
                                                }

                                                wi.AnchorText = wi.Title;
                                                wi.GeneralImageTags = wi.Title;
                                                wi.Image = generalImage;
                                                wi.MainCategory = "Wallcovering";
                                                wi.SubCategory = wi.Brand;
                                                wi.Suppliers = "Mahones Commercial Wallpape";
                                                wi.Warehouse = "N/A";
                                                wi.PartNumber = sku;

                                                foreach (var item in ColorsPN)
                                                {
                                                    string colorPart = item.Split('^')[0];
                                                    string colorC = item.Split('^')[1];

                                                    if (colorPart == wi.PartNumber)
                                                    {
                                                        wi.Color = colorC;
                                                    }
                                                }
                                                
                                                wi.MetaDescription = wi.Description;

                                                AddWareInfo(wi);
                                                OnItemLoaded(wi);
                                            }
                                        }
                                        catch (Exception e)
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

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
                case ProcessType.ProcessingMainPage:
                    act = ProcessMainPage;
                    break;
                case ProcessType.ProcessingCategoryPage:
                    act = ProcessCategoryPage;
                    break;
                case ProcessType.ProcessingProductCategory:
                    act = ProcessProductCategory;
                    break;
                case ProcessType.ProcessingProduct:
                    act = ProcessProduct;
                    break;
                default:
                    act = null;
                    break;
            }

            return act;
        }
    }
}
