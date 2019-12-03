using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using GreenHouseFabricsScraper;
using Databox.Libs.GreenHouseFabricsScraper;
using GreenHouseFabricsScraper.DataItems;
using GreenHouseFabricsScraper.Helpers;

namespace WheelsScraper
{
    public class GreenHouseFabricsScraper : BaseScraper
    {
        public GreenHouseFabricsScraper()
        {
            Name = "GreenHouseFabricsScraper";
            Url = "https://www.GreenHouseFabricsScraper.com/";
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
            try
            {
                MessagePrinter.PrintMessage("Starting login");
                var Login = GetLoginInfo();
                if (Login == null)
                    throw new Exception("No valid login found");

                string loginURL = "https://www.greenhousefabrics.com/user/login?destination=home";
                var html = PageRetriever.ReadFromServer(loginURL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = CreateDoc(html);
                var form = htmlDoc.DocumentNode.SelectSingleNode("//form[@id='user-login']/div");

                if (form != null)
                {
                    string login, password, formBuildId, formId, op;
                    string dataPOST;

                    login = Login.Login;
                    password = Login.Password;
                    
                    var tagInput1 = form.SelectSingleNode("input[@name='form_build_id']");
                    if (tagInput1 != null)
                    {
                        formBuildId = HttpUtility.UrlEncode(tagInput1.Attributes["value"].Value);
                    }
                    else
                    {
                        formBuildId = string.Empty;
                    }
                    
                    var tagInput2 = form.SelectSingleNode("input[@name='form_id']");
                    if (tagInput2 != null)
                    {
                        formId = HttpUtility.UrlEncode(tagInput2.Attributes["value"].Value);
                    }
                    else
                    {
                        formId = string.Empty;
                    }
                    
                    var tagInput3 = form.SelectSingleNode("div/input[@name='op']");
                    if (tagInput3 != null)
                    {
                        op = HttpUtility.UrlEncode(tagInput3.Attributes["value"].Value);
                    }
                    else
                    {
                        op = string.Empty;
                    }

                    dataPOST = string.Format("name={0}&pass={1}&form_build_id={2}&form_id={3}&op={4}",
                        login, password, formBuildId, formId, op);

                    html = PageRetriever.WriteToServer(loginURL, dataPOST, true, true);

                    if (html.Contains("Log out"))
                    {
                        MessagePrinter.PrintMessage("Login done.");
                        return true;
                    }
                    else if (html.Contains("Log Out"))
                    {
                        MessagePrinter.PrintMessage("Login done.");
                        return true;
                    }
                }

                if (html.Contains("Log out"))
                {
                    MessagePrinter.PrintMessage("Login done.");
                    return true;
                }
                else if (html.Contains("Log Out"))
                {
                    MessagePrinter.PrintMessage("Login done.");
                    return true;
                }
            }
            catch (Exception err)
            {
                MessagePrinter.PrintMessage(err.Message, ImportanceLevel.Critical);
            }

            MessagePrinter.PrintMessage("Login Failed");

            return false;
        }

        protected override void RealStartProcess()
        {
            int start = 0;
            bool next = false;

            do
            {
                next = false;
                HtmlJson html = ApiHelper.GetHtml(start);

                var htmlDocMainPage = new HtmlDocument();
                html.fabrics = HttpUtility.HtmlDecode(html.fabrics);
                htmlDocMainPage.LoadHtml(html.fabrics);

                var links = htmlDocMainPage.DocumentNode.SelectNodes("//div[@class = 'teaser fabric-teaser']/div[2]/a");
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        var linkProduct = link.AttributeOrNull("href");
                        var partNumber = link.InnerTextOrNull();

                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                ItemType = 10,
                                URL = $"https://www.greenhousefabrics.com{linkProduct}",
                                Name = partNumber
                            });
                        }
                    }
                }

                var nextPage = htmlDocMainPage.DocumentNode.InnerHtml;
                if (nextPage != null)
                {
                    if (nextPage.Contains("Load More Fabrics"))
                    {
                        next = true;
                        start += 100;
                    }
                }

            } while (next);

            StartOrPushPropertiesThread();
        }

        protected void ProcessProductPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var wi = new ExtWareInfo();

                var price = htmlDoc.DocumentNode.SelectSingleNode("//span[@class = 'uc-price']");
                if (price != null)
                {
                    wi.WholesalePrice = double.Parse(price.InnerTextOrNull().Replace("$", ""));
                }

                var use = htmlDoc.DocumentNode.SelectSingleNode("//span[@class = 'price-suffixes']");
                if (use != null)
                {
                    wi.Use = use.InnerTextOrNull().Replace("/", "");
                }

                var yards = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'field-fabric-inventory']");
                if (yards != null)
                {
                    wi.Yards = int.Parse(yards.InnerTextOrNull()
                        .Replace("¼", "")
                        .Replace("½", "")
                        .Replace("¾", "")
                        .Replace(" yards in stock", "")
                        .Replace(" yard in stock", "")
                        .Replace("\n", ""));
                }

                string productKeyword = string.Empty;
                var keywords = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'field-fabric-keywords inline']/a");
                if (keywords != null)
                {
                    foreach (var item in keywords)
                    {
                        productKeyword += $"{item.InnerTextOrNull()} ";
                    }
                }
                wi.Keywords = productKeyword.Trim(' ');

                var specifications = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'group-clearfix field-group-div']/div");
                if (specifications != null)
                {
                    string specs = "Specification##";

                    foreach (var specification in specifications)
                    {
                        string name = specification.SelectSingleNode(".//span").InnerTextOrNull();
                        if (!string.IsNullOrEmpty(name))
                        {
                            name = name.Trim(':');
                        }

                        string value = specification.SelectSingleNode(".//div").InnerTextOrNull();
                        if (string.IsNullOrEmpty(value))
                        {
                            value = specification.SelectSingleNode(".//span[2]").InnerTextOrNull();
                        }

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                        {
                            specs += $"{name}~{value}^";
                        }
                    }

                    if (specs != "Specification##")
                    {
                        wi.Specifications = specs.Trim('^');
                    }
                }

                string book = string.Empty;
                var bookSpec = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'book']/a");
                if (bookSpec != null)
                {
                    book = bookSpec.InnerTextOrNull();
                }

                var image = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'group-swatch field-group-div']/a");
                if (image != null)
                {
                    wi.Image = image.AttributeOrNull("href");
                }

                wi.URL = pqi.URL;
                wi.PartNumber = pqi.Name;

                MessagePrinter.PrintMessage("Product page processed");
                AddWareInfo(wi);
                OnItemLoaded(wi);
            }
            catch (Exception exc)
            {
                MessagePrinter.PrintMessage($"{exc.Message} on this page - {pqi.URL}", ImportanceLevel.High);
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 10)
                act = ProcessProductPage;
            else act = null;

            return act;
        }
    }
}
