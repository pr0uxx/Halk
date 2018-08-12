using Markdig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HakunaMatataWeb.Utilities
{
    public class Helper
    {
        public static String CleanInputString(string dirtyString)
        {
            return new string(dirtyString.Where(Char.IsLetterOrDigit).ToArray());
        }

        public static SelectList GetEnumSelectList<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            List<ListItem> items = new List<ListItem>();

            var count = 0;
            foreach (var i in values)
            {
                items.Add(new ListItem()
                {
                    Text = Enum.GetName(typeof(T), i),
                    Value = count.ToString(),
                });
                count++;
            }

            return new SelectList(items);
        }

        public static List<KeyValuePair<string, int>> GetEnumValues<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            var items = new List<KeyValuePair<string, int>>();

            var count = 0;
            foreach (var i in values)
            {
                items.Add(new KeyValuePair<string, int>(Enum.GetName(typeof(T), i), count));
                count++;
            }

            return items;
        }

        public static SelectList GetTimeZoneList()
        {
            var items = new List<ListItem>()
            {
                new ListItem()
                {
                    Value = "0",
                    Text = "Select an Option"
                }
            };

            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                items.Add(new ListItem()
                {
                    Value = tz.Id,
                    Text = tz.DisplayName
                });
            }

            //var list = from tz in TimeZoneInfo.GetSystemTimeZones()
            //           select new SelectListItem { Value = tz.Id, Text = tz.DisplayName };

            return new SelectList(items, "Value", "Text", "0");
        }

        public static DateTime ToLocalTime(DateTime utcDate, string localTimeZoneId)
        {
            try
            {
                var localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(localTimeZoneId);
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcDate, localTimeZone);
                return localTime;
            }
            catch
            {
                return utcDate;
            }
        }

        public static bool IsImageUrl(string URL)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(URL);
            req.Method = "HEAD";
            req.Timeout = 1000;
            try
            {
                using (var resp = req.GetResponse())
                {
                    return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                               .StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts Markdown string to Html
        /// </summary>
        /// <param name="markdown"></param>
        /// <returns></returns>
        public static string ConvertMarkdown(string markdown)
        {
            var result = string.Empty;

            try
            {
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                //m = Markdown.Normalize(m, null, pipeline);
                result = Markdown.ToHtml(markdown, pipeline);
            }
            catch (Exception ex)
            {
                result = string.Concat("Error: ", ex.Message);
            }

            return result;
        }

        public static string Base64Encode(string s)
        {
            try
            {
                var result = System.Text.Encoding.UTF8.GetBytes(s);
                return System.Convert.ToBase64String(result);
            }
            catch
            {
                return s;
            }
            
        }

        public static string Base64Decode(string s)
        {
            try
            {
                var result = System.Convert.FromBase64String(s);
                return System.Text.Encoding.UTF8.GetString(result);
            }
            catch
            {
                return s;
            }
            
        }

        public static string GetFirstUrlFromContent(string content)
        {
            if (content != null)
            {
                var m = Regex.Matches(content, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");

                if (m != null)
                {
                    foreach (Match i in m)
                    {
                        if (IsImageUrl(i.Value))
                        {
                            return (i.Value);
                        }
                    }
                }

            }

            return string.Empty;
        }

        public static DateTime GetNextEventDate(bool isBiWeekly, bool isMonthly, bool isUnique, bool isWeekly, DateTime firstEventDate)
        {
            var result = new DateTime();
            DateTime today = DateTime.Today;
            var thisMonth = DateTime.Now.Month;
            var thisYear = DateTime.Now.Year;
            var eventDayOfWeek = firstEventDate.DayOfWeek;

            if (isUnique)
            {
                return firstEventDate;
            }
            if (isMonthly)
            {
                result = new DateTime(thisYear, thisMonth, firstEventDate.Day);
            }
            if (isBiWeekly)
            {
                bool oddWeek = Convert.ToBoolean(firstEventDate.GetWeekOfMonth() % 2);
                int daysUntilEvent = ((int)eventDayOfWeek - (int)today.DayOfWeek + 7) % 7;
                var nextEventDate = DateTime.Today.AddDays(daysUntilEvent).AddHours(firstEventDate.Hour).AddMinutes(firstEventDate.Minute);

                bool isEventOddWeek = Convert.ToBoolean(nextEventDate.GetWeekOfMonth() % 2);

                if (oddWeek == isEventOddWeek)
                {
                    return nextEventDate;
                }
                else
                {
                    return nextEventDate.AddDays(7);
                }
            }

            return result;
        }
    }
}