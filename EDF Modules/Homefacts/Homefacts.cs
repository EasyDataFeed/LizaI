using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Scraper.Shared;
using System.Web;
using HtmlAgilityPack;
using Homefacts;
using Databox.Libs.Homefacts;
using System.Net;
using System.Text.RegularExpressions;
using Homefacts.DataItems.School;
using Homefacts.Helpers;

namespace WheelsScraper
{
    public class Homefacts : BaseScraper
    {
        private List<School> SchoolDataItems { get; set; }

        public Homefacts()
        {
            Name = "Homefacts";
            Url = "https://www.Homefacts.com/";
            PageRetriever.Referer = Url;
            WareInfoList = new List<ExtWareInfo>();
            SchoolDataItems = new List<School>();
            Wares.Clear();
            BrandItemType = 2;

            SpecialSettings = new ExtSettings();
            Complete += Homefacts_Complete;
        }

        private void Homefacts_Complete(object sender, EventArgs e)
        {
            try
            {
                if (SchoolDataItems.Any())
                {
                    //string filePath = FileHelper.CreateTestFile(FileHelper.GetSettingsPath("HomefactsTest.csv"), SchoolDataItems);
                }
            }
            catch (Exception)
            {
                
            }
        }

        private ExtSettings extSett
        {
            get
            {
                return (ExtSettings)Settings.SpecialSettings;
            }
        }

        public override Type[] GetTypesForXmlSerialization()
        {
            return new Type[] { typeof(ExtSettings) };
        }

        //public override System.Windows.Forms.Control SettingsTab
        //{
        //	get
        //	{
        //		var frm = new ucExtSettings();
        //		frm.Sett = Settings;
        //		return frm;
        //	}
        //}

        public override WareInfo WareInfoType
        {
            get
            {
                return new ExtWareInfo();
            }
        }

        protected override bool Login()
        {
            return true;
        }

        protected override void RealStartProcess()
        {
            SchoolDataItems.Clear();

            List<string> links = new List<string>();
            links.Add("https://www.homefacts.com/zip-code/New-York/Richmond-County/Staten-Island/10301.html");
            links.Add("https://www.homefacts.com/zip-code/Alaska/Anchorage-Municipality/Anchorage/99509.html");
            links.Add("https://www.homefacts.com/zip-code/Alabama/Jefferson-County/Birmingham/35215.html");
            links.Add("https://www.homefacts.com/zip-code/Alabama/Shelby-County/Chelsea/35043.html");
            links.Add("https://www.homefacts.com/zip-code/Arkansas/Lafayette-County/Bradley/71826.html");
            links.Add("https://www.homefacts.com/zip-code/Arizona/Apache-County/Alpine/85920.html");
            links.Add("https://www.homefacts.com/zip-code/California/Los-Angeles-County/Artesia/90701.html");
            links.Add("https://www.homefacts.com/zip-code/Washington-Dc/District-Of-Columbia-County/Washington/20041.html");
            links.Add("https://www.homefacts.com/zip-code/Florida/St.-Lucie-County/Port-St.-Lucie/34984.html");
            links.Add("https://www.homefacts.com/zip-code/Minnesota/Big-Stone-County/Graceville/56240.html");

            foreach (var link in links)
            {
                lstProcessQueue.Add(new ProcessQueueItem { URL = link, ItemType = 10 });
            }

            StartOrPushPropertiesThread();
        }

