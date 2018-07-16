using System;
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
using HakunaMatataWeb.Utilities;
using HakunaMatataWeb.Extensions;
using HakunaMatataWeb.Models;
using HakunaMatataWeb.Models.ViewModels.GuildEvents;
using Microsoft.AspNet.Identity;
using HakunaMatataWeb.Extensions;
using System.Data.Entity.Validation;

namespace HakunaMatataWeb.Controllers
{
    [AuthorizeGuildRank(GuildRank.EventMaster)]
    public class GuildEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GuildEvents
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var guildEvents = await db.GuildEvents.Where(x => x.FirstEventDate < DateTime.Now && x.LastEventDate > DateTime.Now).ToListAsync();


            var localTimeZone = User.GetClaimValueString(CustomClaims.LocalTimezone.ToString());
            var m = new GuildEventCalendarViewModel();
            m.DayDataList = new List<DayData>();
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var dateCounter = firstDayOfMonth;
            m.Month = DateTime.Today.Month;

            while (dateCounter < lastDayOfMonth)
            {
                var d = new DayData()
                {
                    Date = dateCounter,
                    DayName = dateCounter.DayOfWeek.ToString(),
                    DayOfWeek = (int)dateCounter.DayOfWeek,
                    DayOfMonth = (int)dateCounter.Day,
                    WeekOfMonth = (int)dateCounter.GetWeekOfMonth()
                };

                if (guildEvents.Count > 0)
                {
                    //var r = guildEvents.Where(x => (x.EventDayOfMonth == d.DayOfMonth && x.IsMonthly) || (x.EventDayOfWeek.Equals(d.DayOfWeek) && x.IsWeekly) ||
                    //    (x.IsBiWeekly && ((d.WeekOfMonth.Equals(x.FirstEventDate.GetWeekOfMonth()) || (d.WeekOfMonth + 2) == x.FirstEventDate.GetWeekOfMonth()) && x.EventDayOfWeek == d.DayOfWeek)))
                    //    .Select(x => new Tuple<string, int>(x.Title, x.Id)).ToList();
                    d.GuildEvents = new List<Tuple<string, int>>();
                    foreach (var r in guildEvents)
                    {
                        if (r.EventDayOfMonth.Equals(d.DayOfMonth) && r.IsMonthly)
                        {
                            d.GuildEvents.Add(new Tuple<string, int>(r.Title, r.Id));
                            continue;
                        }
                        if (r.EventDayOfWeek.Equals(d.DayOfWeek) && r.IsWeekly)
                        {
                            d.GuildEvents.Add(new Tuple<string, int>(r.Title, r.Id));
                            continue;
                        }
                        if (r.IsBiWeekly)
                        {
                            bool oddWeek = Convert.ToBoolean(r.FirstEventDate.GetWeekOfMonth() % 2);
                            bool isDayOddWeek = Convert.ToBoolean(d.WeekOfMonth % 2);

                            if (oddWeek == isDayOddWeek && r.EventDayOfWeek.Equals(d.DayOfWeek))
                            {
                                d.GuildEvents.Add(new Tuple<string, int>(r.Title, r.Id));
                            }
                        }
                    }
                }



                m.DayDataList.Add(d);
                dateCounter = dateCounter.AddDays(1);
            }
            string strong = "stronk";



            return View(m);
        }

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
            return View(guildEvent);
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
                guildEvent.EventDayOfMonth = (int)guildEvent.FirstEventDate.DayOfWeek;
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
                i.EventDayOfMonth = (int)i.FirstEventDate.DayOfWeek;
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
                catch(DbEntityValidationException e)
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
        #endregion
    }
}
