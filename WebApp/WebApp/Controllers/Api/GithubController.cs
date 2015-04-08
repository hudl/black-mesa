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

namespace WebApp.Controllers.Api
{
    class GitHubRepo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    [CookieAuthenticated]
    [RoutePrefix("github")]
    public class GithubController : BaseController
    {
        [GET("repos")]
        public async Task<ActionResult> GetAllRepositories()
        {
            int pageSize = 100;
            int currentPage = 1;

            var allRepos = await GetRepositoryPage(pageSize, currentPage);

            // GitHub does pass the next page back in the headers,
            // but this code was easier to write and will behave in any sane use case.

            while (allRepos.Count % pageSize == 0)
            {
                currentPage++;
                allRepos.AddRange(await GetRepositoryPage(pageSize, currentPage));
            }

            return JsonNet(allRepos, false);
        }

        private async Task<List<GitHubRepo>> GetRepositoryPage(int pageSize, int page)
        {
            var request = String.Format("{0}/repos?per_page={1}&page={2}", PrivateConfig.GithubConfig.OrganizationUrl, pageSize, page);
            var response = await GetPageSource(request, GetHeaders());
            return JsonConvert.DeserializeObject<List<GitHubRepo>>(response);
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
