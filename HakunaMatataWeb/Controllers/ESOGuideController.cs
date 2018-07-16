﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HakunaMatataWeb.Data.DataConnection;
using HakunaMatataWeb.Data.Models;
using HakunaMatataWeb.Extensions;
using Microsoft.AspNet.Identity;
using HakunaMatataWeb.Models.ViewModels.ESOGuides;
using Markdig;
using System.Text.RegularExpressions;
using HakunaMatataWeb.Utilities;

namespace HakunaMatataWeb.Controllers
{
    public class ESOGuideController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ESOGuide
        public async Task<ActionResult> Index()
        {
            var dbResult = await db.ESOGuides.ToListAsync();

            List<ESOGuideViewModel> model = new List<ESOGuideViewModel>();

            foreach (var d in dbResult)
            {
                var imgList = new List<string>();
                foreach (var img in d.ImageUrls)
                {
                    imgList.Add(img.Uri);
                }


                var m = new ESOGuideViewModel()
                {
                    Content = d.Content,
                    GuideType = d.GuideType,
                    ImageUrls = imgList,
                    SubTitle = d.SubTitle,
                    Title = d.Title,
                    Id = d.EsoGuideId,
                    Author = d.Author,
                    CreationDate = d.CreationDate.ToString("dd/MM/yyyy")
                };

                model.Add(m);
            }


            return View(model);
        }

