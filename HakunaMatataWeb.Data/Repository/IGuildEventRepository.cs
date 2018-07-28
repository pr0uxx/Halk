using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Data.Services
{
    public interface IGuildEventRepository
    {
        Task<(int Rows, Exception Error)> CreateGuildEventAsync(GuildEvent guildEvent);

        Task<(int Rows, IEnumerable<Exception> Errors)> BatchCreateGuildEventsAsync(IEnumerable<GuildEvent> guildEvents);
    }
}