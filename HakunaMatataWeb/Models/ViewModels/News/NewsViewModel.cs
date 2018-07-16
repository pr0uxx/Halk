using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HakunaMatataWeb.Models.ViewModels.News
{
    public class NewsViewModel
    {
        public List<NewsItem> NewsList { get; set; } = new List<NewsItem>();

        public int Id { get; set; }
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
    }
}