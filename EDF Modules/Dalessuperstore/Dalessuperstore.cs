using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using Scraper.Shared;
using System.Web;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using Dalessuperstore;
using Dalessuperstore.DataItems;
using Databox.Libs.Dalessuperstore;

namespace WheelsScraper
{
    public class Dalessuperstore : BaseScraper
    {
        public Dalessuperstore()
        {
            Name = "Dalessuperstore";
            Url = "https://www.Dalessuperstore.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
        }

        Func<List<Filter>> GetSelectBrands { get; set; }

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
                frm.GetListBrands = GetListBrand;
                GetSelectBrands = frm.GetSelectBrands;
                return frm;
            }
        }

        private List<Filter> GetListBrand()
        {
            //List<string> exceptionList = new List<string> { "Flo~Pro", "Fleece Performance",
                //"BD Diesel", "aFe Power", "ARP", "Autometer | Competition Instruments",
                //"Bully Dog Technologies", "Edge", "FASS Diesel Fuel Systems®", "Ford",
                //"S&B Filters", "SCT"};

            List<Filter> brands = new List<Filter>();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var html = PageRetriever.ReadFromServer("https://dalessuperstore.com/p-24889-brands.html");
            var doc = CreateDoc(html);

            var nodes = doc.DocumentNode.SelectNodes("//ul[@class = 'widget_brands_links clearfix']/li/a");
            foreach (var item in nodes)
            {
                string name = item.AttributeOrNull("title").Replace("We Carry Products From ", string.Empty);

                //if (CheckBrands(name, exceptionList))
                //    continue;

                brands.Add(new Filter() { Name = name, Link = "https://dalessuperstore.com" + item.AttributeOrNull("href") });
            }

            return brands;
        }

        private bool CheckBrands(string brands, List<string> exceptionList)
        {
            foreach (string exceptionItem in exceptionList)
            {
                if (string.Equals(brands, exceptionItem,
                    StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
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
            var brands = GetSelectBrands();

            foreach (Filter filter in brands)
            {
                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem { URL = filter.Link, ItemType = ProcessType.ProcessingBrandPage });
                }
            }

            StartOrPushPropertiesThread();
        }

        protected void ProcessBrandPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var collectionProducts = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'wsm-cat-image']/a");
                if (collectionProducts != null)
                {
                    foreach (var item in collectionProducts)
                    {
                        string productLink;
                        productLink = item.AttributeOrNull("href");

                        if (!productLink.StartsWith("/"))
                            continue;
                        var pqi1 = new ProcessQueueItem();
                        pqi1.URL = "https://dalessuperstore.com" + productLink;
                        pqi1.ItemType = ProcessType.ProcessingProduct;
                        lock (this)
                        {
                            lstProcessQueue.Add(pqi1);
                        }
                    }
                    var nextPageElement = htmlDoc.DocumentNode.SelectSingleNode("//li[@class = 'wsm-cat-pag-next']/a");
                    if (nextPageElement != null)
                    {
                        string nextPage;
                        nextPage = nextPageElement.AttributeOrNull("href");
                        ProcessQueueItem pqi1 = new ProcessQueueItem();
                        pqi1.URL = nextPage;
                        pqi1.ItemType = ProcessType.ProcessingBrandPage;
                        lock (this)
                        {
                            lstProcessQueue.Add(pqi1);
                        }
                    }
                }
            }
            catch
            {
                MessagePrinter.PrintMessage($"Error in this page - {pqi.URL}", ImportanceLevel.High);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brand processed");
            StartOrPushPropertiesThread();
        }
        private void ProcessProduct(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                //pqi.URL = "https://dalessuperstore.com/i-23895060-bd-diesel-differential-cover-1989-2016-ford-single-wheel-w-sterling-12-10-25-10-5-rear-differential.html";
                //pqi.URL = "https://dalessuperstore.com/i-13336705-flo-pro-ss831nm-stainless-4-turbo-back-exhaust-no-muffler-1999-2003-ford-7-3l-powerstroke.html";
                //pqi.URL = "https://dalessuperstore.com/i-23898374-fleece-performance-63mm-fmw-cheetah-turbocharger-2015-2016-ford-powerstroke-6-7l.html";
                //pqi.URL = "https://dalessuperstore.com/i-23894231-flopro-4-cat-dpf-race-pipes-2007-2010-6-6l-gm-duramax-lmm.html";

                var wi = new ExtWareInfo();
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                wi.URL = pqi.URL;

                string hiddenUpsellOptions = string.Empty;
                JavaScriptSerializer jss = new JavaScriptSerializer();

                var script = htmlDoc.DocumentNode.SelectSingleNode("//script[contains(text(),'Select an option...')]").InnerTextOrNull();
                if (script != null)
                {
                    var scriptGroupValue = Regex.Matches(script, @"\{(.)*\}");

                    hiddenUpsellOptions = (from Group @group in scriptGroupValue
                                           where @group.Value.Contains("price")
                                           select jss.Deserialize<OptionItem>(@group.Value) 
                                           into responceInfo
                                           from value in responceInfo.Values
                                           where !value.Name.Contains("Select an option") && !value.Name.Contains("*****************")
                                           select value).Aggregate(hiddenUpsellOptions, (current, value) => current + $"Name:{value.Name}^Price:{value.Price}~!~");

                    //foreach (Group group in scriptGroupValue)                    //{                    //    if (!group.Value.Contains("price")) continue;                    //    var responceInfo = jss.Deserialize<OptionItem>(@group.Value);                    //    foreach (Values value in responceInfo.Values)                    //    {                    //        if (value.Name.Contains("Select an option") || value.Name.Contains("*****************"))                    //            continue;                    //        hiddenUpsellOptions += $"Name:{value.Name}^Price:{value.Price}~!~";                    //    }                    //}

                    if (!string.IsNullOrEmpty(hiddenUpsellOptions))
                        wi.HiddenUpsellOptions = hiddenUpsellOptions.Trim('~', '!');
                }

                string generalImage = string.Empty;

                var imageCollection = htmlDoc.DocumentNode.SelectNodes("//li[@class = 'wsm_product_thumb']/a[1]");
                if (imageCollection != null)
                {
                    foreach (var item2 in imageCollection)
                    {
                        generalImage += item2.AttributeOrNull("href") + ",";
                    }
                }
                else
                {
                    var image = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'productRotateImage']/a");
                    if (image != null)
                    {
                        generalImage = image.AttributeOrNull("href");
                    }
                }

                if (!string.IsNullOrEmpty(generalImage))
                    wi.GeneralImage = generalImage.TrimEnd(',');

                var productBrand = htmlDoc.DocumentNode.SelectSingleNode("//a[@itemprop = 'brand']");
                if (productBrand != null)
                {
                    wi.Brand = productBrand.InnerTextOrNull().Replace("~", " ");
                }

                var productTitle = htmlDoc.DocumentNode.SelectSingleNode("//h1[@itemprop = 'name']");
                if (productTitle != null)
                {
                    wi.ProductTitle = productTitle.InnerTextOrNull().Replace(wi.Brand, string.Empty).Trim('-');
                }

                var productPartNumber = htmlDoc.DocumentNode.SelectSingleNode("//span[@class = 'wsm-prod-sku']");
                if (productPartNumber != null)
                {
                    wi.PartNumber = productPartNumber.InnerTextOrNull();
                }

                var productSubTitle = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'wsm-prod-summary']");
                if (productSubTitle != null)
                {
                    if (!productSubTitle.InnerHtml.Contains("<li>"))
                        wi.SubTitle = productSubTitle.InnerTextOrNull();
                }

                var productPrice = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop = 'price']");
                if (productPrice != null)
                {
                    wi.Price = productPrice.InnerTextOrNull();
                }

                string fitmentCategories = string.Empty;

                var productFitmentCategories = htmlDoc.DocumentNode.SelectNodes("//ul[@class = 'productCats']/li");
                if (productFitmentCategories != null)
                {
                    int counter = productFitmentCategories.Count;

                    foreach (var fitment2 in productFitmentCategories)
                    {
                        if (counter == 1)
                            if (productFitmentCategories.Count == 1)
                                fitmentCategories += fitment2.InnerTextOrNull().Replace("-", "~!~");
                            else
                                break;
                        else
                        {
                            counter--;
                            fitmentCategories += fitment2.InnerTextOrNull().Replace("-", "~!~") + ":";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(fitmentCategories))
                    wi.FitmentCategories = fitmentCategories.Trim(':');

                var productWeight = htmlDoc.DocumentNode.SelectSingleNode("//li[@class = 'wsm_product_info_weight']");
                if (productWeight != null)
                {
                    wi.ItemWeight = productWeight.InnerTextOrNull();
                }

                var productManual = htmlDoc.DocumentNode.SelectSingleNode("//li[@class = 'wsm-file-pdf-small']/a");
                if (productManual != null)
                {
                    wi.PDFManual = "https://dalessuperstore.com" + productManual.AttributeOrNull("href");
                }

                string pBullets = string.Empty;

                var productBullets = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'wsm-prod-summary']/ul/li");
                if (productBullets != null)
                {
                    foreach (var bullets in productBullets)
                    {
                        pBullets += bullets.InnerTextOrNull() + "~!~";
                    }
                }

                if (!string.IsNullOrEmpty(pBullets))
                    wi.Bullets = pBullets.Trim('~', '!');

                var productUpsellTitle = htmlDoc.DocumentNode.SelectSingleNode("//label[@class = 'wsm_option_label wsm_option_select_label']");
                if (productUpsellTitle != null)
                {
                    wi.UpsellTitle = productUpsellTitle.InnerTextOrNull();
                }

                var productDescription = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'wsm-prod-tab-content']/p");
                if (productDescription != null)
                {
                    foreach (HtmlNode htmlNode in productDescription)
                    {
                        wi.Description += htmlNode.InnerTextOrNull();
                    }
                }
                else
                {
                    wi.Description = wi.SubTitle;
                }

                string features = string.Empty;

                var productDescriptionFeatures = htmlDoc.DocumentNode.SelectSingleNode("//h3[contains(text(),'Features')]");
                if (productDescriptionFeatures != null)
                {
                    var productFeatures = productDescriptionFeatures.NextSibling?.NextSibling?.SelectNodes(".//li");

                    if (productFeatures != null)
                    {
                        foreach (HtmlNode htmlNode in productFeatures)
                        {
                            features += htmlNode.InnerTextOrNull() + "~!~";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(features))
                    wi.DescriptionFeatures = features.Trim('~', '!');

                var productShippingDimensions = htmlDoc.DocumentNode.SelectSingleNode("//li[@class = 'wsm_product_info_dimensions']");
                if (productShippingDimensions != null)
                {
                    wi.ShippingDimensions = productShippingDimensions.InnerTextOrNull();
                }

                var productVideo = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'wsm-prod-tab-content']/iframe");
                if (productVideo != null)
                {
                    foreach (var video in productVideo)
                    {
                        wi.ProductVideo = video.AttributeOrNull("src");
                    }
                }

                var productFitmentDetails = htmlDoc.DocumentNode.SelectNodes("//li[@class = 'wsm_product_details_tags2']/a");
                if (productFitmentDetails != null)
                {
                    List<ExtWareInfo> waresForGroup = new List<ExtWareInfo>();
                    foreach (var fitment in productFitmentDetails)
                    {
                        ExtWareInfo fitmentWare = new ExtWareInfo(wi);

                        var fitmnt = fitment.InnerTextOrNull();
                        if (fitmnt != null)
                        {
                            var fitmentInfo = fitmnt.Replace($"\t", "");
                            var a = fitmentInfo.Split(new string[] { "\n" }, StringSplitOptions.None);

                            int counter = 1;
                            foreach (string s in a)
                            {
                                switch (counter)
                                {
                                    case 1:
                                        fitmentWare.Year = int.Parse(s);
                                        break;
                                    case 2:
                                        fitmentWare.Make = s;
                                        break;
                                    case 3:
                                        fitmentWare.Model = s;
                                        break;
                                    case 4:
                                        fitmentWare.Engine = s;
                                        break;
                                    default:
                                        break;
                                }
                                counter++;
                            }
                        }

                        waresForGroup.Add(fitmentWare);
                    }

                    List<ExtWareInfo> groupedWares = GroupWares(waresForGroup);

                    foreach (ExtWareInfo groupedWare in groupedWares)
                    {
                        //groupedWare.ProductTitle = groupedWare.ProductTitle
                        //    .Replace(groupedWare.StartYear.ToString(), string.Empty)
                        //    .Replace(groupedWare.EndYear.ToString(), string.Empty)
                        //    .Replace(groupedWare.Make, string.Empty)
                        //    .Replace(groupedWare.Engine, string.Empty)
                        //    .Trim('-', ' ');

                        AddWareInfo(groupedWare);
                        OnItemLoaded(groupedWare);
                    }
                }
                else
                {
                    AddWareInfo(wi);
                    OnItemLoaded(wi);
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error in this page - {pqi.URL}", ImportanceLevel.High);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Product page processed");
            StartOrPushPropertiesThread();
        }

        private List<ExtWareInfo> GroupWares(List<ExtWareInfo> waresForGroup)
        {
            List<ExtWareInfo> resultWares = new List<ExtWareInfo>();

            var newWares = waresForGroup.GroupBy(x => new
            {
                ((ExtWareInfo)x).PartNumber,
                ((ExtWareInfo)x).Brand,
                ((ExtWareInfo)x).Make,
                ((ExtWareInfo)x).Model,
                ((ExtWareInfo)x).Engine,
            }).Select(g => new ExtWareInfo
            {
                PartNumber = g.First().PartNumber,
                ProductTitle = g.First().ProductTitle,
                SubTitle = g.First().SubTitle,
                FitmentCategories = g.First().FitmentCategories,
                ItemWeight = g.First().ItemWeight,
                PDFManual = g.First().PDFManual,
                Bullets = g.First().Bullets,
                UpsellTitle = g.First().UpsellTitle,
                HiddenUpsellOptions = g.First().HiddenUpsellOptions,
                DescriptionFeatures = g.First().DescriptionFeatures,
                ShippingDimensions = g.First().ShippingDimensions,
                ProductVideo = g.First().ProductVideo,
                Brand = g.First().Brand,
                Price = g.First().Price,
                GeneralImage = g.First().GeneralImage,
                Description = g.First().Description,
                URL = g.First().URL,
                Make = ((ExtWareInfo)g.First()).Make,
                Model = ((ExtWareInfo)g.First()).Model,
                Engine = ((ExtWareInfo)g.First()).Engine,
            }).ToList();

            waresForGroup = waresForGroup.OrderBy(i => i.Year).ToList();

            foreach (ExtWareInfo newWare in newWares)
            {
                var firstOldWare = waresForGroup.First(i => i.Make == newWare.Make && i.Model == newWare.Model && i.Engine == newWare.Engine);
                var lastOldWare = waresForGroup.Last(i => i.Make == newWare.Make && i.Model == newWare.Model && i.Engine == newWare.Engine);

                newWare.StartYear = firstOldWare.Year;
                newWare.EndYear = lastOldWare.Year;

                resultWares.Add(new ExtWareInfo(newWare));
            }

            return resultWares;
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case ProcessType.ProcessingBrandPage:
                    act = ProcessBrandPage;
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