        // GET: ESOGuide/Guides
        public async Task<ActionResult> Guides()
        {
            int take = 20;
            var dbResult = await db.ESOGuides.ToListAsync();
            ViewBag.GuideTypeOptions = Helper.GetEnumValues<EventType>();
            ViewBag.GuideAuthors = dbResult.Select(x => x.Author).Distinct();
            var model = new ESOGuideViewPageViewModel()
            {
                TotalCount = dbResult.Count,
                CurrentCount = (dbResult.Count > 20) ? take : dbResult.Count
            };

            if (model.TotalCount > 20)
            {
                dbResult = dbResult.Take(20).ToList();
            }


            List<ESOGuideViewModel> guides = new List<ESOGuideViewModel>();

            foreach (var d in dbResult)
            {
                var imgList = new List<string>();
                foreach (var img in d.ImageUrls)
                {
                    imgList.Add(img.Uri);
                }


                var m = new ESOGuideViewModel()
                {
                    GuideType = d.GuideType,
                    ImageUrls = imgList,
                    SubTitle = d.SubTitle,
                    Title = d.Title,
                    Id = d.EsoGuideId,
                    Author = d.Author,
                    CreationDate = d.CreationDate.ToString("dd/MM/yyyy"),
                    LastUpdatedDate = d.LastUpdatedDate.ToString("dd/MM/yyyy")
                };

                if (m.ImageUrls.Count < 1)
                {
                    foreach (Match item in Regex.Matches(d.Content, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
                    {
                        if (Helper.IsImageUrl(item.Value))
                        {
                            m.ImageUrls.Add(item.Value);
                        }
                    }
                }


                guides.Add(m);
            }

            model.ESOGuides = guides;

            return View(model);
        }



        // GET: ESOGuide/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ESOGuide eSOGuide = await db.ESOGuides.FindAsync(id);
            if (eSOGuide == null)
            {
                return HttpNotFound();
            }

            var imgList = new List<string>();
            if (eSOGuide.ImageUrls != null)
            {
                foreach (var img in eSOGuide.ImageUrls)
                {
                    imgList.Add(img.Uri);
                }
            }


            var model = new ESOGuideViewModel()
            {
                Author = eSOGuide.Author,
                Content = eSOGuide.Content,
                CreationDate = eSOGuide.CreationDate.ToString("dd/MM/yyyy"),
                GuideType = eSOGuide.GuideType,
                Id = eSOGuide.EsoGuideId,
                ImageUrls = imgList,
                LastUpdatedDate = eSOGuide.LastUpdatedDate.ToString("dd/MM/yyyy"),
                SubTitle = eSOGuide.SubTitle,
                Title = eSOGuide.Title,
                ContentHtml = Helper.ConvertMarkdown(eSOGuide.Content)
            };

            return View(model);
        }

        // GET: ESOGuide/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ESOGuide/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GuideType,Title,SubTitle,Content")] ESOGuideViewModel eSOGuide)
        {

            if (ModelState.IsValid)
            {
                List<ImageUrl> i = new List<ImageUrl>();

                if (eSOGuide.ImageUrls != null)
                {
                    foreach (var item in eSOGuide.ImageUrls)
                    {
                        try
                        {
                            ;
                            if (Uri.TryCreate(item, UriKind.Absolute, out Uri uri))
                            {
                                i.Add(new ImageUrl() { Uri = uri.ToString() });
                            }
                            else
                            {
                                ModelState.AddModelError("Bad Url", new Exception(string.Concat("Unrecognised Url: ", i)));
                            }

                        }
                        catch
                        {
                            ModelState.AddModelError("Bad Url", new Exception(string.Concat("Unrecognised Url: ", i)));
                        }

                    }
                }

                var g = ConvertGuideViewToDbModel(eSOGuide, i);
                g.CreationDate = DateTime.Now;
                db.ESOGuides.Add(g);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(eSOGuide);
        }



        // GET: ESOGuide/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ESOGuide e = await db.ESOGuides.FindAsync(id);
            if (e == null)
            {
                return HttpNotFound();
            }

            var i = new List<string>();
            if (e.ImageUrls != null && e.ImageUrls != null)
            {
                foreach (var item in e.ImageUrls)
                {
                    i.Add(item.Uri);
                }
            }

            var g = ConvertGuideDbToViewModel(e, i);


            return View(g);
        }

        // POST: ESOGuide/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GuideType,Title,SubTitle,Content,Author,CreationDate,LastUpdatedDate,ImageUrls")] ESOGuideViewModel e)
        {
            if (ModelState.IsValid)
            {
                List<ImageUrl> i = new List<ImageUrl>();

                //var guide = db.ESOGuides.FindAsync(e.Id);

                if (e.ImageUrls != null && e.ImageUrls.Count > 0)
                {
                    foreach (var item in e.ImageUrls)
                    {
                        i.Add(new ImageUrl() { Uri = item });
                    }
                }

                var g = ConvertGuideViewToDbModel(e, i);



                db.Entry(g).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(e);
        }

        

        // GET: ESOGuide/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ESOGuide eSOGuide = await db.ESOGuides.FindAsync(id);
            if (eSOGuide == null)
            {
                return HttpNotFound();
            }
            return View(eSOGuide);
        }

        // POST: ESOGuide/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ESOGuide eSOGuide = await db.ESOGuides.FindAsync(id);
            db.ESOGuides.Remove(eSOGuide);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private ESOGuide ConvertGuideViewToDbModel(ESOGuideViewModel e, List<ImageUrl> i)
        {
            var g = new ESOGuide()
            {
                //LastUpdatedDate = DateTime.Now,
                //CreationDate = DateTime.Parse(e.CreationDate),
                //Author = User.Identity.GetDisplayName(),
                //AuthorId = User.Identity.GetUserId(),
                //Content = e.Content,
                //GuideType = e.GuideType,
                //ImageUrls = i,
                //SubTitle = e.SubTitle,
                //Title = e.Title

            };

            g.LastUpdatedDate = DateTime.Now;
            if (e.CreationDate != null)
            {
                g.CreationDate = DateTime.Parse(e.CreationDate);
            }
            else
            {
                g.CreationDate = DateTime.Now;
            }
            g.Author = User.Identity.GetDisplayName();
            g.AuthorId = User.Identity.GetUserId();
            g.Content = e.Content;
            g.GuideType = e.GuideType;
            g.ImageUrls = i;
            g.SubTitle = e.SubTitle;
            g.Title = e.Title;

            if (e.Id > 0)
            {
                g.EsoGuideId = e.Id;
            }


            return g;
        }

        private ESOGuideViewModel ConvertGuideDbToViewModel(ESOGuide e, List<string> i)
        {
            var g = new ESOGuideViewModel()
            {
                Author = e.Author,
                Content = e.Content,
                CreationDate = e.CreationDate.ToString("dd/MM/yyyy"),
                GuideType = e.GuideType,
                Id = e.EsoGuideId,
                ImageUrls = i,
                LastUpdatedDate = e.LastUpdatedDate.ToString("dd/MM/yyyy"),
                SubTitle = e.SubTitle,
                Title = e.Title
            };

            return g;
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
