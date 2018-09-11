using AutoMapper;
using QTrans.WebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QTrans.WebPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserProfile, QTrans.Models.UserProfile>().ForMember(dt=> dt.areaPreferences, options => options.Ignore()) ;
                
                cfg.CreateMap<Company, QTrans.Models.Company>();

            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
