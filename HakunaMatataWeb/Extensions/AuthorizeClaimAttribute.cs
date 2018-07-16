﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace HakunaMatataWeb.Extensions
{


    public class AuthorizeClaimAttribute : AuthorizeAttribute
    {
        private string claimType;
        private string claimValue;

        public AuthorizeClaimAttribute(string type, string value)
        {
            this.claimType = type;
            this.claimValue = value;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User is ClaimsPrincipal user && user.HasClaim(claimType, claimValue))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}