using Acumotors.SCEapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Acumotors.Helpers
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

        public static int BatchUpdate(string url, ScraperSettings settings)
        {
            var client = GetApiClient(settings);
            var resultId = client.RunProductBatch(url, true);
            return resultId;
        }
    }
}
