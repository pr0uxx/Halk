using HakunaMatataWeb.Data.Enums;
using System;
using System.Collections.Generic;

namespace HakunaMatataWeb.Models.GuildEventModels
{
    public class GuildEventCalendarViewModel
    {
        public DateTime CurrentDate = DateTime.Now;
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthString { get; set; }
        public List<DayData> DayDataList { get; set; }
    }

    public class DayData
    {
        public string DayName { get; set; }
        public int DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public int WeekOfMonth { get; set; }
        public bool IsOutsideMonth { get; set; }

        public DateTime Date { get; set; }
        public List<Tuple<string, int, string>> GuildEvents { get; set; }
    }
}