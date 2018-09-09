using QTrans.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace QTrans.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //Enable Cors
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            config.Filters.Add(new RequestValidatorAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
