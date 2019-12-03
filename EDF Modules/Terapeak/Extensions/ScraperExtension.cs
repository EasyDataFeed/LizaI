#region using

using System;
using System.Web;
using HtmlAgilityPack;
using WheelsScraper;

#endregion

namespace Terapeak.Extensions
{
    public static class ScraperExtension
    {
        public static HtmlNode GetHtmlNodeFromServer(this BaseScraper scraper, string url, bool useCookie = false,
            bool isPost = false, string data = null)
        {
            string html;
            if (isPost)
            {
                html = scraper.PageRetriever.WriteToServer(url, data, useCookie);
            }
            else
            {
                html = scraper.PageRetriever.ReadFromServer(url, useCookie);
            }
            var htmlDoc = new HtmlDocument();
            html = HttpUtility.HtmlDecode(html);
            htmlDoc.LoadHtml(html);
            var docNode = htmlDoc.DocumentNode;
            return docNode;
        }

        public static void PrintError(this BaseScraper scraper, Exception e, ProcessQueueItem pqi)
        {
            var numberOfAttemps = pqi.NumberOfAttempts;
            scraper.MessagePrinter.PrintMessage(
                string.Format("Error by URL {0}\n{1}\n{2}\nNumberOfAttempts: {3}", pqi.URL, e.Message, e.StackTrace,
                    numberOfAttemps),
                numberOfAttemps < 3 ? ImportanceLevel.Mid : ImportanceLevel.High);
        }
    }
}
