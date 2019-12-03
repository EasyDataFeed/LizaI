#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Uttermost;
using Databox.Libs.Uttermost;
using System.Net;

#endregion

namespace WheelsScraper
{
    public class Uttermost : BaseScraper
    {
        private readonly string UttermostLoginPage = "https://www.uttermost.com/wholesale_signin.aspx";
        public Uttermost()
        {
            Name = "Uttermost";
            Url = "https://www.Uttermost.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
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
            var account = Settings.Logins.FirstOrDefault();
            if (account != null)
            {
                string data = $"GuesCheckout=login&ShowForgetPassword=True&Email={account.Login}&Password={account.Password}&RememberMe=true";
                string actionUrl = "https://www.uttermost.com/sign-in";

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var html = PageRetriever.WriteToServer(actionUrl, data, true);
                var htmlDoc = CreateDoc(html);
                htmlDoc.LoadHtml(html);

                var logOut = htmlDoc.DocumentNode.SelectSingleNode("//li[@class='nav__item sign-out']/a");
                if (logOut != null)
                {
                    if (logOut.InnerTextOrNull() == "Logout")
                    {
                        MessagePrinter.PrintMessage("Loggin succes");
                        return true;
                    }
                }
            }
            throw new Exception("Login failed");
        }

        #endregion    

