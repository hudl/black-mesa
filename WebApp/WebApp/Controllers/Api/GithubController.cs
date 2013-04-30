using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.Controllers.Api
{
    [RoutePrefix("github")]
    public class GithubController : BaseController
    {
        [GET("pullRequest/{pr}/branch")]
        public ActionResult GetPullRequestBranch(string pr)
        {
            var source = GetPageSource(ConfigurationManager.ConnectionStrings["GitHubPrBranch"].ConnectionString.Replace("{pr}", pr), GetHeaders());
            return JsonNet(source, true);
        }

        [GET("pullRequest/{pr}/comments")]
        public ActionResult GetPullRequestComments(string pr)
        {
            var source = GetPageSource(ConfigurationManager.ConnectionStrings["GitHubPrComments"].ConnectionString.Replace("{pr}", pr), GetHeaders());
            return JsonNet(source, true);
        }

        [GET("commits")]
        public ActionResult GetRecentRequestsToMaster()
        {
            var source = GetPageSource(ConfigurationManager.ConnectionStrings["GitHubCommits"].ConnectionString, GetHeaders());
            return JsonNet(source, true);
        }

        private Dictionary<string, string> GetHeaders()
        {
            var text = ConfigurationManager.ConnectionStrings["GitHubAuth"];
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text.ConnectionString);
            string basicAuth = System.Convert.ToBase64String(toEncodeAsBytes);
            return new Dictionary<string, string>()
            {
                {"Authorization", "Basic " + basicAuth}
            };
        }
    }
}
