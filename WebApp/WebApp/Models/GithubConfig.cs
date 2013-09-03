using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class GithubConfig : AccountConfig
    {
        public string PullRequestBranchUrl { get; set; }
        public string PullRequestCommentsUrl { get; set; }
        public string RecentCommitsUrl { get; set; }
    }
}