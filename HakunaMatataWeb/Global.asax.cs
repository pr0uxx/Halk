using HakunaMatataWeb.Data.Services;
using HakunaMatataWeb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;

namespace HakunaMatataWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_OnAuthenticateRequest(object sender, EventArgs e)
        //{
        //    if (!UserHasAccess())
        //    {
        //        FormsAuthentication.SignOut();
        //    }
        //}

        //private bool UserHasAccess()
        //{
        //    var user = Membership.GetUser(Context.User.Identity);

        //    HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
        //}
    }
}
