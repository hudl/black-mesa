using CsvHelper.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class DeployForExport
    {
        public string Day { get; set; }
        public string DateTime { get; set; }
        public string Action { get; set; }
        public string Component { get; set; }
        public string Type { get; set; }
        public string Project { get; set; }
        public string Branch { get; set; }
        public string PullRequest { get; set; }
        public string PullRequestLink { get; set; }
        public string Jira { get; set; }
        public string QA { get; set; }
        public string Des { get; set; }
        public string Dev { get; set; }
        public string DevCodeReview { get; set; }
        public string ProjectManager { get; set; }
        public string Notes { get; set; }

        public bool IsHotfix { get; set; }

        public string Special { get; set; }
        public string DevTeamCulpability { get; set; }
        public string DevHudlWideImpact { get; set; }
        public string DevAffectedUserImpact { get; set; }
        public string DevInitials { get; set; }
        public string QaTeamCulpability { get; set; }
        public string QaHudlWideImpact { get; set; }
        public string QaAffectedUserImpact { get; set; }
        public string QaInitials { get; set; }
        public string BranchThatBrokeIt { get; set; }
        public string ProdTicketNum { get; set; }
        public string ProdTicket { get; set; }
        public string HfNotes { get; set; }

        public static ICollection<DeployForExport> convertDeploysForExport(ICollection<Deploy> deploys)
        {
            ICollection<DeployForExport> deploysForExport = new List<DeployForExport>(deploys.Count());
            int i = 0;
            foreach (Deploy deploy in deploys)
            {
                i++;
                bool hasHotfix = deploy.Hotfixes != null && deploy.Hotfixes.Count > 0;
                try
                {
                    deploysForExport.Add(new DeployForExport()
                    {
                        Action = deploy.Action,
                        Branch = deploy.Branch,
                        BranchThatBrokeIt = hasHotfix ? deploy.Hotfixes[0].BranchThatBrokeIt : null,
                        Component = deploy.Component,
                        DateTime = deploy.DeployTime.GetValueOrDefault().ToString(),
                        Day = deploy.Day,
                        Des = deploy.People.Designers.FirstOrDefault(),
                        Dev = deploy.People.Developers.FirstOrDefault(),
                        DevAffectedUserImpact = hasHotfix ? deploy.Hotfixes[0].Assessments.Developers.AffectedUserImpact.ToString() : null,
                        DevCodeReview = deploy.People.CodeReviewers.FirstOrDefault(),
                        DevHudlWideImpact = hasHotfix ? deploy.Hotfixes[0].Assessments.Developers.HudlWideImpact.ToString() : null,
                        DevInitials = hasHotfix ? deploy.Hotfixes[0].Assessments.Developers.Initials : null,
                        DevTeamCulpability = hasHotfix ? deploy.Hotfixes[0].Assessments.Developers.Culpability.ToString() : null,
                        HfNotes = hasHotfix ? deploy.Hotfixes[0].Notes : null,
                        IsHotfix = hasHotfix,
                        Jira = deploy.JiraLabel,
                        Notes = deploy.Notes,
                        ProdTicket = hasHotfix ? "http://jira/browse/PROD-" + deploy.Hotfixes[0].ProdTicket : null,
                        ProdTicketNum = hasHotfix ? deploy.Hotfixes[0].ProdTicket.ToString() : null,
                        Project = deploy.Project,
                        ProjectManager = deploy.People.ProjectManagers.FirstOrDefault(),
                        PullRequest = deploy.PullRequestId.ToString(),
                        PullRequestLink = "https://github.com/hudl/hudl/pull/" + deploy.PullRequestId,
                        QA = deploy.People.Quails.FirstOrDefault(),
                        QaAffectedUserImpact = hasHotfix ? deploy.Hotfixes[0].Assessments.Quails.AffectedUserImpact.ToString() : null,
                        QaHudlWideImpact = hasHotfix ? deploy.Hotfixes[0].Assessments.Quails.HudlWideImpact.ToString() : null,
                        QaInitials = hasHotfix ? deploy.Hotfixes[0].Assessments.Quails.Initials : null,
                        QaTeamCulpability = hasHotfix ? deploy.Hotfixes[0].Assessments.Quails.Culpability.ToString() : null,
                        Special = hasHotfix ? deploy.Hotfixes[0].Special : null,
                        Type = deploy.Type
                    });
                }
                catch
                {
                    throw new Exception("Tell somebody who knows black mesa that id " + deploy.Id + " is broken!");
                }
            }
            return deploysForExport;
        }
    }
}
