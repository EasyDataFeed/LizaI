using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Dewalt;
using Databox.Libs.Dewalt;
using Dewalt.Helpers;
using System.Xml;
using System.Net;

namespace WheelsScraper
{
    public class Dewalt : BaseScraper
    {
        private List<string> NotProductPages { get; set; }
        private List<string> ExceptionPages { get; set; }
        public Dewalt()
        {
            Name = "Dewalt";
            Url = "https://www.dewalt.com";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
            Complete += Skraper_Complete;
        }

        private void Skraper_Complete(object sender, EventArgs e)
        {
            
        }

        private ExtSettings extSett
        {
            get { return (ExtSettings)Settings.SpecialSettings; }
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
            get { return new ExtWareInfo(); }
        }

        protected override bool Login()
        {
            return true;
        }

        private List<string> LoadLinksSite()
        {
            List<string> links = new List<string>();

            //Ссылка сайтмапа
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var html = PageRetriever.ReadFromServer("https://www.acmetools.com/sitemap_11301_3.xml.gz"/*"https://www.dewalt.com/sitemap.xml"*/, true);

            //Подгружаешь уже в XML обрати внимание!!!
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(html);

            var xmlLink = xmlDoc.GetElementsByTagName("loc");
            foreach (XmlNode o in xmlLink)
            {
                links.Add(o.InnerText);
            }

            //links = links.Distinct().ToList();
            return links;
        }

        protected override void RealStartProcess()
        {
            NotProductPages = new List<string>();
            ExceptionPages = new List<string>();
            int counter = 0;

            var links = LoadLinksSite();

            foreach (string link in links)
            {
                //if (!link.Contains("product"))
                //{
                //    continue;
                //}

                if (counter != 500)
                {
                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = 10,
                            URL = link
                        });
                    }
                }
                else
                {
                    break;
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
                //pqi.URL = "https://www.dewalt.com/products/product-launches/2016/new-pneumatic-construction-nailers-launch";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var wi = new ExtWareInfo();
                string sku = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='tab__title']").InnerTextOrNull();
                if (string.IsNullOrEmpty(sku))
                {
                    sku = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-bv-show = 'reviews']").AttributeOrNull("data-bv-productid");

                    if(string.IsNullOrEmpty(sku))
                    {
                        sku = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'pdp-bar__details']/div").InnerTextOrNull();
                    } 
                }

                

                if (!string.IsNullOrEmpty(sku))
                {
                    wi.URL = pqi.URL;
                    wi.PartNumber = sku;

                    //categories
                    var categoriesCollection = htmlDoc.DocumentNode.SelectNodes("//ol[@class='page-module__inner']/li/a");
                    if (categoriesCollection != null)
                    {
                        int counter = 0;
                        foreach (var categoryNode in categoriesCollection)
                        {
                            switch (counter)
                            {
                                case 1:
                                    string mainCategory = categoryNode.InnerTextOrNull();
                                    if (!string.IsNullOrEmpty(mainCategory))
                                        wi.MainCategory = mainCategory;
                                    break;
                                case 2:
                                    string subCategory = categoryNode.InnerTextOrNull();
                                    if (!string.IsNullOrEmpty(subCategory))
                                        wi.SubCategory = subCategory;
                                    break;
                                case 3:
                                    string sectionCategory = categoryNode.InnerTextOrNull();
                                    if (!string.IsNullOrEmpty(sectionCategory))
                                        wi.SectionCategory = sectionCategory;
                                    break;
                            }

                            counter++;
                        }
                    }
                    else
                    {
                        MessagePrinter.PrintMessage("It's not product page.");
                        return;
                    }

                    var productTitle = htmlDoc.DocumentNode.SelectSingleNode("//span[@class = 'pdp-details__name']");
                    if (productTitle != null)
                    {
                        wi.ProductTitle = $"Dewalt {sku} {productTitle.InnerTextOrNull()}";
                        wi.AnchorText = $"{sku} {productTitle.InnerTextOrNull()}";
                    }

                    var productDescription =
                        htmlDoc.DocumentNode.SelectSingleNode("//p[@class = 'pdp-details__description']");
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
                    if (!string.IsNullOrEmpty(metaKeywords))
                    {
                        wi.METAKeywords = metaKeywords;
                    }

                    string includes = string.Empty;
                    var includesNodeCollection =
                        htmlDoc.DocumentNode.SelectNodes("//ul[@class = 'pdp-specs__includes clear']/li");
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
                        htmlDoc.DocumentNode.SelectNodes("//ul[@class = 'pdp-specs__uses clear']/li");
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
                        htmlDoc.DocumentNode.SelectNodes("//ul[@class='pdp-specs__specifications']/li");
                    if (specCollectionNodeCollection != null)
                    {
                        foreach (var specNode in specCollectionNodeCollection)
                        {
                            string specName = specNode.SelectSingleNode(".//strong").InnerTextOrNull();
                            string specValue = specNode.SelectSingleNode(".//span").InnerTextOrNull();

                            specifications += $"{specName}~{specValue}^";
                        }
                    }

                    if (specifications != "Specification##")
                        wi.Specifications = specifications.Trim('^');

                    string script = htmlDoc.DocumentNode.SelectSingleNode("//script[@id='pdpGalleryThumbs']").InnerTextOrNull();
                    var htmlDoc1 = new HtmlDocument();
                    htmlDoc1.LoadHtml(script);

                    string generalImage = string.Empty;
                    var generalImageNodeCollection = htmlDoc1.DocumentNode.SelectNodes("//button[@class='pdp-imagery-thumbnail__trigger']");
                    if (generalImageNodeCollection != null)
                    {
                        foreach (var generalImageNode in generalImageNodeCollection)
                        {
                            generalImage += "https://www.dewalt.com" + generalImageNode.AttributeOrNull("data-zoom-image-src") + ".jpg" + ",";
                        }
                    }
                    //string image = string.Empty;
                    //var imageCollections = htmlDoc.DocumentNode.SelectNodes("//img[@class='pdp-imagery__image--zoom']");
                    //if (imageCollections != null)
                    //{
                    //    foreach (var imageCollection in imageCollections)
                    //    {
                    //        image += "https://www.dewalt.com" + imageCollection.AttributeOrNull("src") + ",";
                    //    }
                    //}

                    if (!string.IsNullOrEmpty(generalImage))
                        wi.GeneralImage = generalImage.Trim(',');

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
