using HakunaMatataWeb.Data.DataConnection;
using HakunaMatataWeb.Models.ESOGuides;
using HakunaMatataWeb.Models.HomeModels;
using HakunaMatataWeb.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HakunaMatataWeb.Controllers
{
    public class HomeController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<ActionResult> Index()
        {
            var takeAmount = 4;

            logger.Trace("Starting home method");
            var guildEvents = await db.GuildEvents.OrderBy(x => x.Featured.Equals(true)).OrderBy(x => x.CreationDate).Take(takeAmount).ToListAsync();
            var guides = await db.ESOGuides.OrderBy(x => x.CreationDate).Take(takeAmount).ToListAsync();
            var news = await db.NewsItems.OrderBy(x => x.CreationDate).Take(takeAmount).ToListAsync();

            var model = new HomeViewModel();
            model.ESOGuides = new List<HomeItem>();

            foreach (var g in guides)
            {
                var content = Helper.Base64Decode(g.Content);
                model.ESOGuides.Add(new HomeItem()
                {
                    Author = g.Author,
                    Title = g.Title,
                    ItemImportantDate = g.CreationDate.ToString("dd/MM/yyyy"),
                    ImageUrl = Helper.GetFirstUrlFromContent(content),
                    Id = g.EsoGuideId,
                    SubTitle = g.SubTitle
                });
            };

            model.GuildEvents = new List<HomeItem>();

            foreach (var ge in guildEvents)
            {
                var content = Helper.Base64Decode(ge.Content);
                model.GuildEvents.Add(new HomeItem()
                {
                    Author = ge.EventMaster,
                    ItemImportantDate = Helper.GetNextEventDate(ge.IsBiWeekly, ge.IsMonthly, ge.IsUniqueEvent, ge.IsWeekly, ge.FirstEventDate).ToString("dd/MM/yyyy"),
                    Title = ge.Title,
                    Id = ge.Id,
                    ImageUrl = Helper.GetFirstUrlFromContent(ge.Content),
                    SubTitle = string.Empty
                });
            };

            model.News = new List<HomeItem>();

            foreach (var n in news)
            {
                var content = Helper.Base64Decode(n.Content);
                model.News.Add(new HomeItem()
                {
                    Author = n.Author,
                    Id = n.Id,
                    ImageUrl = Helper.GetFirstUrlFromContent(n.Content),
                    ItemImportantDate = n.CreationDate.ToString("dd/MM/yyyy"),
                    SubTitle = n.SubTitle,
                    Title = n.Title
                });
            }
            logger.Trace("Returning from home method");

            return View(model);


        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}