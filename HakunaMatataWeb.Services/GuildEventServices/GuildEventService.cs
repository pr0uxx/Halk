using HakunaMatataWeb.Data.DataConnection;
using HakunaMatataWeb.Data.Enums;
using HakunaMatataWeb.Models.GuildEventModels;
using HakunaMatataWeb.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Services.GuildEventServices
{
    public class GuildEventService : IGuildEventService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<GuildEventCalendarViewModel> GetMonthEventCalendarAsync(int month, int year, string localTimeZone)
        {
            
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var m = new GuildEventCalendarViewModel();
            m.DayDataList = new List<DayData>();
            var dateCounter = firstDayOfMonth;
            m.Month = firstDayOfMonth.Month;
            m.MonthString = firstDayOfMonth.ToString("MMMM");
            m.Year = firstDayOfMonth.Year;

            var guildEvents = await db.GuildEvents.Where(x => x.LastEventDate >= firstDayOfMonth).ToListAsync();

            while (dateCounter <= lastDayOfMonth)
            {
                var d = new DayData()
                {
                    Date = dateCounter,
                    DayName = dateCounter.DayOfWeek.ToString(),
                    DayOfWeek = (int)dateCounter.DayOfWeek,
                    DayOfMonth = (int)dateCounter.Day,
                    WeekOfMonth = (int)dateCounter.GetWeekOfMonth()
                };

                if (guildEvents.Count > 0)
                {
                    d.GuildEvents = new List<Tuple<string, int, string>>();
                    foreach (var r in guildEvents)
                    {
                        if (r.EventDayOfMonth.Equals(d.DayOfMonth) && r.IsMonthly)
                        {
                            d.GuildEvents.Add(new Tuple<string, int, string>(r.Title, r.Id, r.EventType.ToString()));
                            continue;
                        }
                        if (r.EventDayOfWeek.Equals(d.DayOfWeek) && r.IsWeekly)
                        {
                            d.GuildEvents.Add(new Tuple<string, int, string>(r.Title, r.Id, r.EventType.ToString()));
                            continue;
                        }
                        if (r.IsBiWeekly)
                        {
                            bool oddWeek = Convert.ToBoolean(r.FirstEventDate.GetWeekOfMonth() % 2);
                            bool isDayOddWeek = Convert.ToBoolean(d.WeekOfMonth % 2);

                            if (oddWeek == isDayOddWeek && r.EventDayOfWeek.Equals(d.DayOfWeek))
                            {
                                d.GuildEvents.Add(new Tuple<string, int, string>(r.Title, r.Id, r.EventType.ToString()));
                            }
                        }
                    }
                }

                m.DayDataList.Add(d);
                dateCounter = dateCounter.AddDays(1);
            }

            m.DayDataList = StandardiseDayData(m.DayDataList, firstDayOfMonth, lastDayOfMonth);

            string strong = "stronk";

            return m;
        }

        private List<DayData> StandardiseDayData(List<DayData> data, DateTime firstDay, DateTime lastDay)
        {
            int fd = (int)firstDay.DayOfWeek;

            if (fd != 1)
            {
                int dif = fd - 1;

                //correct for sundays
                if (dif == -1)
                {
                    dif = 6;
                }

                var l = new List<DayData>();

                for (int i = 0; i < dif; i++)
                {
                    DayOfWeek dayOfWeek = (DayOfWeek)(fd);
                    var ds = firstDay.AddDays(-(dif - i + 1)).DayOfWeek.ToString();

                    var thisDay = firstDay.AddDays(-(dif - i));

                    var d = new DayData()
                    {
                        Date = thisDay,
                        DayName = thisDay.DayOfWeek.ToString(),
                        DayOfMonth = thisDay.Day,
                        DayOfWeek = (int)thisDay.DayOfWeek,
                        GuildEvents = new List<Tuple<string, int, string>>(),
                        IsOutsideMonth = true,
                        WeekOfMonth = (int)thisDay.GetWeekOfMonth()
                    };

                    l.Add(d);
                }

                data.InsertRange(0, l);
            }

            return data;
        }
    }
}