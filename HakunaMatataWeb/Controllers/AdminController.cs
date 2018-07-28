using HakunaMatataWeb.Data.Enums;
using HakunaMatataWeb.Services.Extensions;
using HakunaMatataWeb.Models;
using HakunaMatataWeb.Models.ViewModels;
using HakunaMatataWeb.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HakunaMatataWeb.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AdminController()
        {
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //[AuthorizeClaim("SiteRank", "Developer")]
        [AuthorizeSiteRank(SiteRank.Developer)]
        public ActionResult AdministerUsers()
        {
            ViewBag.Title = "Administer Users";

            var m = new AdministerUserViewModel();

            var userList = UserManager.Users.ToList();
            ViewBag.UserList = new List<SelectListItem>();

            foreach (var u in userList)
            {
                var li = new SelectListItem { Text = u.UserName, Value = u.Id };
                ViewBag.UserList.Add(li);
            }
            ViewBag.UserList.Add(new SelectListItem { Text = "Please Select", Value = "0", Selected = true });

            ViewBag.TZSelectList = new SelectList(Helper.GetTimeZoneList(), "Value", "Text", "0");

            ViewBag.GuildRankList = Helper.GetEnumSelectList<GuildRank>();
            ViewBag.SiteRankList = Helper.GetEnumSelectList<SiteRank>();

            return View(m);
        }

        [AuthorizeSiteRank(SiteRank.Developer)]
        public async Task<ActionResult> GetUserClaims(string userId)
        {
            var claims = await UserManager.GetClaimsAsync(userId);

            var returnClaims = new ClaimsViewModel();
            if (claims.Any(x => x.Type.Equals("GuildRank")))
            {
                returnClaims.GuildRank = claims.FirstOrDefault(x => x.Type.ToString().Equals("GuildRank")).Value ?? string.Empty;
            }
            if (claims.Any(x => x.Type.Equals("SiteRank")))
            {
                returnClaims.SiteRank = claims.FirstOrDefault(x => x.Type.Equals("SiteRank")).Value ?? string.Empty;
            }
            if (claims.Any(x => x.Type.Equals("DisplayName")))
            {
                returnClaims.DisplayName = claims.FirstOrDefault(x => x.Type.Equals("DisplayName")).Value ?? string.Empty;
            }
            if (claims.Any(x => x.Type.Equals("LocalTimezone")))
            {
                returnClaims.TimeZone = claims.FirstOrDefault(x => x.Type.Equals("LocalTimezone")).Value ?? "None Selected";
            }

            return Json(JsonConvert.SerializeObject(returnClaims), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeClaim("SiteRank", "Developer")]
        public async Task<ActionResult> EditUser(AdministerUserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.UserName);

            //change password
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (!string.IsNullOrEmpty(model.ConfirmPassword) && model.NewPassword.Equals(model.ConfirmPassword))
                {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                    var result = UserManager.ResetPasswordAsync(user.Id, token, model.NewPassword);
                }
            }

            var claims = await UserManager.GetClaimsAsync(user.Id);

            var guildRank = claims.FirstOrDefault(x => x.Type.Equals("GuildRank", StringComparison.OrdinalIgnoreCase));
            var siteRank = claims.FirstOrDefault(x => x.Type.Equals("SiteRank", StringComparison.OrdinalIgnoreCase));
            var displayName = claims.FirstOrDefault(x => x.Type.Equals("DisplayName", StringComparison.OrdinalIgnoreCase));
            var localTimezone = claims.FirstOrDefault(x => x.Type.Equals("LocalTimezone", StringComparison.OrdinalIgnoreCase));

            //change GuildRank
            if (guildRank != null)
            {
                if (model.GuildRank != null)
                {
                    if (!model.GuildRank.Equals(guildRank))
                    {
                        var result = await UserManager.RemoveClaimAsync(user.Id, guildRank);
                        if (result.Succeeded)
                        {
                            result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("GuildRank", model.GuildRank.ToString()));
                        }
                    }
                }
            }
            else
            {
                var result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("GuildRank", model.GuildRank.ToString()));
            }

            //change SiteRank
            if (siteRank != null)
            {
                if (model.SiteRank != null)
                {
                    if (!model.SiteRank.Equals(siteRank))
                    {
                        var result = await UserManager.RemoveClaimAsync(user.Id, siteRank);
                        if (result.Succeeded)
                        {
                            result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("SiteRank", model.SiteRank.ToString()));
                        }
                    }
                }
            }
            else
            {
                var result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("SiteRank", model.SiteRank.ToString()));
            }

            //change DisplayName
            if (!string.IsNullOrEmpty(model.DisplayName))
            {
                if (!model.DisplayName.Equals(displayName))
                {
                    if ((claims.Any(x => x.Type.ToString().Equals("DisplayName"))))
                    {
                        var result = await UserManager.RemoveClaimAsync(user.Id, new System.Security.Claims.Claim("DisplayName", displayName.Value));
                        if (result.Succeeded)
                        {
                            result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("DisplayName", model.DisplayName.ToString()));
                        }
                    }
                    else
                    {
                        var result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("DisplayName", model.DisplayName.ToString()));
                    }
                }
            }

            if (!model.LocalTimezone.Equals("0"))
            {
                if (localTimezone != null && !string.IsNullOrEmpty(localTimezone.ToString()))
                {
                    var result = await UserManager.RemoveClaimAsync(user.Id, new System.Security.Claims.Claim("LocalTimezone", model.LocalTimezone.ToString()));
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("LocalTimezone", model.LocalTimezone.ToString()));
                    }
                }
                else
                {
                    var result = await UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("LocalTimezone", model.LocalTimezone.ToString()));
                }
            }

            return RedirectToAction("AdministerUsers");
        }
    }
}