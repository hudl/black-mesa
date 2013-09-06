using AttributeRouting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers.Api
{
    [RouteArea("api/v1")]
    public class BaseController : Controller
    {
        protected static PrivateConfig PrivateConfig { get; private set; }

        public BaseController()
        {
            PrivateConfig = new PrivateConfigRepository().GetPrivateConfig();
        }

        protected string GetPageSource(string url, Dictionary<string, string> headers)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback = OnServerCertificateValidationCallback;
                    if (headers != null)
                    {
                        foreach (KeyValuePair<string, string> header in headers)
                        {
                            client.Headers[header.Key] = header.Value;
                        }
                    }
                    client.Headers["Content-Type"] = "application/json; charset=utf-8";
                    client.Headers["User-Agent"] = PrivateConfig.UserAgent;
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

        protected bool OnServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            var request = sender as HttpWebRequest;

            if (request == null)
            {
                var message = String.Format(
                    "OnServerCertificateValidationCallback: expected 'sender' to be HttpWebRequest, but it was {0}", sender == null ? "null" : sender.GetType().Name);
                throw new ArgumentException(message, "sender");
            }

            // tyson.stewart 28 December 2012 - This needs to be expanded to handle multiple certificates if we use
            //   more than just one web service (via HTTPS)
            var certificateIsValid = request.RequestUri.Equals(PrivateConfig.Authorization.Uri)
                && String.Equals(PrivateConfig.Authorization.Certificate.Subject, certificate.Subject)
                && String.Equals(PrivateConfig.Authorization.Certificate.Serial, certificate.GetSerialNumberString());

            return certificateIsValid;
        }
    }
}
