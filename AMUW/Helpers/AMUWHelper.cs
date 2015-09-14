using AMUW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMUW.Helpers
{
    public class AMUWHelper
    {
        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (!string.IsNullOrWhiteSpace(appUrl)) appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        public static string GetRole()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var userRole = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var currentUser = user.FindByEmail(username);
            var role = userRole.FindById(currentUser.Roles.FirstOrDefault(x => x.UserId == currentUser.Id).RoleId).Name;

            return role;
        }
    }
}