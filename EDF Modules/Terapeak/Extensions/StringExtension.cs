#region using

using System;
using System.Text.RegularExpressions;
using System.Web;
using Terapeak.DataItems;

#endregion

namespace Terapeak.Extensions
{
    public static class StringExtension
    {
        public static string ToLowerCaseTags(this string input)
        {
            return Regex.Replace(
                input,
                @"<[^<>]+>",
                m => { return m.Value.ToLower(); },
                RegexOptions.Multiline | RegexOptions.Singleline);
        }

        public static string HtmlStrip(this string input)
        {
            input = Regex.Replace(input, @"<style>(.|\n)*?</style>", string.Empty);
            input = Regex.Replace(input, @"<STYLE>(.|\n)*?</STYLE>", string.Empty);
            input = Regex.Replace(input, @"<script(.|\n)*?</script>", string.Empty);
            input = Regex.Replace(input, @"<SCRIPT(.|\n)*?</SCRIPT>", string.Empty);
            input = Regex.Replace(input, @"style(.|\n)*?;", string.Empty);
            input = Regex.Replace(input, @"<xml>.[\n]$", string.Empty);
            //input = Regex.Replace(input, @"<img(.|\n)*?height", string.Empty);
            input = Regex.Replace(input, @"<img(.|\n)*?>;", string.Empty);
            input = Regex.Replace(input, @"<img(.|\n)*?(.gif |.png |.jpg )>", string.Empty);
            input = Regex.Replace(input, @"background-image: url((.|\n)*?);", string.Empty);
            input = Regex.Replace(input, @"<img(.|\n)*? class=prVerified>", string.Empty);
            return input;
        }

        public static string RemoveDirtyData(this string p)
        {
            return
                p.RemoveUnicode()
                    .Replace("--", "-")
                    .Replace(" - ", "-")
                    .Replace("*", "")
                    .Replace("€", " ")
                    .Replace("œ", " ")
                    .Replace("â", " ")
                    .Replace("¢", " ")
                    .Replace("Â", " ")
                    .Replace("®", " ")
                    .Replace("„", " ")
                    .Replace("  ", " ")
                    .Replace("[", " ")
                    .Replace("]", " ")
                    .Replace("{", " ")
                    .Replace("}", "")
                    .Replace("~~", "")
                    .Replace("varchar", "")
                    .Replace("sp_", "")
                    .Replace("xp_", "")
                    .Replace("™", "")
                    .Replace("“", "")
                    .Replace("Ã‚â€•", "")
                    .Replace("Ã¢â", "")
                    .Replace("¬â„¢s", "")
                    .Replace("€•", "")
                    .Replace("â€™", "")
                    .Replace("¢", "")
                    .Replace("@@", "")
                    .Replace("@", "")
                    .Replace("â€œ", "")
                    .Replace("â€”", "")
                    .Replace("insert into", "")
                    .Replace("/script", "")
                    .Replace("delete from", "")
                    .Replace("drop table", "")
                    .Replace("exec(", "")
                    .Replace("declare()*@", "")
                    .Replace("cast(", "")
                    .Replace("<strong>", "")
                    .Replace("</strong>", "")
                    .Replace("\r\n", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ");
        }

        public static string ReplaceTextForAnchorText(this string p)
        {
            return p.RemoveDirtyData().Replace("&amp;", " and ").Replace("&", " and ");
        }

        public static string RemoveUnicode(this string input)
        {
            var s = input;
            s = Regex.Replace(s, @"[^\u0000-\u007F]", string.Empty);
            return s;
        }

        public static string PrepeadForLoad(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.RemoveUnicode().ReplaceTextForAnchorText().RemoveDirtyData().Trim();
                str = Regex.Replace(str, @"(\s|\t|\r?\n)\1+", " ", RegexOptions.Multiline);
            }
            return str;
        }

        public static CategoryFilter GetSearchFilter(this string url)
        {
            var pageReg = new Regex(@"\?page=(.*?)[#,&]");
            var categoryReg = new Regex(@"categoriesList\?id=(\d+)");
            var siteReg = new Regex(@"siteID=(\d+)");
            var dateRangeReg = new Regex(@"date_range=(\d+)");

            if (pageReg.IsMatch(url) && categoryReg.IsMatch(url) && siteReg.IsMatch(url) && dateRangeReg.IsMatch(url))
            {
                var filter = new CategoryFilter
                {
                    PageName = pageReg.Match(url).Groups[1].Value,
                    CategoryId = int.Parse(categoryReg.Match(url).Groups[1].Value),
                    SiteId = int.Parse(siteReg.Match(url).Groups[1].Value),
                    DateRange = int.Parse(dateRangeReg.Match(url).Groups[1].Value)
                };

                return filter;
            }
            throw new ArgumentException(string.Format("Research URL: {0} is not valid", url));
        }

        public static string CategorySearchUrl(this string url, int categoryId)
        {
            var categoryIdReg = new Regex(@"categoriesList\?id=(\d+)");

            if (categoryIdReg.IsMatch(url))
            {
                return categoryIdReg.Replace(url, string.Format("categoriesList?id={0}", categoryId));
            }
            throw new ArgumentException(string.Format("Research URL: {0} is not valid", url));
        }
    }
}
