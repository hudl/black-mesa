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
using WebApp.Attributes;

namespace WebApp.Controllers.Api
{
    [CookieAuthenticated]
    [RoutePrefix("github")]
    public class GithubController : BaseController
    {
        [GET("repos")]
        public ActionResult GetAllRepositories()
        {
            var source = GetPageSource(PrivateConfig.GithubConfig.OrganizationUrl + "/repos", GetHeaders());
            return JsonNet(source, true);
        }

        [GET("{repo}/pullRequest/{pr}/branch")]
        public ActionResult GetPullRequestBranch(string repo, string pr)
        {
            var url = PrivateConfig.GithubConfig.BaseUrl + "/" + repo +"/pulls/" + pr;
            var source = GetPageSource(url, GetHeaders());
            return JsonNet(source, true);
        }

        [GET("{repo}/pullRequest/{pr}/comments")]
        public ActionResult GetPullRequestComments(string repo, string pr)
        {
            var url = PrivateConfig.GithubConfig.BaseUrl + "/" + repo + "/pulls/" + pr + "/comments";
            var source = GetPageSource(url, GetHeaders());
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
