using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HakunaMatataWeb.Models.ViewModels.GuildEvents
{
    public class GuildEventCalendarViewModel
    {
        public DateTime CurrentDate = DateTime.Now;
        public int Month { get; set; }
        public List<DayData> DayDataList { get; set; }
    }

    public class DayData
    {
        public string DayName { get; set; }
        public int DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public int WeekOfMonth { get; set; }

        public DateTime Date { get; set; }
        public List<Tuple<string, int>> GuildEvents { get; set; }
    }
}