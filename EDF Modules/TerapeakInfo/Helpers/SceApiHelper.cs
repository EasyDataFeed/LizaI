using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TerapeakInfo.SCEapi;
using WheelsScraper;

namespace TerapeakInfo.Helpers
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

        public static List<string> LoadProductsExport(ScraperSettings settings)
        {
            var files = new List<string>();
            var client = GetApiClient(settings);
            var zippedFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");
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
                        var fileName = Path.Combine(Path.GetTempPath(), z.FileName);
                        z.Extract(Path.GetDirectoryName(fileName), ExtractExistingFileAction.OverwriteSilently);
                        files.Add(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                throw new Exception($"Load products from SCE failed.\n\r{ex.Message}", ex);
            }
            finally
            {
                if (File.Exists(zippedFile))
                {
                    File.Delete(zippedFile);
                }
            }
            return files;
        }
    }
}
