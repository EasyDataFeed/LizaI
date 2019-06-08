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

namespace WheelsScraper
{
    public class Pinterest : BaseScraper
    {
        private const int PageWaitSleep = 6000;
        private const int PageScrollSleep = 3000;
        private const int PageSendKeysSleep = 1500;
        private const int DescriptionLength = 500;

        public Pinterest()
        {
            Name = "Pinterest";
            Url = "https://www.Pinterest.com/";
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

        protected override bool Login()
        {
            return true;
        }

        protected override void RealStartProcess()
        {
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
            
            List<PinterestItem> pinterestItems = new List<PinterestItem>();
            var pinterestFilePath = FileHelper.GetSettingsPath("AddedPinterestProducts.csv");

            if (File.Exists(pinterestFilePath))
            {
                pinterestItems = FileHelper.ReadPinterestAddItem(pinterestFilePath);
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

                sceExportItems = sceExportItems.Distinct(SceExportItem.PartNumberBrandComparer).ToList();

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

                    foreach (var sceItem in sceExportItems)
                    {
                        bool found = false;

                        foreach (var pinterestItem in pinterestItems)
                        {
                            if (cancel)
                                return;

                            if (sceItem.PartNumber == pinterestItem.PartNumber && sceItem.Brand == pinterestItem.Brand)
                            {
                                found = true;
                                break;
                            }
                        }

                        foreach (var brandItem in extSett.BrandItems)
                        {
                            if (cancel)
                                return;

                            if (sceItem.Brand != brandItem.Brand)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found == false)
                        {
                            MessagePrinter.PrintMessage($"Processing Product. For Brand - {sceItem.Brand}. Part Number - {sceItem.PartNumber}.");

                            if (extSett.UseBusinessesAccount)
                            {
                                Thread.Sleep(PageWaitSleep);
                                Thread.Sleep(PageScrollSleep);
                                htmlDoc = GetHtmlDocument(driver);
                                var addPinButtonBusiness = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy yTZ pBj tg7 mWe']");
                                IWebElement addPinBusiness = driver.FindElement(By.XPath(addPinButtonBusiness.ParentNode.XPath));
                                addPinBusiness.Click();
                                Thread.Sleep(PageScrollSleep);
                            }
                            else
                            {
                                Thread.Sleep(PageWaitSleep);
                                Thread.Sleep(PageScrollSleep);
                                htmlDoc = GetHtmlDocument(driver);
                                var addPinButton = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-test-id = 'addPinButton']");
                                IWebElement addPin = driver.FindElement(By.XPath(addPinButton.ParentNode.XPath));
                                addPin.Click();
                                Thread.Sleep(PageScrollSleep);

                                htmlDoc = GetHtmlDocument(driver);
                                var notValid = htmlDoc.DocumentNode.SelectSingleNode("//span[@class = 'buttonText']");
                                if (notValid != null)
                                {
                                    IWebElement not = driver.FindElement(By.XPath(notValid.ParentNode.XPath));
                                    not.Click();
                                    Thread.Sleep(PageScrollSleep);

                                    htmlDoc = GetHtmlDocument(driver);
                                    var addNewPin2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-test-id = 'addPinButton']");
                                    IWebElement addPin2 = driver.FindElement(By.XPath(addNewPin2.ParentNode.XPath));
                                    addPin2.Click();
                                    Thread.Sleep(PageScrollSleep);
                                }

                                htmlDoc = GetHtmlDocument(driver);
                                var addNewPin = htmlDoc.DocumentNode.SelectSingleNode("//div[@title = 'Create a Pin']");
                                IWebElement newPin = driver.FindElement(By.XPath(addNewPin.ParentNode.XPath));
                                newPin.Click();
                                Thread.Sleep(PageScrollSleep);
                            }

                            Thread.Sleep(PageWaitSleep);
                            IWebElement title = driver.FindElement(By.XPath("//textarea[@class = 'TextArea__textArea TextArea__bold TextArea__light TextArea__enabled TextArea__large TextArea__wrap']"));
                            actions.MoveToElement(title);
                            actions.Click();
                            title.SendKeys(sceItem.ProductTitle);
                            Thread.Sleep(PageScrollSleep);

                            IWebElement link = driver.FindElement(By.XPath("//textarea[@class = 'TextArea__textArea TextArea__dark TextArea__enabled TextArea__medium TextArea__nowrap']"));
                            actions.MoveToElement(link);
                            actions.Click();
                            link.SendKeys($"https://www.mahoneswallpapershop.com/product/{sceItem.SpiderURL}");
                            Thread.Sleep(PageScrollSleep);

                            IWebElement info = driver.FindElement(By.XPath("//textarea[@class = 'TextArea__textArea TextArea__light TextArea__enabled TextArea__medium TextArea__wrap']"));
                            actions.MoveToElement(info);
                            actions.Click();
                            string descWOtags = Regex.Replace(sceItem.Description, @"<[^>]*>", String.Empty);
                            string desc500 = descWOtags.Truncate(DescriptionLength);
                            string descriptionPoint = desc500.Substring(0, desc500.LastIndexOf('.'));
                            info.SendKeys(descriptionPoint);
                            Thread.Sleep(PageScrollSleep);

                            htmlDoc = GetHtmlDocument(driver);
                            var clickButton = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu F10 xD4 GmH adn a_A gpV hNT iyn BG7 gn8 L4E kVc _Vw']");
                            IWebElement click = driver.FindElement(By.XPath(clickButton.ParentNode.XPath));
                            click.Click();
                            Thread.Sleep(PageScrollSleep);

                            IWebElement addLink = driver.FindElement(By.XPath("//input[@id = 'pin-draft-website-link' and @placeholder = 'Enter website']"));
                            actions.MoveToElement(addLink);
                            actions.Click();
                            addLink.SendKeys(sceItem.GeneralImage);
                            Thread.Sleep(PageScrollSleep);

                            htmlDoc = GetHtmlDocument(driver);
                            var clickButton2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-test-id = 'website-link-submit-button']");
                            IWebElement click2 = driver.FindElement(By.XPath(clickButton2.ParentNode.XPath));
                            click2.Click();
                            Thread.Sleep(PageScrollSleep);

                            Thread.Sleep(PageWaitSleep);
                            htmlDoc = GetHtmlDocument(driver);
                            var image = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'Pj7 sLG XiG ZKv mix m1e']");
                            IWebElement img = driver.FindElement(By.XPath(image.ParentNode.XPath));
                            img.Click();
                            Thread.Sleep(PageScrollSleep);

                            htmlDoc = GetHtmlDocument(driver);
                            var addImage = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'RCK Hsu Vxj aZc GmH adn Il7 Jrn hNT iyn BG7 NTm KhY jJP']");
                            IWebElement addImg = driver.FindElement(By.XPath(addImage.ParentNode.XPath));
                            addImg.Click();
                            Thread.Sleep(PageWaitSleep);

