using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WorkOrderEMS.App_Start;

namespace WorkOrderEMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // config.Formatters.Remove(config.Formatters.XmlFormatter);
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.UseDataContractJsonSerializer = true;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //var jsonFormatter = new JsonMediaTypeFormatter
            //{
            //    SerializerSettings =
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //    NullValueHandling = NullValueHandling.Ignore
            //}
            //};
            var jsonFormatter = new JsonMediaTypeFormatter();

            //optional: set serializer settings here
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));
            config.EnableSystemDiagnosticsTracing();

            StructuremapWebApi.Start();
        }
    }
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }

        public ContentNegotiationResult Negotiate(
                Type type,
                HttpRequestMessage request,
                IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(
                _jsonFormatter,
                new MediaTypeHeaderValue("application/json"));
        }
    }
}