using HakunaMatataWeb.Data.DataConnection;
using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Services.ESOGuideServices
{
    public class ESOGuideFilterServices : IESOGuideFilterService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<List<ESOGuide>> FilterByGuideType(string guideType, List<ESOGuide> existingGuides)
        {
            throw new NotImplementedException();
        }
    }
}
