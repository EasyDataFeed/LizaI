#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Databox.Libs.Terapeak;
using Terapeak.DataItems;
using Terapeak.Extensions;
using Terapeak.Helpers;
using WheelsScraper;

#endregion

namespace Terapeak
{
    public class Terapeak : BaseScraper
    {
        public Terapeak()
        {
            Name = "Terapeak";
            Url = "https://www.Terapeak.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Wares.Clear();

            SpecialSettings = new ExtSettings();
        }

        private const string Token = "4c0d42c85b29e1f3e34d200eaa1920e1552b3f34b7c2b6cce16dd3d4780fa6e6";
        private const string Domain = "https://sell.terapeak.com";
        private const int SellersPerPage = 20;

        private string _searchUrl;

        private ExtSettings ExtSett
        {
            get { return (ExtSettings) Settings.SpecialSettings; }
        }

        public override Type[] GetTypesForXmlSerialization()
        {
            return new[] {typeof (ExtSettings)};
        }

        public override Control SettingsTab
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
            get { return new ExtWareInfo(); }
        }

        protected override bool Login()
        {
            PageRetriever.ReadFromServer(ExtSett.ResearchUrl, true);
            var loginInfo = GetLoginInfo();
            string data = "username=" + HttpUtility.UrlEncode(loginInfo.Login) + "&password=" +
                          HttpUtility.UrlEncode(loginInfo.Password) + "&r=y&login=&url=%2Fverify%2F";

            var html = PageRetriever.WriteToServer("https://data.terapeak.com/verify/", data, true);
            if (!html.Contains("Wrong username or password"))
            {
                return true;
            }
            return false;
        }

        protected override void RealStartProcess()
        {
            Wares.Clear();
            _searchUrl = ExtSett.ResearchUrl;
            lock (this)
            {
                lstProcessQueue.Add(new ProcessQueueItem
                {
                    URL = _searchUrl,
                    ItemType = ItemType.CategoriesList
                });
            }
            StartOrPushPropertiesThread();
        }

        private void ProcessCategoriesListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            MessagePrinter.PrintMessage("Category list process");
            try
            {
                var searchFilter = pqi.URL.GetSearchFilter();
                var data = searchFilter.GetJsonCategoriesReq();
                var url = ScraperHelper.GetCategoriesListUrl(Token, searchFilter.PageName);

                PageRetriever.ContentType = "application/json";
                PageRetriever.Accept = "application/json, text/javascript, */*; q=0.01";
                PageRetriever.Origin = Domain;

                var json = PageRetriever.WriteToServer(url, data, true);
                var jss = new JavaScriptSerializer();
                var categoriesList = jss.Deserialize<Dictionary<string, CategoryResearch>>(json);

                if (categoriesList.Any())
                {
                    foreach (var category in categoriesList.Values)
                    {
                        if (category.childList.Length == 0)
                        {
                            var sellerFilter = searchFilter.Convert2SellerFilter();
                            sellerFilter.CategoryId = category.category_id;
                            sellerFilter.Limit = SellersPerPage;
                            sellerFilter.CategoryFullName = category.category_fullname.PrepeadForLoad();
                            var pqiCat = new ProcessQueueItem
                            {
                                Name = category.category_name.PrepeadForLoad(),
                                URL = ScraperHelper.GetSellersListUrl(Token, searchFilter.PageName),
                                ItemType = ItemType.SellersList,
                                Item = sellerFilter
                            };
                            lock (this)
                            {
                                lstProcessQueue.Add(pqiCat);
                            }
                        }
                    }
                    MessagePrinter.PrintMessage("Category list processed");
                }
                else
                {
                    MessagePrinter.PrintMessage(string.Format("Category list is empty by URL: {0}", pqi.URL), ImportanceLevel.High);
                }
                pqi.Processed = true;
            }

            catch (Exception ex)
            {
                this.PrintError(ex, pqi);
            }
            StartOrPushPropertiesThread();
        }

        private void ProcessSellersListPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;
            var sellerFilter = (SellerFilter) pqi.Item;
            var pageNumber = sellerFilter.Offset/sellerFilter.Limit + 1;
            MessagePrinter.PrintMessage(string.Format("Process sellers list for {0}, Page: {1}", pqi.Name, pageNumber));
            try
            {
                var data = sellerFilter.GetJsonSellersReq();
                var url = pqi.URL;

                PageRetriever.ContentType = "application/json";
                PageRetriever.Accept = "application/json, text/javascript, */*; q=0.01";
                PageRetriever.Origin = Domain;

                var json = PageRetriever.WriteToServer(url, data, true);
                var jss = new JavaScriptSerializer();
                var sellerList = jss.Deserialize<Seller>(json).seller;
                if (sellerList.Any())
                {
                    foreach (var seller in sellerList)
                    {
                        lock (Wares)
                        {
                            var wi = Wares.FirstOrDefault(w => ((ExtWareInfo) w).SellerId == seller.SellerId);
                            if (wi != null)
                            {
                                lock (wi)
                                {
                                    ((ExtWareInfo) wi).TotalSales += seller.Revenue;
                                    ((ExtWareInfo) wi).TotalListings += seller.Listings;
                                }
                            }
                            else
                            {
                                var wiN = new ExtWareInfo
                                {
                                    SellerId = seller.SellerId,
                                    SellerName = seller.SellerName.PrepeadForLoad(),
                                    TotalSales = seller.Revenue,
                                    TotalListings = seller.Listings,
                                    Category = sellerFilter.CategoryFullName.PrepeadForLoad()
                                };
                                AddWareInfo(wiN);
                                OnItemLoaded(null);
                            }
                        }
                    }

                    if (sellerList.Length == SellersPerPage)
                    {
                        sellerFilter.Offset += SellersPerPage;
                        var pqiNextPage = new ProcessQueueItem
                        {
                            Name = pqi.Name,
                            URL = ScraperHelper.GetSellersListUrl(Token, sellerFilter.PageName),
                            ItemType = ItemType.SellersList,
                            Item = sellerFilter.Clone()
                        };
                        lock (this)
                        {
                            lstProcessQueue.Add(pqiNextPage);
                        }
                    }
                    MessagePrinter.PrintMessage(string.Format("Sellers list for {0}, Page: {1} processed", pqi.Name,
                        pageNumber));
                }
                else
                {
                    MessagePrinter.PrintMessage(
                        string.Format("Not found sellers for category: {0}", sellerFilter.CategoryFullName),
                        ImportanceLevel.High);
                }
                pqi.Processed = true;
            }
            catch (WebException)
            {
                var pqiStruct = new ProcessQueueItem
                {
                    URL = _searchUrl.CategorySearchUrl(sellerFilter.CategoryId),
                    ItemType = ItemType.CategoriesList
                };
                lock (this)
                {
                    lstProcessQueue.Add(pqiStruct);
                }
                MessagePrinter.PrintMessage(
                    string.Format("Bad response from server. Run Category list process for: {0}",
                        sellerFilter.CategoryFullName),
                    ImportanceLevel.Mid);
                pqi.Processed = true;
            }
            catch (Exception ex)
            {
                this.PrintError(ex, pqi);
            }
            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            switch (item.ItemType)
            {
                case ItemType.CategoriesList:
                    act = ProcessCategoriesListPage;
                    break;
                case ItemType.SellersList:
                    act = ProcessSellersListPage;
                    break;
                default:
                    act = null;
                    break;
            }
            return act;
        }
    }
}
