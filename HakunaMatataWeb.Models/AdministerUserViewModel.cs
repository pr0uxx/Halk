using HakunaMatataWeb.Data.Enums;
using HakunaMatataWeb.Utilities;
using System.Web.Mvc;

namespace HakunaMatataWeb.Models
{
    public class AdministerUserViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public GuildRank GuildRank { get; set; } = GuildRank.Uninitiated;
        public SiteRank SiteRank { get; set; } = SiteRank.User;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string LocalTimezone { get; set; } = string.Empty;
        public SelectList TimeZoneSelectList { get { return new SelectList(Helper.GetTimeZoneList(), "Value", "Text", "0"); } }
    }
}