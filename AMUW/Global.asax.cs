﻿using AMUW.App_Start;
using AMUW.AutoMapper;
using AMUW.Data.Installer;
using AMUW.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AMUW
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbContext.DropCreateAlwaysInitializer());
            Database.SetInitializer(new DataInitializer());
        }
    }
}
