using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using ScraperEbay;
using Databox.Libs.ScraperEbay;
using System.IO;
using System.Text;
using System.Threading;
using ScraperEbay.Enums;
using ScraperEbay.DataItems;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using OpenQA.Selenium;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Net;

namespace WheelsScraper
{
    public class ScraperEbay : BaseScraper
    {
        #region constants

        private readonly string FTP_UPLOADING = "Uploading to SCE FTP...";
        private readonly string FTP_UPLOADED = "File upload to SCE FTP";
        private const string EBAY_STORE_PAGE = "https://www.ebay.com/usr/";
        private const int PageWaitSleep = 6000;
        private const int PageScrollSleep = 3000;
        private const int PageSendKeysSleep = 1500;
        private const string EbayLoginPage =
            "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&ru=https%3A%2F%2Fwww.ebay.com%2F";
        private const string SingleItemsUrl =
            "http://bulksell.ebay.com/ws/eBayISAPI.dll?SingleList&sellingMode=ReviseItem&lineId=";

        private const string ebayLogin = "https://signin.ebay.com/ws/eBayISAPI.dll?co_partnerId=2&siteid=0&UsingSSL=1";
        private const string EbayPage = "https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&ru=";

        #endregion

        List<ExtWareInfo> waresList = new List<ExtWareInfo>();

        public ScraperEbay()
        {
            Name = "ScraperEbay";
            Url = "https://www.ScraperEbay.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            Complete += ScraperEbay_Complete;

            SpecialSettings = new ExtSettings();
        }

        private void ScraperEbay_Complete(object sender, EventArgs e)
        {
            if (extSett.FlgScrap)
            {
                extSett.EndDate = DateTime.Now;

                waresList.Clear();
                if (Wares.Count == 0)
                {
                    return;
                }
                foreach (WareInfo ware in Wares)
                {
                    if (ware != null)
                        waresList.Add((ExtWareInfo)ware);
                }

                waresList = waresList.Distinct(new ExtWareInfo.EbayItemNumberSoldEqualityComparer()).ToList();

                string startDate = extSett.StarDate.ToShortDateString();


                string fileName = $"{extSett.StarDate.ToString()}_{extSett.EndDate.ToString()}_{waresList.Count}.csv".Replace("/", "-").Replace(":", "-");
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string separator = ",";
                string headers = "Ebay Item Number, Sold";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(headers);

                foreach (ExtWareInfo ware in waresList)
                {
                    string[] productArr = new string[2] { ware.EbayItemNumber, ware.Sold };

                    string product = String.Join(separator, productArr);
                    sb.AppendLine(product);
                }

                File.WriteAllText(filePath, sb.ToString());

                MessagePrinter.PrintMessage($"File created - {filePath}");
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

        private static HtmlDocument GetHtmlDocument(IWebDriver driver)
        {
            var answerHtml = driver.PageSource;
            var answerHtmlDoc = new HtmlDocument();
            answerHtml = HttpUtility.HtmlDecode(answerHtml);
            answerHtmlDoc.LoadHtml(answerHtml);

            return answerHtmlDoc;
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
                    ShowWaitBox();
            }

            var connectionErrorNode = answerHtmlDoc.DocumentNode.SelectSingleNode("//*[contains(text(),'There is no Internet connection')]");
            if (connectionErrorNode != null)
            {
                string error = connectionErrorNode.InnerText;
                if (error == "There is no Internet connection")
                    ShowWaitBox();
            }
        }

        private static void ShowWaitBox()
        {
            DialogResult dialogResultConfirm = MessageBox.Show(
                $"Сonnection problems, press 'OK' when the site will work",
                $"Quora connection error", MessageBoxButtons.OK,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button2,
                (MessageBoxOptions)0x40000);
            if (dialogResultConfirm == DialogResult.OK) { }
        }

        private void CheckVerify(HtmlDocument htmlDoc, IWebDriver driver)
        {
            int counter = 0;
            var verify = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'pgHeading' and @id = 'areaTitle']/h1");

            while (verify != null)
            {
                MessagePrinter.PrintMessage("Please verify yourself", ImportanceLevel.Critical);
                Thread.Sleep(1 * 60 * 1000);
                counter++;

                htmlDoc = GetHtmlDocument(driver);

                verify = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'pgHeading' and @id = 'areaTitle']/h1");
                if (verify == null || counter == 10)
                    break;
            }
        }

