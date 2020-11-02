using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Databox.Libs.GoogleRequestsScraper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using GoogleRequestsScraper.DataItems;
using WheelsScraper;

namespace GoogleRequestsScraper.Helpers
{
    public class GoogleDocApiHelper
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };

        private static ValueRange GetSpreadSheet(ExtSettings settings, string range)
        {
            string link = settings.GoogleSheetsLink;
            var linkStr = link.Split('/');
            var key = linkStr.Length > 0 ? linkStr[5] : string.Empty;

            SheetsService service = GetGoogleService();
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(key, range);

            ValueRange response = request.Execute();

            return response;
        }

        private static IList<Object> GenerateNewRow(ValueRange vr)
        {
            List<IList<Object>> objNewRecords = new List<IList<Object>>();

            IList<Object> obj = new List<Object>();

            IList<Object> objectsHeaders = vr.Values[0];
            foreach (var values in objectsHeaders)
            {
                obj.Add(string.Empty);
            }

            objNewRecords.Add(obj);

            return obj;
        }

        public static SheetsService GetGoogleService()
        {
            var assembly = Assembly.GetExecutingAssembly();

            UserCredential credential;

            using (Stream stream = assembly.GetManifestResourceStream("GoogleRequestsScraper.Resources.client_secret(not web).json"))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    Environment.UserName,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Easy Data Feed",
            });

            return service;
        }

        private static void UploadSpreadSheet(ExtSettings settings, ValueRange valuesToUpload, BaseScraper scraper)
        {
            try
            {
                string link = settings.GoogleSheetsLink;
                var linkStr = link.Split('/');
                var key = linkStr.Length > 0 ? linkStr[5] : string.Empty;

                SheetsService service = GetGoogleService();
                String spreadsheetId = key;

                SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valuesToUpload, spreadsheetId, valuesToUpload.Range);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                UpdateValuesResponse result2 = update.Execute();
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"Error with uploading spread sheet. {e.Message}", ImportanceLevel.High);
            }
        }

        public static void UploadToGoogleDoc(ExtSettings settings, List<GoogleScrapedItem> googleScrapedItems, BaseScraper scraper)
        {
            String rangeToStart = string.Empty;
            String rangeHeaders = "A1:Z1";
            String range = string.Empty;

            ValueRange valuesToUpload = new ValueRange();
            valuesToUpload.Values = new List<IList<object>>();
            try
            {
                ValueRange values = new ValueRange();
                ValueRange headers = new ValueRange();

                rangeToStart = "A1:Z";
                values = GetSpreadSheet(settings, rangeToStart);

                string link = settings.GoogleSheetsLink;
                var linkStr = link.Split('/');
                var key = linkStr.Length > 0 ? linkStr[5] : string.Empty;

                SheetsService service = GetGoogleService();
                String spreadsheetId = key;

                SpreadsheetsResource.ValuesResource.ClearRequest clear =
                    service.Spreadsheets.Values.Clear(null, spreadsheetId, $"A2:Z");
                ClearValuesResponse result = clear.Execute();

                headers = GetSpreadSheet(settings, rangeHeaders);
                //range = $"A{values.Values.Count + 1}:H{(values.Values.Count + 1) + googleScrapedItems.Count}";
                range = $"A2:Z";

                int rowIndex = 0;
                var date = $"{DateTime.Now:hh-mm}";
                foreach (GoogleScrapedItem item in googleScrapedItems)
                {
                    rowIndex++;
                    valuesToUpload.Values.Add(GenerateNewRow(headers));
                    item.Time = date;
                    valuesToUpload.Values[rowIndex - 1] = FillData(valuesToUpload.Values[rowIndex - 1], headers, item);
                }
            }
            catch (Exception e)
            {
                scraper.MessagePrinter.PrintMessage($"Error with uploading to Google doc. {e.Message}", ImportanceLevel.High);
                scraper.Log.Error(e);
            }

            if (valuesToUpload.Values.Count > 0)
            {
                valuesToUpload.Range = range;
                UploadSpreadSheet(settings, valuesToUpload, scraper);
                scraper.MessagePrinter.PrintMessage("Uploaded");
            }
            else
            {
                scraper.MessagePrinter.PrintMessage("Nothing to upload");
            }
        }

        private static IList<object> FillData(IList<object> row, ValueRange headers, GoogleScrapedItem item)
        {
            SetRowValue(row, headers, "keyword", item.Keyword);
            SetRowValue(row, headers, "domain", item.Domain);
            SetRowValue(row, headers, "placement", item.Placement);
            SetRowValue(row, headers, "state", item.State);
            SetRowValue(row, headers, "device", item.Device);
            SetRowValue(row, headers, "time", item.Time);
            SetRowValue(row, headers, "position", item.Position);
            SetRowValue(row, headers, "company name", item.CompanyName);
            SetRowValue(row, headers, "dump page id", item.DumpPageId);
            SetRowValue(row, headers, "unique domains", item.UniqueDomains);
            SetRowValue(row, headers, "unique domains qty", item.UniqueDomainsQty);
            SetRowValue(row, headers, "title", item.Title);
            SetRowValue(row, headers, "tag", item.Tag);
            return row;
        }

        private static void SetRowValue(IList<object> row, ValueRange headers, string columnName, string value)
        {
            var idx = headers.Values[0].IndexOf(columnName);
            if (idx < 0) return;
            row[idx] = value;
        }
    }
}