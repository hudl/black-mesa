using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
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
            var source = GetPageSource(PrivateConfig.GithubConfig.PullRequestBranchUrl.Replace("{pr}", pr), GetHeaders());
            return JsonNet(source, true);
        }

        [GET("pullRequest/{pr}/comments")]
        public ActionResult GetPullRequestComments(string pr)
        {
            var source = GetPageSource(PrivateConfig.GithubConfig.PullRequestCommentsUrl.Replace("{pr}", pr), GetHeaders());
            return JsonNet(source, true);
        }

        [GET("commits")]
        public ActionResult GetRecentRequestsToMaster()
        {
            var source = GetPageSource(PrivateConfig.GithubConfig.RecentCommitsUrl, GetHeaders());
            return JsonNet(source, true);
        }

        private Dictionary<string, string> GetHeaders()
        {
            var auth = PrivateConfig.GithubConfig.Username + ":" + PrivateConfig.GithubConfig.Password;
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(auth);
            string basicAuth = System.Convert.ToBase64String(toEncodeAsBytes);
            return new Dictionary<string, string>()
            {
                {"Authorization", "Basic " + basicAuth}
            };
        }
    }
}
