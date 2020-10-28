using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using Scraper.Shared;
using System.Web;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using GoogleRequestsScraper;
using Databox.Libs.GoogleRequestsScraper;
using Google.Apis.Sheets.v4.Data;
using GoogleRequestsScraper.DataItems;
using GoogleRequestsScraper.Enums;
using GoogleRequestsScraper.Extensions;
using GoogleRequestsScraper.Helpers;
using System.Text.RegularExpressions;
using GoogleRequestsScraper.DataItems.MySql;
using GoogleRequestsScraper.Helpers.MySql;

namespace WheelsScraper
{
    public class GoogleRequestsScraper : BaseScraper
    {
        public List<UuleDataItem> Uules;
        public List<DomainsDataItem> Domains;
        private List<StatesDataItem> states = new List<StatesDataItem>();

        public GoogleRequestsScraper()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Name = "GoogleRequestsScraper";
            Url = "https://www.GoogleRequestsScraper.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            Uules = new List<UuleDataItem>();
            Domains = new List<DomainsDataItem>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
            Complete += GoogleRequestsScraper_Complete;
        }

        private void GoogleRequestsScraper_Complete(object sender, EventArgs e)
        {
            try
            {
                List<GoogleScrapedItem> googleScrapedItems = new List<GoogleScrapedItem>();
                googleScrapedItems.AddRange(Wares.Select(i => new GoogleScrapedItem()
                {
                    Device = ((ExtWareInfo)i).Device,
                    Placement = ((ExtWareInfo)i).Placement,
                    Domain = ((ExtWareInfo)i).Domain,
                    Time = ((ExtWareInfo)i).Time,
                    State = ((ExtWareInfo)i).State,
                    Keyword = ((ExtWareInfo)i).Keyword,
                    Position = ((ExtWareInfo)i).Position,
                    CompanyName = ((ExtWareInfo)i).CompanyName,
                    DumpPageId = ((ExtWareInfo)i).DumpPageId,
                    UniqueDomains = ((ExtWareInfo)i).UniqueDomains,
                    UniqueDomainsQty = ((ExtWareInfo)i).UniqueDomainsQty,
                    Title = ((ExtWareInfo)i).Title,
                }));

                if (googleScrapedItems.Any())
                {
                    var googleScrapedItemGrouped = googleScrapedItems.GroupBy(x => new
                    {
                        ((GoogleScrapedItem)x).CompanyName,
                        ((GoogleScrapedItem)x).Domain
                    });

                    foreach (var googleScrapedItem in googleScrapedItems)
                    {
                        int uniqueDomainsQty = 0;
                        var findAllGoogleScrapedItems = googleScrapedItemGrouped.ToList().FindAll(i => i.Key.CompanyName == googleScrapedItem.CompanyName);

                        foreach (var findAllGoogleScrapedItem in findAllGoogleScrapedItems)
                        {
                            string uniqueDomains = string.Empty;
                            if (!string.IsNullOrEmpty(googleScrapedItem.UniqueDomains))
                            {
                                uniqueDomains = googleScrapedItem.UniqueDomains.Replace("https://", "")
                                                                               .Replace("http://", "")
                                                                               .Replace("www.", "");
                            }
                            
                            string findAllDomains = findAllGoogleScrapedItem.Key.Domain.Replace("https://", "")
                                                                                       .Replace("http://", "")
                                                                                       .Replace("www.", "");
                            if (!uniqueDomains.Contains(findAllDomains))
                            {
                                var domainSpl = findAllDomains.Split('.');
                                string domain = domainSpl.Length > 1 ? domainSpl[1] : string.Empty;
                                if (!string.IsNullOrEmpty(domain))
                                {
                                    if (!uniqueDomains.Contains(domain))
                                    {
                                        googleScrapedItem.UniqueDomains += $"{findAllGoogleScrapedItem.Key.Domain},";
                                        uniqueDomainsQty++;
                                    }
                                }
                                else
                                {
                                    googleScrapedItem.UniqueDomains += $"{findAllGoogleScrapedItem.Key.Domain},";
                                    uniqueDomainsQty++;
                                }
                            }
                        }

                        googleScrapedItem.UniqueDomainsQty = uniqueDomainsQty.ToString();
                        googleScrapedItem.UniqueDomains = googleScrapedItem.UniqueDomains.Trim(',');
                    }

                    GoogleDocApiHelper.UploadToGoogleDoc(extSett, googleScrapedItems, this);

                    var date = $"{DateTime.Now:MM-dd-yyyy-hh-mm}";
                    var file = FileHelper.CreateGoogleScrapedItemsFile(FileHelper.GetSettingsPath($"GoogleReport-{date}.csv", "GoogleScrapedData"), googleScrapedItems);
                    if (file != null)
                    {
                        MessagePrinter.PrintMessage($"File path {file}");
                    }
                }
                else
                {
                    MessagePrinter.PrintMessage($"Nothing data scraped", ImportanceLevel.Mid);
                }
            }
            catch (Exception exception)
            {
                MessagePrinter.PrintMessage($"Error with Complete. {exception.Message}", ImportanceLevel.High);
            }
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var libName = args.Name.ToLower().Replace('.', '_');
            var p1 = libName.IndexOf(',');
            if (p1 != -1)
                libName = libName.Substring(0, p1);
            if (libName.Contains("_resources"))
                return null;
            Assembly asm = null;

            var rmgr = new ResourceManager("GoogleRequestsScraper.Libs", typeof(EmbededLibs).Assembly);
            rmgr.IgnoreCase = true;

            object obj = rmgr.GetObject(libName);
            var asmBytes = ((byte[])(obj));
            if (asmBytes != null)
            {
                asm = Assembly.Load(asmBytes);
            }

            return asm;
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
            //CheckConnection();

            states = FileHelper.FillCountryStateZip();
            Uules.Clear();
            Wares.Clear();
            Domains.Clear();

            if (string.IsNullOrEmpty(extSett.LuminatiAddress))
                throw new Exception($"Write Luminati address");
            if (string.IsNullOrEmpty(extSett.LuminatiLogin))
                throw new Exception($"Write Luminati login");
            if (string.IsNullOrEmpty(extSett.LuminatiPassword))
                throw new Exception($"Write Luminati password");
            if (string.IsNullOrEmpty(extSett.GoogleSheetsLink))
                throw new Exception($"Write Google Sheets Link");

            string credPath = System.Environment.GetFolderPath(
                System.Environment.SpecialFolder.Personal);
            credPath = Path.Combine(credPath, $".credentials/sheets.googleapis.com-dotnet-quickstart.json/Google.Apis.Auth.OAuth2.Responses.TokenResponse-{Environment.UserName}");
            if (!File.Exists(credPath))
                throw new Exception($"Authorize at Google");

            if (!extSett.KeywordsForScrape.Any())
            {
                throw new Exception($"Please add some keywords for scrape!");
            }

            List<GeotargetsDataItem> geotargetsDataItems = new List<GeotargetsDataItem>();
            if (!string.IsNullOrEmpty(extSett.GeotargetsFilePath))
                geotargetsDataItems.AddRange(FileHelper.ReadGeotargetsFile(extSett.GeotargetsFilePath));

            if (!string.IsNullOrEmpty(extSett.DomainsFilePath))
                Domains.AddRange(FileHelper.ReadDomainsFile(extSett.DomainsFilePath));

            if (geotargetsDataItems.Any())
            {
                foreach (var geotargetsDataItem in geotargetsDataItems)
                {
                    if (geotargetsDataItem.Check.ToLower() == "yes")
                    {
                        UuleDataItem uule = new UuleDataItem();
                        uule.Uule = $"w+CAIQICI{LengthSecret.CheckLengthSecret(geotargetsDataItem.CanonicalName.Length)}{geotargetsDataItem.CanonicalName.Base64Encode().Replace("=", "")}";
                        uule.CanonicalName = geotargetsDataItem.CanonicalName;

                        Uules.Add(uule);
                    }
                }
            }

            string directoryPath = FileHelper.GetSettingsDirectory() + "GoogleScrapedData";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (var keyword in extSett.KeywordsForScrape)
            {
                foreach (var uule in Uules)
                {
                    if (cancel)
                        break;
                    lock (this)
                    {
                        if (extSett.Mobile)
                            lstProcessQueue.Add(new ExtProcessQueueItem
                            {
                                Name = keyword,
                                ItemType = 20,
                                Item = uule,
                                DeviceType = DeviceType.Mobile
                            });
                        if (extSett.Desktop)
                            lstProcessQueue.Add(new ExtProcessQueueItem
                            {
                                Name = keyword,
                                ItemType = 20,
                                Item = uule,
                                DeviceType = DeviceType.Desktop
                            });
                    }
                }
            }
            
            StartOrPushPropertiesThread();
        }

