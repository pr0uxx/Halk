using HakunaMatataWeb.Data.Enums;
using HakunaMatataWeb.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HakunaMatataWeb.Data.Models
{
    public class GuildEvent
    {
        public int Id { get; set; }

        [StringLength(60, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Event Type")]
        public EventType EventType { get; set; }

        public bool IsUniqueEvent { get; set; }
        public bool IsMonthly { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsBiWeekly { get; set; }
        public int EventDayOfWeek { get; set; }
        public int EventDayOfMonth { get; set; }

        [StringLength(6000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Description")]
        public string Content { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Event Master")]
        public string EventMaster { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        [Display(Name = "Minimum Recommended Level")]
        public int MinLevel { get; set; }

        [Display(Name = "Maximum Recommended Level")]
        public int MaxLevel { get; set; }

        public bool Featured { get; set; }

        [Required]
        [NotMapped]
        public string LocalTimeZoneId { get; set; }

        [Required]
        [Display(Name = "First Event Date")]
        public DateTime FirstEventDate
        {
            get { return Helper.ToLocalTime(_FirstEventDate, LocalTimeZoneId); }
            set { _FirstEventDate = value.ToUniversalTime(); }
        }

        [Required]
        [Display(Name = "Last Event Date")]
        public DateTime LastEventDate
        {
            get { return Helper.ToLocalTime(_LastEventDate, LocalTimeZoneId); }
            set { _LastEventDate = value.ToUniversalTime(); }
        }

        [NotMapped]
        private DateTime _FirstEventDate { get; set; }

        [NotMapped]
        private DateTime _LastEventDate { get; set; }
    }
}