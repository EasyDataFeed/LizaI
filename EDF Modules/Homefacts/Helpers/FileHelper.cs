using Homefacts.DataItems.School;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homefacts.Helpers
{
    public class FileHelper
    {
        private const char ComaSeparator = ',';
        private const string Separator = ",";

        public static string GetSettingsPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        //public static string CreateTestFile(string filePath, List<SchoolDataItem> schoolDataItems)
        //{
        //    try
        //    {
        //        string headers = "SchoolName,SchoolAddress,SchoolSummary,InstitutionType,TotalEnrollment,InStateTuition,TotalExpensesInState,TotalExpensesOutOfState,Female,Male," +
        //            "TuitionAndFeesInState,TuitionAndFeesOutOfState,TuitionAndFeesBooksAndSupplies,OnCampusRoomAndBoard,OnCampusOtherExpenses,OffCampusRoomAndBoard,OffCampusOtherExpenses," +
        //            "OffCampusWFRoomAndBoard,OffCampusWFOtherExpenses,HomefactsRatingPerYear,Ethnicities,AllTables,EnrollmentbyGrade,Characteristics,Admission,SchoolType";
        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendLine(headers);

        //        foreach (SchoolDataItem item in schoolDataItems)
        //        {
        //            string[] productArr = new string[26]
        //            {
        //                item.SchoolName, item.SchoolAddress, item.SchoolSummary, item.InstitutionType, item.TotalEnrollment, item.InStateTuition, item.TotalExpensesInState, item.TotalExpensesOutOfState, item.Female, item.Male,
        //                item.TuitionAndFeesInState, item.TuitionAndFeesOutOfState, item.TuitionAndFeesBooksAndSupplies, item.OnCampusRoomAndBoard, item.OnCampusOtherExpenses, item.OffCampusRoomAndBoard, item.OffCampusOtherExpenses,
        //                item.OffCampusWFRoomAndBoard, item.OffCampusWFOtherExpenses, item.HomefactsRatingPerYear, item.Ethnicities, item.AllTables, item.EnrollmentbyGrade, item.Characteristics, item.Admission, item.SchoolType
        //            };
        //            for (int i = 0; i < productArr.Length; i++)
        //                if (!String.IsNullOrEmpty(productArr[i]) && !String.IsNullOrWhiteSpace(productArr[i]))
        //                    productArr[i] = StringToCSVCell(productArr[i]);

        //            string product = String.Join(Separator, productArr);
        //            sb.AppendLine(product);
        //        }

        //        File.WriteAllText(filePath, sb.ToString());

        //        return filePath;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        private static string StringToCSVCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }
    }
}
