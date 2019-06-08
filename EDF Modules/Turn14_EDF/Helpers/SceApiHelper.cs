using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Databox.Libs.Turn14_EDF;
using Ionic.Zip;
using Turn14_EDF.SCEapi;
using WheelsScraper;

namespace Turn14_EDF.Helpers
{
    class SceApiHelper
    {
        private const string Separator = ",";

        public static sceApi GetApiClient(ScraperSettings settings)
        {
            if (string.IsNullOrEmpty(settings.SCEAccessKey) || string.IsNullOrEmpty(settings.SCEAPIKey) ||
                string.IsNullOrEmpty(settings.SCEAPISecret))
            {
                throw new ArgumentNullException("API fields have to be filled!");
            }
            var client = new sceApi();
            var auth = new AuthHeaderAPI
            {
                ApiAccessKey = settings.SCEAccessKey,
                ApiKey = settings.SCEAPIKey,
                ApiSecretKey = settings.SCEAPISecret
            };
            client.AuthHeaderAPIValue = auth;
            client.Timeout = 300 * 60 * 1000;
            return client;
        }

        public static string LoadProductsExport(ScraperSettings scraperSet, ExtSettings settings)
        {
            string name = "Turn14OrderSyncExport.csv";
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);

            //if (!settings.UpdateSceExport)
            //{
            //    if (File.Exists(fileName))
            //        return fileName;
            //}

            var client = GetApiClient(scraperSet);
            var zippedFile = fileName.Replace(".csv", ".zip");
            var prodBytes = client.GetFullProductsExport(Separator);
            try
            {
                using (var fs = File.OpenWrite(zippedFile))
                {
                    fs.Write(prodBytes, 0, prodBytes.Length);
                }

                using (var zipArc = ZipFile.Read(zippedFile))
                {
                    foreach (var z in zipArc)
                    {
                        z.FileName = Path.GetFileName(fileName);
                        z.Extract(Path.GetDirectoryName(fileName), ExtractExistingFileAction.OverwriteSilently);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            finally
            {
                if (File.Exists(zippedFile))
                {
                    File.Delete(zippedFile);
                }
            }
            return fileName;
        }

        public static Order[] LoadSceOrders(ScraperSettings scraperSet, DateTime dateFrom, DateTime dateTo)
        {
            var objsceApi = GetApiClient(scraperSet);
            var orders = objsceApi.OrderSearch(dateFrom, dateFrom, -1, -1, "", "", "", "", "",
                eOrderFilterShipping.None, eOrderFilterPayment.FullyCharged, "", "", "", "", "", "", "", "", "", "",
                false, false, false);

            return orders;
        }

        public static int BatchUpdate(string sourceFile, ScraperSettings settings)
        {
            var client = GetApiClient(settings);
            var resultId = client.RunProductBatch(sourceFile, true);
            return resultId;
        }
    }
}
