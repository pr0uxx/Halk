using Microsoft.Owin.Security;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace HakunaMatataWeb.Services.Extensions
{  

    public static class IdentityExtenstions
    {
        public static string GetGuildRank(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("GuildRank");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetDisplayName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("DisplayName");
            return (claim != null) ? claim.Value : string.Empty;
        }

        /// <summary>
        /// Adds a claim if it doesn't already exist. Updates pre-exisiting claims
        /// </summary>
        /// <param name="currentPrincipal"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddUpdateClaimById(this IPrincipal currentPrincipal, string key, string value)
        {
            if (!(currentPrincipal.Identity is ClaimsIdentity identity))
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }

        public static string GetClaimValueString(this IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            if (claim != null)
            {
                return claim.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool HasClaim<T>(this IPrincipal currentPrincipal)
        {
            if (!(currentPrincipal.Identity is ClaimsIdentity identity))
                return false;
            var x = typeof(T);
            if (identity.Claims.Any(c => c.Type.Equals(x)))
            {
                return true;
            };

            return false;
        }

        public static int GetClaimValueInt<T>(this IPrincipal currentPrincipal)
        {
            if (!(currentPrincipal.Identity is ClaimsIdentity identity))
                return 0;
            var x = typeof(T);
            var claim = identity.Claims.FirstOrDefault(c => c.Type == x.Name);
            if (claim != null)
            {
                Assembly asm = typeof(HakunaMatataWeb.Models.ApplicationUser).Assembly;
                Type t = claim.Type.GetType();
                int rank = (int)Enum.Parse(x, claim.Value);
                return rank;
            }
            else
            {
                return 0;
            }
        }

        public static T? ToNullable<T>(this string s) where T : struct
        {
            T? result = new T?();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }

        private static GregorianCalendar _gc = new GregorianCalendar();

        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        private static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }
}