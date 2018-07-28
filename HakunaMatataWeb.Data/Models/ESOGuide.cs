using HakunaMatataWeb.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HakunaMatataWeb.Data.Models
{
    public class ESOGuide
    {
        public int EsoGuideId { get; set; }
        public EventType GuideType { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(160, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string SubTitle { get; set; }

        [StringLength(6000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Content { get; set; }

        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string AuthorId { get; set; }
        public virtual List<ImageUrl> ImageUrls { get; set; }
    }
}