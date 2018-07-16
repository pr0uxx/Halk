using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HakunaMatataWeb.Models.ViewModels
{
    public class ClaimsViewModel
    {
        public string GuildRank { get; set; }
        public string SiteRank { get; set; }
        public string DisplayName { get; set; }
        public string TimeZone { get; set; }
    }
}