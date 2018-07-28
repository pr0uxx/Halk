using System;
using System.ComponentModel.DataAnnotations;

namespace HakunaMatataWeb.Data.Models
{
    public class NewsItem
    {
        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(160, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string SubTitle { get; set; }

        [StringLength(4000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Content { get; set; }

        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string AuthorId { get; set; }
    }
}