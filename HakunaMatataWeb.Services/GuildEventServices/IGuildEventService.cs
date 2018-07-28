using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HakunaMatataWeb.Models.GuildEventModels;

namespace HakunaMatataWeb.Services.GuildEventServices
{
    public interface IGuildEventService
    {
        Task<GuildEventCalendarViewModel> GetMonthEventCalendarAsync(int month, int year, string localTimeZone);
    }
}
