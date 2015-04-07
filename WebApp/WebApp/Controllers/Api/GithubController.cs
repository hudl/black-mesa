using System.Threading.Tasks;
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
using System.Linq;

namespace WebApp.Controllers.Api
{
    class GitHubRepo
    {
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }
    }

    [CookieAuthenticated]
    [RoutePrefix("github")]
    public class GithubController : BaseController
    {
        [GET("repos")]
        public async Task<ActionResult> GetAllRepositories()
        {
            int pageSize = 100;
            var allRepos = JsonConvert.DeserializeObject<List<GitHubRepo>>(await GetPageSource(PrivateConfig.GithubConfig.OrganizationUrl + "/repos?per_page=" + pageSize + "&page=1", GetHeaders()));
            var nextPage = JsonConvert.DeserializeObject<List<GitHubRepo>>(await GetPageSource(PrivateConfig.GithubConfig.OrganizationUrl + "/repos?per_page=" + pageSize + "&page=2", GetHeaders()));
            allRepos.AddRange(nextPage);
            int currentPage = 2;

            while(nextPage.Count == pageSize) {
                currentPage++;
                nextPage = JsonConvert.DeserializeObject<List<GitHubRepo>>(await GetPageSource(PrivateConfig.GithubConfig.OrganizationUrl + "/repos?per_page=" + pageSize + "&page=" + currentPage, GetHeaders()));
                allRepos.AddRange(nextPage);
            }

            return JsonNet(allRepos, false);
        }

        [GET("{repo}/pullRequest/{pr}/branch")]
        public async Task<ActionResult> GetPullRequestBranch(string repo, string pr)
        {
            var url = PrivateConfig.GithubConfig.BaseUrl + "/" + repo +"/pulls/" + pr;
            var source = await GetPageSource(url, GetHeaders());
            return JsonNet(source, true);
        }

        [GET("{repo}/pullRequest/{pr}/comments")]
        public async Task<ActionResult> GetPullRequestComments(string repo, string pr)
        {
            var url = PrivateConfig.GithubConfig.BaseUrl + "/" + repo + "/pulls/" + pr + "/comments";
            var source = await GetPageSource(url, GetHeaders());
            return JsonNet(source, true);
        }

        [GET("commits")]
        public async Task<ActionResult> GetRecentRequestsToMaster()
        {
            var source = await GetPageSource(PrivateConfig.GithubConfig.RecentCommitsUrl, GetHeaders());
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