        protected override bool Login()
        {
            var account = Settings.Logins.FirstOrDefault();
            if (account != null)
            {
                using (IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
                {
                    Thread.Sleep(PageWaitSleep);
                    Thread.Sleep(PageWaitSleep);
                    driver.Navigate().GoToUrl("https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&ru=https%3A%2F%2Fwww.ebay.com%2F");
                    Actions actions = new Actions(driver);

                    Thread.Sleep(PageWaitSleep);
                    var htmlDoc = GetHtmlDocument(driver);

                    CheckVerify(htmlDoc, driver);
                    htmlDoc = GetHtmlDocument(driver);
                    IWebElement email = driver.FindElement(By.XPath("//input[@class = 'textbox__control textbox__control--underline' and @id = 'userid']"));
                    actions.MoveToElement(email);
                    actions.Click();
                    email.SendKeys(account.Login);
                    Thread.Sleep(PageScrollSleep);

                    IWebElement password = driver.FindElement(By.XPath("//input[@class = 'textbox__control textbox__control--underline' and @id = 'pass']"));
                    actions.MoveToElement(password);
                    actions.Click();
                    password.SendKeys(account.Password);
                    Thread.Sleep(PageScrollSleep);

                    var inButton = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'btn btn--primary btn--large btn--fluid' and @type = 'submit']");
                    IWebElement signInButton = driver.FindElement(By.XPath(inButton.XPath));
                    signInButton.Click();
                    Thread.Sleep(PageWaitSleep);

                    htmlDoc = GetHtmlDocument(driver);
                    CheckVerify(htmlDoc, driver);

                    htmlDoc = GetHtmlDocument(driver);
                    var accInfoNode = htmlDoc.DocumentNode.SelectSingleNode(".//script[contains(text(), 'fn:')]");

                    if (accInfoNode != null)
                    {
                        MessagePrinter.PrintMessage("Loggin succes");

                        var cookies = driver.Manage().Cookies.AllCookies;
                        driver.Manage().Cookies.DeleteAllCookies();

                        foreach (var cookie in cookies)
                        {
                            driver.Manage().Cookies.AddCookie(cookie);
                        }

                        var newCookies = driver.Manage().Cookies.AllCookies;
                        CookieContainer cc = new CookieContainer();

                        foreach (var cookie in newCookies)
                        {
                            cc.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
                        }

                        PageRetriever.cc = cc;

                        driver.Quit();
                        Thread.Sleep(3000);

                        return true;
                    }
                }
            }
            throw new Exception("Login failed");
        }

