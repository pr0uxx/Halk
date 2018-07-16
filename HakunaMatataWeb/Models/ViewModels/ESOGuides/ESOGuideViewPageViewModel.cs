using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HakunaMatataWeb.Models.ViewModels.ESOGuides
{
    public class ESOGuideViewPageViewModel
    {
        public List<ESOGuideViewModel> ESOGuides { get; set; }
        public string CoverUrl { get; set; }
        public int TotalCount { get; set; }
        public int CurrentCount { get; set; }
    }
}