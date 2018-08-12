using HakunaMatataWeb.Models.ESOGuides;
using HakunaMatataWeb.Models.GuildEventModels;
using HakunaMatataWeb.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Models.HomeModels
{
    public class HomeViewModel
    {
        public List<HomeItem> GuildEvents { get; set; }
        public List<HomeItem> News { get; set; }
        public List<HomeItem> ESOGuides { get; set; }
    }

    public class HomeItem
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ItemImportantDate { get; set; }
        public string ImageUrl { get; set; }
        public int Id { get; set; }
    }
}