        protected override void RealStartProcess()
        {
            lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = (int)ItemType.ProcessingMainPage });
            StartOrPushPropertiesThread();
        }

        protected void ProcessMainPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                pqi.URL = "https://www.uttermost.com/Accent-Furniture-Shop-By-Type-Tables/";
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);
                int counter = 0;

                string nextPageLink = htmlDoc.DocumentNode.SelectSingleNode("//a[@class = 'pageLink__next']").AttributeOrNull("href");
                if (nextPageLink != null)
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = (int)ItemType.ProcessingMainPage,
                            URL = "https://www.uttermost.com/Accent-Furniture-Shop-By-Type-Tables/" + nextPageLink,
                        });
                    }
                }

                //pqi.URL = "https://www.uttermost.com/";
                //var html = PageRetriever.ReadFromServer(pqi.URL, true);
                //var htmlDoc = new HtmlDocument();
                //html = HttpUtility.HtmlDecode(html);
                //htmlDoc.LoadHtml(html);
                //int counter = 0;

                //HtmlNodeCollection allUrlCollection = htmlDoc.DocumentNode.SelectNodes("//li[@class='emphasis']/a");
                HtmlNodeCollection allUrlCollection = htmlDoc.DocumentNode.SelectNodes("//ul[@id='desktop']/li/ul/li/a");
                if (allUrlCollection != null)
                {
                    foreach (HtmlNode node in allUrlCollection)
                    {
                        string checkUrl = node.InnerTextOrNull();
                        if (checkUrl != null)
                        {
                            if (!checkUrl.Contains("View All"))
                            {
                                counter++;
                                string categoryUrl = "https://www.uttermost.com" + node.AttributeOrNull("href");

                                lock (this)
                                {
                                    lstProcessQueue.Add(new ProcessQueueItem()
                                    {
                                        ItemType = (int)ItemType.ProcessingGridPage,
                                        URL = categoryUrl
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessagePrinter.PrintMessage($"Categories on main Page not found", ImportanceLevel.Critical);
                }

                MessagePrinter.PrintMessage($"Main Page processed - {counter} categories found");
                pqi.Processed = true;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this Url - {pqi.URL}", ImportanceLevel.Critical);
                pqi.Processed = false;
            }

            StartOrPushPropertiesThread();
        }

        private void ProcessGridPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                HtmlNodeCollection productsCollection = htmlDoc.DocumentNode.SelectNodes("//div[@class='img_thumb_border']/a");
                if (productsCollection != null)
                {
                    foreach (HtmlNode node in productsCollection)
                    {
                        string productUrl = "https://www.uttermost.com" + node.AttributeOrNull("href");

                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                ItemType = (int)ItemType.ProcessingProduct,
                                URL = productUrl
                            });
                        }
                    }
                }

                string nextPageLink = htmlDoc.DocumentNode.SelectSingleNode("//a[contains(text(),'»')]").AttributeOrNull("href");

                if (!string.IsNullOrEmpty(nextPageLink))
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = (int)ItemType.ProcessingGridPage,
                            URL = "https://www.uttermost.com/" + nextPageLink,
                        });
                    }


                MessagePrinter.PrintMessage("Main Page processed");
                pqi.Processed = true;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this Url - {pqi.URL}", ImportanceLevel.Critical);
                pqi.Processed = false;
            }

            StartOrPushPropertiesThread();
        }

        private void ProcessProduct(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                //pqi.URL = "https://www.uttermost.com/p-4471-aegean.aspx?EID=153&EN=Category";
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                ExtWareInfo wi = new ExtWareInfo();

                #region Categories

                string subCategory = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='SectionTitleText'][1]").InnerTextOrNull();
                if (!string.IsNullOrEmpty(subCategory))
                    wi.SubCategory = subCategory;

                string sectionCategory = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='SectionTitleText'][2]").InnerTextOrNull();
                if (!string.IsNullOrEmpty(sectionCategory))
                    wi.SectionCategory = sectionCategory;

                #endregion

                #region Description

                string description = htmlDoc.DocumentNode.SelectSingleNode("//td[@class='prodDetailText']/p")
                    .InnerTextOrNull();
                if (!string.IsNullOrEmpty(description))
                {
                    wi.Description = description;
                }

                #endregion

                #region Cost

                string cost = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='RegularPrice']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(cost))
                {
                    wi.CostPrice = cost.Replace("Price:", string.Empty).Trim();
                }

                #endregion

                #region Retail Price

                string retailPrice = htmlDoc.DocumentNode.SelectSingleNode("//p[@class='detail-prices']/small")
                    .InnerTextOrNull();
                if (!string.IsNullOrEmpty(retailPrice))
                {
                    wi.RetailPrice = retailPrice.Replace("Suggested Retail Price:", string.Empty).Trim();
                }

                #endregion

                #region Specifications

                HtmlNodeCollection specificationCollection = htmlDoc.DocumentNode.SelectNodes("//table[@class='specs']/tr");
                if (specificationCollection == null)
                    specificationCollection = htmlDoc.DocumentNode.SelectNodes("//div[@class='divShow']/table/tr");
                if (specificationCollection != null)
                {
                    string specifications = string.Empty;
                    string SPECIFICATIONS = "Specification##";
                    specifications += $"{SPECIFICATIONS}";
                    foreach (var itemSpecific in specificationCollection)
                    {
                        string header = itemSpecific.SelectSingleNode(".//td/b").InnerTextOrNull();
                        string value = itemSpecific.SelectSingleNode(".//td[2]").InnerTextOrNull();
                        string assemblyLink = itemSpecific.SelectSingleNode(".//a").AttributeOrNull("href");
                        if (assemblyLink != null)
                            value = assemblyLink;
                        specifications += $"{header}~{value}^";

                        wi.Bullets += $"{header} {value}~!~";

                        switch (header)
                        {
                            case "Designer:":
                                wi.Designer = value;
                                break;
                            case "Construction:":
                                wi.Construction = value;
                                break;
                            case "Material:":
                                wi.Material = value;
                                break;
                            case "Dimensions:":
                                wi.Dimensions = value;
                                break;
                            case "Weight (lbs):":
                                wi.Weight = value;
                                break;
                            case "Ship Via UPS:":
                                wi.ShipViaUps = value;
                                break;
                            case "UPC Number:":
                                wi.UpcNumber = value;
                                break;
                            case "Wattage/Bulb Type:":
                                wi.WattageBulbType = value;
                                break;
                            case "# of Bulbs:":
                                wi.OfBulbs = value;
                                break;
                            case "Availability:":
                                wi.Availability = value;
                                break;
                            case "Assembly Instructions:":
                                string assembly = itemSpecific.SelectSingleNode(".//a").AttributeOrNull("href");
                                if (assembly != null)
                                    wi.AssemblyInstructions = assembly;
                                break;

                        }

                    }
                    if (!string.IsNullOrEmpty(specifications))
                    {
                        wi.Specifications = specifications.Trim('^');
                        wi.Bullets = wi.Bullets.Substring(0, wi.Bullets.LastIndexOf("~!~"));
                    }
                }

                #endregion

                #region Please Note

                string pleaseNote = htmlDoc.DocumentNode.SelectSingleNode("//b[contains(text(),'Please note:')]").NextSibling.InnerTextOrNull();
                if (!string.IsNullOrEmpty(pleaseNote))
                    wi.PleaseNote = pleaseNote;

                #endregion

                #region General Image

                string generalImage = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='ProductDiv']/img").AttributeOrNull("src");
                if (!string.IsNullOrEmpty(generalImage))
                    wi.GeneralImage = "https://www.uttermost.com" + generalImage;

                #endregion

                #region Other images

                HtmlNodeCollection otherImagesCollection = htmlDoc.DocumentNode.SelectNodes("//img[@class='alt-image']");
                if (otherImagesCollection != null)
                {
                    string otherImages = string.Empty;
                    foreach (HtmlNode node in otherImagesCollection)
                    {
                        string image = "https://www.uttermost.com/" + node.AttributeOrNull("src");
                        if (!image.Contains(".gif"))
                            otherImages += $"{image},";
                    }
                    if (otherImages != string.Empty)
                        wi.OtherImages = otherImages.Trim(',');
                }

                #endregion

                #region Part Number

                string sku = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='ProductSku']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(sku))
                    wi.PartNumber = sku.Substring(sku.LastIndexOf("#") + 1);
                else
                {
                    sku = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='ProductSKU']").InnerTextOrNull();
                    if (!string.IsNullOrEmpty(sku))
                        wi.PartNumber = sku.Substring(sku.LastIndexOf("#") + 1);
                }

                #endregion

                #region Product Title

                string productTitle = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='ProductNameText']").InnerTextOrNull();
                if (!string.IsNullOrEmpty(productTitle))
                {
                    wi.ProductTitle = $"{sku.Substring(sku.LastIndexOf("#") + 1)} | {productTitle} - Uttermost";
                }

                #endregion

                wi.URL = pqi.URL;

                AddWareInfo(wi);
                OnItemLoaded(wi);

                MessagePrinter.PrintMessage("Product processed");
                pqi.Processed = true;
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"{e.Message} on this Url - {pqi.URL}", ImportanceLevel.High);
                pqi.Processed = false;
            }

            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case (int)ItemType.ProcessingMainPage:
                    act = ProcessMainPage;
                    break;
                case (int)ItemType.ProcessingGridPage:
                    act = ProcessGridPage;
                    break;
                case (int)ItemType.ProcessingProduct:
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
