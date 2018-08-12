using HakunaMatataWeb.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HakunaMatataWeb.Models.GuildEventModels
{
    public class GuildEventViewModel
    {
        public int Id { get; set; }

        [StringLength(60, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Event Type")]
        public EventType EventType { get; set; }

        [Display(Name = "Unique Event")]
        public bool IsUniqueEvent { get; set; }
        [Display(Name = "Monthly")]
        public bool IsMonthly { get; set; }
        [Display(Name = "Weekly")]
        public bool IsWeekly { get; set; }
        [Display(Name = "Twice Monthly")]
        public bool IsBiWeekly { get; set; }
        [Display(Name = "Day Of Week")]
        public int EventDayOfWeek { get; set; }
        [Display(Name = "Day Of Month")]
        public int EventDayOfMonth { get; set; }

        [StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
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

        [NotMapped]
        public string LocalTimeZoneId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Event Date")]
        public DateTime FirstEventDate { get; set; }

        [Required]
        [Display(Name = "Last Event Date")]
        public DateTime LastEventDate { get; set; }

        public DateTime NextEventDate { get; set; }
    }
}