        protected override void RealStartProcess()
        {
            extSett.FlgScrap = false;
            extSett.LastCheck = false;

            if (extSett.SotreIdsToScrap.Count != 0)
            {
                foreach (string storeId in extSett.SotreIdsToScrap)
                {
                    if (!string.IsNullOrEmpty(storeId))
                    {
                        extSett.StarDate = DateTime.Now;
                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                ItemType = (int)ItemType.ProcessingStartPage,
                                URL = $"https://www.ebay.com/sch/{storeId}/m.html?_nkw&_armrs=1&_ipg&_from&rt=nc&_mPrRngCbx=1&_udlo=0&_udhi",
                                Name = storeId,
                            });
                        }
                    }
                }
            }
            else
            {
                MessagePrinter.PrintMessage("Wrire store names in special settings tab!", ImportanceLevel.Critical);
            }

            StartOrPushPropertiesThread();
        }

        private void ProcessStartPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                int countItems = 0;

                string items = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='rcnt']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(items))
                {
                    items = items.Replace(",", string.Empty);
                    countItems = int.Parse(items);
                    MessagePrinter.PrintMessage($"Count products in \"{pqi.Name}\" store - {countItems} ");
                }

                if (!string.IsNullOrEmpty(items))
                {
                    if (countItems < 10000 && countItems != 0)
                    {
                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                ItemType = (int)ItemType.ProcessingStorePage,
                                URL = $"https://www.ebay.com/sch/m.html?_nkw=&_armrs=1&_from=&_mPrRngCbx=1&_udlo=0&_udhi=&_ssn={pqi.Name}&_sop=10&_ipg=200&rt=nc",
                                Name = pqi.Name,
                            });
                        }
                    }
                    else
                    {
                        TransportItem transportItem = new TransportItem()
                        {
                            CurrentItems = countItems,
                            CurrentMaxPrice = 50,
                            CurrentMinPrice = 0,
                        };
                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                ItemType = (int)ItemType.ProcessingPagePriceFilter,
                                URL = $"https://www.ebay.com/sch/{pqi.Name}/m.html?_nkw&_armrs=1&_from&_mPrRngCbx=1&_udlo=0&_udhi=50&_sop=10&_ipg=200&rt=nc",
                                Name = pqi.Name,
                                Item = transportItem,
                            });
                        }
                    }
                }

                pqi.Processed = true;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this URL - {pqi.URL}", ImportanceLevel.High);
                pqi.Processed = false;
                Thread.Sleep(60000);
            }

            StartOrPushPropertiesThread();
        }

        private void ProcessPagePriceFilter(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                Thread.Sleep(3000);
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                TransportItem transportItem = (TransportItem)pqi.Item;

                int countItems = 0;
                bool itemsIsNull = true;

                string items = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='rcnt']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(items))
                {
                    items = items.Replace(",", string.Empty);
                    countItems = int.Parse(items);
                    itemsIsNull = false;
                    transportItem.Try = 0;
                }

                if (itemsIsNull && transportItem.Try < 3)
                {
                    transportItem.Try++;
                    MessagePrinter.PrintMessage($"Not received items on page. Waiting 30s for next try  - [attempt - {transportItem.Try} of 3]", ImportanceLevel.High);
                    Thread.Sleep(30 * 1000);

                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = (int)ItemType.ProcessingPagePriceFilter,
                            URL = pqi.URL,
                            Name = pqi.Name,
                            Item = transportItem,
                        });
                    }
                }
                else
                {
                    if ((countItems < 10000 && countItems != 0) || transportItem.CurrentMinPrice == transportItem.CurrentMaxPrice)
                    {
                        if (!string.IsNullOrEmpty(transportItem.SmallСhange))
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem()
                                {
                                    ItemType = (int)ItemType.ProcessingStorePage,
                                    URL = pqi.URL,
                                    Name = pqi.Name,
                                    Item = transportItem,
                                });
                            }

                            transportItem.CurrentMaxPrice = transportItem.CurrentMaxPrice + 1;
                            transportItem.CurrentMinPrice = transportItem.CurrentMaxPrice;
                            transportItem.SmallСhange = string.Empty;

                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem()
                                {
                                    ItemType = (int)ItemType.ProcessingPagePriceFilter,
                                    URL = $"https://www.ebay.com/sch/{pqi.Name}/m.html?_nkw&_armrs=1&_from&_mPrRngCbx=1&_udlo={transportItem.CurrentMinPrice - 1}.50&_udhi={transportItem.CurrentMaxPrice}&_sop=10&_ipg=200&rt=nc",
                                    Name = pqi.Name,
                                    Item = transportItem,
                                });
                            }


                        }
                        else
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem()
                                {
                                    ItemType = (int)ItemType.ProcessingStorePage,
                                    URL = pqi.URL,
                                    Name = pqi.Name,
                                });
                            }

                            transportItem.CurrentMinPrice = transportItem.CurrentMaxPrice;
                            transportItem.CurrentMaxPrice = transportItem.CurrentMaxPrice * 2;
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem()
                                {
                                    ItemType = (int)ItemType.ProcessingPagePriceFilter,
                                    URL = $"https://www.ebay.com/sch/{pqi.Name}/m.html?_nkw&_armrs=1&_from&_mPrRngCbx=1&_udlo={transportItem.CurrentMinPrice}&_udhi={transportItem.CurrentMaxPrice}&_sop=10&_ipg=200&rt=nc",
                                    Name = pqi.Name,
                                    Item = transportItem,
                                });
                            }
                        }
                    }
                    else if (countItems != 0)
                    {
                        transportItem.CurrentMaxPrice = transportItem.CurrentMaxPrice / 2;
                        if (transportItem.CurrentMaxPrice == transportItem.CurrentMinPrice || transportItem.CurrentMaxPrice < transportItem.CurrentMinPrice)
                            transportItem.CurrentMaxPrice = transportItem.CurrentMinPrice / 2 + transportItem.CurrentMaxPrice;

                        if (transportItem.CurrentMinPrice == transportItem.CurrentMaxPrice)
                        {
                            if (countItems < 10000)
                            {
                                transportItem.CurrentMaxPrice = transportItem.CurrentMaxPrice + 1;

                                transportItem.CurrentItems = countItems;
                                lock (this)
                                {
                                    lstProcessQueue.Add(new ProcessQueueItem()
                                    {
                                        ItemType = (int)ItemType.ProcessingPagePriceFilter,
                                        URL = $"https://www.ebay.com/sch/{pqi.Name}/m.html?_nkw&_armrs=1&_from&_mPrRngCbx=1&_udlo={transportItem.CurrentMinPrice}&_udhi={transportItem.CurrentMaxPrice}&_sop=10&_ipg=200&rt=nc",
                                        Name = pqi.Name,
                                        Item = transportItem,
                                    });
                                }
                            }
                            else
                            {
                                transportItem.SmallСhange = ".50";
                                lock (this)
                                {
                                    lstProcessQueue.Add(new ProcessQueueItem()
                                    {
                                        ItemType = (int)ItemType.ProcessingPagePriceFilter,
                                        URL = $"https://www.ebay.com/sch/{pqi.Name}/m.html?_nkw&_armrs=1&_from&_mPrRngCbx=1&_udlo={transportItem.CurrentMinPrice}&_udhi={transportItem.CurrentMinPrice}{transportItem.SmallСhange}&_sop=10&_ipg=200&rt=nc",
                                        Name = pqi.Name,
                                        Item = transportItem,
                                    });
                                }
                            }

                        }
                        else
                        {
                            transportItem.CurrentItems = countItems;
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem()
                                {
                                    ItemType = (int)ItemType.ProcessingPagePriceFilter,
                                    URL = $"https://www.ebay.com/sch/{pqi.Name}/m.html?_nkw&_armrs=1&_from&_mPrRngCbx=1&_udlo={transportItem.CurrentMinPrice}&_udhi={transportItem.CurrentMaxPrice}&_sop=10&_ipg=200&rt=nc",
                                    Name = pqi.Name,
                                    Item = transportItem,
                                });
                            }
                        }
                    }
                    else if (!extSett.LastCheck)
                    {
                        extSett.LastCheck = true;
                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                ItemType = (int)ItemType.ProcessingPagePriceFilter,
                                URL = $"https://www.ebay.com/sch/{pqi.Name}/m.html?_nkw&_armrs=1&_from&_mPrRngCbx=1&_udlo={transportItem.CurrentMaxPrice}&_udhi&_sop=10&_ipg=200&rt=nc",
                                Name = pqi.Name,
                                Item = transportItem,
                            });
                        }
                    }
                }

                pqi.Processed = true;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this URL - {pqi.URL}", ImportanceLevel.High);
                pqi.Processed = false;
                Thread.Sleep(60000);
            }

            StartOrPushPropertiesThread();
        }

        protected void ProcessStorePage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                Thread.Sleep(7000);

                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                string nextPage = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='gspr next']").AttributeOrNull("href");
                if (!string.IsNullOrEmpty(nextPage))
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = (int)ItemType.ProcessingStorePage,
                            URL = nextPage,
                        });
                    }
                }

                var itemGrid = htmlDoc.DocumentNode.SelectNodes("//ul[@id='ListViewInner']/li");
                if (itemGrid != null)
                    foreach (HtmlNode node in itemGrid)
                    {
                        if (!extSett.FlgScrap)
                            extSett.FlgScrap = true;

                        ExtWareInfo wi = new ExtWareInfo();

                        wi.EbayItemNumber = node.AttributeOrNull("listingid");
                        wi.Sold = node.SelectSingleNode(".//div[contains(text(),'sold')]").InnerTextOrNull();

                        AddWareInfo(wi);
                        OnItemLoaded(wi);
                    }

                pqi.Processed = true;
                MessagePrinter.PrintMessage($"Grid Page Processed!");
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this URL - {pqi.URL}", ImportanceLevel.High);
                pqi.Processed = false;
                Thread.Sleep(60000);
            }

            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case (int)ItemType.ProcessingStartPage:
                    act = ProcessStartPage;
                    break;
                case (int)ItemType.ProcessingPagePriceFilter:
                    act = ProcessPagePriceFilter;
                    break;
                case (int)ItemType.ProcessingStorePage:
                    act = ProcessStorePage;
                    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }
    }
}
