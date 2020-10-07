using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using DevExpress.XtraPrinting.Native;
using GoogleRequestsScraper.Extensions;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using WheelsScraper;

namespace GoogleRequestsScraper.Helpers
{
    public class SeleniumHelper
    {
        private const string Background = "background.js";
        private const string Manifest = "manifest.json";
        private const string ZipSettings = "proxy_auth.zip";
        private const int PageScrollSleep = 3000;
        private const int PageWaitSleep = 6000;
        private const string LoginUrl = "https://www.jffabrics.com/wp-json/myjf/v1/auth/login";
        private const string FileUrl = "https://www.jffabrics.com/jf-price-list/wholesale-price-usa-price-list/?format=excel";


        public string AuthorizeAndDownloadFile(ProxyInfo proxyInfo)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                string backgroundFile = string.Empty;
                using (Stream stream = assembly.GetManifestResourceStream("GoogleRequestsScraper.Resources.background.js"))
                using (StreamReader reader = new StreamReader(stream))
                    backgroundFile = reader.ReadToEnd();

                string manifestFile = string.Empty;
                using (Stream stream = assembly.GetManifestResourceStream("GoogleRequestsScraper.Resources.manifest.json"))
                using (StreamReader reader = new StreamReader(stream))
                    manifestFile = reader.ReadToEnd();

                List<string> files = new List<string>();
                FileHelper.CreateFile(FileHelper.GetSettingsPath(Background), backgroundFile.FillProfile(proxyInfo));
                FileHelper.CreateFile(FileHelper.GetSettingsPath(Manifest), manifestFile);
                files.Add(FileHelper.GetSettingsPath(Background));
                files.Add(FileHelper.GetSettingsPath(Manifest));
                FileHelper.CreateZipFile(FileHelper.GetSettingsPath(ZipSettings), files);

                var chromeOptions = new ChromeOptions();
                chromeOptions.AddExtension(FileHelper.GetSettingsPath(ZipSettings));
                //chromeOptions.AddUserProfilePreference("download.default_directory", FileHelper.GetSettingsPath("azaza.xls"));
                //chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                //chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

                using (IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, chromeOptions))
                {
                    driver.Navigate().GoToUrl("https://www.google.com");
                    Thread.Sleep(PageWaitSleep);
                    var htmlDocument = GetHtmlDocument(driver);

                    Actions actions = new Actions(driver);

                    IWebElement email = driver.FindElement(By.XPath("//input[@class = 'gLFyf gsfi' and @name = 'q']"));
                    actions.MoveToElement(email);
                    actions.Click();
                    email.SendKeys("Usps");
                    Thread.Sleep(PageScrollSleep);

                    //IWebElement pass = driver.FindElement(By.XPath("//input[@name = 'password' and @type = 'password']"));
                    //actions.MoveToElement(pass);
                    //actions.Click();
                    ////pass.SendKeys(password);
                    //Thread.Sleep(PageScrollSleep);


                    var submitButton = htmlDocument.DocumentNode.SelectSingleNode("//input[@type = 'submit']");
                    IWebElement signInButton = driver.FindElement(By.XPath("//input[@type = 'submit']"));
                    signInButton.Click();


                    Thread.Sleep(PageWaitSleep);
                    //download file
                    driver.Navigate().GoToUrl(FileUrl);


                    driver.Quit();
                    Thread.Sleep(PageScrollSleep);
                    return "";
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SeleniumHelper()
        {

        }

        private static HtmlDocument GetHtmlDocument(IWebDriver driver)
        {
            var answerHtml = driver.PageSource;
            var answerHtmlDoc = new HtmlDocument();
            answerHtml = HttpUtility.HtmlDecode(answerHtml);
            answerHtmlDoc.LoadHtml(answerHtml);

            return answerHtmlDoc;
        }
    }
}