        protected void ProcessZipPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                
                var html = PageRetriever.ReadFromServer(pqi.URL, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                //var address2 = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-12 col-md-12 pull-left']/h1").InnerTextOrNull();
                //var addressInfo = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-12 col-md-12 pull-left']/article").InnerTextOrNull();
                //var statenIsland = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-12 col-md-6 pull-left']/div/h2").InnerTextOrNull();

                //var statenIslandInfo = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'col-xs-12 col-sm-12 col-md-6 pull-left']/div/div");
                //if (statenIslandInfo != null)
                //{
                //    string infoes = string.Empty;
                //    foreach (var item in statenIslandInfo)
                //    {
                //        infoes += $"{item.SelectSingleNode(".//div[2]/h2").InnerTextOrNull()}~{item.SelectSingleNode(".//div[2]/div").InnerTextOrNull()},";
                //    }
                //}

                var publicSchools = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'col-xs-12 col-sm-6 col-md-6 pull-left  publicSchool']/ul/li/span/a");
                if (publicSchools != null)
                {
                    foreach (var school in publicSchools)
                    {
                        var link = school.AttributeOrNull("href");
                        if (link != null)
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem { URL = $"https://www.homefacts.com{link}", Name = "public", ItemType = 20 });
                            }
                        }
                    }
                }

                var districtSchools = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'col-xs-12 col-sm-6 col-md-6 pull-right schoolDistrict']/ul/li/span/a");
                if (districtSchools != null)
                {
                    foreach (var school in districtSchools)
                    {
                        var link = school.AttributeOrNull("href");
                        if (link != null)
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem { URL = $"https://www.homefacts.com{link}", Name = "district", ItemType = 20 });
                            }
                        }
                    }
                }

                var privateSchools = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'col-xs-12 col-sm-6 col-md-6 pull-right schoolDistrict']/p/a");
                if (privateSchools != null)
                {
                    foreach (var school in privateSchools)
                    {
                        var link = school.AttributeOrNull("href");
                        if (link != null)
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem { URL = $"https://www.homefacts.com{link}", Name = "private", ItemType = 20 });
                            }
                        }
                    }
                }

                var collegesAndUniversities = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'searchColleges']/ul/li/a");
                if (collegesAndUniversities != null)
                {
                    foreach (var collegeAndUniversity in collegesAndUniversities)
                    {
                        var link = collegeAndUniversity.AttributeOrNull("href");
                        if (link != null)
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem { URL = $"https://www.homefacts.com{link}", Name = "collegesAndUniversities", ItemType = 20 });
                            }
                        }
                    }
                }

                var crimes = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'searchColleges registeredOffenders']/ul/li/a");
                if (crimes != null)
                {
                    foreach (var crime in crimes)
                    {
                        var link = crime.AttributeOrNull("href");
                        if (link != null)
                        {
                            lock (this)
                            {
                                lstProcessQueue.Add(new ProcessQueueItem { URL = $"https:{link}", Name = "crime", ItemType = 20 });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error with zip []. {e.Message}", ImportanceLevel.High);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("School processed");
            StartOrPushPropertiesThread();
        }

        protected void ProcessSchoolPage(ProcessQueueItem pqi)
        {
            if (cancel)
                return;

            try
            {


                if (pqi.Name == "public")
                    PublicSchool(pqi.URL, pqi.Name);
                //else if (pqi.Name == "district")
                //    DistrictSchool(pqi.URL, pqi.Name);
                //else if (pqi.Name == "private")
                //    PrivateSchool(pqi.URL, pqi.Name);
                //else if (pqi.Name == "collegesAndUniversities")
                //    CollegesAndUniversities(pqi.URL, pqi.Name);

                if (pqi.Name == "crime")
                    Crime(pqi.URL);
            }
            catch (Exception e)
            {
                MessagePrinter.PrintMessage($"Error with school page. {e.Message}", ImportanceLevel.Mid);
            }

            pqi.Processed = true;
            MessagePrinter.PrintMessage("School processed");
            StartOrPushPropertiesThread();
        }

        public void PublicSchool(string url, string schoolType)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var html = PageRetriever.ReadFromServer(url, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);
                School school = new School();
                List<SchoolHomefact> homefacts = new List<SchoolHomefact>();

                school.SchoolType = schoolType;
                school.SchoolName = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop = 'legalName']").InnerTextOrNull();
                var addressSchool = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'school_details']/span");
                if (addressSchool != null)
                {
                    string address = string.Empty;
                    foreach (var schoolAddr in addressSchool)
                    {
                        if (!string.IsNullOrEmpty(schoolAddr.InnerTextOrNull()))
                            address += $"{schoolAddr.InnerTextOrNull()} ";
                    }

                    school.SchoolAddress = address.Trim(' ');
                }

                var summary = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-12 col-md-6 pull-left schooldetailReportLeft']/article/p").InnerTextOrNull();
                if (!string.IsNullOrEmpty(summary))
                    school.SchoolSummary = summary.Replace("  ", "").Replace("\n", "");

                string homefactsRatingPerYear = string.Empty;
                var homefactsRating = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'homefacts_rating_summary']");
                if (homefactsRating != null)
                {
                    List<string> homefactsRatingYears = new List<string>();
                    var years = homefactsRating.SelectNodes(".//ul/li/a");
                    if (years != null)
                    {
                        foreach (var year in years)
                        {
                            homefactsRatingYears.Add(year.InnerTextOrNull());
                        }
                    }

                    var schoolGrades = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'homefacts_rating_summary']/div");
                    if (schoolGrades != null)
                    {
                        for (int i = 0; i < schoolGrades.Count(); i++)
                        {
                            SchoolHomefact homefact = new SchoolHomefact();
                            string gradeYear = string.Empty;
                            var gradeYears = schoolGrades[i].SelectNodes(".//div");
                            if (gradeYears != null)
                            {
                                foreach (var grade in gradeYears)
                                {
                                    string ratingLevel = string.Empty;
                                    string gradeName = grade.SelectSingleNode(".//div").InnerTextOrNull();
                                    if (!string.IsNullOrEmpty(gradeName))
                                    {
                                        var rating = grade.SelectSingleNode(".//div[2]").AttributeOrNull("class");
                                        if (rating != null)
                                        {
                                            var ratingSpl = rating.Split(new string[] { "grade" }, StringSplitOptions.None)[1];

                                            if (ratingSpl.Contains("NA"))
                                                ratingLevel = "N/A";
                                            else if (ratingSpl.Contains("A_Plus"))
                                                ratingLevel = "A+";
                                            else if (ratingSpl.Contains("A_Minus"))
                                                ratingLevel = "A-";
                                            else if (ratingSpl.Contains("A"))
                                                ratingLevel = "A";
                                            else if (ratingSpl.Contains("B_Plus"))
                                                ratingLevel = "B+";
                                            else if (ratingSpl.Contains("B_Minus"))
                                                ratingLevel = "B-";
                                            else if (ratingSpl.Contains("B"))
                                                ratingLevel = "B";
                                            else if (ratingSpl.Contains("C_Plus"))
                                                ratingLevel = "C+";
                                            else if (ratingSpl.Contains("C_Minus"))
                                                ratingLevel = "C-";
                                            else if (ratingSpl.Contains("C"))
                                                ratingLevel = "C";
                                            else if (ratingSpl.Contains("D_Plus"))
                                                ratingLevel = "D+";
                                            else if (ratingSpl.Contains("D_Minus"))
                                                ratingLevel = "D-";
                                            else if (ratingSpl.Contains("D"))
                                                ratingLevel = "D";
                                            else if (ratingSpl.Contains("E_Plus"))
                                                ratingLevel = "E+";
                                            else if (ratingSpl.Contains("E_Minus"))
                                                ratingLevel = "E-";
                                            else if (ratingSpl.Contains("E"))
                                                ratingLevel = "E";
                                            else if (ratingSpl.Contains("F_Plus"))
                                                ratingLevel = "F+";
                                            else if (ratingSpl.Contains("F_Minus"))
                                                ratingLevel = "F-";
                                            else if (ratingSpl.Contains("F"))
                                                ratingLevel = "F";
                                            else
                                            {

                                            }

                                            if (!string.IsNullOrEmpty(ratingLevel))
                                            {
                                                if (gradeName == "Overall")
                                                    homefact.Overall = ratingLevel;
                                                else if (gradeName == "Math")
                                                    homefact.Math = ratingLevel;
                                                else if (gradeName == "Language Arts")
                                                    homefact.LanguageArts = ratingLevel;
                                                else if (gradeName == "Science")
                                                    homefact.Science = ratingLevel;

                                                //gradeYear += $"{gradeName}~{ratingLevel}^";
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                            }

                            //homefactsRatingPerYear += $"{homefactsRatingYears[i]}:{gradeYear}";
                            homefact.Year = homefactsRatingYears[i];
                            homefacts.Add(homefact);
                        }
                    }
                }

                //school.HomefactsRatingPerYear = homefactsRatingPerYear.Trim('^');

                List<SchoolAllTables> allTables = new List<SchoolAllTables>();
                string tables = string.Empty;
                var grafics = htmlDoc.DocumentNode.SelectNodes("//div[@id = 'test_score_container']/div");
                if (grafics != null)
                {
                    string infoes = string.Empty;
                    foreach (var item in grafics)
                    {
                        var gradeTabels = item.SelectNodes(".//div");
                        foreach (var table in gradeTabels)
                        {
                            string grade = item.SelectSingleNode(".//span").InnerTextOrNull();
                            if (!string.IsNullOrEmpty(table.InnerTextOrNull()))
                            {
                                string graficName = table.InnerTextOrNull();
                                tables += $"{grade}~{graficName}^";
                            }
                        }
                    }
                }

                tables = tables.Trim('^');
                string persent = string.Empty;
                string tablesInfo = string.Empty;
                string genderStatistic = string.Empty;
                //string ethnicities = string.Empty;

                SchoolEthnicity ethnicity = new SchoolEthnicity();
                var grafInfoes = htmlDoc.DocumentNode.SelectSingleNode($"//*[contains(text(),'drawTestChart')]").InnerTextOrNull();
                var persentesInfo = Regex.Matches(grafInfoes, $@"\[(.*?)\]");
                foreach (var persentInfo in persentesInfo)
                {
                    try
                    {
                        if (persentInfo.ToString().Contains("\"School\"") || persentInfo.ToString().Contains("\"District\"") || persentInfo.ToString().Contains("\"State\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            persent += $"{info[0].ToString().Trim('\"')}~{info[2].ToString().Trim('\"')}~";
                        }

                        if (persent.Contains("School") && persent.Contains("District") && persent.Contains("State"))
                        {
                            tablesInfo += $"{persent.Trim('~')}^";
                            persent = string.Empty;
                        }

                        if (persentInfo.ToString().Contains("Female"))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
                            int.TryParse(info[0].ToString().Trim(',').Trim(' '), out int female);
                            school.Female = female;

                            //var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
                            //var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
                            //persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
                        }

                        if (persentInfo.ToString().Contains("Male"))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
                            int.TryParse(info[0].ToString().Trim(',').Trim(' '), out int male);
                            school.Male = male;

                            //var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
                            //var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
                            //persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
                        }

                        //if (persent.Contains("Female") && persent.Contains("Male"))
                        //{
                        //    genderStatistic = persent.Trim('^');
                        //    persent = string.Empty;
                        //}

                        if (persentInfo.ToString().Contains("\"Asian\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
                            //ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
                            ethnicity.Asian = info2[0].ToString().Trim(',').Trim(' ').Trim(']');
                        }

                        if (persentInfo.ToString().Contains("\"African American\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
                            //ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
                            ethnicity.AfricanAmerican = info2[0].ToString().Trim(',').Trim(' ').Trim(']');
                        }

                        if (persentInfo.ToString().Contains("\"Hispanic\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
                            //ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
                            ethnicity.Hispanic = info2[0].ToString().Trim(',').Trim(' ').Trim(']');
                        }

                        if (persentInfo.ToString().Contains("\"Hawaiian/Pacific Islander\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
                            //ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
                            ethnicity.HawaiianOrPacificIslander = info2[0].ToString().Trim(',').Trim(' ').Trim(']');
                        }

                        if (persentInfo.ToString().Contains("\"American Indian/Alaskan\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
                            //ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
                            ethnicity.AmericanIndianOrAlaskan = info2[0].ToString().Trim(',').Trim(' ').Trim(']');
                        }

                        if (persentInfo.ToString().Contains("\"White\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
                            //ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
                            ethnicity.White = info2[0].ToString().Trim(',').Trim(' ').Trim(']');
                        }

                        if (persentInfo.ToString().Contains("\"2 or More Races\""))
                        {
                            var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
                            var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
                            //ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
                            ethnicity.TwoOrMoreRaces = info2[0].ToString().Trim(',').Trim(' ').Trim(']');
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

                //school.Ethnicities = ethnicities.Trim('^');
                tablesInfo = tablesInfo.Trim('^');
                string allTables2 = string.Empty;
                var tablesSpl = tables.Split('^');
                var tablesInfoSpl = tablesInfo.Split('^');
                for (int i = 0, t = 1; i < tablesSpl.Count(); i++, t++)
                {
                    allTables2 += $"{tablesSpl[i]}:{tablesInfoSpl[t]}^";
                }

                allTables2 = allTables2.Trim('^');
                string enrollmentbyGrade = string.Empty;
                var enrollments = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'enrollment-stats']/div/div/div");
                if (enrollments != null)
                {
                    foreach (var enrollment in enrollments)
                    {
                        if (!string.IsNullOrEmpty(enrollment.SelectSingleNode(".//div").InnerTextOrNull()) && !string.IsNullOrEmpty(enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull()))
                        {
                            if (enrollment.SelectSingleNode(".//div").InnerTextOrNull() == "Grade Level" && enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull() == "Enrollment")
                                continue;

                            enrollmentbyGrade += $"{enrollment.SelectSingleNode(".//div").InnerTextOrNull()}~{enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull()}^";
                        }
                    }
                }

                enrollmentbyGrade = enrollmentbyGrade.Trim('^');
                SchoolDataItems.Add(school);
            }
            catch (Exception e)
            {

            }
        }

        //public void DistrictSchool(string url, string schoolType)
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        //        var html = PageRetriever.ReadFromServer(url, true);
        //        var htmlDoc = new HtmlDocument();
        //        html = HttpUtility.HtmlDecode(html);
        //        htmlDoc.LoadHtml(html);
        //        School school = new School();

        //        school.SchoolType = schoolType;
        //        school.SchoolName = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop = 'legalName']").InnerTextOrNull();

        //        string address = string.Empty;
        //        var addressSchool = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'school_details']/span");
        //        if (addressSchool != null)
        //        {
        //            foreach (var schoolAdrr in addressSchool)
        //            {
        //                if (!string.IsNullOrEmpty(schoolAdrr.InnerTextOrNull()))
        //                    address += $"{schoolAdrr.InnerTextOrNull()} ";
        //            }

        //            school.SchoolAddress = address.Trim(' ');
        //        }

        //        var summary = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-12 col-md-6 pull-left schooldetailReportLeft']/article").InnerTextOrNull();
        //        if (!string.IsNullOrEmpty(summary))
        //            school.SchoolSummary = summary.Replace("  ", "").Replace("\n", "");

        //        string homefactsRatingPerYear = string.Empty;
        //        var homefactsRating = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'homefacts_rating_summary']");
        //        if (homefactsRating != null)
        //        {
        //            List<string> homefactsRatingYears = new List<string>();
        //            var years = homefactsRating.SelectNodes(".//ul/li/a");
        //            if (years != null)
        //            {
        //                foreach (var year in years)
        //                {
        //                    homefactsRatingYears.Add(year.InnerTextOrNull());
        //                }
        //            }

        //            var schoolGrades = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'homefacts_rating_summary']/div");
        //            if (schoolGrades != null)
        //            {
        //                for (int i = 0; i < schoolGrades.Count(); i++)
        //                {
        //                    string gradeYear = string.Empty;
        //                    var gradeYears = schoolGrades[i].SelectNodes(".//div");
        //                    if (gradeYears != null)
        //                    {
        //                        foreach (var grade in gradeYears)
        //                        {
        //                            string ratingLevel = string.Empty;
        //                            string gradeName = grade.SelectSingleNode(".//div").InnerTextOrNull();
        //                            if (!string.IsNullOrEmpty(gradeName))
        //                            {
        //                                var rating = grade.SelectSingleNode(".//div[2]").AttributeOrNull("class");
        //                                if (rating != null)
        //                                {
        //                                    var ratingSpl = rating.Split(new string[] { "grade" }, StringSplitOptions.None)[1];

        //                                    if (ratingSpl.Contains("NA"))
        //                                        ratingLevel = "N/A";
        //                                    else if (ratingSpl.Contains("A_Plus"))
        //                                        ratingLevel = "A+";
        //                                    else if (ratingSpl.Contains("A_Minus"))
        //                                        ratingLevel = "A-";
        //                                    else if (ratingSpl.Contains("A"))
        //                                        ratingLevel = "A";
        //                                    else if (ratingSpl.Contains("B_Plus"))
        //                                        ratingLevel = "B+";
        //                                    else if (ratingSpl.Contains("B_Minus"))
        //                                        ratingLevel = "B-";
        //                                    else if (ratingSpl.Contains("B"))
        //                                        ratingLevel = "B";
        //                                    else if (ratingSpl.Contains("C_Plus"))
        //                                        ratingLevel = "C+";
        //                                    else if (ratingSpl.Contains("C_Minus"))
        //                                        ratingLevel = "C-";
        //                                    else if (ratingSpl.Contains("C"))
        //                                        ratingLevel = "C";
        //                                    else if (ratingSpl.Contains("D_Plus"))
        //                                        ratingLevel = "D+";
        //                                    else if (ratingSpl.Contains("D_Minus"))
        //                                        ratingLevel = "D-";
        //                                    else if (ratingSpl.Contains("D"))
        //                                        ratingLevel = "D";
        //                                    else if (ratingSpl.Contains("E_Plus"))
        //                                        ratingLevel = "E+";
        //                                    else if (ratingSpl.Contains("E_Minus"))
        //                                        ratingLevel = "E-";
        //                                    else if (ratingSpl.Contains("E"))
        //                                        ratingLevel = "E";
        //                                    else if (ratingSpl.Contains("F_Plus"))
        //                                        ratingLevel = "F+";
        //                                    else if (ratingSpl.Contains("F_Minus"))
        //                                        ratingLevel = "F-";
        //                                    else if (ratingSpl.Contains("F"))
        //                                        ratingLevel = "F";
        //                                    else
        //                                    {

        //                                    }

        //                                    if (!string.IsNullOrEmpty(ratingLevel))
        //                                        gradeYear += $"{gradeName}~{ratingLevel}~";
        //                                    else
        //                                    {

        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    homefactsRatingPerYear += $"{homefactsRatingYears[i]}:{gradeYear.Trim('~')}^";
        //                }
        //            }
        //        }

        //        school.HomefactsRatingPerYear = homefactsRatingPerYear.Trim('^');
        //        string tables = string.Empty;
        //        var grafics = htmlDoc.DocumentNode.SelectNodes("//div[@id = 'test_score_container']/div");
        //        if (grafics != null)
        //        {
        //            string infoes = string.Empty;
        //            foreach (var item in grafics)
        //            {
        //                var gradeTabels = item.SelectNodes(".//div");
        //                foreach (var table in gradeTabels)
        //                {
        //                    string grade = item.SelectSingleNode(".//span").InnerTextOrNull();
        //                    if (!string.IsNullOrEmpty(table.InnerTextOrNull()))
        //                    {
        //                        string graficName = table.InnerTextOrNull();
        //                        tables += $"{grade}~{graficName}^";
        //                    }
        //                }
        //            }
        //        }

        //        tables = tables.Trim('^');
        //        string persent = string.Empty;
        //        string tablesInfo = string.Empty;
        //        string genderStatistic = string.Empty;
        //        string ethnicities = string.Empty;

        //        var grafInfoes = htmlDoc.DocumentNode.SelectSingleNode($"//*[contains(text(),'drawTestChart')]").InnerTextOrNull();
        //        var persentesInfo = Regex.Matches(grafInfoes, $@"\[(.*?)\]");
        //        foreach (var persentInfo in persentesInfo)
        //        {
        //            try
        //            {
        //                if (persentInfo.ToString().Contains("\"District\"") || persentInfo.ToString().Contains("\"State\""))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
        //                    persent += $"{info[0].ToString().Trim('\"')}~{info[2].ToString().Trim('\"')}~";
        //                }

        //                if (persent.Contains("District") && persent.Contains("State"))
        //                {
        //                    tablesInfo += $"{persent.Trim('~')}^";
        //                    persent = string.Empty;
        //                }

        //                if (persentInfo.ToString().Contains("Female"))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    school.Female = info[0].ToString().Trim(',').Trim(' ');

        //                    //var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
        //                    //var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    //persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
        //                }

        //                if (persentInfo.ToString().Contains("Male"))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    school.Male = info[0].ToString().Trim(',').Trim(' ');

        //                    //var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
        //                    //var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    //persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
        //                }

        //                //if (persentInfo.ToString().Contains("Female") || persentInfo.ToString().Contains("Male"))
        //                //{
        //                //    var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
        //                //    var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                //    persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
        //                //}

        //                //if (persent.Contains("Female") && persent.Contains("Male"))
        //                //{
        //                //    genderStatistic = persent.Trim('^');
        //                //    persent = string.Empty;
        //                //}

        //                if (persentInfo.ToString().Contains("\"Asian\"") || persentInfo.ToString().Contains("\"African American\"") || persentInfo.ToString().Contains("\"Hispanic\"") ||
        //                    persentInfo.ToString().Contains("\"Hawaiian/Pacific Islander\"") || persentInfo.ToString().Contains("\"American Indian/Alaskan\"") || persentInfo.ToString().Contains("\"White\"") ||
        //                    persentInfo.ToString().Contains("\"2 or More Races\""))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
        //                    var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
        //                    ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
        //                }
        //            }
        //            catch (Exception e)
        //            {

        //            }
        //        }

        //        school.Ethnicities = ethnicities.Trim('^');
        //        tablesInfo = tablesInfo.Trim('^');
        //        string allTables = string.Empty;
        //        var tablesSpl = tables.Split('^');
        //        var tablesInfoSpl = tablesInfo.Split('^');
        //        for (int i = 0; i < tablesSpl.Count(); i++)
        //        {
        //            allTables += $"{tablesSpl[i]}:{tablesInfoSpl[i]}^";
        //        }

        //        school.AllTables = allTables.Trim('^');
        //        string enrollmentbyGrade = string.Empty;
        //        var enrollments = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'enrollment-stats']/div/div/div");
        //        if (enrollments != null)
        //        {
        //            foreach (var enrollment in enrollments)
        //            {
        //                if (!string.IsNullOrEmpty(enrollment.SelectSingleNode(".//div").InnerTextOrNull()) && !string.IsNullOrEmpty(enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull()))
        //                {
        //                    if (enrollment.SelectSingleNode(".//div").InnerTextOrNull() == "Grade Level" && enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull() == "Enrollment")
        //                        continue;

        //                    enrollmentbyGrade += $"{enrollment.SelectSingleNode(".//div").InnerTextOrNull()}~{enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull()}^";
        //                }
        //            }
        //        }

        //        school.EnrollmentbyGrade = enrollmentbyGrade.Trim('^');
        //        SchoolDataItems.Add(school);
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //public void PrivateSchool(string url, string schoolType)
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        //        var html = PageRetriever.ReadFromServer(url, true);
        //        var htmlDoc = new HtmlDocument();
        //        html = HttpUtility.HtmlDecode(html);
        //        htmlDoc.LoadHtml(html);
        //        SchoolDataItem school = new SchoolDataItem();

        //        school.SchoolType = schoolType;
        //        school.SchoolName = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop = 'legalName']").InnerTextOrNull();

        //        string address = string.Empty;
        //        var addressSchool = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'school_details']/span");
        //        if (addressSchool != null)
        //        {
        //            foreach (var schoolAdrr in addressSchool)
        //            {
        //                if (!string.IsNullOrEmpty(schoolAdrr.InnerTextOrNull()))
        //                    address += $"{schoolAdrr.InnerTextOrNull()} ";
        //            }

        //            school.SchoolAddress = address.Trim(' ');
        //        }

        //        var summary = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-12 col-md-6 pull-left schooldetailReportLeft']/article/p").InnerTextOrNull();
        //        if (!string.IsNullOrEmpty(summary))
        //            school.SchoolSummary = summary.Replace("  ", "").Replace("\n", "");

        //        string characteristics = string.Empty;
        //        var schoolCharacteristics = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'row schooldetailReport']/div/div[4]/div");
        //        if (schoolCharacteristics != null)
        //        {
        //            foreach (var schoolCharacteristic in schoolCharacteristics)
        //            {
        //                if (!string.IsNullOrEmpty(schoolCharacteristic.InnerTextOrNull()))
        //                    characteristics += $"{schoolCharacteristic.InnerTextOrNull().Replace("  ", "").Replace("\n", "^")}^";
        //            }

        //            school.Characteristics = characteristics.Trim('^');
        //        }

        //        string persent = string.Empty;
        //        string genderStatistic = string.Empty;
        //        string ethnicities = string.Empty;

        //        var grafInfoes = htmlDoc.DocumentNode.SelectSingleNode($"//*[contains(text(),'drawTestChart')]").InnerTextOrNull();
        //        var persentesInfo = Regex.Matches(grafInfoes, $@"\[(.*?)\]");
        //        foreach (var persentInfo in persentesInfo)
        //        {
        //            try
        //            {
        //                if (persentInfo.ToString().Contains("Female"))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    school.Female = info[0].ToString().Trim(',').Trim(' ');

        //                    //var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
        //                    //var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    //persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
        //                }

        //                if (persentInfo.ToString().Contains("Male"))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    school.Male = info[0].ToString().Trim(',').Trim(' ');

        //                    //var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
        //                    //var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                    //persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
        //                }

        //                //if (persentInfo.ToString().Contains("Female") || persentInfo.ToString().Contains("Male"))
        //                //{
        //                //    var info = Regex.Matches(persentInfo.ToString(), $@"\'(.*?)\'");
        //                //    var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)\,");
        //                //    persent += $"{info[0].ToString().Trim('\'')}~{info2[0].ToString().Trim(',').Trim(' ')}^";
        //                //}

        //                //if (persent.Contains("Female") && persent.Contains("Male"))
        //                //{
        //                //    genderStatistic = persent.Trim('^');
        //                //    persent = string.Empty;
        //                //}

        //                if (persentInfo.ToString().Contains("\"Asian\"") || persentInfo.ToString().Contains("\"African American\"") || persentInfo.ToString().Contains("\"Hispanic\"") ||
        //                    persentInfo.ToString().Contains("\"Hawaiian/Pacific Islander\"") || persentInfo.ToString().Contains("\"American Indian/Alaskan\"") || persentInfo.ToString().Contains("\"White\"") ||
        //                    persentInfo.ToString().Contains("\"2 or More Races\""))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
        //                    var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
        //                    ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
        //                }
        //            }
        //            catch (Exception e)
        //            {

        //            }
        //        }

        //        school.Ethnicities = ethnicities.Trim('^');

        //        string enrollmentbyGrade = string.Empty;
        //        var enrollments = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'enrollment-stats']/div/div/div");
        //        if (enrollments != null)
        //        {
        //            foreach (var enrollment in enrollments)
        //            {
        //                if (!string.IsNullOrEmpty(enrollment.SelectSingleNode(".//div").InnerTextOrNull()) && !string.IsNullOrEmpty(enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull()))
        //                {
        //                    if (enrollment.SelectSingleNode(".//div").InnerTextOrNull() == "Grade Level" && enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull() == "Enrollment")
        //                        continue;

        //                    enrollmentbyGrade += $"{enrollment.SelectSingleNode(".//div").InnerTextOrNull()}~{enrollment.SelectSingleNode(".//div[2]").InnerTextOrNull()}^";
        //                }
        //            }
        //        }

        //        school.EnrollmentbyGrade = enrollmentbyGrade.Trim('^');
        //        SchoolDataItems.Add(school);
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //public void CollegesAndUniversities(string url, string schoolType)
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        //        var html = PageRetriever.ReadFromServer(url, true);
        //        var htmlDoc = new HtmlDocument();
        //        html = HttpUtility.HtmlDecode(html);
        //        htmlDoc.LoadHtml(html);
        //        SchoolDataItem school = new SchoolDataItem();

        //        school.SchoolType = schoolType;
        //        school.SchoolName = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'content-part']/h1").InnerTextOrNull();

        //        string address = string.Empty;
        //        var addressSchool = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'content-part']/span[2]").InnerTextOrNull();
        //        var phone = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'content-part']/a").InnerTextOrNull();
        //        if (!string.IsNullOrEmpty(addressSchool) && !string.IsNullOrEmpty(phone))
        //            school.SchoolAddress = $"{addressSchool} {phone}";

        //        string institutionType = string.Empty;
        //        string totalEnrollment = string.Empty;
        //        string tuition = string.Empty;
        //        var instituteDetails = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'institute-detail']/div/div/div");
        //        if (instituteDetails != null)
        //        {
        //            foreach (var instituteDetail in instituteDetails)
        //            {
        //                var detailName = instituteDetail.SelectSingleNode(".//span").InnerTextOrNull();
        //                var detailValue = instituteDetail.SelectSingleNode(".//span[2]").InnerTextOrNull();

        //                if (detailName == "Institution Type")
        //                    school.InstitutionType = detailValue;
        //                else if (detailName == "Total Enrollment")
        //                    school.TotalEnrollment = detailValue;
        //                else if (detailName == "In-State Tuition")
        //                    school.InStateTuition = detailValue;
        //                else
        //                {

        //                }
        //            }
        //        }

        //        //string genderStatistic = string.Empty;
        //        var genders = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'gender-ratio']/div");
        //        if (genders != null)
        //        {
        //            foreach (var gender in genders)
        //            {
        //                var genderName = gender.SelectSingleNode(".//span[2]").InnerTextOrNull();
        //                var genderValue = gender.SelectSingleNode(".//span").InnerTextOrNull();

        //                if (!string.IsNullOrEmpty(genderName) && !string.IsNullOrEmpty(genderValue))
        //                {
        //                    if (genderName.ToLower() == "female")
        //                        school.Female = genderValue;
        //                    else if (genderName.ToLower() == "male")
        //                        school.Male = genderValue;
        //                }
        //            }
        //        }

        //        //genderStatistic = genderStatistic.Trim('^');
        //        string persent = string.Empty;
        //        string ethnicities = string.Empty;

        //        var grafInfoes = htmlDoc.DocumentNode.SelectSingleNode($"//*[contains(text(),'drawRacebreakdownChart')]").InnerTextOrNull();
        //        var persentesInfo = Regex.Matches(grafInfoes, $@"\[(.*?)\]");
        //        foreach (var persentInfo in persentesInfo)
        //        {
        //            try
        //            {
        //                if (persentInfo.ToString().Contains("\"African American\"") || persentInfo.ToString().Contains("\"American Indian or Alaska Native\"") || persentInfo.ToString().Contains("\"Asian\"") ||
        //                    persentInfo.ToString().Contains("\"Hispanic\"") || persentInfo.ToString().Contains("\"Native Hawaiian or Pacific Islander\"") || persentInfo.ToString().Contains("\"Two or More Races\"") ||
        //                    persentInfo.ToString().Contains("\"White (Non Hispanic)\"") || persentInfo.ToString().Contains("\"Unknown\""))
        //                {
        //                    var info = Regex.Matches(persentInfo.ToString(), $@"\""(.*?)\""");
        //                    var info2 = Regex.Matches(persentInfo.ToString(), $@"\,(.*?)]");
        //                    ethnicities += $"{info[0].ToString().Trim('\"')}~{info2[0].ToString().Trim(',').Trim(' ').Trim(']')}^";
        //                }
        //            }
        //            catch (Exception e)
        //            {

        //            }
        //        }

        //        school.Ethnicities = ethnicities.Trim('^');
        //        string admission = string.Empty;
        //        var instituteAdmissions = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'institute-admission']/div/ul/li");
        //        if (instituteAdmissions != null)
        //        {
        //            foreach (var instituteAdmission in instituteAdmissions)
        //            {
        //                var admissionValue = instituteAdmission.InnerTextOrNull();
        //                if (!string.IsNullOrEmpty(admissionValue))
        //                    admission += $"{admissionValue},";
        //            }

        //            school.Admission = admission.Trim(',');
        //        }

        //        //string totalExpenses = string.Empty;
        //        var expenses = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'col-xs-12 col-sm-6 col-md-6 col-lg-6 estimated-exepense-left']/div/div/div");
        //        if (expenses != null)
        //        {
        //            foreach (var expense in expenses)
        //            {
        //                var expenseName = expense.SelectSingleNode(".//p[2]").InnerTextOrNull();
        //                var expenseValue = expense.SelectSingleNode(".//p").InnerTextOrNull();

        //                if (!string.IsNullOrEmpty(expenseName) && !string.IsNullOrEmpty(expenseValue))
        //                {
        //                    if (expenseName.ToLower() == "in-state")
        //                        school.TotalExpensesInState = expenseValue;
        //                    else if (expenseName.ToLower() == "out-of-state")
        //                        school.TotalExpensesOutOfState = expenseValue;
        //                }
        //            }

        //            //totalExpenses = totalExpenses.Trim('^');
        //        }

        //        //string tutionAndFeesInfo = string.Empty;
        //        var tuitionAndFees = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'university-costs']/div[2]/div/div");
        //        if (tuitionAndFees != null)
        //        {
        //            int count = 0;

        //            foreach (var tuitionAndFeesItem in tuitionAndFees)
        //            {
        //                var infoes = tuitionAndFeesItem.SelectNodes(".//div");

        //                if (infoes[0].InnerTextOrNull().ToLower() == "tuition and fees")
        //                    count = infoes.Count() - 1;

        //                if (infoes[0].InnerTextOrNull().ToLower() == "in-state")
        //                    school.TuitionAndFeesInState = infoes[count].InnerTextOrNull();
        //                else if (infoes[0].InnerTextOrNull().ToLower() == "out-of-state")
        //                    school.TuitionAndFeesOutOfState = infoes[count].InnerTextOrNull();
        //                 else if (infoes[0].InnerTextOrNull().ToLower() == "books and supplies")
        //                    school.TuitionAndFeesBooksAndSupplies = infoes[count].InnerTextOrNull();
        //            }

        //            //tutionAndFeesInfo = tutionAndFeesInfo.Trim('^');
        //        }

        //        string livingArrangementsInfo = string.Empty;
        //        var livingArrangements = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'university-costs']/div[3]/div/div");
        //        if (livingArrangements != null)
        //        {
        //            int count = 0;

        //            foreach (var livingArrangementsItem in livingArrangements)
        //            {
        //                var infoes = livingArrangementsItem.SelectNodes(".//div");

        //                if (infoes[0].InnerTextOrNull().ToLower() == "living arrangements")
        //                    count = infoes.Count() - 1;

        //                if (infoes[0].InnerTextOrNull().ToLower() == "on campus")
        //                    livingArrangementsInfo += $"On campus:";
        //                else if (infoes[0].InnerTextOrNull().ToLower() == "off campus")
        //                    livingArrangementsInfo += $"Off campus:";
        //                else if (infoes[0].InnerTextOrNull().ToLower() == "off campus (with family)")
        //                    livingArrangementsInfo += $"Off campus (with family):";
        //                else
        //                {
        //                    if (infoes[0].InnerTextOrNull().ToLower() == "room and board")
        //                        livingArrangementsInfo += $"Room and board~{infoes[count].InnerTextOrNull()}^";
        //                    else if (infoes[0].InnerTextOrNull().ToLower() == "other expenses")
        //                        livingArrangementsInfo += $"Other expenses~{infoes[count].InnerTextOrNull()}|";
        //                }
        //            }

        //            livingArrangementsInfo = livingArrangementsInfo.Trim('|');
        //        }

        //        var livingSpl = livingArrangementsInfo.Split('|');
        //        {
        //            foreach (var living in livingSpl)
        //            {
        //                var spl = living.Split(':');
        //                if (spl[0] == "On campus")
        //                {
        //                    var info = spl[1].Split('^');
        //                    foreach (var item in info)
        //                    {
        //                        var itemSpl = item.Split('~');
        //                        if (itemSpl[0].Contains("Room and board"))
        //                            school.OnCampusRoomAndBoard = itemSpl[1];
        //                        else if (itemSpl[0].Contains("Other expenses"))
        //                            school.OnCampusOtherExpenses = itemSpl[1];
        //                    }
        //                }
        //                else if (spl[0] == "Off campus")
        //                {
        //                    var info = spl[1].Split('^');
        //                    foreach (var item in info)
        //                    {
        //                        var itemSpl = item.Split('~');
        //                        if (itemSpl[0].Contains("Room and board"))
        //                            school.OffCampusRoomAndBoard = itemSpl[1];
        //                        else if (itemSpl[0].Contains("Other expenses"))
        //                            school.OffCampusOtherExpenses = itemSpl[1];
        //                    }
        //                }
        //                else if (spl[0] == "Off campus (with family)")
        //                {
        //                    var info = spl[1].Split('^');
        //                    foreach (var item in info)
        //                    {
        //                        var itemSpl = item.Split('~');
        //                        if (itemSpl[0].Contains("Room and board"))
        //                            school.OffCampusWFRoomAndBoard = itemSpl[1];
        //                        else if (itemSpl[0].Contains("Other expenses"))
        //                            school.OffCampusWFOtherExpenses = itemSpl[1];
        //                    }
        //                }
        //            }
        //        }

        //        SchoolDataItems.Add(school);
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        public void Crime(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var html = PageRetriever.ReadFromServer(url, true);
                var htmlDoc = new HtmlDocument();
                html = HttpUtility.HtmlDecode(html);
                htmlDoc.LoadHtml(html);

                var name = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-12 col-md-12']/h1/strong/span[1]").InnerTextOrNull();
                var lastKnownAddress = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'offender-address']/h2/span/dd").InnerTextOrNull();
                var image = htmlDoc.DocumentNode.SelectSingleNode("//div[@class = 'col-xs-12 col-sm-6 col-md-8 text-center']/figure/span/img").AttributeOrNull("src");
                if (image != null)
                    image = $"https://www.homefacts.com{image}";

                var dob = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section1_details']/dl/dd[1]/span").InnerTextOrNull();
                var race = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section1_details']/dl/dd[2]/span").InnerTextOrNull();
                var sex = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section1_details']/dl/dd[3]/span").InnerTextOrNull();
                var eyes = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section1_details']/dl/dd[4]/span").InnerTextOrNull();
                var height = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section1_details']/dl/dd[5]/span").InnerTextOrNull();
                var hair = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section1_details']/dl/dd[6]/span").InnerTextOrNull();
                var weight = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section1_details']/dl/dd[7]/span").InnerTextOrNull();
                var offenseOrStatute = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section2']/dl/dd[1]/span").InnerTextOrNull();
                var dateConvicted = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section2']/dl/dd[2]/span").InnerTextOrNull();
                var alias = htmlDoc.DocumentNode.SelectSingleNode("//dl[@class = 'aliases']/p/span[1]").InnerTextOrNull();
                var scars = htmlDoc.DocumentNode.SelectSingleNode("//dl[@class = 'scars']/p").InnerTextOrNull();
                var disclaimer = htmlDoc.DocumentNode.SelectSingleNode("//div[@id = 'offender_section2']/p").InnerTextOrNull();
            }
            catch (Exception e)
            {
                
            }
        }

        protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
        {
            Action<ProcessQueueItem> act;
            if (item.ItemType == 10)
                act = ProcessZipPage;
            else if (item.ItemType == 20)
                act = ProcessSchoolPage;
            else act = null;

            return act;
        }
    }
}
