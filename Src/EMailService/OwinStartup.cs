using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(EMailService.OwinStartup))]

namespace EMailService
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{token}/{id}",
            defaults: new { id = RouteParameter.Optional, token = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
            appBuilder.Run(sample =>
            {
                sample.Response.ContentType = "text/plain";
                return sample.Response.WriteAsync("Hello from Email Service");
            });
        }
    }
}
