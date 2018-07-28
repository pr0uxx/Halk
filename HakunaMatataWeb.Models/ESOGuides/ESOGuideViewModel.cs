using HakunaMatataWeb.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HakunaMatataWeb.Models.ESOGuides
{
    public class ESOGuideViewModel
    {
        public int Id { get; set; }

        [DisplayName("Guide Type")]
        public EventType GuideType { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(160, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DisplayName("Sub-title")]
        public string SubTitle { get; set; }

        [StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Content { get; set; }

        [DisplayName("Image Urls")]
        public List<string> ImageUrls { get; set; }

        public string Author { get; set; }

        [DisplayName("Created")]
        public string CreationDate { get; set; }

        [DisplayName("Last Updated")]
        public string LastUpdatedDate { get; set; }

        public string ContentHtml { get; set; } = string.Empty;

        //add file upload
    }
}