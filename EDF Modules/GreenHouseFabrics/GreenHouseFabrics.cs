#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GreenHouseFabrics;
using Databox.Libs.GreenHouseFabrics;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Text.RegularExpressions;

#endregion


namespace WheelsScraper
{
    public class GreenHouseFabrics : BaseScraper
    {
        public GreenHouseFabrics()
        {
            Name = "GreenHouseFabrics";
            Url = "https://www.GreenHouseFabrics.com/";
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
                    //HtmlNode tagInput;
                    string login, password, formBuildId, formId, op;
                    string dataPOST;

                    login = Login.Login;
                    password = Login.Password;
                    // form_build_id
                    var tagInput1 = form.SelectSingleNode("input[@name='form_build_id']");
                    if (tagInput1 != null)
                    {
                        formBuildId = HttpUtility.UrlEncode(tagInput1.Attributes["value"].Value);
                    }
                    else
                    {
                        formBuildId = string.Empty;
                    }
                    // form_id
                    var tagInput2 = form.SelectSingleNode("input[@name='form_id']");
                    if (tagInput2 != null)
                    {
                        formId = HttpUtility.UrlEncode(tagInput2.Attributes["value"].Value);
                    }
                    else
                    {
                        formId = string.Empty;
                    }
                    //op
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

        #endregion


        protected override void RealStartProcess()
        {
            string loginURL = "https://www.greenhousefabrics.com";

            var html = PageRetriever.ReadFromServer(loginURL, true);
            html = HttpUtility.HtmlDecode(html);

            var htmlDoc = CreateDoc(html);

            string data = null;
            var tagHiddenInputs = htmlDoc.DocumentNode.SelectNodes("//form//input[@type='hidden' or @type='submit']");
            if (tagHiddenInputs != null)
            {
                foreach (var itemHiddenInputs in tagHiddenInputs)
                {
                    var name = itemHiddenInputs.GetAttributeValue("name", null);
                    var value = itemHiddenInputs.GetAttributeValue("value", null);
                    if (name != null)
                        data += name + "=" + value + "&";
                }
            }

            List<string> partnumbers = GetPartNumbersFromFile();
            List<ExtWareInfo> products = GetProductsByPartNumbers(partnumbers);

            foreach (var itemProduct in products)
            {
                lstProcessQueue.Add(new ProcessQueueItem { URL = Url, ItemType = 1, Item = itemProduct, Name = data + "fabric_search=" + Uri.EscapeDataString(itemProduct.PartNumber) });
            }

            StartOrPushPropertiesThread();
        }

        protected void ProcessBrandsListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            ExtWareInfo wi = (ExtWareInfo)pqi.Item;
            try
            {
                var html = PageRetriever.WriteToServer("https://www.greenhousefabrics.com/", pqi.Name, true);
                html = HttpUtility.HtmlDecode(html);

                if (html.Contains("No results found"))
                {
                    pqi.Processed = true;
                    MessagePrinter.PrintMessage(wi.PartNumber + " not found.");
                    return;
                }

                var htmlDoc = CreateDoc(html);
                var price = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='product-info sell-price']/span[@class='uc-price']").InnerTextOrNull();
                var inventory = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='field-fabric-inventory']").InnerTextOrNull();

                if (price == null)
                {
                    var info = pqi.Item as ExtWareInfo;
                    var product = htmlDoc.DocumentNode.SelectSingleNode($"//*[@id='greenhouse-search-results']/div/div/a[translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')= '{info.PartNumber.ToLower()}']");
                    if (product != null)
                    {
                        var link = product.GetAttributeValue("href", null);
                        html = PageRetriever.ReadFromServer("https://www.greenhousefabrics.com" + link, true);
                        html = HttpUtility.HtmlDecode(html);

                        htmlDoc = CreateDoc(html);
                        price = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='product-info sell-price']/span[@class='uc-price']").InnerTextOrNull();
                        inventory = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='field-fabric-inventory']").InnerTextOrNull();

                        if (price == null)
                        {
                            pqi.Processed = true;
                            MessagePrinter.PrintMessage(wi.PartNumber + " not found.");
                            return;
                        }
                    }
                    else
                    {
                        pqi.Processed = true;
                        MessagePrinter.PrintMessage(wi.PartNumber + " not found.");
                        return;
                    }
                }

                wi.Cost = ParseDouble(price.Replace("$", "").Trim());
                wi.MSRP = wi.Cost * 2;
                wi.WebPrice = wi.MSRP - (wi.MSRP * 0.3);

                var matchInventory = Regex.Match(inventory, @"(\d+)");
                if (matchInventory.Success && matchInventory.Groups.Count > 1)
                {
                    wi.Inventory = matchInventory.Groups[1].Value;
                }

                pqi.Processed = true;
                AddWareInfo(wi);
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message + " " + wi.PartNumber, ImportanceLevel.Critical);
            }

            OnItemLoaded(null);
            MessagePrinter.PrintMessage(wi.PartNumber + " processed");
            StartOrPushPropertiesThread(); 
        }

        private List<string> GetPartNumbersFromFile()
        {
            List<string> Partnumbers = new List<string>();

            var localPath = Settings.Location;
            var FilePath = localPath.Substring(0, localPath.Length - (localPath.Length - localPath.LastIndexOf('\\')) + 1) + "Data\\GreenhousePartNumbers.csv";

            using (var sr = File.OpenText(FilePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    csv.GetFieldHeaders();
                    while (csv.ReadNextRecord())
                    {
                        var partNumber = csv["Part Number"];
                        if (!string.IsNullOrEmpty(partNumber))
                        {
                            Partnumbers.Add(partNumber.Trim());
                        }
                    }
                }
            }

            return Partnumbers;
        }

        private List<ExtWareInfo> GetProductsByPartNumbers(List<string> partNumbers)
        {
            List<ExtWareInfo> Products = new List<ExtWareInfo>();

            var localPath = Settings.Location;
            var FilePath = localPath.Substring(0, localPath.Length - (localPath.Length - localPath.LastIndexOf('\\')) + 1) + "Data\\ExportFeedWallPaper.csv";

            using (var sr = File.OpenText(FilePath))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    csv.GetFieldHeaders();
                    while (csv.ReadNextRecord())
                    {
                        string productPartnumber = csv["Part Number"].Trim('@');
                        if (partNumbers.Contains(productPartnumber, StringComparer.OrdinalIgnoreCase))
                        {
                            ExtWareInfo wi = new ExtWareInfo();
                            wi.Prodid = csv["Prodid"];
                            wi.PartNumber = csv["Part Number"].Trim('@');
                            wi.ProductType = csv["Product Type"];

                            Products.Add(wi);
                        }
                    }
                }
            }

            Products = Products.GroupBy((x => new { x.PartNumber, x.Prodid })).Select(x => new ExtWareInfo { Prodid = x.Key.Prodid, PartNumber = x.Key.PartNumber, ProductType = x.First().ProductType }).ToList();

            return Products;
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 1)
                act = ProcessBrandsListPage;
            else act = null;

            return act;
        }
    }
}
