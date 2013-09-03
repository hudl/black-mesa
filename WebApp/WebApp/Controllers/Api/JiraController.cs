using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.Controllers.Api
{
    [RoutePrefix("jira")]
    public class JiraController : BaseController
    {
        [GET("tickets/{label}")]
        public ActionResult GetTicketsForLabel(string label)
        {
            var source = GetPageSource(PrivateConfig.JiraConfig.TicketsUrl.Replace("{label}", label), GetHeaders());
            return JsonNet(source, true);
        }

        private Dictionary<string, string> GetHeaders()
        {
            var auth = PrivateConfig.JiraConfig.Username + ":" + PrivateConfig.JiraConfig.Password;
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(auth);
            string basicAuth = System.Convert.ToBase64String(toEncodeAsBytes);
            return new Dictionary<string, string>()
            {
                {"Authorization", "Basic " + basicAuth}
            };
        }
    }
}
