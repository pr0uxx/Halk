using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Data.Services
{
    public interface IGuildEventRepository
    {
        Task<(int Rows, Exception Error)> CreateGuildEventAsync(GuildEvent guildEvent);

        Task<(int Rows, IEnumerable<Exception> Errors)> BatchCreateGuildEventsAsync(IEnumerable<GuildEvent> guildEvents);
    }
}
