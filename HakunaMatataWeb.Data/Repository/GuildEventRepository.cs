using HakunaMatataWeb.Data.DataConnection;
using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Data.Services
{
    public class GuildEventRepository : IGuildEventRepository
    {
        public async Task<(int Rows, Exception Error)> CreateGuildEventAsync(GuildEvent guildEvent)
        {
            var rows = 0;
            Exception error = null;

            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.GuildEvents.Add(guildEvent);

                    rows = await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }

            return (rows, error);
        }

        public async Task<(int Rows, IEnumerable<Exception> Errors)> BatchCreateGuildEventsAsync(IEnumerable<GuildEvent> guildEvents)
        {
            int rows = 0;
            var errors = new List<Exception>();

            foreach (var e in guildEvents)
            {
                var iResult = await CreateGuildEventAsync(e);
                if (iResult.Error != null)
                {
                    errors.Add(iResult.Error);
                }
                else
                {
                    rows += iResult.Rows;
                }
            }

            return (rows, errors);
        }
    }
}