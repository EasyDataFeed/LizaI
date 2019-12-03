using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Redfin;
using Databox.Libs.Redfin;
using System.Net;
using System.Xml;
using System.IO.Compression;
using System.IO;
using Redfin.Helpers;
using Ionic.Zip;
using System.Threading;
using Redfin.DataItems;

namespace WheelsScraper
{
    public class Redfin : BaseScraper
    {
        private List<RedfinSitemap> RedfinItems { get; set; }
        private static object _lockObject = new object();

        public Redfin()
        {
            Name = "Redfin";
            Url = "https://www.Redfin.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            RedfinItems = new List<RedfinSitemap>();
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

        public void Decompress(FileInfo fileToDecompress)
        {
            if (File.Exists(fileToDecompress.FullName.Replace(".gz", "")))
                return;

            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        MessagePrinter.PrintMessage($"Decompressed: {fileToDecompress.Name}");
                    }
                }
            }
        }

        private void SaveArchives()
        {
            try
            {
                if (!Directory.Exists(FileHelper.GetSettingsPath("RedfinSitemap")))
                {
                    var directoryName = Directory.CreateDirectory(FileHelper.GetSettingsPath("RedfinSitemap"));
                }

                if (!Directory.Exists(FileHelper.GetSettingsPath("RedfinSitemapResearched")))
                {
                    var directoryName = Directory.CreateDirectory(FileHelper.GetSettingsPath("RedfinSitemapResearched"));
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string sitemaps = string.Empty;

                int attempt = 0;
                while (attempt < 3)
                {
                    try
                    {
                        sitemaps = PageRetriever.ReadFromServer("https://www.redfin.com/ssl_sitemap_index.xml", true);
                        break;
                    }
                    catch (Exception e)
                    {
                        attempt++;
                    }
                }

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sitemaps);
                //int count = 0;

                var xmlLink = xmlDoc.GetElementsByTagName("loc");
                foreach (XmlNode link in xmlLink)
                {
                    //if (count != 10)
                    //{
                    var file = link.InnerText.Split('/');
                    var fileName = file.Length > 4 ? file[4] : string.Empty;

                    attempt = 0;
                    while (attempt < 3)
                    {
                        try
                        {
                            PageRetriever.SaveFromServer(link.InnerText, FileHelper.GetSettingsPath(fileName, "RedfinSitemap"));
                            break;
                        }
                        catch (Exception e)
                        {
                            attempt++;
                        }

                        if (attempt == 3)
                        {
                            MessagePrinter.PrintMessage($"{link.InnerText}", ImportanceLevel.High);
                        }
                    }

                    //count += 1;
                    //}
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage(e.Message);
            }
        }

        protected override void RealStartProcess()
        {
            if (extSett.UpdateSiteMap)
                SaveArchives();

            string directoryPath = FileHelper.GetSettingsPath("RedfinSitemap");
            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
            {
                Decompress(fileToDecompress);

                lock (this)
                {
                    lstProcessQueue.Add(new ProcessQueueItem()
                    {
                        ItemType = 1,
                        Name = fileToDecompress.Name
                    });
                }
            }

            StartOrPushPropertiesThread();
        }

        private void ProcessLoadLinksSite(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                int count = 0;
                XmlDocument doc = new XmlDocument();
                List<string> listForResearched = Directory.GetFiles(FileHelper.GetSettingsPath("RedfinSitemapResearched")).ToList();
                doc.Load(FileHelper.GetSettingsPath(pqi.Name.Replace(".gz", ""), "RedfinSitemap"));
                if (pqi.Name.Contains("listings") || pqi.Name.Contains("buildings") || pqi.Name.Contains("properties"))
                {
                    var productLinks = doc.GetElementsByTagName("loc");
                    foreach (XmlNode productLink in productLinks)
                    {
                        bool found = false;

                        if (listForResearched.Any())
                        {
                            var matchedLink = listForResearched.Find(i => i.Contains(pqi.Name.Replace(".xml.gz", ".txt")));
                            if (matchedLink != null)
                            {
                                var links = File.ReadAllLines(FileHelper.GetSettingsPath(matchedLink, "RedfinSitemapResearched"));
                                foreach (var link in links)
                                {
                                    if (link.Replace("~Researched", "") == productLink.InnerText)
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                            }
                        }

                        //if (count < 1000)
                        //{
                        if (!found)
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem()
                                {
                                    ItemType = 20,
                                    Name = pqi.Name.Replace(".xml.gz", ""),
                                    URL = productLink.InnerText
                                });
                            }
                            count++;
                        }
                        //}
                        //else break;
                    }

                    lock (this)
                    {
                        lstProcessQueue.Add(new ProcessQueueItem()
                        {
                            ItemType = 2,
                            Name = pqi.Name.Replace(".xml.gz", "")
                        });
                    }
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error in {pqi.Name}", ImportanceLevel.High);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage($"{pqi.Name.Replace(".xml.gz", "")} link processed");
            StartOrPushPropertiesThread();
        }

        private void ProcessCreateFile(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            if (RedfinItems.Count() > 0)
            {
                string filePath = FileHelper.GetSettingsPath($"{pqi.Name}.txt", "RedfinSitemapResearched");
                foreach (var item in RedfinItems)
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine($"{item.Url}~Researched");
                    }
                }

                CreateRedfinItemsFile(RedfinItems.First().Name);
                RedfinItems.Clear();
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage($"{pqi.Name.Replace(".xml.gz", "")} link processed");
            StartOrPushPropertiesThread();
        }

        private void CreateRedfinItemsFile(string fileName)
        {
            if (!Directory.Exists(FileHelper.GetSettingsPath("RedfinFiles")))
            {
                var directoryName = Directory.CreateDirectory(FileHelper.GetSettingsPath("RedfinFiles"));
            }

            string filePath = FileHelper.CreateRedfinItemsFile(FileHelper.GetSettingsPath($"{fileName}.csv", "RedfinFiles"), RedfinItems);
            MessagePrinter.PrintMessage($"File with added products created - {filePath}");
        }

        protected void ProcessHouseLinkPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //pqi.URL = "https://www.redfin.com/OH/Beachwood/Atrium-Two/building/45105";
                string html = PageRetriever.ReadFromServer(pqi.URL, true);
                html = HttpUtility.HtmlDecode(html);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                string address = string.Empty;
                string city = string.Empty;
                string state = string.Empty;
                string zip = string.Empty;
                string builtYear = string.Empty;

                var addressHouse = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='address inline-block']/span/span/span");
                if (addressHouse != null)
                {
                    address = addressHouse.InnerTextOrNull();
                }

                var addressDet = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='address inline-block']/span/span/span[2]");
                if (addressDet != null)
                {
                    city = addressDet.SelectSingleNode(".//span[1]").InnerTextOrNull().Replace("<!-- -->,", "");
                    state = addressDet.SelectSingleNode(".//span[2]").InnerTextOrNull();
                    zip = addressDet.SelectSingleNode(".//span[3]").InnerTextOrNull();
                }
                else
                {
                    addressDet = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='AboutThisBuildingSection--InfoAddress ']");
                    if (addressDet != null)
                    {
                        var addrSp = addressDet.InnerTextOrNull().Split(',');

                        address = addrSp.Length > 0 ? addrSp[0] : string.Empty;
                        city = addrSp.Length > 0 ? addrSp[1] : string.Empty;
                        state = addrSp.Length > 0 ? addrSp[2] : string.Empty;
                        zip = addrSp.Length > 0 ? addrSp[3] : string.Empty;
                    }
                }

                var built = htmlDoc.DocumentNode.SelectNodes("//div[@class='facts-table']/div");
                if (built != null)
                {
                    foreach (var item in built)
                    {
                        var spec = item.SelectSingleNode(".//span").InnerTextOrNull();
                        if (spec == "Year Built")
                            builtYear = item.SelectSingleNode(".//div").InnerTextOrNull();
                    }
                }
                else
                {
                    built = htmlDoc.DocumentNode.SelectNodes("//div[@class='keyDetailsList']/div");
                    if (built != null)
                    {
                        foreach (var item in built)
                        {
                            var spec = item.SelectSingleNode(".//span[1]").InnerTextOrNull();
                            if (spec == "Year Built")
                                builtYear = item.SelectSingleNode(".//span[2]").InnerTextOrNull();
                        }
                    }
                }

                RedfinItems.Add(new RedfinSitemap()
                {
                    Address = address,
                    City = city,
                    State = state,
                    Zip = zip,
                    BuiltYear = builtYear,
                    Url = pqi.URL,
                    Name = pqi.Name
                });

                //lock (_lockObject)
                //{
                //    string filePath = FileHelper.GetSettingsPath($"{pqi.Name}.txt", "RedfinSitemapResearched");
                //    using (StreamWriter sw = new StreamWriter(filePath, true))
                //    {
                //        sw.WriteLine($"{pqi.URL}~Researched");
                //    }
                //}

                pqi.Processed = true;
            }
            catch (Exception e)
            {
                pqi.Processed = false;

                if (e.Message != "The remote server returned an error: (403) Forbidden.")
                {
                    MessagePrinter.PrintMessage(e.Message);
                }
            }

            StartOrPushPropertiesThread();
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 1)
                act = ProcessLoadLinksSite;
            else if (item.ItemType == 2)
                act = ProcessCreateFile;
            else if (item.ItemType == 20)
                act = ProcessHouseLinkPage;
            else act = null;

            return act;
        }
    }
}
