using HakunaMatataWeb.Data.DataConnection;
using HakunaMatataWeb.Data.Enums;
using HakunaMatataWeb.Data.Models;
using HakunaMatataWeb.Services.Extensions;
using HakunaMatataWeb.Models.GuildEventModels;
using HakunaMatataWeb.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using HakunaMatataWeb.Services.GuildEventServices;
using System.Security.Principal;

namespace HakunaMatataWeb.Controllers
{
    [AuthorizeGuildRank(GuildRank.EventMaster)]
    public class GuildEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IGuildEventService eventService;

        public GuildEventsController()
            : this(new GuildEventService())
        {
        }

        public GuildEventsController(IGuildEventService EventService)
        {
            eventService = EventService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetMonthGuildEvents(string month, string year)
        {
            int Month;
            int Year;
            try
            {
                Month = Convert.ToInt32(month);
                Year = Convert.ToInt32(year);
            }
            catch (Exception ex)
            {
                return (Json("You sent me a bad month or year!", JsonRequestBehavior.AllowGet));
            }
            

            var firstDayOfMonth = new DateTime(Year, Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var m = new GuildEventCalendarViewModel();

            var localTimeZone = User.GetClaimValueString(CustomClaims.LocalTimezone.ToString());

            m = await eventService.GetMonthEventCalendarAsync(Month, Year, localTimeZone);

            return Json(m, JsonRequestBehavior.AllowGet);
        }

        // GET: GuildEvents
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var m = new GuildEventCalendarViewModel();
            var localTimeZone = User.GetClaimValueString(CustomClaims.LocalTimezone.ToString());
            m = await eventService.GetMonthEventCalendarAsync(now.Month, now.Year, localTimeZone); 

            string strong = "stronk";

            return View(m);
        }

        //private List<DayData> StandardiseDayData(List<DayData> data, DateTime firstDay, DateTime lastDay)
        //{
        //    int fd = (int)firstDay.DayOfWeek;

        //    if (fd != 1)
        //    {
        //        int dif = fd - 1;

        //        //correct for sundays
        //        if (dif == -1)
        //        {
        //            dif = 6;
        //        }

        //        var l = new List<DayData>();

        //        for (int i = 0; i < dif; i++)
        //        {
        //            DayOfWeek dayOfWeek = (DayOfWeek)(fd);
        //            var ds = firstDay.AddDays(-(dif - i + 1)).DayOfWeek.ToString();

        //            var thisDay = firstDay.AddDays(-(dif - i));

        //            var d = new DayData()
        //            {
        //                Date = thisDay,
        //                DayName = thisDay.DayOfWeek.ToString(),
        //                DayOfMonth = thisDay.Day,
        //                DayOfWeek = (int)thisDay.DayOfWeek,
        //                GuildEvents = new List<Tuple<string, int, string>>(),
        //                IsOutsideMonth = true,
        //                WeekOfMonth = (int)thisDay.GetWeekOfMonth()
        //            };

        //            l.Add(d);
        //        }

        //        data.InsertRange(0, l);
        //    }

        //    return data;
        //}

        public async Task<ActionResult> Administrate()
        {
            var result = new List<GuildEvent>();

            if (User.GetClaimValueInt<GuildRank>() > (int)GuildRank.EventMaster)
            {
                result = await db.GuildEvents.ToListAsync();
            }
            else
            {
                var uid = User.Identity.GetUserId();

                result = await db.GuildEvents.Where(x => x.UserId.Equals(uid)).ToListAsync();
            }

            return View(await db.GuildEvents.ToListAsync());
        }

        // GET: GuildEvents/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuildEvent guildEvent = await db.GuildEvents.FindAsync(id);
            if (guildEvent == null)
            {
                return HttpNotFound();
            }
            guildEvent.Content = Helper.Base64Decode(guildEvent.Content);

            var model = new GuildEventDetailsViewModel()
            {
                Description = Helper.ConvertMarkdown(guildEvent.Content),
                EvenetDayOfWeek = guildEvent.EventDayOfWeek.ToString(),
                EventDayOfMonth = guildEvent.EventDayOfMonth,
                EventMaster = guildEvent.EventMaster,
                EventTimeOfDay = guildEvent.FirstEventDate.ToString("dddd"),
                EventType = guildEvent.EventType.ToString(),
                FirstEventDate = guildEvent.FirstEventDate,
                Id = guildEvent.Id,
                IsBiWeekly = guildEvent.IsBiWeekly,
                IsMonthly = guildEvent.IsMonthly,
                IsUniqueEvent = guildEvent.IsUniqueEvent,
                LastEventDate = guildEvent.LastEventDate,
                MaxLevel = guildEvent.MaxLevel,
                MinLevel = guildEvent.MinLevel,
                IsWeekly = guildEvent.IsWeekly,
                Title = guildEvent.Title
            };

            return View(model);
        }

        // GET: GuildEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuildEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,EventType,IsUniqueEvent,IsMonthly,IsWeekly,IsBiWeekly,EventDayOfWeek,EventDayOfMonth,Content,UserId,EventMaster,CreationDate,LastUpdatedDate,MinLevel,MaxLevel,Featured,FirstEventDate,LastEventDate")] GuildEventViewModel guildEvent)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            guildEvent.LocalTimeZoneId = User.GetClaimValueString(CustomClaims.LocalTimezone.ToString());
            guildEvent.Content = Helper.Base64Encode(guildEvent.Content);
            guildEvent.UserId = User.Identity.GetUserId();
            guildEvent.EventMaster = User.Identity.GetDisplayName();

