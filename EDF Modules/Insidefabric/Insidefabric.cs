using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Insidefabric;
using Databox.Libs.Insidefabric;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace WheelsScraper
{
    public class Insidefabric : BaseScraper
    {
        private readonly string FABRICS_BRANDS_URL = "https://www.insidefabric.com/manufacturers.aspx";
        private readonly string WALLPAPERS_BRANDS_URL = "https://www.insidewallpaper.com/manufacturers.aspx";
        Func<List<Brand>> GetSelectedBrands { get; set; }

        public Insidefabric()
        {
            Name = "Insidefabric";
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
                frm.GetBrandsList = GetBrandsList;
                GetSelectedBrands = frm.GetSelectedBrands;
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

        private List<Brand> GetBrandsList(string category)
        {
            Url = category == "Insidefabric" ? 
                "https://www.insidefabric.com/" : 
                "https://www.insidewallpaper.com/";

            List<Brand> brands = new List<Brand>();
            HtmlDocument doc = CreateDoc(PageRetriever.ReadFromServer(
                category == "Insidefabric" ? FABRICS_BRANDS_URL : WALLPAPERS_BRANDS_URL));

            var brandNodes = doc.DocumentNode.SelectNodes(
                "//*[@id='ctl00_PageContent_pnlContent']/div/div[1]/div/a");

            foreach (var brandNode in brandNodes)
            {
                string name = brandNode.FirstChild.FirstChild.Attributes["alt"].Value.Replace("&amp;", "and");
                string link = Url + brandNode.Attributes["href"].Value;

                brands.Add(new Brand
                {
                    Name = name,
                    Link = link
                });
            }

            return brands;
        }

        protected override void RealStartProcess()
        {
            lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = 1 });
            StartOrPushPropertiesThread();
        }

        protected void ProcessBrandsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            MessagePrinter.PrintMessage("Processing brands links");
            List<Brand> selectedBrands = GetSelectedBrands();

            if (selectedBrands.Count == 0)
            {
                MessagePrinter.PrintMessage("Brands list is empty");
            }
            else
            {
                foreach (Brand brand in selectedBrands)
                {
                    lock (this)
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = 2, Item = brand, URL = brand.Link
                        });
                }
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brands list processed");
            StartOrPushPropertiesThread();
        }

        protected void ProcessFirstBrandPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            MessagePrinter.PrintMessage("Reading brand pages");
            HtmlDocument doc = CreateDoc(PageRetriever.ReadFromServer(pqi.URL));

            // Remove tbody after table[1] in case of copying xpath from browser

            HtmlNode lastPage = doc.DocumentNode.SelectSingleNode(
                "//*[@id='ctl00_PageContent_pnlContent']/div[1]/table[1]/tr/td[2]/nav/div/div/a[2]|" +
                "//*[@id='ctl00_PageContent_pnlContent']/div[1]/table[1]/tr/td/nav/div/div/a[2]|" +
                "//*[@id='ctl00_PageContent_pnlContent']/div[1]/nav[1]/div/div/a[2]");

            int pagesCount = lastPage == null ? 0 :
                Int32.Parse(lastPage.InnerText);

            if(pagesCount == 0)
            {
                MessagePrinter.PrintMessage("There are no products in " + pqi.URL);
                pqi.Processed = true;
                return;
            }

            for (int pageNum = 1; pageNum <= pagesCount; pageNum++)
            {
                lock (this)
                    lstProcessQueue.Add(new ProcessQueueItem()
                    {
                        ItemType = 3,
                        Item = pqi.Item,
                        URL = pqi.URL + "?pagenum=" + pageNum
                    });
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brand pages processed");
            StartOrPushPropertiesThread();
        }

        protected void ProcessProductsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            MessagePrinter.PrintMessage("Processing products list page");
            HtmlDocument doc = CreateDoc(PageRetriever.ReadFromServer(pqi.URL));

            var products = doc.DocumentNode.SelectNodes(
                "//*[@id='productList']/article/div");

            foreach (var product in products)
            {
                string anchorText = product.SelectSingleNode(
                    "div[2]/span/a").InnerText.Replace("&amp;", "and");

                string webPrice = product.SelectSingleNode(
                    "div[3]/span/text()").InnerText.Substring(1);

                string url = Url.TrimEnd('/') + product.SelectSingleNode(
                    "div[1]/div[1]/div/a").Attributes["href"].Value;

                ExtWareInfo newWare = new ExtWareInfo
                {
                    ProductType = "1",
                    Brand = ((Brand)pqi.Item).Name,
                    AnchorText = anchorText,
                    SpiderUrl = anchorText.ToLower().Replace(" ", "-"),
                    WebPrice = webPrice,
                    URL = url
                };

                lock(this)
                    lstProcessQueue.Add(new ProcessQueueItem()
                    {
                        ItemType = 4,
                        Item = newWare,
                        URL = url
                    });
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Products list page processed");
            StartOrPushPropertiesThread();
        }

        protected void ProcessProductPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            ExtWareInfo ware = (ExtWareInfo)pqi.Item;

            MessagePrinter.PrintMessage("Processing product " + ware.AnchorText);
            HtmlDocument doc = CreateDoc(PageRetriever.ReadFromServer(pqi.URL));

            if(doc.DocumentNode.SelectSingleNode("//*[@id='ctl00_Head1']/title") == null)
            {
                MessagePrinter.PrintMessage("Error 404: " + pqi.URL, ImportanceLevel.High);
                pqi.Processed = true;
                return;
            }

            HtmlNode metaKeywordsNode = doc.DocumentNode.SelectSingleNode(
                "//*[@id='ctl00_Head1']/meta[4]");

            string metaKeywords = metaKeywordsNode == null ? "" :
                metaKeywordsNode.Attributes["content"].Value.Replace("&amp;", "and");

            HtmlNode retailPriceNode = doc.DocumentNode.SelectSingleNode(
                "//*[@id='productInfoSection']/div[1]/div/div[1]/div[1]/span");

            string retailPrice = retailPriceNode == null ? "" :
                retailPriceNode.InnerText.Substring(1);

            HtmlNode priceUnitNode = doc.DocumentNode.SelectSingleNode(
                "//*[@id='productInfoSection']/div[1]/div/div[5]/div[1]");

            string priceUnit = priceUnitNode == null ? "" : priceUnitNode.InnerText.Trim();

            HtmlNode minimumIndicator = doc.DocumentNode.SelectSingleNode(
                "//*[@id='indicateMinimum']");

            string minimum = minimumIndicator == null ? "" :
                minimumIndicator.InnerText.TrimStart('(').TrimEnd(')');

            HtmlNode generalImageNode = doc.DocumentNode.SelectSingleNode(
                "//*[@id='mainProductImage']");

            string generalImage = generalImageNode == null ? "" :
                Url + generalImageNode.FirstChild.Attributes["src"].
                Value.Replace("/medium/", "/large/");

            HtmlNode discontinuedNode = doc.DocumentNode.SelectSingleNode(
                "//*[@class='stockMessage']/table/tr/td/h2");

            string discontinued = discontinuedNode == null ? "" :
                "Yes";

            // Remove tbody before tr in case of copying xpath from browser

            var relatedProductsNodes = doc.DocumentNode.SelectNodes(
                "//*[@id='additionalImages']/tr/td/div/a[2]");

            string relatedProducts = relatedProductsNodes == null ? "" : string.Join(",",
                relatedProductsNodes.Select(p => Url.TrimEnd('/') + p.Attributes["href"].Value));

            if (relatedProducts != "")
            {
                ware.RelatedSKU = "";

                foreach (string url in relatedProducts.Split(','))
                {
                    lock (this)
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = 5,
                            Item = ware,
                            URL = url
                        });
                }
            }

            ware.METAKeywords = metaKeywords;
            ware.RetailPrice = retailPrice;
            ware.PriceUnit = priceUnit;
            ware.MinimumIndicator = minimum;
            ware.GeneralImage = generalImage;
            ware.Discontinued = discontinued;
            ware.RelatedProducts = relatedProducts;            

            var detailsTable = doc.DocumentNode.SelectNodes(
                "//*[@id='productInfoTable']/tr");

            if (detailsTable != null)
            {
                List<Tuple<string, string>> productDetails = GetProductDetails(detailsTable);

                Tuple<string, string> sku = productDetails.Find(t => t.Item1 == "SKU");
                Tuple<string, string> manufacturer = productDetails.Find(t => t.Item1 == "Manufacturer");
                Tuple<string, string> detailsProductType = productDetails.Find(t => t.Item1 == "Product Type");
                Tuple<string, string> categories = productDetails.Find(t => t.Item1 == "Categories");
                Tuple<string, string> crossSells = productDetails.Find(t => t.Item1 == "CrossSells");                

                ware.SKU = sku == null ? "" : sku.Item2;
                ware.Manufacturer = manufacturer == null ? "" : manufacturer.Item2;
                ware.DetailsProductType = detailsProductType == null ? "" : detailsProductType.Item2;
                ware.Categories = categories == null ? "" : categories.Item2;
                ware.CrossSells = crossSells == null ? "" : crossSells.Item2;

                productDetails.Remove(sku);
                productDetails.Remove(manufacturer);
                productDetails.Remove(detailsProductType);
                productDetails.Remove(categories);
                productDetails.Remove(crossSells);

                string spec = "PRODUCT DETAILS##";

                foreach (var pair in productDetails)
                {
                    spec += pair.Item1 + "~" + pair.Item2 + "^";
                }

                ware.Specification = spec.TrimEnd('^');
            }
            else
            {
                ware.Specification = "";
            }

            var featuredProductsNode = doc.DocumentNode.SelectSingleNode(
                "//*[@id='product']/div[1]/div/aside/script");

            ware.FeaturedProducts = featuredProductsNode == null ? "" : 
                GetFeaturedProducts(featuredProductsNode);            

            AddWareInfo(ware);
            OnItemLoaded(null);
            pqi.Processed = true;
            MessagePrinter.PrintMessage("Product processed");
            StartOrPushPropertiesThread();
        }

        private List<Tuple<string, string>> GetProductDetails(HtmlNodeCollection detailsTable)
        {
            List<Tuple<string, string>> productDetails = new List<Tuple<string, string>>();

            foreach (var row in detailsTable)
            {
                string attributeName = row.SelectSingleNode("th").InnerText;
                var attributeValueNode = row.SelectSingleNode("td");
                string attributeValue = "";
                string crossSells = "";

                if (attributeValueNode.ChildNodes.Count > 1)
                {
                    attributeValue = string.Join(",", attributeValueNode.SelectNodes("a").
                        Select(v => Regex.Replace(v.InnerText.Replace("&amp;", "and"), @"\s+", " ")));

                    if (attributeName == "Categories")
                    {
                        crossSells = "CATEGORIES##" + string.Join("^", attributeValueNode.SelectNodes("a").
                            Select(v => Regex.Replace(v.InnerText.Replace("&amp;", "and"), @"\s+", " ") +
                            "~" + Url.TrimEnd('/') + v.Attributes["href"].Value));

                        productDetails.Add(Tuple.Create("CrossSells", crossSells));
                    }
                }
                else
                {
                    attributeValue = Regex.Replace(attributeValueNode.InnerText.Replace("&amp;", "and"), @"\s+", " ");
                }

                productDetails.Add(Tuple.Create(attributeName, attributeValue));
            }

            return productDetails;
        }

        private string GetFeaturedProducts(HtmlNode featuredProductsNode)
        {
            string productsJS = featuredProductsNode.InnerText.
                Replace("\r", "").Replace("\n", "").Substring(17);

            var deserializer = new JavaScriptSerializer();
            dynamic featuredProducts = deserializer.DeserializeObject(productsJS);
            string result = "";

            for (int i = 0; i < 9; i++)
            {
                result += "Title: " + featuredProducts[i]["name"] + " Image: " + featuredProducts[i]["image"] + ";";
            }

            return result;
        }

        protected void ProcessRelatedSKU(ProcessQueueItem pqi)
        {            
            ExtWareInfo ware = (ExtWareInfo)pqi.Item;
            HtmlDocument doc = CreateDoc(PageRetriever.ReadFromServer(pqi.URL));
            HtmlNode skuNode = doc.DocumentNode.SelectSingleNode("//*[@id='productInfoTable']/tr[1]/td/span");

            if (skuNode != null)
            {
                if (ware.RelatedSKU == "")
                    ware.RelatedSKU = skuNode.InnerText;
                else if(skuNode.InnerText != "")
                    ware.RelatedSKU += "," + skuNode.InnerText;
            }

            pqi.Processed = true;
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 1)
                act = ProcessBrandsListPage;
            else if (item.ItemType == 2)
                act = ProcessFirstBrandPage;
            else if (item.ItemType == 3)
                act = ProcessProductsListPage;
            else if (item.ItemType == 4)
                act = ProcessProductPage;
            else if (item.ItemType == 5)
                act = ProcessRelatedSKU;
            else act = null;

            return act;
        }
    }
}
