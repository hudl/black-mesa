using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Attributes;

namespace WebApp.Controllers.Api
{
    [CookieAuthenticated]
    [RoutePrefix("basecamp")]
    public class BasecampController : BaseController
    {
        // Note that "message" is like a thread id to basecamp, content is the actual message we will be posting.
        [POST("/"), ValidateInput(false)]
        public ActionResult PostBasecampMessage(string project, string message, string content)
        {
            return JsonNet(PostMessage(PrivateConfig.BasecampConfig.PostUrl
                                           .Replace("{project}", project)
                                           .Replace("{message}", message),
                                       content, GetUsers(), GetHeaders()));
        }

        private List<BasecampUser> GetUsers()
        {
            var page = 1;
            var ret = new List<BasecampUser>();
            var rawurl = PrivateConfig.BasecampConfig.GetAccessesUrl.Replace("{project}", project);
            do {
                var raw = GetPageSource(rawurl + "?page=" + page, GetHeaders());
                ret.AddRange(JsonConvert.DeserializeObject<List<BasecampUser>>(raw));
                ++page;
            } while (raw.Length == 50);
            return ret;
        }

        private string PostMessage(string url, string content, List<BasecampUser> usersToShare, Dictionary<string, string> headers)
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
                var postData = JsonConvert.SerializeObject(new BasecampPost
                {
                    content = content,
                    subscribers = usersToShare.Select(user => user.id).ToList()
                });
                client.Headers["User-Agent"] = PrivateConfig.UserAgent;
                client.Headers["Content-Type"] = "application/json; charset=utf-8";
                return client.UploadString(new Uri(url), postData);
            }
        }

        private Dictionary<string, string> GetHeaders()
        {
            var auth = PrivateConfig.BasecampConfig.Username + ":" + PrivateConfig.BasecampConfig.Password;
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(auth);
            string basicAuth = System.Convert.ToBase64String(toEncodeAsBytes);
            return new Dictionary<string, string>()
            {
                {"Authorization", "Basic " + basicAuth}
            };
        }

        public class BasecampUser
        {
            public string id { get; set; }
        }

        public class BasecampPost
        {
            public string content { get; set; }
            public List<string> subscribers { get; set; }
        }
    }
}
