using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbayNewParts.SCEapi;
using Ionic.Zip;
using WheelsScraper;

namespace EbayNewParts.Helpers
{
    public static class SceApiHelper
    {
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
    }
}
