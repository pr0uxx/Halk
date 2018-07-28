using HakunaMatataWeb.Data.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HakunaMatataWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim(CustomClaims.GuildRank.ToString(), GuildRank.Uninitiated.ToString()));
            userIdentity.AddClaim(new Claim(CustomClaims.SiteRank.ToString(), SiteRank.User.ToString()));

            return userIdentity;
        }

        public GuildRank GuildRank { get; set; }
        public SiteRank SiteRank { get; set; }
        public string DisplayName { get; set; }
    }
}