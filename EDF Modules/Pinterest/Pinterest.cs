using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Pinterest;
using Databox.Libs.Pinterest;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Windows.Forms;
using OpenQA.Selenium.Interactions;
using Pinterest.Helpers;
using Pinterest.DataItems;
using System.IO;
using System.Text.RegularExpressions;
using Pinterest.Extensions;
using Pinterest.Enums;

namespace WheelsScraper
{
    public class Pinterest : BaseScraper
    {
        private ActionType _actionType;
        private List<PinterestItem> PinterestItems { get; set; }
        private const int PageWaitSleep = 6000;
        private const int PageScrollSleep = 3000;
        private const int PageSendKeysSleep = 1500;
        private const int DescriptionLength = 500;
        private const string LogFileName = @"PinterestErrorsLog.txt";
        private string ftpResultPath = string.Empty;

        public Pinterest()
        {
            Name = "Pinterest";
            Url = "https://www.Pinterest.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            PinterestItems = new List<PinterestItem>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
            Complete += Pinterest_Complete;
        }

        private void Pinterest_Complete(object sender, EventArgs e)
        {
            if (PinterestItems.Count() > 0)
            {
                string filePath = FileHelper.CreatePinterestAddItem(FileHelper.GetSettingsPath("AddedPinterestProducts.csv"), PinterestItems);
                MessagePrinter.PrintMessage($"File with added products created - {filePath}");
            }

            if (!string.IsNullOrEmpty(ftpResultPath))
            {
                MessagePrinter.PrintMessage(string.Format("Error's log uploaded to FTP: {0}", ftpResultPath),
                    ImportanceLevel.High);
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

        protected override bool Login()
        {
            return true;
        }

        protected override void RealStartProcess()
        {
            _actionType = extSett.ActionType;

            PinterestProductsProcessed();

            StartOrPushPropertiesThread();
        }

        private static HtmlDocument GetHtmlDocument(IWebDriver driver)
        {
            var answerHtml = driver.PageSource;
            var answerHtmlDoc = new HtmlDocument();
            answerHtml = HttpUtility.HtmlDecode(answerHtml);
            answerHtmlDoc.LoadHtml(answerHtml);

            return answerHtmlDoc;
        }

        private void LogError(string errorMes)
        {
            var logFile = FileHelper.GetSettingsPath(LogFileName);
            using (var writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine(
                    $"{DateTime.Now}----------------------------------------------------------------------------------");
                writer.WriteLine(errorMes);
                writer.WriteLine();
            }

            ftpResultPath = FileHelper.UploadFileToFTP(this, Path.GetFileName(logFile), logFile);
            //if (!string.IsNullOrEmpty(ftpResultPath))
            //{
            //    MessagePrinter.PrintMessage(string.Format("Error's log uploaded to FTP: {0}", ftpResultPath),
            //        ImportanceLevel.High);
            //}
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

        protected void PinterestProductsProcessed()
        {
            var loginInfo = Settings.Logins;
            string login = "";
            string pass = "";

            foreach (var activLogin in loginInfo)
            {
                if (activLogin.IsActive == true)
                {
                    login = activLogin.Login;
                    pass = activLogin.Password;
                }
            }

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
            {
                MessagePrinter.PrintMessage("Login or password is empty", ImportanceLevel.Critical);
                return;
            }

            var pinterestFilePath = FileHelper.GetSettingsPath("AddedPinterestProducts.csv");

            if (File.Exists(pinterestFilePath))
            {
                PinterestItems = FileHelper.ReadPinterestAddItem(pinterestFilePath);
            }

            try
            {
                List<string> sceFiles = new List<string>();

                if (extSett.UseExistingExport)
                {
                    MessagePrinter.PrintMessage($"Use existing export");
                    sceFiles = new List<string>() { extSett.ExportFilePath };
                }
                else
                {
                    MessagePrinter.PrintMessage($"Download SCE export... please wait");
                    sceFiles = SceApiHelper.LoadProductsExport(Settings);

                    if (sceFiles.Count > 0)
                    {
                        MessagePrinter.PrintMessage($"SCE export downloaded");
                    }
                    else
                    {
                        MessagePrinter.PrintMessage($"SCE export not downloaded, please try again later", ImportanceLevel.Critical);
                        return;
                    }
                }

                MessagePrinter.PrintMessage($"Read SCE export... please wait");
                List<SceExportItem> sceExportItems = new List<SceExportItem>();
                foreach (string sceFile in sceFiles)
                {
                    sceExportItems.AddRange(FileHelper.ReadSceExportFile(sceFile));
                }

                MessagePrinter.PrintMessage($"SCE export Readed... please wait");

                sceExportItems = sceExportItems.Distinct(SceExportItem.ProdIdBrandComparer).ToList();

                if (extSett.UseExistingExport == false)
                {
                    foreach (string sceFile in sceFiles)
                    {
                        if (File.Exists(sceFile))
                        {
                            File.Delete(sceFile);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(extSett.Brand))
                {
                    MessagePrinter.PrintMessage($"Read Brand file... please wait");
                    extSett.BrandItems = FileHelper.ReadBrandFile(extSett.Brand);
                }

                //string filePath = FileHelper.CreateSce(FileHelper.GetSettingsPath("Export.csv"), sceExportItems);
                //ChromeOptions options = new ChromeOptions();
                //options.AddArguments("--disable-popup-blocking");
                //options.AddArguments("--disable-extensions"); // to disable extension
                //options.AddArguments("--disable-notifications"); // to disable notification
                //options.AddArguments("--disable-application-cache"); // to disable cache
                //options.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;

                using (IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory))
                {
                    MessagePrinter.PrintMessage($"Start processing products");

                    Thread.Sleep(PageWaitSleep);
                    Thread.Sleep(PageWaitSleep);
                    driver.Navigate().GoToUrl("https://www.pinterest.com");

                    Thread.Sleep(PageWaitSleep);
                    Thread.Sleep(PageWaitSleep);
                    CheckServerError(driver);
                    Actions actions = new Actions(driver);

                    IWebElement logIn = driver.FindElement(By.XPath("//div[@class = 'tBJ dyH iFc SMy yTZ pBj tg7 mWe']"));
                    actions.MoveToElement(logIn);
                    actions.Click();
                    actions.Perform();
                    Thread.Sleep(PageScrollSleep);

                    IWebElement email = driver.FindElement(By.XPath("//input[@id = 'email' and @type = 'email']"));
                    actions.MoveToElement(email);
                    actions.Click();
                    email.SendKeys(login/*"kiatut@givememail.club"*/);
                    Thread.Sleep(PageScrollSleep);

                    IWebElement password = driver.FindElement(By.XPath("//input[@id = 'password' and @name = 'password']"));
                    actions.MoveToElement(password);
                    actions.Click();
                    password.SendKeys(pass/*"xclo#4521KNJg!"*/);
                    Thread.Sleep(PageScrollSleep);

                    var htmlDoc = GetHtmlDocument(driver);
                    var inButton = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'red SignupButton active' and @type = 'submit']");
                    IWebElement signInButton = driver.FindElement(By.XPath(inButton.ParentNode.XPath));
                    signInButton.Click();
                    Thread.Sleep(PageWaitSleep);

                    if (extSett.UseBusinessesAccount)
                    {
                        if (extSett.ChangeWebsite)
                        {
                            bool someItemFound = false;

                            MessagePrinter.PrintMessage("Processing 'website' changes");

                            foreach (var item in PinterestItems)
                            {
                                if (!string.IsNullOrEmpty(item.PinUrl) && string.IsNullOrEmpty(item.Changed))
                                {
                                    var sceItem = sceExportItems.Find(i => i.ProdId == item.ProdId || (i.Brand == item.Brand && i.PartNumber == item.PartNumber));

                                    if (sceItem != null)
                                    {
                                        Thread.Sleep(PageWaitSleep);
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com{item.PinUrl}");
                                        Thread.Sleep(PageWaitSleep);
                                        Thread.Sleep(PageWaitSleep);
                                        if (!driver.Url.Contains($"{item.PinUrl}"))
                                        {
                                            MessagePrinter.PrintMessage($"Pin Brand - [{item.Brand}] PN - [{item.PartNumber}] - not found", ImportanceLevel.Mid);

                                            item.Changed = "not found";
                                            continue;
                                        }

                                        htmlDoc = GetHtmlDocument(driver);
                                        var selectPin = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-test-id = 'closeup-pin-edit-button']");
                                        if (selectPin == null || selectPin.ParentNode == null)
                                        {
                                            LogError("selectPin not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement selectPin1 = driver.FindElement(By.XPath(selectPin.XPath));
                                        if (selectPin1 == null || selectPin1.Displayed == false || selectPin1.Enabled == false)
                                        {
                                            LogError("selectPin1 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        selectPin1.Click();
                                        Thread.Sleep(PageScrollSleep);

                                        htmlDoc = GetHtmlDocument(driver);
                                        var website = htmlDoc.DocumentNode.SelectSingleNode("//input[@class = 'wyq iyn Hsu aZc tBJ dyH iFc SMy yTZ L4E edc pBj qJc fgH' and @id = 'WebsiteField']");
                                        if (website == null || website.ParentNode == null)
                                        {
                                            LogError("website not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement changedWebsite = driver.FindElement(By.XPath(website.XPath));
                                        if (changedWebsite == null || changedWebsite.Displayed == false || changedWebsite.Enabled == false)
                                        {
                                            LogError("changedWebsite not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        changedWebsite.Click();
                                        changedWebsite.Clear();
                                        var withoutHttpPart1 = sceItem.GeneralImage.Replace("http://", string.Empty)
                                                                          .Replace("https://", string.Empty);
                                        if (withoutHttpPart1.Contains("/"))
                                        {
                                            var store = withoutHttpPart1.Substring(0, withoutHttpPart1.IndexOf("/", StringComparison.Ordinal));
                                            changedWebsite.SendKeys($"https://{store}/product/{sceItem.SpiderURL}"/*$"https://www.mahoneswallpapershop.com/product/{sceItem.SpiderURL}"*/);
                                        }
                                        Thread.Sleep(PageScrollSleep);

                                        htmlDoc = GetHtmlDocument(driver);
                                        var savePin = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu mix Vxj aZc GmH adn Il7 Jrn hNT iyn BG7 gn8 L4E kVc']");
                                        if (savePin == null || savePin.ParentNode == null)
                                        {
                                            LogError("savePin not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement savePin1 = driver.FindElement(By.XPath(savePin.XPath));
                                        if (savePin1 == null || savePin1.Displayed == false || savePin1.Enabled == false)
                                        {
                                            LogError("savePin1 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        savePin1.Click();
                                        Thread.Sleep(PageScrollSleep);

                                        item.Changed = "true";
                                        someItemFound = true;
                                    }
                                }
                            }

                            if (someItemFound)
                                driver.Navigate().GoToUrl("https://www.pinterest.com");

                            Thread.Sleep(PageWaitSleep);
                            MessagePrinter.PrintMessage("'website' changes processed");
                        }
                    }

                    Thread.Sleep(PageWaitSleep);

                    foreach (var sceItem in sceExportItems)
                    {
                        bool itemAdded = false;
                        bool conditionFound = false;
                        bool hiddenYes = false;

                        if (sceItem.Hidden == "yes")
                        {
                            if (extSett.SkipHidden)
                            {
                                hiddenYes = true;
                            }
                        }

                        //if (sceItem.SpiderURL != "canada-maple-leaf-ring-charm")
                        //{
                        //    continue;
                        //}

                        if (hiddenYes == false)
                        {
                            foreach (var pinterestItem in PinterestItems)
                            {
                                if (cancel)
                                    return;

                                if (sceItem.ProdId == pinterestItem.ProdId || (sceItem.PartNumber == pinterestItem.PartNumber && sceItem.Brand == pinterestItem.Brand))
                                {
                                    itemAdded = true;
                                    break;
                                }
                            }

                            if (!itemAdded)
                            {
                                if (extSett.BrandItems.Count() < 1)
                                    conditionFound = true;

                                foreach (var brandItem in extSett.BrandItems)
                                {
                                    if (cancel)
                                        return;

                                    if (_actionType == ActionType.Brand)
                                    {
                                        if (sceItem.Brand == brandItem.Brand)
                                        {
                                            conditionFound = true;
                                            break;
                                        }
                                    }

                                    if (_actionType == ActionType.MainCategory)
                                    {
                                        if (sceItem.MainCategory == brandItem.MainCategory)
                                        {
                                            conditionFound = true;
                                            break;
                                        }
                                    }

                                    if (_actionType == ActionType.SubCategory)
                                    {
                                        if (sceItem.SubCategory == brandItem.SubCategory)
                                        {
                                            conditionFound = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!itemAdded && conditionFound)
                            {
                                MessagePrinter.PrintMessage($"Processing Product. For Brand - {sceItem.Brand}. Part Number - {sceItem.PartNumber}.");

                                if (extSett.UseBusinessesAccount)
                                {
                                    Thread.Sleep(PageWaitSleep);
                                    Thread.Sleep(PageScrollSleep);
                                    htmlDoc = GetHtmlDocument(driver);
                                    var addPinButtonBusiness = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy MF7 B9u tg7 swG']");
                                    if (addPinButtonBusiness == null || addPinButtonBusiness.ParentNode == null)
                                    {
                                        LogError("addPinButtonBusiness not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    IWebElement addPinBusiness = driver.FindElement(By.XPath(addPinButtonBusiness.ParentNode.XPath));
                                    if (addPinBusiness == null || addPinBusiness.Displayed == false || addPinBusiness.Enabled == false)
                                    {
                                        LogError("addPinBusiness not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    addPinBusiness.Click();
                                    Thread.Sleep(PageScrollSleep);
                                }
                                else
                                {
                                    Thread.Sleep(PageWaitSleep);
                                    Thread.Sleep(PageScrollSleep);
                                    htmlDoc = GetHtmlDocument(driver);
                                    var skipForNow = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy yTZ pBj DrD IZT mWe']");
                                    if (skipForNow != null)
                                    {
                                        IWebElement skip = driver.FindElement(By.XPath(skipForNow.ParentNode.XPath));
                                        skip.Click();
                                        Thread.Sleep(PageScrollSleep);
                                    }

                                    Thread.Sleep(PageWaitSleep);
                                    Thread.Sleep(PageScrollSleep);
                                    htmlDoc = GetHtmlDocument(driver);
                                    var addPinButton = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-test-id = 'addPinButton']");
                                    if (addPinButton == null || addPinButton.ParentNode == null)
                                    {
                                        LogError("addPinButton not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    IWebElement addPin = driver.FindElement(By.XPath(addPinButton.ParentNode.XPath));
                                    if (addPin == null || addPin.Displayed == false || addPin.Enabled == false)
                                    {
                                        LogError("addPinBusiness not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    addPin.Click();
                                    Thread.Sleep(PageScrollSleep);

                                    htmlDoc = GetHtmlDocument(driver);
                                    var notValid = htmlDoc.DocumentNode.SelectSingleNode("//span[@class = 'buttonText']");
                                    if (notValid != null)
                                    {
                                        IWebElement not = driver.FindElement(By.XPath(notValid.ParentNode.XPath));
                                        if (not == null || not.Displayed == false || not.Enabled == false)
                                        {
                                            LogError("\"not\" not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        not.Click();
                                        Thread.Sleep(PageScrollSleep);

                                        htmlDoc = GetHtmlDocument(driver);
                                        var addNewPin2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-test-id = 'addPinButton']");
                                        if (addNewPin2 == null || addNewPin2.ParentNode == null)
                                        {
                                            LogError("addNewPin2 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement addPin2 = driver.FindElement(By.XPath(addNewPin2.ParentNode.XPath));
                                        if (addPin2 == null || addPin2.Displayed == false || addPin2.Enabled == false)
                                        {
                                            LogError("addPin2 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        addPin2.Click();
                                        Thread.Sleep(PageScrollSleep);
                                    }

                                    htmlDoc = GetHtmlDocument(driver);
                                    var addNewPin = htmlDoc.DocumentNode.SelectSingleNode("//div[@title = 'Create a Pin']");
                                    if (addNewPin == null || addNewPin.ParentNode == null)
                                    {
                                        LogError("addNewPin not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    IWebElement newPin = driver.FindElement(By.XPath(addNewPin.ParentNode.XPath));
                                    if (newPin == null || newPin.Displayed == false || newPin.Enabled == false)
                                    {
                                        LogError("newPin not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    newPin.Click();
                                    Thread.Sleep(PageScrollSleep);
                                }

                                Thread.Sleep(PageWaitSleep);
                                IWebElement title = driver.FindElement(By.XPath("//textarea[@class = 'TextArea__textArea TextArea__bold TextArea__light TextArea__enabled TextArea__large TextArea__wrap']"));
                                if (title == null || title.Displayed == false || title.Enabled == false)
                                {
                                    LogError("title not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                actions.MoveToElement(title);
                                actions.Click();
                                title.SendKeys(sceItem.ProductTitle);
                                Thread.Sleep(PageScrollSleep);

                                IWebElement link = driver.FindElement(By.XPath("//textarea[@class = 'TextArea__textArea TextArea__dark TextArea__enabled TextArea__medium TextArea__nowrap']"));
                                if (link == null || link.Displayed == false || link.Enabled == false)
                                {
                                    LogError("link not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                actions.MoveToElement(link);
                                actions.Click();
                                var withoutHttpPart = sceItem.GeneralImage.Replace("http://", string.Empty)
                                                                          .Replace("https://", string.Empty);
                                if (withoutHttpPart.Contains("/"))
                                {
                                    var store = withoutHttpPart.Substring(0, withoutHttpPart.IndexOf("/", StringComparison.Ordinal));
                                    link.SendKeys($"https://{store}/product/{sceItem.SpiderURL}"/*$"https://www.mahoneswallpapershop.com/product/{sceItem.SpiderURL}"*/);
                                }
                                Thread.Sleep(PageScrollSleep);

                                IWebElement info = driver.FindElement(By.XPath("//textarea[@class = 'TextArea__textArea TextArea__light TextArea__enabled TextArea__medium TextArea__wrap']"));
                                if (info == null || info.Displayed == false || info.Enabled == false)
                                {
                                    LogError("info not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                actions.MoveToElement(info);
                                actions.Click();
                                string descWOtags = Regex.Replace(sceItem.Description, @"<[^>]*>", String.Empty);
                                string desc500 = descWOtags.Truncate(DescriptionLength);
                                if (desc500.Contains('.'))
                                {
                                    string descriptionPoint = desc500.Substring(0, desc500.LastIndexOf('.'));
                                    info.SendKeys(descriptionPoint);
                                }
                                else
                                {
                                    info.SendKeys(desc500);
                                }
                                Thread.Sleep(PageScrollSleep);

                                htmlDoc = GetHtmlDocument(driver);
                                var clickButton = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu F10 xD4 GmH adn a_A gpV hNT iyn BG7 gn8 L4E kVc _Vw']");
                                if (clickButton == null)
                                {
                                    clickButton = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu mix F10 xD4 GmH adn a_A gpV hNT iyn BG7 gn8 L4E kVc']");
                                    if (clickButton == null || clickButton.ParentNode == null)
                                    {
                                        LogError("clickButton not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                }
                                IWebElement click = driver.FindElement(By.XPath(clickButton.ParentNode.XPath));
                                if (click == null || click.Displayed == false || click.Enabled == false)
                                {
                                    LogError("click not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                click.Click();
                                Thread.Sleep(PageScrollSleep);

                                IWebElement addLink = driver.FindElement(By.XPath("//input[@id = 'pin-draft-website-link' and @placeholder = 'Enter website']"));
                                if (addLink == null || addLink.Displayed == false || addLink.Enabled == false)
                                {
                                    LogError("addLink not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                actions.MoveToElement(addLink);
                                actions.Click();
                                if (sceItem.GeneralImage.Contains(','))
                                {
                                    sceItem.GeneralImage = sceItem.GeneralImage.Substring(0, sceItem.GeneralImage.IndexOf(','));
                                }
                                addLink.SendKeys(sceItem.GeneralImage);
                                Thread.Sleep(PageScrollSleep);

                                htmlDoc = GetHtmlDocument(driver);
                                var clickButton2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-test-id = 'website-link-submit-button']");
                                if (clickButton2 == null || clickButton2.ParentNode == null)
                                {
                                    LogError("clickButton2 not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                IWebElement click2 = driver.FindElement(By.XPath(clickButton2.ParentNode.XPath));
                                if (click2 == null || click2.Displayed == false || click2.Enabled == false)
                                {
                                    LogError("click2 not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                click2.Click();
                                Thread.Sleep(PageScrollSleep);

                                Thread.Sleep(PageWaitSleep);
                                htmlDoc = GetHtmlDocument(driver);
                                var image = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'Pj7 sLG XiG ZKv mix m1e']");
                                if (image == null || image.ParentNode == null)
                                {
                                    LogError("image not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                IWebElement img = driver.FindElement(By.XPath(image.ParentNode.XPath));
                                if (img == null || img.Displayed == false || img.Enabled == false)
                                {
                                    LogError("img not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                img.Click();
                                Thread.Sleep(PageWaitSleep);
                                Thread.Sleep(PageWaitSleep);

                                htmlDoc = GetHtmlDocument(driver);
                                var addImage = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu Vxj aZc GmH adn Il7 Jrn hNT iyn BG7 NTm KhY jJP']");
                                if (addImage == null)
                                {
                                    addImage = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu mix Vxj aZc GmH adn Il7 Jrn hNT iyn BG7 NTm KhY']");
                                    if (addImage == null || addImage.ParentNode == null)
                                    {
                                        LogError("addImage not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                }
                                IWebElement addImg = driver.FindElement(By.XPath(addImage.ParentNode.XPath));
                                if (addImg == null || addImg.Displayed == false || addImg.Enabled == false)
                                {
                                    LogError("addImg not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                addImg.Click();
                                Thread.Sleep(PageWaitSleep);

                                htmlDoc = GetHtmlDocument(driver);
                                var saveProduct1 = htmlDoc.DocumentNode.SelectSingleNode("//button[@data-test-id = 'board-dropdown-select-button']");
                                if (saveProduct1 == null || saveProduct1.ParentNode == null)
                                {
                                    LogError("saveProduct1 not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                IWebElement save1 = driver.FindElement(By.XPath(saveProduct1.ParentNode.XPath));
                                if (save1 == null || save1.Displayed == false || save1.Enabled == false)
                                {
                                    LogError("save1 not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                save1.Click();
                                Thread.Sleep(PageScrollSleep);

                                htmlDoc = GetHtmlDocument(driver);
                                var saveProduct4 = htmlDoc.DocumentNode.SelectSingleNode("//input[@aria-label = 'Search through your boards']");
                                if (saveProduct4 == null || saveProduct4.ParentNode == null)
                                {
                                    LogError("saveProduct4 not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                IWebElement save4 = driver.FindElement(By.XPath(saveProduct4.XPath));
                                if (save4 == null || save4.Displayed == false || save4.Enabled == false)
                                {
                                    LogError("save4 not found");
                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                    continue;
                                }
                                actions.MoveToElement(save4);
                                actions.Click();
                                if (_actionType == ActionType.Brand)
                                {
                                    save4.SendKeys($"{sceItem.Brand}");
                                }
                                else if (_actionType == ActionType.MainCategory)
                                {
                                    save4.SendKeys($"{sceItem.MainCategory}");
                                }
                                else if (_actionType == ActionType.SubCategory)
                                {
                                    save4.SendKeys($"{sceItem.SubCategory}");
                                }
                                Thread.Sleep(PageScrollSleep);

                                try
                                {
                                    htmlDoc = GetHtmlDocument(driver);
                                    var saveProduct2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'C9q sLG ujU xvE zI7 iyn Hsu']");
                                    if (saveProduct2 == null)
                                    {
                                        saveProduct2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy yTZ pBj tg7 IZT mWe z-6' and @title = 'Create board']");
                                        if (saveProduct2 == null || saveProduct2.ParentNode == null)
                                        {
                                            LogError("saveProduct2 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement addNewBoard = driver.FindElement(By.XPath(saveProduct2.ParentNode.XPath));
                                        if (addNewBoard == null || addNewBoard.Displayed == false || addNewBoard.Enabled == false)
                                        {
                                            LogError("addNewBoard not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        addNewBoard.Click();
                                        Thread.Sleep(PageWaitSleep);

                                        htmlDoc = GetHtmlDocument(driver);
                                        var nameBoard = htmlDoc.DocumentNode.SelectSingleNode("//input[@class = 'wyq iyn Hsu aZc tBJ dyH iFc SMy yTZ L4E edc pBj qJc fgH']");
                                        if (nameBoard == null || nameBoard.ParentNode == null)
                                        {
                                            LogError("nameBoard not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement newNameBoard = driver.FindElement(By.XPath(nameBoard.XPath));
                                        if (newNameBoard == null || newNameBoard.Displayed == false || newNameBoard.Enabled == false)
                                        {
                                            LogError("newNameBoard not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        actions.MoveToElement(newNameBoard);
                                        actions.Click();
                                        newNameBoard.Clear();
                                        if (_actionType == ActionType.Brand)
                                        {
                                            newNameBoard.SendKeys($"{sceItem.Brand}");
                                        }
                                        else if (_actionType == ActionType.MainCategory)
                                        {
                                            newNameBoard.SendKeys($"{sceItem.MainCategory}");
                                        }
                                        else if (_actionType == ActionType.SubCategory)
                                        {
                                            newNameBoard.SendKeys($"{sceItem.SubCategory}");
                                        }
                                        Thread.Sleep(PageWaitSleep);

                                        htmlDoc = GetHtmlDocument(driver);
                                        var createBoard = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu mix Vxj aZc GmH adn Il7 Jrn hNT iyn BG7 NTm KhY']");
                                        if (createBoard == null || createBoard.ParentNode == null)
                                        {
                                            LogError("createBoard not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement create = driver.FindElement(By.XPath(createBoard.ParentNode.XPath));
                                        if (create == null || create.Displayed == false || create.Enabled == false)
                                        {
                                            LogError("create not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        create.Click();
                                        Thread.Sleep(PageScrollSleep);

                                        htmlDoc = GetHtmlDocument(driver);
                                        var saveProduct = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy yTZ erh DrD IZT mWe']");
                                        if (saveProduct == null || saveProduct.ParentNode == null)
                                        {
                                            LogError("saveProduct not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement saveProd = driver.FindElement(By.XPath(saveProduct.ParentNode.XPath));
                                        if (saveProd == null || saveProd.Displayed == false || saveProd.Enabled == false)
                                        {
                                            LogError("saveProd not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        saveProd.Click();
                                        Thread.Sleep(PageWaitSleep);
                                        Thread.Sleep(PageWaitSleep);
                                    }
                                    else
                                    {
                                        IWebElement save2 = driver.FindElement(By.XPath(saveProduct2.ParentNode.XPath));
                                        if (save2 == null || save2.Displayed == false || save2.Enabled == false)
                                        {
                                            LogError("save2 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        save2.Click();
                                        Thread.Sleep(PageWaitSleep);
                                        Thread.Sleep(PageWaitSleep);
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessagePrinter.PrintMessage($"Not found board for Brand - {sceItem.Brand}. Part Number - {sceItem.PartNumber}. Error: {e.Message}", ImportanceLevel.High);
                                }

                                try
                                {
                                    string pinURL = string.Empty;

                                    if (extSett.UseBusinessesAccount == false)
                                    {
                                        htmlDoc = GetHtmlDocument(driver);
                                        var saveProductNotBusiness = htmlDoc.DocumentNode.SelectSingleNode("//button[@data-test-id = 'board-dropdown-save-button']");
                                        if (saveProductNotBusiness != null)
                                        {
                                            var cancel = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'x8f INd _O1 gjz mQ8 OGJ YbY']");
                                            if (cancel == null)
                                            {
                                                IWebElement saveNotBusiness = driver.FindElement(By.XPath(saveProductNotBusiness.XPath));
                                                if (saveNotBusiness == null || saveNotBusiness.Displayed == false || saveNotBusiness.Enabled == false)
                                                {
                                                    LogError("saveNotBusiness not found");
                                                    driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                                    continue;
                                                }
                                                saveNotBusiness.Click();
                                                Thread.Sleep(PageWaitSleep);
                                                Thread.Sleep(PageScrollSleep);
                                            }
                                        }

                                        Thread.Sleep(PageWaitSleep);
                                        driver.Navigate().GoToUrl("https://www.pinterest.com");
                                        Thread.Sleep(PageWaitSleep);
                                        Thread.Sleep(PageScrollSleep);
                                    }
                                    else
                                    {
                                        htmlDoc = GetHtmlDocument(driver);
                                        var saveProduct3 = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy yTZ erh DrD IZT mWe']");
                                        if (saveProduct3 == null || saveProduct3.ParentNode == null)
                                        {
                                            LogError("saveProduct3 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        IWebElement save3 = driver.FindElement(By.XPath(saveProduct3.ParentNode.XPath));
                                        if (save3 == null || save3.Displayed == false || save3.Enabled == false)
                                        {
                                            LogError("save3 not found");
                                            driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                            continue;
                                        }
                                        save3.Click();
                                        Thread.Sleep(PageWaitSleep);
                                        Thread.Sleep(PageScrollSleep);

                                        htmlDoc = GetHtmlDocument(driver);
                                        var pinUrl = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='Jea LCN fBv jx- zI7 iyn Hsu']/a");
                                        if (pinUrl != null)
                                        {
                                            pinURL = pinUrl.AttributeOrNull("href");
                                        }

                                        Thread.Sleep(PageWaitSleep);
                                        driver.Navigate().GoToUrl("https://www.pinterest.com");
                                        Thread.Sleep(PageWaitSleep);
                                        Thread.Sleep(PageScrollSleep);
                                    }

                                    PinterestItems.Add(new PinterestItem()
                                    {
                                        PartNumber = sceItem.PartNumber,
                                        Brand = sceItem.Brand,
                                        MainCategory = sceItem.MainCategory,
                                        SubCategory = sceItem.SubCategory,
                                        PinUrl = pinURL,
                                        ProdId = sceItem.ProdId
                                    });

                                    MessagePrinter.PrintMessage($"Product processed. For Brand - {sceItem.Brand}. Part Number - {sceItem.PartNumber}.");
                                }
                                catch (Exception e)
                                {
                                    Thread.Sleep(PageScrollSleep);
                                    htmlDoc = GetHtmlDocument(driver);
                                    var homeButton = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'C9q XiG wYR zI7 iyn Hsu']");
                                    if (homeButton == null || homeButton.ParentNode == null)
                                    {
                                        LogError("homeButton not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    IWebElement home = driver.FindElement(By.XPath(homeButton.ParentNode.XPath));
                                    if (home == null || home.Displayed == false || home.Enabled == false)
                                    {
                                        LogError("home not found");
                                        driver.Navigate().GoToUrl($"https://www.pinterest.com");
                                        continue;
                                    }
                                    home.Click();
                                    Thread.Sleep(PageScrollSleep);
                                }
                            }
                        }
                    }

                    driver.Quit();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} {e.StackTrace}", ImportanceLevel.Critical);
            }
        }
    }
}
