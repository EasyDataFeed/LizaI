#region using

using System;
using System.Collections.Generic;
using System.IO;
using RPMoutletInventory.Extensions;
using RPMoutletInventory.Models;
using LumenWorks.Framework.IO.Csv;
using RPMoutletInventory.DataItems;

#endregion

namespace RPMoutletInventory.Helpers
{
    public static class FileHelper
    {
        public static string WriteEbayTemplate(List<TransferInfoItem> items, string fileName = null, bool useTemp = true)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                var date = string.Format("_{0:dd-MM-yyyy_HH-mm}", DateTime.Now);
                fileName = "eBay_upload" + date + ".csv";
            }
            if (useTemp)
            {
                fileName = Path.Combine(Path.GetTempPath(), fileName);
            }

            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(
                    "Action(SiteID=US|Country=US|Currency=USD|Version=941),ItemID,Relationship,RelationshipDetails,Title,Quantity,StartPrice");

                foreach (var item in items)
                {
                    writer.WriteLine("Revise,{0},,,,{1},", item.EbayId, item.EbayQuantity >= 0 ? item.EbayQuantity : 0);
                }
            }
            return fileName;
        }

        public static string WriteEbayPriceInvTemplate(List<TransferInfoItem> items, string fileName = null, bool useTemp = true)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                var date = string.Format("_{0:dd-MM-yyyy_HH-mm}", DateTime.Now);
                fileName = "eBay_upload" + date + ".csv";
            }
            if (useTemp)
            {
                fileName = Path.Combine(Path.GetTempPath(), fileName);
            }

            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(
                    "Action(SiteID=US|Country=US|Currency=USD|Version=941),ItemID,Relationship,RelationshipDetails,Title,Quantity,StartPrice");

                foreach (var item in items)
                {
                    writer.WriteLine("Revise,{0},,,,{1},{2}", item.EbayId, item.EbayQuantity >= 0 ? item.EbayQuantity : 0, item.EbayPrice);
                }
            }
            return fileName;
        }

        public static List<ReportItem> ReadUploadingReport(string fileName)
        {
            var reportItems = new List<ReportItem>();
            using (var sr = File.OpenText(fileName))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    while (csv.ReadNextRecord())
                    {
                        var curProduct = new ReportItem
                        {
                            ItemID = csv["ItemID"],
                            Status = csv["Status"],
                            ErrorCode = csv["ErrorCode"],
                            ErrorMessage = csv["ErrorMessage"]
                        };
                        reportItems.Add(curProduct);
                    }
                }
            }
            return reportItems;
        }

        public static List<EbayItem> ReadEbayItems(string fileName)
        {
            var ebayItems = new List<EbayItem>();
            using (var sr = File.OpenText(fileName))
            {
                using (var csv = new CsvReader(sr, true, ','))
                {
                    csv.GetFieldHeaders();
                    while (csv.ReadNextRecord())
                    {
                        var ebayId = csv["Item ID"];
                        var partNumber = csv["Custom Label"];
                        if (!string.IsNullOrEmpty(ebayId)  /* && !string.IsNullOrEmpty(partNumber) */ && partNumber != "0")
                        {
                            var curProduct = new EbayItem
                            {
                                EbayId = ebayId.Trim(),
                                PartNumber = partNumber.Trim(),
                                ProductTitle = csv["Item Title"].TrimNull()
                            };

                            ebayItems.Add(curProduct);
                        }
                    }
                }
            }
            return ebayItems;
        }

        public static void WriteEbayItems(List<EbayItem> ebayItems)
        {
            var fileName = Path.Combine(Path.GetTempPath(), "Duplicate_parts.csv");

            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine("EbayID, Part Number, Product Title");

                foreach (var item in ebayItems)
                {
                    writer.WriteLine("{0},{1},{2}", item.EbayId, item.PartNumber.PrepareForCSV(), item.ProductTitle.PrepareForCSV());
                }
            }
        }
    }
}
