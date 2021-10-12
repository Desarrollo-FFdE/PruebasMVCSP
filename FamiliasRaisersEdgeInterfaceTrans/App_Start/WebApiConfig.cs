using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FamiliasRaisersEdgeInterfaceTrans
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "FamiliasRaisersEdgeInterfaceTrans",
                routeTemplate: "familias/sponsorship/{controller}",
                defaults: new { controller = "FamiliasRaisersEdgeInterfaceTrans", id = RouteParameter.Optional }
            );
        }
    }
}