        //private void CheckConnection()
        //{
        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        var items = db.GoogleItems.ToList();
        //    }
        //}

        //private void CreateDB()
        //{
        //    try
        //    {
        //        using (ApplicationContext db = new ApplicationContext())
        //        {
        //            var items = db.GoogleItems.ToList();
        //            items.Clear();

        //            items.AddRange(Wares.Select(i => new GoogleItem()
        //            {
        //                Device = ((ExtWareInfo)i).Device,
        //                Placement = ((ExtWareInfo)i).Placement,
        //                Domain = ((ExtWareInfo)i).Domain,
        //                Time = ((ExtWareInfo)i).Time,
        //                State = ((ExtWareInfo)i).State,
        //                Keyword = ((ExtWareInfo)i).Keyword,
        //                Position = ((ExtWareInfo)i).Position,
        //                CompanyName = ((ExtWareInfo)i).CompanyName
        //            }));

        //            if (items.Any())
        //            {
        //                foreach (var mySqlDataItem in items)
        //                {
        //                    db.GoogleItems.Add(mySqlDataItem);
        //                }

        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                MessagePrinter.PrintMessage($"Nothing data scraped", ImportanceLevel.Mid);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessagePrinter.PrintMessage($"Nothing data scraped", ImportanceLevel.Mid);
        //    }
        //}

