using HakunaMatataWeb.Data.Models;
using HakunaMatataWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HakunaMatataWeb.Data.DataConnection
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<GuildEvent> GuildEvents { get; set; }
        public DbSet<ESOGuide> ESOGuides { get; set; }
        public DbSet<ImageUrl> ImageUrls { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}