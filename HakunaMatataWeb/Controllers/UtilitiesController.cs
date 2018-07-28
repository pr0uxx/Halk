using HakunaMatataWeb.Utilities;
using System.Web.Mvc;

namespace HakunaMatataWeb.Controllers
{
    public class UtilitiesController : Controller
    {
        // GET: Utilities
        [HttpPost]
        public string ConvertMarkdown(string markdown)
        {
            return Helper.ConvertMarkdown(markdown);
        }
    }
}