using HakunaMatataWeb.Data.DataConnection;
using HakunaMatataWeb.Data.Models;
using HakunaMatataWeb.Extensions;
using HakunaMatataWeb.Models.ViewModels.News;
using HakunaMatataWeb.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HakunaMatataWeb.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: News
        [AllowAnonymous]
        public  async Task<ActionResult> Index()
        {
            var dbResult = await db.NewsItems.OrderBy(x=> x.CreationDate).Take(10).ToListAsync();

            NewsViewModel model = new NewsViewModel()
            {
                NewsList = dbResult
            };

            return View(model);
        }

        [AuthorizeSiteRank(SiteRank.SiteAdmin)]
        public async Task<ActionResult> Administrate()
        {
            var dbResult = await db.NewsItems.OrderBy(x => x.CreationDate).Take(50).ToListAsync();

            NewsViewModel model = new NewsViewModel()
            {
                NewsList = dbResult
            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NewsItem d = await db.NewsItems.FindAsync(id);
            if (d == null)
            {
                return HttpNotFound();
            }

            var model = new NewsViewModel()
            {
                Title = d.Title,
                Author = d.Author,
                AuthorId = d.AuthorId,
                Content = Helper.Base64Decode(d.Content),
                CreationDate = d.CreationDate,
                Id = d.Id,
                LastUpdatedDate = d.LastUpdatedDate,
                SubTitle = d.SubTitle
            };

            return View(model);

        }

        // GET: News/Create
        [AuthorizeSiteRank(SiteRank.SiteAdmin)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [AuthorizeSiteRank(SiteRank.SiteAdmin)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Title,SubTitle,Content")] NewsViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var n = new NewsItem()
                {
                    Author = User.Identity.GetDisplayName(),
                    AuthorId = User.Identity.GetUserId(),
                    Content = Helper.Base64Encode(model.Content),
                    CreationDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    SubTitle = model.SubTitle,
                    Title = model.Title
                };

                db.NewsItems.Add(n);
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: News/Delete
        [AuthorizeSiteRank(SiteRank.SiteAdmin)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = await db.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            return View(newsItem);
        }

        [AuthorizeSiteRank(SiteRank.SiteAdmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NewsItem newsItem = await db.NewsItems.FindAsync(id);
            db.NewsItems.Remove(newsItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET News/Edit
        [AuthorizeSiteRank(SiteRank.SiteAdmin)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem n = await db.NewsItems.FindAsync(id);
            if (n == null)
            {
                return HttpNotFound();
            }

            var model = new NewsViewModel()
            {
                Author = n.Author,
                AuthorId = n.AuthorId,
                Content = Helper.Base64Decode(n.Content),
                CreationDate = n.CreationDate,
                Id = n.Id,
                LastUpdatedDate = n.LastUpdatedDate,
                SubTitle = n.SubTitle,
                Title = n.Title
            };

            return View(model);

        }

        //POST News/Edit
        [HttpPost]
        [AuthorizeSiteRank(SiteRank.SiteAdmin)]
        public async Task<ActionResult> Edit([Bind(Include = "Title,SubTitle,Content")] NewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var n = new NewsItem()
                {
                    Author = model.Author,
                    AuthorId = model.AuthorId,
                    Content = Helper.Base64Encode(model.Content),
                    CreationDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    SubTitle = model.SubTitle,
                    Title = model.Title
                };

                db.NewsItems.Add(n);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}