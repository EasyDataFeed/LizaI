#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

#endregion

namespace RPMoutletInventory.Extensions
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
                    .Replace("/", "")
                    .Replace("\"", "")
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
                    .Replace("\r\n", "")
                    .Replace("\r", "")
                    .Replace("\n", "");
        }

        public static string ReplaceTextForAnchorText(this string p)
        {
            return p.RemoveDirtyData().Replace("&", " and ");
        }

        public static string RemoveUnicode(this string input)
        {
            var s = input;
            s = Regex.Replace(s, @"[^\u0000-\u007F]", string.Empty);
            return s;
        }

        public static string ReplaceUnicode(this string input)
        {
            return input.Replace("\\u002d", "-");
        }

        public static string PrepeadForLoad(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.RemoveUnicode().ReplaceTextForAnchorText().RemoveDirtyData().Trim();
            }
            return str;
        }

        public static string PrepareForCSV(this string str)
        {
            if (str != null && str.Contains(","))
            {
                str = string.Format("\"{0}\"", str);
            }
            return str;
        }

        public static DateTime? GetDateFromTimeStamp(this string timeStamp)
        {
            if (!string.IsNullOrEmpty(timeStamp))
            {
                if (timeStamp.Length > 10)
                {
                    timeStamp = timeStamp.Substring(0, 10);
                }
                var seconds = double.Parse(timeStamp);
                return
                    new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(seconds))
                        .ToLocalTime();
            }
            return null;
        }

        public static string PrepareDownloadReqFilter(this string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Email is empty. Please go to Settings tab and fill it");
            }
            email = HttpUtility.UrlEncode(email);
            return string.Format("isChangedView=1&prevHiddenListingFilter=3&ListingFilter=3&DownloadFormatType=1&DateRangeType=1&EmailAddress={0}&Save=Save&FEActiveDownloadType=1&FESoldDownloadType=1", email);
        }

        public static HtmlNode GetDocNode(this string html)
        {
            var htmlDoc = new HtmlDocument();
            html = HttpUtility.HtmlDecode(html);
            htmlDoc.LoadHtml(html);
            var docNode = htmlDoc.DocumentNode;
            return docNode;
        }

        public static void AddToListIfNotExist(this string str, List<string> list)
        {
            var exist = list.FirstOrDefault(s => s == str);
            if (exist == null)
            {
                list.Add(str);
            }
        }
    }
}
