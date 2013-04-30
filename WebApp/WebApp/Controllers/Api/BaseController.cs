using AttributeRouting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.Controllers.Api
{
    [RouteArea("api/v1")]
    public class BaseController : Controller
    {
        protected string GetPageSource(string url, Dictionary<string, string> headers)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, string> header in headers)
                        {
                            client.Headers[header.Key] = header.Value;
                        }
                    }
                    client.Headers["Content-Type"] = "application/json; charset=utf-8";
                    client.Headers["User-Agent"] = System.Configuration.ConfigurationManager.ConnectionStrings["User-Agent"].ConnectionString;
                    return client.DownloadString(url);
                }
            }
            catch (WebException)
            {
                return null;
            }
        }

        protected static JsonNetResult JsonNet(object data, bool alreadyJson = false)
        {
            return new JsonNetResult()
            {
                Data = data,
                AlreadyJson = alreadyJson
            };
        }

        protected class JsonNetResult : ActionResult
        {
            public string ContentType { get; set; }
            public object Data { get; set; }
            public bool AlreadyJson { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                var response = context.HttpContext.Response;

                response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

                if (Data != null)
                {
                    if (!AlreadyJson)
                    {
                        var writer = new JsonTextWriter(response.Output);
                        var settings = new JsonSerializerSettings();
                        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        var serializer = JsonSerializer.Create(settings);
                        serializer.Serialize(writer, Data);
                        writer.Flush();
                    }
                    else
                    {
                        var writer = new JsonTextWriter(response.Output);
                        writer.WriteRaw(Data.ToString());
                        writer.Flush();
                    }
                }
            }
        }
    }
}
