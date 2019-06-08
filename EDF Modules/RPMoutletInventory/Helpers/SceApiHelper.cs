using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using RPMoutletInventory.SCEapi;
using WheelsScraper;

namespace RPMoutletInventory.Helpers
{
    public static class SceApiHelper
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

        public static string LoadProductsExport(ScraperSettings settings)
        {
            var client = GetApiClient(settings);
            var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
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

        public static int BatchUpdate(string sourceFile, ScraperSettings settings)
        {
            var client = GetApiClient(settings);
            var resultId = client.RunProductBatch(sourceFile, true);
            return resultId;
        }
    }
}
