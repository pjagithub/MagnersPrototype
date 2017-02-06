using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Magners
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // The following route depends on the installation of a NuGet package to 
            // provide a JSONP (JSON padded with callback function) formatter for Web API
            //  Install-Package WebApiContrib.Formatting.Jsonp
            // http://www.codeproject.com/Tips/631685/JSONP-in-ASP-NET-Web-API-Quick-Get-Started
            // 
            routes.MapHttpRoute(
                name: "DefaultJsonpApi",
                routeTemplate: "{controller}/{action}/{id}/{format}",
                defaults: new { id = RouteParameter.Optional, format = RouteParameter.Optional }
            );
        }
    }
}