﻿using HakunaMatataWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace HakunaMatataWeb.Extensions
{
    public class AuthorizeGuildRankAttribute : AuthorizeAttribute
    {
        private readonly GuildRank claimValue;

        public AuthorizeGuildRankAttribute(GuildRank value)
        {
            this.claimValue = value;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = (ClaimsPrincipal)filterContext.HttpContext.User;
            var claim = user.GetClaimValueString("SiteRank");
            if (!string.IsNullOrEmpty(claim))
            {
                var enums = new List<KeyValuePair<string, int>>();
                dynamic evals = Enum.GetValues(typeof(GuildRank));

                foreach (var i in evals)
                {
                    int val = (int)i;
                    string name = ((GuildRank)val).ToString();
                    enums.Add(new KeyValuePair<string, int>(name, val));
                }

                var rankToMatch = enums.FirstOrDefault(x => x.Key.Equals(this.claimValue)).Value;
                var userRank = enums.FirstOrDefault(x => x.Key.Equals(claim)).Value;

                if (userRank >= rankToMatch)
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
}