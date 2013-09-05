using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Attributes;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers.Api
{
    [RoutePrefix("auth")]
    public class AuthenticationController : BaseController
    {
        [POST("authenticate")]
        public ActionResult Login(string username, string password)
        {
            ServicePointManager.ServerCertificateValidationCallback = OnServerCertificateValidationCallback;
            AuthResponse data;
            using (WebClient client = new WebClient())
            {
                var postData = JsonConvert.SerializeObject(new { username = username, password = password });
                client.Headers["User-Agent"] = PrivateConfig.UserAgent;
                client.Headers["Content-Type"] = "application/json; charset=utf-8";
                var rawData = client.UploadString(PrivateConfig.Authorization.Uri, postData);
                data = JsonConvert.DeserializeObject<AuthResponse>(rawData);
            }
            if (data != null && data.Success)
            {
                var session = new Session()
                {
                    DisplayName = data.Name,
                    Groups = data.Groups,
                    Username = username
                };
                new SessionRepository().Collection.Insert(session);
                var cookie = new HttpCookie(CookieAuthenticatedAttribute.CookieName, session.Id);
                HttpContext.Response.Cookies.Clear();
                HttpContext.Response.Cookies.Add(cookie);
            }
            return JsonNet(data);
        }

        private bool OnServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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

        private class AuthResponse
        {
            [JsonProperty(PropertyName = "success")]
            public bool Success { get; set; }
            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }
            [JsonProperty(PropertyName = "groups")]
            public List<string> Groups { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }
    }
}
