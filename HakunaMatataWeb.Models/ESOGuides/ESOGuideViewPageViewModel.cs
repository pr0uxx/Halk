using System.Collections.Generic;

namespace HakunaMatataWeb.Models.ESOGuides
{
    public class ESOGuideViewPageViewModel
    {
        public List<ESOGuideViewModel> ESOGuides { get; set; }
        public string CoverUrl { get; set; }
        public int TotalCount { get; set; }
        public int CurrentCount { get; set; }
    }
}