        private GoogleJsonItem ParseSearchHtmlResponse(string rawHtml)
        {
            var res = new GoogleJsonItem { general = new General() };
            res.top_ads = new List<Top_Ads>();

            var rgx = new Regex(@"\\x22uul_text\\x22:\\x22(.*?)\\x22");
            var m = rgx.Match(rawHtml);
            if (m.Success)
                res.general.location = m.Groups[1].Value;

            var doc = new HtmlDocument();
            doc.LoadHtml(rawHtml);

            string adsTitle = string.Empty;
            var title = doc.DocumentNode.SelectSingleNode(".//div[@class = 'cfxYMc JfZTW c4Djg MUxGbd v0nnCb']");
            if (title != null)
            {
                adsTitle = title.InnerTextOrNull();
            }

            //var ads = doc.DocumentNode.SelectNodes("//div[@id='tvcap']//div[@data-hveid][.//a]");
            var ads = doc.DocumentNode.SelectNodes(".//a//span[./text()[1]='Ad']");
            int rank = 1;
            if (ads != null)
            {
                foreach (var adb in ads)
                {
                    if (adb.NextSibling == null)
                        continue;
                    var url = adb.NextSibling.InnerTextOrNull();

                    if (string.IsNullOrEmpty(url))
                        continue;
                    if (url.Length < 5)
                        url = adb.ParentNode.NextSibling.InnerTextOrNull();

                    if (!url.StartsWith("http"))
                        url = $"http://{url}";

                    var uri = new Uri(new Uri(url), "/");
                    res.top_ads.Add(new Top_Ads
                    {
                        rank = rank.ToString(),
                        link = uri.ToString(),
                        title = adsTitle
                    });
                    rank++;
                }
            }

            return res;
        }

