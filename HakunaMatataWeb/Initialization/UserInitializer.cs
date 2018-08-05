using HakunaMatataWeb.Data.DataConnection;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using HakunaMatataWeb.Data.Models;
using System;

namespace HakunaMatataWeb.Data.Initialization
{
    public class UserInitializer : System.Data.Entity.CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            //var guide = new ESOGuide
            //{
            //    Author = "Test",
            //    AuthorId = "0",
            //    Content = string.Empty,
            //    CreationDate = DateTime.Now,
            //    EsoGuideId = 0,
            //    GuideType = Enums.EventType.PvP,
            //    ImageUrls = new List<ImageUrl>(),
            //    LastUpdatedDate = DateTime.Now,
            //    SubTitle = "test",
            //    Title = "test"
            //};

            //context.ESOGuides.Add(guide);

            //context.SaveChangesAsync();

            base.Seed(context);
        }
    }
}