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
using System.Windows.Forms;
using HtmlAgilityPack;
using GoogleRequestsScraper;
using Databox.Libs.GoogleRequestsScraper;
using Google.Apis.Sheets.v4.Data;
using GoogleRequestsScraper.DataItems;
using GoogleRequestsScraper.Enums;
using GoogleRequestsScraper.Extensions;
using GoogleRequestsScraper.Helpers;

namespace WheelsScraper
{
    public class GoogleRequestsScraper : BaseScraper
    {
        public List<UuleDataItem> Uules;
        public List<DomainsDataItem> Domains;

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
                    CompanyName = ((ExtWareInfo)i).CompanyName
                }));

                if (googleScrapedItems.Any())
                {
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
                if (cancel)
                    break;

                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem()
                    {
                        Name = keyword,
                        ItemType = 10,
                    });
                }
            }

            StartOrPushPropertiesThread();
        }

        private List<GoogleScrapedItem> ScrapeGoogleByKeyword(string uule, string keyword, DeviceType deviceType)
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

                        if (!string.IsNullOrEmpty(uule))
                        {
                            urlForScrape = deviceType == DeviceType.Desktop ?
                                $"http://www.google.com/search?q={keyword}&uule={uule}&lum_json=1" :
                                $"http://www.google.com/search?q={keyword}&lum_mobile=1&uule={uule}&lum_json=1";
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
                            var time = $"{DateTime.Now:hh tt}";
                            List<GoogleScrapedItem> items = new List<GoogleScrapedItem>();
                            foreach (var top_ad in googleJsonItem.top_ads)
                            {
                                if (top_ad.link == "#")
                                    link = top_ad.display_link;
                                else
                                    link = top_ad.link;

                                var item = new GoogleScrapedItem
                                {
                                    State = googleJsonItem?.general.location,
                                    Device = deviceType.ToString(),
                                    Domain = link,
                                    Keyword = keyword.Replace("+", " "),
                                    Placement = top_ad.rank,
                                    Time = time,
                                    Position = "top"
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
                        var desktopResult = ScrapeGoogleByKeyword(string.Empty, keyword.Replace(" ", "+"), DeviceType.Desktop);
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
                        var mobileResult = ScrapeGoogleByKeyword(string.Empty, keyword.Replace(" ", "+"), DeviceType.Mobile);
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

        protected void ProcessUuleByKeyword(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                string uule = ((UuleDataItem)pqi.Item).Uule;
                string stateName = ((UuleDataItem)pqi.Item).CanonicalName;
                string keyword = pqi.Name;

                List<StatesDataItem> states = new List<StatesDataItem>();
                states = FileHelper.FillCountryStateZip();

                if (extSett.Desktop)
                {
                    var desktopResult = ScrapeGoogleByKeyword(uule, keyword.Replace(" ", "+"), DeviceType.Desktop);
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

                            var state = states.Find(i => stateName.Contains(i.State));
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

                        var state = states.Find(i => stateName.Contains(i.State));
                        if (state != null)
                            wi.State = state.StateCode;

                        AddWareInfo(wi);
                        OnItemLoaded(wi);

                        MessagePrinter.PrintMessage($"Nothing ADS for [{keyword}], uule [{uule}] found. Device [{DeviceType.Desktop}]", ImportanceLevel.Mid);
                    }
                }

                if (extSett.Mobile)
                {
                    var mobileResult = ScrapeGoogleByKeyword(uule, keyword.Replace(" ", "+"), DeviceType.Mobile);
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

                            var state = states.Find(i => stateName.Contains(i.State));
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

                        var state = states.Find(i => stateName.Contains(i.State));
                        if (state != null)
                            wi.State = state.StateCode;

                        AddWareInfo(wi);
                        OnItemLoaded(wi);

                        MessagePrinter.PrintMessage($"Nothing ADS for [{keyword}], uule [{uule}] found. Device [{DeviceType.Mobile}]", ImportanceLevel.Mid);
                    }
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error with uule. {e.Message}");
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("Brands list processed");
            StartOrPushPropertiesThread();
        }

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
