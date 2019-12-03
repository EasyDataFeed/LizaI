using Acumotors.DataItems;
using Databox.Libs.Acumotors;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Acumotors.Helpers
{
    public class GoogleDocApiHelper
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };

        static public ValueRange GetGoogleData(ExtSettings settings)
        {
            string link = settings.GoogleSheetsLink;
            var linkStr = link.Split('/');
            var key = linkStr.Length > 0 ? linkStr[5] : string.Empty;

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDWMEws1e_3ZbD0fpCrqZn84G3D4HepU2I"
            });

            // Define request parameters.
            String spreadsheetId = key;
            String range = "A2:C";
            String rangeHeaders = "A1:C";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            SpreadsheetsResource.ValuesResource.GetRequest requestHeaders =
                service.Spreadsheets.Values.Get(spreadsheetId, rangeHeaders);
            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            ValueRange responseHeaders = requestHeaders.Execute();
            IList<IList<Object>> values = responseHeaders.Values;

            return response;
        }
    }
}