            if (guildEvent.IsBiWeekly)
            {
                guildEvent.EventDayOfWeek = (int)guildEvent.FirstEventDate.DayOfWeek;
            }
            if (guildEvent.IsMonthly)
            {
                guildEvent.EventDayOfMonth = (int)guildEvent.FirstEventDate.Day;
            }

            if (ModelState.IsValid)
            {
                var i = new GuildEvent()
                {
                    LocalTimeZoneId = User.GetClaimValueString(CustomClaims.LocalTimezone.ToString()),
                    Content = guildEvent.Content,
                    CreationDate = DateTime.Now,
                    EventDayOfMonth = guildEvent.EventDayOfMonth,
                    EventDayOfWeek = guildEvent.EventDayOfWeek,
                    EventMaster = guildEvent.EventMaster,
                    EventType = guildEvent.EventType,
                    Featured = guildEvent.Featured,
                    FirstEventDate = guildEvent.FirstEventDate,
                    IsBiWeekly = guildEvent.IsBiWeekly,
                    IsMonthly = guildEvent.IsMonthly,
                    IsUniqueEvent = guildEvent.IsUniqueEvent,
                    IsWeekly = guildEvent.IsWeekly,
                    LastEventDate = guildEvent.LastEventDate,
                    LastUpdatedDate = DateTime.Now,
                    MaxLevel = guildEvent.MaxLevel,
                    MinLevel = guildEvent.MinLevel,
                    Title = guildEvent.Title,
                    UserId = guildEvent.UserId
                };

                db.GuildEvents.Add(i);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(guildEvent);
        }

        // GET: GuildEvents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuildEvent guildEvent = await db.GuildEvents.FindAsync(id);
            var uid = User.Identity.GetUserId();
            if (guildEvent == null)
            {
                return HttpNotFound();
            }

            if (!guildEvent.UserId.Equals(uid) && User.GetClaimValueInt<GuildRank>() <= (int)GuildRank.EventMaster)
            {
                return HttpNotFound();
            }

            GuildEventViewModel model = new GuildEventViewModel()
            {
                Content = Helper.Base64Decode(guildEvent.Content),
                EventDayOfMonth = guildEvent.EventDayOfMonth,
                EventDayOfWeek = guildEvent.EventDayOfWeek,
                EventMaster = guildEvent.EventMaster,
                EventType = guildEvent.EventType,
                Featured = guildEvent.Featured,
                FirstEventDate = guildEvent.FirstEventDate,
                Id = guildEvent.Id,
                IsBiWeekly = guildEvent.IsBiWeekly,
                IsMonthly = guildEvent.IsMonthly,
                IsUniqueEvent = guildEvent.IsUniqueEvent,
                IsWeekly = guildEvent.IsWeekly,
                LastEventDate = guildEvent.LastEventDate,
                LocalTimeZoneId = guildEvent.LocalTimeZoneId,
                MaxLevel = guildEvent.MaxLevel,
                MinLevel = guildEvent.MinLevel,
                Title = guildEvent.Title,
                UserId = guildEvent.UserId
            };

            guildEvent.Content = Helper.Base64Decode(guildEvent.Content);
            return View(guildEvent);
        }

        // POST: GuildEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,EventType,IsUniqueEvent,IsMonthly,IsWeekly,IsBiWeekly,EventDayOfWeek,EventDayOfMonth,Content,UserId,EventMaster,CreationDate,LastUpdatedDate,MinLevel,MaxLevel,Featured,FirstEventDate,LastEventDate")] GuildEventViewModel m)
        {
            var i = await db.GuildEvents.FindAsync(m.Id);

            i.Content = m.Content;
            i.EventMaster = m.EventMaster;
            i.EventType = m.EventType;
            i.Featured = m.Featured;
            i.FirstEventDate = m.FirstEventDate;
            i.IsBiWeekly = m.IsBiWeekly;
            i.IsMonthly = m.IsMonthly;
            i.IsUniqueEvent = m.IsUniqueEvent;
            i.IsWeekly = m.IsWeekly;
            i.LastEventDate = m.LastEventDate;
            i.MaxLevel = m.MaxLevel;
            i.MinLevel = m.MinLevel;
            i.Title = m.Title;
            i.LastUpdatedDate = DateTime.Now;
            i.LocalTimeZoneId = i.LocalTimeZoneId ?? User.GetClaimValueString(CustomClaims.LocalTimezone.ToString());

            if (i.IsBiWeekly)
            {
                i.EventDayOfWeek = (int)i.FirstEventDate.DayOfWeek;
            }
            if (i.IsMonthly)
            {
                i.EventDayOfMonth = (int)i.FirstEventDate.Day;
            }
            i.LastUpdatedDate = DateTime.Now;
            i.Content = Helper.Base64Encode(m.Content);
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(i).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
            }
            return View(m);
        }

        // GET: GuildEvents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuildEvent guildEvent = await db.GuildEvents.FindAsync(id);
            if (guildEvent == null)
            {
                return HttpNotFound();
            }
            return View(guildEvent);
        }

        // POST: GuildEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GuildEvent guildEvent = await db.GuildEvents.FindAsync(id);
            db.GuildEvents.Remove(guildEvent);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #region helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion helpers
    }
}