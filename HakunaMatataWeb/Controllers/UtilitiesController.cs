using HakunaMatataWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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