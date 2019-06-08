#region using

using System;
using WheelsScraper;

#endregion

namespace RPMoutletInventory.Extensions
{
    public static class ScraperExtension
    {
#if DEBUG
        private const bool IsDebug = true;
#else
        private const bool IsDebug = false;
#endif
        public static void PrintError(this BaseScraper scraper, Exception e)
        {
            if (IsDebug)
            {
                scraper.MessagePrinter.PrintMessage(
                    string.Format("{0}\n{1}", e.Message, e.StackTrace));
            }
            else
            {
                //scraper.MessagePrinter.PrintMessage(e.Message);
            }
        }
    }
}