#region using

using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Turn14Connector.DataItems;
using Turn14Connector.DataItems.Turn14;
using Turn14Connector.SCEapi;
using WheelsScraper;

#endregion

namespace Turn14Connector.Helpers
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

        //public static int BatchUpdate(string url, ScraperSettings settings)
        //{
        //    var client = GetApiClient(settings);
        //    var resultId = client.RunProductBatch(url, true);

        //    return resultId;
        //}

        public static List<OrderSync> LoadSceOrders(ScraperSettings settings, DateTime startDate, DateTime endDate)
        {
            var client = GetApiClient(settings);

            Order[] orders = client.OrderSearch(startDate, endDate, 0, 0, "", "", "", "", "", eOrderFilterShipping.NotShipped,
                eOrderFilterPayment.None, "", "", "", "", "", "", "", "", "", "", false, true, true);

            //Order[] orders = client.OrderSearch(startDate, endDate, 0, 0, "", "", "", "", "", eOrderFilterShipping.NotShipped,
            //eOrderFilterPayment.FullyCharged, "", "", "", "", "", "", "", "", "", "", false, true, true);
            return orders.Select(order => new OrderSync(order)).ToList();
        }

        public static void MarkOrderSynced(ScraperSettings settings, OrderSync order)
        {
            var client = GetApiClient(settings);

            client.MarkOrderERPSynced(order.OrderId);
        }

        public static string UpdateShipmentTrackingNumber(ScraperSettings settings, ProcessedOrder order)
        {
            string error = string.Empty;
            try
            {
                var client = GetApiClient(settings);
                Shipment shipment = new Shipment();
                shipment.Service = new ShippingService();
                shipment.TrackingNo = order.TrackNumber;
                shipment.ShipDate = order.TrackNumberDate;
                shipment.Service.Code = order.Code;
                shipment.Service.SceCode = order.SceCode;
                shipment.Service.CarrierID = order.CarrierID;

                client.UpdateShipmentDetails(shipment);
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return error;
        }
    }
}
