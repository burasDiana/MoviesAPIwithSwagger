﻿using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestWebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //does not work as it affects the home route
            //routes.MapHttpRoute( 
            //name: "Error404",
            //routeTemplate: "",
            //defaults: new { controller = "Error", action = "Handle404" });

            routes.MapHttpRoute(
            name: "swagger_root" ,
            routeTemplate: "" ,
            defaults: null ,
            constraints: null ,
            handler: new RedirectHandler(( message => message.RequestUri.ToString() ) , "swagger"));

            //routes.MapRoute(
            //    name: "Default" ,
            //    url: "{controller}/{action}/{id}" ,
            //    defaults: new { controller = "Home" , action = "Index" , id = UrlParameter.Optional }
            //);
        }
    }
}
