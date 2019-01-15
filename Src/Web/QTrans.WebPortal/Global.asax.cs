using AutoMapper;
using Castle.Windsor;
using Castle.Windsor.Installer;
using QTrans.Utility.Common;
using QTrans.WebPortal.Common;
using QTrans.WebPortal.Infrastructure;
using QTrans.WebPortal.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QTrans.WebPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IWindsorContainer windsorContainer;
        protected void Application_Start()
        {
            InitializeWindsor();
            RegisterAutoMappers();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InMemoryStorage.Instance.Init();
        }

        /// <summary>
        /// Initialize all dependency injections and its related components
        /// </summary>
        private void InitializeWindsor()
        {
            windsorContainer = new WindsorContainer().Install(FromAssembly.This());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(windsorContainer.Kernel));
            DependencyResolver.SetResolver(new WindsorDependencyResolver(windsorContainer));
        }

        /// <summary>
        /// Register all required auto mappers
        /// </summary>
        private void RegisterAutoMappers()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserProfile, QTrans.Models.UserProfile>().ForMember(dt => dt.areaPreferences, options => options.Ignore());

                cfg.CreateMap<Company, QTrans.Models.Company>();

            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}
