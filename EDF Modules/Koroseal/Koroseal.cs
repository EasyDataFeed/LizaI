using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Koroseal;
using Databox.Libs.Koroseal;
using System.Net;
using System.Xml;

namespace WheelsScraper
{
    public class Koroseal : BaseScraper
    {
        private List<string> NotProductPages { get; set; }
        private List<string> ExceptionPages { get; set; }

        public Koroseal()
        {
            Name = "Koroseal";
            Url = "https://www.Koroseal.com/";
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

        private List<string> LoadLinksSite()
        {
            List<string> links = new List<string>();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var html = PageRetriever.ReadFromServer("https://koroseal.com/sitemap", true);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(html);

            var xmlLink = xmlDoc.GetElementsByTagName("loc");
            foreach (XmlNode o in xmlLink)
            {
                links.Add(o.InnerText);
            }

            return links;
        }

        protected override void RealStartProcess()
        {
            ExceptionPages = new List<string>();

            var links = LoadLinksSite();

            foreach (string link in links)
            {
                //if (!link.Contains(" "))
                //{
                //    continue;
                //}

                if (!link.Contains("products"))
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
            StartOrPushPropertiesThread();
        }

        protected void ProcessBrandsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //pqi.URL = "https://koroseal.com/products/wall-protection/handrails/korogard-handrails/korogard-hs5r-aluminum-handrail";
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var wi = new ExtWareInfo();
                var links = LoadLinksSite();
                string product = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='titleblock']/h2/span/following-sibling::text()[1]").InnerTextOrNull();
                if (!string.IsNullOrEmpty(product))
                {

                    wi.URL = pqi.URL;

                    var title = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='titleblock']/h1");
                    if (title != null)
                    {
                        wi.ProductTitle = title.InnerTextOrNull();
                    }

                    var productPartNumber = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='titleblock']/h2/span/following-sibling::text()[1]");
                    if (productPartNumber != null)
                    {
                        wi.ProductPartNumber = productPartNumber.InnerTextOrNull();
                    }

                    var specificColor = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='titleblock']/h2/span");
                    if (specificColor != null)
                    {
                        wi.SpecificColor = specificColor.InnerTextOrNull();
                    }

                    //var type = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='third specsitems']/p/span");
                    //if (type != null)
                    //{
                    //    wi.ProductType = type.InnerTextOrNull();
                    //}

                    var generalImage = htmlDoc.DocumentNode.SelectSingleNode("//a[@class='demo-trigger']/img");
                    if (generalImage != null)
                    {
                        var generalImageJpg = "https://koroseal.com" + generalImage.AttributeOrNull("src");

                        if (!generalImageJpg.EndsWith(".jpg"))
                        {
                            wi.GeneralImage = generalImageJpg + ".jpg";
                        }
                        else
                        {
                            wi.GeneralImage = generalImageJpg;
                        }
                    }

                    string prodSpec = string.Empty;
                    var specifications = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'third specsitems']/p");
                    if (specifications != null)
                    {
                        int counter = specifications.Count;
                        int curentSpec = 0;
                        string specification = "Specification##";

                        bool haveMaterial = false;
                        bool havePatternMatch = false;
                        bool haveType = false;
                        bool haveColor = false;

                        if (!string.IsNullOrEmpty(wi.SpecificColor)) haveColor = true;

                        foreach (var item in specifications)
                        {
                            var spec = item.InnerTextOrNull();
                            string specName = spec.Split(':')[0];

                            if (specName.Contains("Material"))
                            {
                                haveMaterial = true;
                            }
                            else if (specName.Contains("Pattern Match"))
                            {
                                havePatternMatch = true;
                            }
                            else if (specName.Contains("Type"))
                            {
                                haveType = true;
                            }
                        }

                        int whilecounter = 0;

                        while (counter != 0)
                        {
                            whilecounter++;

                            if (whilecounter > 20)
                            {
                                break;
                            }

                            foreach (var item in specifications)
                            {
                                var spec = item.InnerTextOrNull();

                                if (!spec.Contains(":"))
                                {
                                    continue;
                                }
                                string specName = spec.Split(':')[0];
                                string specValue = spec.Split(':')[1].Trim().Replace("\r\n", "");

                                if (curentSpec == 0 && specName.Contains("Material") && haveMaterial == true)
                                {
                                    specification += $"{specName}~{specValue}^";
                                    curentSpec++;
                                    counter--;
                                }
                                else if (curentSpec == 0 && haveMaterial == false)
                                {
                                    curentSpec++;
                                }

                                if (curentSpec == 1)
                                {
                                    specification += "Color~" + wi.SpecificColor + "^";
                                    curentSpec++;
                                }
                                else if (curentSpec == 1 && haveColor == false)
                                {
                                    curentSpec++;
                                }

                                if (curentSpec == 2 && specName.Contains("Pattern Match"))
                                {
                                    specification += $"{specName}~{specValue}^";
                                    curentSpec++;
                                    counter--;
                                }
                                else if (curentSpec == 2 && havePatternMatch == false)
                                {
                                    curentSpec++;
                                }

                                if (curentSpec == 3 && specName.Contains("Type"))
                                {
                                    if (specName.Contains("Product Type")) continue;

                                    specification += $"{specName}~{specValue}^";
                                    curentSpec++;
                                    counter--;
                                    wi.ProductType = specValue;
                                }
                                else if (curentSpec == 3 && haveType == false)
                                {
                                    curentSpec++;
                                }

                                if (curentSpec > 3)
                                {
                                    if (specName == "Brand")
                                    {
                                        wi.SubCategory = specValue;
                                    }

                                    if (specification.Contains($"{specName}~{specValue}")) continue;

                                    specification += $"{specName}~{specValue}^";
                                    curentSpec++;
                                    counter--;
                                }
                            }
                        }

                        if (specification != "Specification##")
                        {
                            wi.Specifications = specification.Trim('^');
                        }

                    }

                    if (wi.ProductType == "Type I")
                    {
                        wi.CrossSellSubCategory1 = wi.ProductType;
                    }

                    if (wi.ProductType == "Type II")
                    {
                        wi.CrossSellSubCategory2 = wi.ProductType;
                    }

                    wi.Brand = "Koroseal";
                    wi.MainCategory = "Wallcovering";
                    wi.Suppliers = "Mahones Commercial Wallpaper";
                    wi.Warehouse = "N/A";

                    if (wi.SpecificColor == wi.ProductPartNumber)
                    {
                        wi.Title = $"{wi.Brand} {wi.ProductTitle} {wi.ProductPartNumber} {wi.ProductType}";
                        wi.AnchorText = $"{wi.ProductTitle} {wi.ProductPartNumber} {wi.ProductType}";
                    }
                    else
                    {
                        wi.Title = $"{wi.Brand} {wi.ProductTitle} {wi.SpecificColor} {wi.ProductPartNumber} {wi.ProductType}";
                        wi.AnchorText = $"{wi.ProductTitle} {wi.SpecificColor} {wi.ProductPartNumber} {wi.ProductType}";
                    }
                    
                    wi.GeneralImageTags = wi.Title;

                    MessagePrinter.PrintMessage("Product page processed");

                    AddWareInfo(wi);
                    OnItemLoaded(wi);
                }
                else
                {
                    MessagePrinter.PrintMessage("It's not product page.");
                }
            }
            catch (Exception exc)
            {
                ExceptionPages.Add(pqi.URL);
                MessagePrinter.PrintMessage($"{exc.Message} on this page - {pqi.URL}", ImportanceLevel.High);
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