        private GoogleJsonItem ParseSearchJsonResponse(string rawHtml)
        {
            var jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;

            var googleJsonItem = jss.Deserialize<GoogleJsonItem>(rawHtml);
            return googleJsonItem;
        }

        private List<GoogleScrapedItem> ScrapeGoogleByKeywordRaw(UuleDataItem uule, string keyword, DeviceType deviceType)
        {
            var proxyInfo = new
            {
                Address = extSett.LuminatiAddress,
                Login = extSett.LuminatiLogin,
                Password = extSett.LuminatiPassword
            };

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    using (var client = new MyWebClient(TimeSpan.FromSeconds(10)))
                    {
                        //var client = new PageRetriever();
                        //client.Proxies = new List<ProxyInfo> { new ProxyInfo { Address = proxyInfo.Address, Login = proxyInfo.Login, Password = proxyInfo.Password } };


                        client.Proxy = new WebProxy(proxyInfo.Address);
                        if (!string.IsNullOrEmpty(proxyInfo.Login) && !string.IsNullOrEmpty(proxyInfo.Password))
                            client.Proxy.Credentials = new NetworkCredential()
                            {
                                UserName = proxyInfo.Login,
                                Password = proxyInfo.Password
                            };

                        string urlForScrape = string.Empty;

                        if (uule != null && !string.IsNullOrEmpty(uule.Uule))
                            urlForScrape = $"http://www.google.com/search?q={keyword}&uule={uule.Uule}";
                        else
                            urlForScrape = $"http://www.google.com/search?q={keyword}";

                        if (deviceType == DeviceType.Mobile)
                            client.Headers.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 7_0 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) Version/7.0 Mobile/11A465 Safari/9537.53");
                        //client.AfterSetHeaders += (x, y) => x.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 7_0 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) Version/7.0 Mobile/11A465 Safari/9537.53";
                        else
                            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36");
                        //client.AfterSetHeaders += (x, y) => x.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";

                        var resp = client.DownloadString(urlForScrape);
                        //var resp = client.ReadFromServer(urlForScrape);
                        string dumpPageId = string.Empty;
                        if (extSett.DumpPages)
                        {
                            if (uule != null)
                            {
                                var fn = $"{keyword}_{Guid.NewGuid()}_{deviceType}_{uule.CanonicalName}.html";
                                dumpPageId = fn.Replace($"{keyword}_", "").Replace($"_{deviceType}_{uule.CanonicalName}.html", "");
                                var f = Path.Combine(Path.GetDirectoryName(extSett.DomainsFilePath), fn);
                                File.WriteAllText(f, resp);
                            }
                            else
                            {
                                var fn = $"{keyword}_{Guid.NewGuid()}_{deviceType}.html";
                                dumpPageId = fn.Replace($"{keyword}_", "").Replace($"_{deviceType}.html", "");
                                var f = Path.Combine(Path.GetDirectoryName(extSett.DomainsFilePath), fn);
                                File.WriteAllText(f, resp);
                            }
                        }

                        var googleJsonItem = ParseSearchHtmlResponse(resp);
                        if (googleJsonItem != null)
                        {
                            //if (googleJsonItem.top_ads != null && deviceType == DeviceType.Mobile)
                            //{

                            //}

                            if ((googleJsonItem.top_ads?.Count ?? 0) == 0 /*&& googleJsonItem.bot_ads*/)
                                continue;

                            string link = string.Empty;
                            string domain = string.Empty;
                            var time = $"{DateTime.Now:hh tt}";
                            List<GoogleScrapedItem> items = new List<GoogleScrapedItem>();
                            foreach (var top_ad in googleJsonItem.top_ads)
                            {
                                if (top_ad.link == "#")
                                    link = top_ad.display_link;
                                else
                                    link = top_ad.link;
                                
                                domain = $"{link.Substring(0, link.IndexOf('/') + 2)}";
                                //link = $"{link.Replace(domain, "").Substring(0, link.Replace(domain, "").LastIndexOf('/') + 1)}";
                                link = $"{link.Replace(domain, "").Substring(0, link.Replace(domain, "").IndexOf('/') + 1)}";
                                string fullLink = domain + link;

                                var item = new GoogleScrapedItem
                                {
                                    State = googleJsonItem.general?.location,
                                    Device = deviceType.ToString(),
                                    Domain = fullLink,
                                    Keyword = keyword.Replace("+", " "),
                                    Placement = top_ad.rank,
                                    Time = time,
                                    Position = "top",
                                    DumpPageId = dumpPageId,
                                    Title = top_ad.title
                                };

                                items.Add(item);
                            }

                            return items;
                        }
                    }
                }

            }
            catch (WebException exc)
            {
                if (exc.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)exc.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            throw new Exception(error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessagePrinter.PrintMessage($"Error with scraping Google. {ex.Message}");
                throw;
            }

            return null;
        }

        private List<GoogleScrapedItem> ScrapeGoogleByKeyword(UuleDataItem uule, string keyword, DeviceType deviceType)
        {
            if (extSett.ScanMethod == ScanMethod.LuminatiJson)
                return ScrapeGoogleByKeywordLuminatiJson(uule, keyword, deviceType);
            else
                return ScrapeGoogleByKeywordRaw(uule, keyword, deviceType);
        }

        private List<GoogleScrapedItem> ScrapeGoogleByKeywordLuminatiJson(UuleDataItem uule, string keyword, DeviceType deviceType)
        {
            var proxyInfo = new
            {
                Address = extSett.LuminatiAddress,
                Login = extSett.LuminatiLogin,
                Password = extSett.LuminatiPassword
            };

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Proxy = new WebProxy(proxyInfo.Address);
                        if (!string.IsNullOrEmpty(proxyInfo.Login) && !string.IsNullOrEmpty(proxyInfo.Password))
                            client.Proxy.Credentials = new NetworkCredential()
                            {
                                UserName = proxyInfo.Login,
                                Password = proxyInfo.Password
                            };

                        string urlForScrape = string.Empty;

                        if (uule != null && !string.IsNullOrEmpty(uule.Uule))
                        {
                            urlForScrape = deviceType == DeviceType.Desktop ?
                                $"http://www.google.com/search?q={keyword}&uule={uule.Uule}&lum_json=1" :
                                $"http://www.google.com/search?q={keyword}&lum_mobile=1&uule={uule.Uule}&lum_json=1";
                        }
                        else
                        {
                            urlForScrape = deviceType == DeviceType.Desktop ?
                                $"http://www.google.com/search?q={keyword}&lum_json=1" :
                                $"http://www.google.com/search?q={keyword}&lum_mobile=1&lum_json=1";
                        }

                        var resp = client.DownloadString(urlForScrape);

                        var jss = new JavaScriptSerializer();
                        jss.MaxJsonLength = Int32.MaxValue;

                        var googleJsonItem = jss.Deserialize<GoogleJsonItem>(resp);
                        if (googleJsonItem != null)
                        {
                            //if (googleJsonItem.top_ads != null && deviceType == DeviceType.Mobile)
                            //{

                            //}

                            if (googleJsonItem.top_ads == null /*&& googleJsonItem.bot_ads*/)
                                continue;

                            string link = string.Empty;
                            string domain = string.Empty;
                            var time = $"{DateTime.Now:hh tt}";
                            List<GoogleScrapedItem> items = new List<GoogleScrapedItem>();
                            foreach (var top_ad in googleJsonItem.top_ads)
                            {
                                if (top_ad.link == "#")
                                    link = top_ad.display_link;
                                else
                                    link = top_ad.link;

                                domain = $"{link.Substring(0, link.IndexOf('/') + 2)}";
                                //link = $"{link.Replace(domain, "").Substring(0, link.Replace(domain, "").LastIndexOf('/') + 1)}";
                                link = $"{link.Replace(domain, "").Substring(0, link.Replace(domain, "").IndexOf('/') + 1)}";
                                string fullLink = domain + link;

                                var item = new GoogleScrapedItem
                                {
                                    State = googleJsonItem?.general.location,
                                    Device = deviceType.ToString(),
                                    Domain = fullLink,
                                    Keyword = keyword.Replace("+", " "),
                                    Placement = top_ad.rank,
                                    Time = time,
                                    Position = "top",
                                    Title = top_ad.title
                                };

                                items.Add(item);
                            }

                            return items;
                        }
                    }
                }

            }
            catch (WebException exc)
            {
                if (exc.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)exc.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            throw new Exception(error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessagePrinter.PrintMessage($"Error with scraping Google. {ex.Message}");
                throw;
            }

            return null;
        }

        protected void ProcessKeyword(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                string keyword = pqi.Name;
                MessagePrinter.PrintMessage($"Processing {keyword}");

                if (Uules.Any())
                {
                    foreach (var uule in Uules)
                    {
                        if (cancel)
                            break;

                        lock (this)
                        {
                            lstProcessQueue.Add(new ProcessQueueItem()
                            {
                                Name = keyword,
                                ItemType = 20,
                                Item = uule
                            });
                        }
                    }
                }
                else
                {
                    List<StatesDataItem> states = new List<StatesDataItem>();
                    states = FileHelper.FillCountryStateZip();

                    if (extSett.Desktop)
                    {
                        var desktopResult = ScrapeGoogleByKeyword(null, keyword.Replace(" ", "+"), DeviceType.Desktop);
                        if (desktopResult != null)
                        {
                            foreach (var result in desktopResult)
                            {
                                string domain = string.Empty;
                                var domainSpl = result.Domain.Split('/');
                                domain = domainSpl.Length > 2 ? domainSpl[2] : string.Empty;
                                var companyName = Domains.Find(i => i.Website == domain.Replace("www.", ""));
                                if (companyName != null && !string.IsNullOrEmpty(companyName.Legal))
                                    result.CompanyName = companyName.Legal;
                                else
                                    result.CompanyName = "Unknown";

                                var state = states.Find(i => result.State.Contains(i.State));
                                if (state != null)
                                    result.State = state.StateCode;

                                ExtWareInfo wi = new ExtWareInfo(result);
                                AddWareInfo(wi);
                                OnItemLoaded(wi);
                            }
                        }
                        else
                        {
                            ExtWareInfo wi = new ExtWareInfo();
                            wi.Device = DeviceType.Desktop.ToString();
                            wi.Domain = "N/A";
                            wi.Keyword = keyword;
                            wi.Time = $"{DateTime.Now:hh tt}";
                            wi.State = "N/A";

                            AddWareInfo(wi);
                            OnItemLoaded(wi);

                            MessagePrinter.PrintMessage($"Nothing ADS for [{keyword}] found. Device [{DeviceType.Desktop}]", ImportanceLevel.Mid);
                        }
                    }

                    if (extSett.Mobile)
                    {
                        var mobileResult = ScrapeGoogleByKeyword(null, keyword.Replace(" ", "+"), DeviceType.Mobile);
                        if (mobileResult != null)
                        {
                            foreach (var result in mobileResult)
                            {
                                string domain = string.Empty;
                                var domainSpl = result.Domain.Split('/');
                                domain = domainSpl.Length > 2 ? domainSpl[2] : string.Empty;
                                var companyName = Domains.Find(i => i.Website == domain.Replace("www.", ""));
                                if (companyName != null && !string.IsNullOrEmpty(companyName.Legal))
                                    result.CompanyName = companyName.Legal;
                                else
                                    result.CompanyName = "Unknown";

                                var state = states.Find(i => result.State.Contains(i.State));
                                if (state != null)
                                    result.State = state.StateCode;

                                ExtWareInfo wi = new ExtWareInfo(result);
                                AddWareInfo(wi);
                                OnItemLoaded(wi);
                            }
                        }
                        else
                        {
                            ExtWareInfo wi = new ExtWareInfo();
                            wi.Device = DeviceType.Mobile.ToString();
                            wi.Domain = "N/A";
                            wi.Keyword = keyword;
                            wi.Time = $"{DateTime.Now:hh tt}";
                            wi.State = "N/A";

                            AddWareInfo(wi);
                            OnItemLoaded(wi);

                            MessagePrinter.PrintMessage($"Nothing ADS for [{keyword}] found. Device [{DeviceType.Mobile}]", ImportanceLevel.Mid);
                        }
                    }
                }

                MessagePrinter.PrintMessage($"{keyword} processed");
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error with keyword. {e.Message}");
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brands list processed");
            StartOrPushPropertiesThread();
        }

        private void ProcessKeywordSearchResult(ProcessQueueItem pqi, DeviceType deviceType)
        {
            var extPqi = ((UuleDataItem)pqi.Item);
            string uule = extPqi?.Uule;
            string stateName = extPqi?.CanonicalName ?? "";

            string keyword = pqi.Name;
            var searchResult = ScrapeGoogleByKeyword(extPqi, keyword.Replace(" ", "+"), deviceType);
            if (searchResult != null)
            {
                foreach (var result in searchResult)
                {
                    string domain = string.Empty;
                    var domainSpl = result.Domain.Split('/');
                    domain = domainSpl.Length > 2 ? domainSpl[2] : string.Empty;
                    var companyName = Domains.Find(i => string.Compare(i.Website, domain.Replace("www.", ""), true) == 0);
                    if (companyName != null && !string.IsNullOrEmpty(companyName.Legal))
                        result.CompanyName = companyName.Legal;
                    else
                        result.CompanyName = "Unknown";

                    var state = states.Find(i => stateName.Contains(i.State));
                    if (state != null)
                        result.State = state.StateCode;

                    ExtWareInfo wi = new ExtWareInfo(result);
                    AddWareInfo(wi);
                    OnItemLoaded(wi);
                }
                MessagePrinter.PrintMessage($"Received {searchResult.Count} results for {keyword} @{stateName}");
            }
            else
            {
                ExtWareInfo wi = new ExtWareInfo();
                wi.Device = deviceType.ToString();
                wi.Domain = "N/A";
                wi.Keyword = keyword;
                wi.Time = $"{DateTime.Now:hh tt}";

                var state = states.Find(i => stateName.Contains(i.State));
                if (state != null)
                    wi.State = state.StateCode;
                else
                    wi.State = "N/A";

                AddWareInfo(wi);
                OnItemLoaded(wi);

                var msg = $"No ADS for [{keyword}]" + (uule != null ? $", uule [{uule}] ({stateName})" : "") + $" found.Device[{deviceType}]";
                MessagePrinter.PrintMessage(msg, ImportanceLevel.Mid);
            }
        }

        protected void ProcessUuleByKeyword(ProcessQueueItem pqi)
        {
            var extPqi = (ExtProcessQueueItem)pqi;
            ProcessKeywordSearchResult(pqi, extPqi.DeviceType);
            pqi.Processed = true;
            //if (extSett.Mobile)
            //    ProcessKeywordSearchResult(pqi, DeviceType.Mobile);
            //if (extSett.Desktop)
            //    ProcessKeywordSearchResult(pqi, DeviceType.Desktop);
        }


        //protected void ProcessUuleByKeyword_old(ProcessQueueItem pqi)
        //{
        //    if (cancel)
        //        return;

        //    try
        //    {
        //        string uule = ((UuleDataItem)pqi.Item).Uule;
        //        string stateName = ((UuleDataItem)pqi.Item).CanonicalName;
        //        string keyword = pqi.Name;

        //        List<StatesDataItem> states = new List<StatesDataItem>();
        //        states = FileHelper.FillCountryStateZip();

        //        if (extSett.Desktop)
        //        {
        //            var desktopResult = ScrapeGoogleByKeyword(uule, keyword.Replace(" ", "+"), DeviceType.Desktop);
        //            if (desktopResult != null)
        //            {
        //                foreach (var result in desktopResult)
        //                {
        //                    string domain = string.Empty;
        //                    var domainSpl = result.Domain.Split('/');
        //                    domain = domainSpl.Length > 2 ? domainSpl[2] : string.Empty;
        //                    var companyName = Domains.Find(i => i.Website == domain.Replace("www.", ""));
        //                    if (companyName != null && !string.IsNullOrEmpty(companyName.Legal))
        //                        result.CompanyName = companyName.Legal;
        //                    else
        //                        result.CompanyName = "Unknown";

        //                    var state = states.Find(i => stateName.Contains(i.State));
        //                    if (state != null)
        //                        result.State = state.StateCode;

        //                    ExtWareInfo wi = new ExtWareInfo(result);
        //                    AddWareInfo(wi);
        //                    OnItemLoaded(wi);
        //                }
        //            }
        //            else
        //            {
        //                ExtWareInfo wi = new ExtWareInfo();
        //                wi.Device = DeviceType.Desktop.ToString();
        //                wi.Domain = "N/A";
        //                wi.Keyword = keyword;
        //                wi.Time = $"{DateTime.Now:hh tt}";

        //                var state = states.Find(i => stateName.Contains(i.State));
        //                if (state != null)
        //                    wi.State = state.StateCode;

        //                AddWareInfo(wi);
        //                OnItemLoaded(wi);

        //                MessagePrinter.PrintMessage($"Nothing ADS for [{keyword}], uule [{uule}] found. Device [{DeviceType.Desktop}]", ImportanceLevel.Mid);
        //            }
        //        }

        //        if (extSett.Mobile)
        //        {
        //            var mobileResult = ScrapeGoogleByKeyword(uule, keyword.Replace(" ", "+"), DeviceType.Mobile);
        //            if (mobileResult != null)
        //            {
        //                foreach (var result in mobileResult)
        //                {
        //                    string domain = string.Empty;
        //                    var domainSpl = result.Domain.Split('/');
        //                    domain = domainSpl.Length > 2 ? domainSpl[2] : string.Empty;
        //                    var companyName = Domains.Find(i => i.Website == domain.Replace("www.", ""));
        //                    if (companyName != null && !string.IsNullOrEmpty(companyName.Legal))
        //                        result.CompanyName = companyName.Legal;
        //                    else
        //                        result.CompanyName = "Unknown";

        //                    var state = states.Find(i => stateName.Contains(i.State));
        //                    if (state != null)
        //                        result.State = state.StateCode;

        //                    ExtWareInfo wi = new ExtWareInfo(result);
        //                    AddWareInfo(wi);
        //                    OnItemLoaded(wi);
        //                }
        //            }
        //            else
        //            {
        //                ExtWareInfo wi = new ExtWareInfo();
        //                wi.Device = DeviceType.Mobile.ToString();
        //                wi.Domain = "N/A";
        //                wi.Keyword = keyword;
        //                wi.Time = $"{DateTime.Now:hh tt}";

        //                var state = states.Find(i => stateName.Contains(i.State));
        //                if (state != null)
        //                    wi.State = state.StateCode;

        //                AddWareInfo(wi);
        //                OnItemLoaded(wi);

        //                MessagePrinter.PrintMessage($"Nothing ADS for [{keyword}], uule [{uule}] found. Device [{DeviceType.Mobile}]", ImportanceLevel.Mid);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessagePrinter.PrintMessage($"Error with uule. {e.Message}");
        //    }

        //    pqi.Processed = true;
        //    MessagePrinter.PrintMessage("Brands list processed");
        //    StartOrPushPropertiesThread();
        //}

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 10)
                act = ProcessKeyword;
            else if (item.ItemType == 20)
                act = ProcessUuleByKeyword;
            else act = null;

            return act;
        }
    }
}
