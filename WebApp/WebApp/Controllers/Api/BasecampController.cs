using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.Controllers.Api
{
    [RoutePrefix("basecamp")]
    public class BasecampController : BaseController
    {
        // Note that "message" is like a thread id to basecamp, content is the actual message we will be posting.
        [POST("/"), ValidateInput(false)]
        public ActionResult PostBasecampMessage(string project, string message, string content)
        {
            var usersRaw = GetPageSource(ConfigurationManager.ConnectionStrings["BasecampGetAccesses"].ConnectionString.Replace("{project}", project), GetHeaders());
            var users = JsonConvert.DeserializeObject<List<BasecampUser>>(usersRaw);
            return JsonNet(PostMessage(ConfigurationManager.ConnectionStrings["BasecampPost"].ConnectionString.Replace("{project}", project).Replace("{message}", message), content, users, GetHeaders()));
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
                client.Headers["User-Agent"] = System.Configuration.ConfigurationManager.ConnectionStrings["User-Agent"].ConnectionString;
                client.Headers["Content-Type"] = "application/json; charset=utf-8";
                return client.UploadString(new Uri(url), postData);
            }
        }

        private Dictionary<string, string> GetHeaders()
        {
            var text = ConfigurationManager.ConnectionStrings["BasecampAuth"];
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text.ConnectionString);
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
