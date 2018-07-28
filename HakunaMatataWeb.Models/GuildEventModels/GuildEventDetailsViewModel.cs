using System;

namespace HakunaMatataWeb.Models.GuildEventModels
{
    public class GuildEventDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EventType { get; set; }
        public bool IsUniqueEvent { get; set; }
        public bool IsMonthly { get; set; }
        public bool IsBiWeekly { get; set; }
        public bool IsWeekly { get; set; }
        public string EvenetDayOfWeek { get; set; }
        public int EventDayOfMonth { get; set; }
        public string Description { get; set; }
        public string EventMaster { get; set; }
        public string EventTimeOfDay { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public DateTime FirstEventDate { get; set; }
        public DateTime LastEventDate { get; set; }
    }
}