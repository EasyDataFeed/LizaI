using System.Collections.Generic;
using GoogleRequestsScraper.DataItems;
using GoogleRequestsScraper.Enums;

namespace Databox.Libs.GoogleRequestsScraper
{
    public class ExtSettings
    {
        public ExtSettings()
        {
            KeywordsForScrape = new List<string>();
        }

        public List<string> KeywordsForScrape { get; set; }
        public string LuminatiAddress { get; set; }
        public string LuminatiLogin { get; set; }
        public string LuminatiPassword { get; set; }
        public string GeotargetsFilePath { get; set; }
        public string StatesFilePath { get; set; }
        public string DomainsFilePath { get; set; }
        public string GoogleSheetsLink { get; set; }
        public List<GoogleDocItem> GoogleDocItems { get; set; }
        public bool Desktop { get; set; }
        public bool Mobile { get; set; }
        public ScanMethod ScanMethod { get; set; }
        public bool DumpPages { get; set; }
    }
}
