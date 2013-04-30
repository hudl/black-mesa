using AttributeRouting;
using AttributeRouting.Web.Mvc;
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
    [RoutePrefix("jira")]
    public class JiraController : BaseController
    {
        [GET("tickets/{label}")]
        public ActionResult GetTicketsForLabel(string label)
        {
            var source = GetPageSource(ConfigurationManager.ConnectionStrings["JiraTickets"].ConnectionString.Replace("{label}", label), GetHeaders());
            return JsonNet(source, true);
        }

        private Dictionary<string, string> GetHeaders()
        {
            var text = ConfigurationManager.ConnectionStrings["JiraAuth"];
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text.ConnectionString);
            string basicAuth = System.Convert.ToBase64String(toEncodeAsBytes);
            return new Dictionary<string, string>()
            {
                {"Authorization", "Basic " + basicAuth}
            };
        }
    }
}
