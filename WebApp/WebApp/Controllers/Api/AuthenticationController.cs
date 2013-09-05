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
            // TODO: Don't just disable security, store a certificate in the private config.
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            AuthResponse data;
            using (WebClient client = new WebClient())
            {
                var postData = JsonConvert.SerializeObject(new { username = username, password = password });
                client.Headers["User-Agent"] = PrivateConfig.UserAgent;
                client.Headers["Content-Type"] = "application/json; charset=utf-8";
                var rawData = client.UploadString(PrivateConfig.AuthServer, postData);
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