                            htmlDoc = GetHtmlDocument(driver);
                            var saveProduct1 = htmlDoc.DocumentNode.SelectSingleNode("//button[@data-test-id = 'board-dropdown-select-button']");
                            IWebElement save1 = driver.FindElement(By.XPath(saveProduct1.ParentNode.XPath));
                            save1.Click();
                            Thread.Sleep(PageScrollSleep);

                            htmlDoc = GetHtmlDocument(driver);
                            var saveProduct4 = htmlDoc.DocumentNode.SelectSingleNode("//input[@aria-label = 'Search through your boards']");
                            IWebElement save4 = driver.FindElement(By.XPath(saveProduct4.XPath));
                            actions.MoveToElement(save4);
                            actions.Click();
                            save4.SendKeys($"{sceItem.Brand}");
                            Thread.Sleep(PageScrollSleep);

                            try
                            {
                                htmlDoc = GetHtmlDocument(driver);
                                var saveProduct2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'C9q sLG ujU xvE zI7 iyn Hsu']");
                                IWebElement save2 = driver.FindElement(By.XPath(saveProduct2.ParentNode.XPath));
                                save2.Click();
                                Thread.Sleep(PageWaitSleep);
                            }
                            catch (Exception e)
                            {
                                MessagePrinter.PrintMessage($"Not found board for Brand - {sceItem.Brand}. Part Number - {sceItem.PartNumber}. Error: {e.Message}", ImportanceLevel.High);
                            }

                            try
                            {
                                if (extSett.UseBusinessesAccount == false)
                                {
                                    htmlDoc = GetHtmlDocument(driver);
                                    var saveProductNotBusiness = htmlDoc.DocumentNode.SelectSingleNode("//button[@class = 'rYa kVc adn yQo BG7' and @aria-label = 'close']");
                                    IWebElement saveNotBusiness = driver.FindElement(By.XPath(saveProductNotBusiness.ParentNode.XPath));
                                    saveNotBusiness.Click();
                                    Thread.Sleep(PageWaitSleep);
                                }
                                else
                                {
                                    htmlDoc = GetHtmlDocument(driver);
                                    var saveProduct3 = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy yTZ erh DrD IZT mWe']");
                                    IWebElement save3 = driver.FindElement(By.XPath(saveProduct3.ParentNode.XPath));
                                    save3.Click();
                                    Thread.Sleep(PageWaitSleep);

                                    htmlDoc = GetHtmlDocument(driver);
                                    var saveProduct = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'tBJ dyH iFc SMy yTZ pBj tg7 mWe']");
                                    IWebElement save = driver.FindElement(By.XPath(saveProduct.ParentNode.XPath));
                                    save.Click();
                                    Thread.Sleep(PageWaitSleep);

                                    Thread.Sleep(PageWaitSleep);
                                    driver.Navigate().GoToUrl("https://www.pinterest.com");
                                    Thread.Sleep(PageWaitSleep);
                                }

                                pinterestItems.Add(new PinterestItem()
                                {
                                    PartNumber = sceItem.PartNumber,
                                    Brand = sceItem.Brand,
                                    SpiderURL = $"https://www.mahoneswallpapershop.com/product/{sceItem.SpiderURL}"
                                });

                                MessagePrinter.PrintMessage($"Product processed. For Brand - {sceItem.Brand}. Part Number - {sceItem.PartNumber}.");
                            }
                            catch (Exception e)
                            {
                                Thread.Sleep(PageScrollSleep);
                                htmlDoc = GetHtmlDocument(driver);
                                var homeButton = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'C9q XiG wYR zI7 iyn Hsu']");
                                IWebElement home = driver.FindElement(By.XPath(homeButton.ParentNode.XPath));
                                home.Click();
                                Thread.Sleep(PageScrollSleep);
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

            string filePath = FileHelper.CreatePinterestAddItem(FileHelper.GetSettingsPath("AddedPinterestProducts.csv"), pinterestItems);
            MessagePrinter.PrintMessage($"File with added products created - {filePath}");
        }
    }
}
