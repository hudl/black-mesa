using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DeployImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var deploys = doDeployDoc();
            var hotfixes = doHotfixDoc();
            List<Deploy> finalDeploys = new List<Deploy>();
            foreach (DeployDTO deploy in deploys)
            {
                finalDeploys.Add(new Deploy
                {
                    Action = deploy.ACTION,
                    Branch = deploy.BRANCH,
                    Component = deploy.COMPONENT,
                    DateTime = deploy.DATE_TIME,
                    Day = deploy.Day,
                    Des = deploy.DES,
                    Dev = deploy.DEV,
                    DevCodeReview = deploy.DEVCR,
                    LineNumber = deploy.ID,
                    IsHotfix = false,
                    Jira = "http://jira/secure/IssueNavigator.jspa?reset=true&jqlQuery=labels+%3D+" + deploy.BRANCH,
                    Notes = deploy.NOTES,
                    Project = deploy.PROJECT,
                    ProjectManager = deploy.PM,
                    PullRequest = deploy.PR,
                    PullRequestLink = "https://github.com/hudl/hudl/pull/" + deploy.PR,
                    QA = deploy.QA,
                    Type = deploy.TYPE
                });
            }
            var i = 0;
            foreach (HotfixDTO hotfix in hotfixes)
            {
                Deploy deploy = null;
                if (hotfix.BRANCH.ToLower().Contains("(2nd pr)"))
                {
                    hotfix.BRANCH = hotfix.BRANCH.ToLower().Replace("(2nd pr)", "").Trim();
                    deploy = finalDeploys.Where(d => d.Branch.ToLower().Trim().Equals(hotfix.BRANCH.ToLower().Trim())).LastOrDefault();
                }
                else
                {
                    deploy = finalDeploys.Where(d => d.Branch.ToLower().Trim().Equals(hotfix.BRANCH.ToLower().Trim())).FirstOrDefault();
                }
                if (deploy == null)
                {
                    Console.WriteLine("NO MATCH!! -- Line: " + (Int32.Parse(hotfix.ID) + 17) + " - HF Branch: " + hotfix.BRANCH);
                    i++;
                }
                else
                {
                    deploy.BranchThatBrokeIt = hotfix.Breaker_Branch;
                    deploy.DevAffectedUserImpact = hotfix.DEV_Affected_User_Impact;
                    deploy.DevHudlWideImpact = hotfix.DEV_Hudl_Wide_Impact;
                    deploy.DevInitials = hotfix.DEV_Initials;
                    deploy.DevTeamCulpability = hotfix.DEV_Team_Culpability;
                    deploy.HfNotes = hotfix.Notes;
                    deploy.IsHotfix = true;
                    deploy.ProdTicket = "http://jira/browse/PROD-" + hotfix.PROD_Ticket_Num;
                    deploy.ProdTicketNum = hotfix.PROD_Ticket_Num;
                    deploy.QaAffectedUserImpact = hotfix.QA_Affected_User_Impact;
                    deploy.QaHudlWideImpact = hotfix.QA_Hudl_Wide_Impact;
                    deploy.QaInitials = hotfix.QA_Initials;
                    deploy.QaTeamCulpability = hotfix.QA_Team_Culpability;
                    deploy.Special = hotfix.SPECIAL;
                }
            }
            Console.WriteLine(i + " unmatched hotfixes");
            var repo = new DeployRepository();
            foreach (Deploy d in finalDeploys)
            {
                repo.UpdateSomething(d);
            }
        }

        public static List<DeployDTO> doDeployDoc()
        {
            if (!File.Exists("deploy_doc.csv"))
            {
                Console.WriteLine("Please place \"deploy_doc.csv\" in the directory this is running in");
                throw new Exception("I WANT WHAT I WANT OK?");
            }
            string line;
            StringBuilder builder = new StringBuilder();
            var file = new StreamReader("deploy_doc.csv");
            var lineNumber = -1;
            bool dontWrite = false;
            while ((line = file.ReadLine()) != null)
            {
                if (lineNumber++ == -1)
                {
                    if (line.Substring(0, 2).Equals("ID"))
                    {
                        dontWrite = true;
                        break;
                    }
                    line = "ID," + line.Replace("DATE TIME", "DATE_TIME").Replace("PR,PR", "PR,PRL").Replace("DEV - CR", "DEVCR").Replace("NOTES (reference branch that caused rollback/hotfix; errors)", "NOTES");
                }
                else
                {
                    line = lineNumber + "," + line;
                }
                builder.Append(line + "\n");
            }
            if (!dontWrite)
            {
                file.Close();
                var writer = new StreamWriter("deploy_doc.csv", false);
                writer.Write(builder.ToString());
                writer.Close();
            }

            var csv = new CsvReader(new StreamReader("deploy_doc.csv"));
            var deploys = csv.GetRecords<DeployDTO>().ToList();
            return deploys;
        }

        public static List<HotfixDTO> doHotfixDoc()
        {
            if (!File.Exists("hotfix_doc.csv"))
            {
                Console.WriteLine("Please place \"hotfix_doc.csv\" in the directory this is running in");
                throw new Exception("I WANT WHAT I WANT OK?");
            }
            string line;
            StringBuilder builder = new StringBuilder();
            var file = new StreamReader("hotfix_doc.csv");
            var lineNumber = -1;
            bool dontWrite = false;
            while ((line = file.ReadLine()) != null)
            {
                if (lineNumber++ == -1)
                {
                    if (line.Substring(0, 17).Equals("ID,,,,,,,,,BRANCH"))
                    {
                        dontWrite = true;
                        break;
                    }
                }
                else if (lineNumber == 17)
                {
                    line = "ID,,,,,,,,,BRANCH,,,,,,,,,,,,,,,,,,COMPONENT,SPECIAL,DEV_Team_Culpability,DEV_Hudl_Wide_Impact,DEV_Affected_User_Impact,DEV_Initials,QA_Team_Culpability,QA_Hudl_Wide_Impact,QA_Affected_User_Impact,QA_Initials,,Breaker_Branch,PROD_Ticket_Num,PROD_Ticket,Notes";
                    builder.Append(line + "\n");
                }
                else if (lineNumber > 17)
                {
                    line = (lineNumber - 17) + "," + line;
                    builder.Append(line + "\n");
                }
            }
            if (!dontWrite)
            {
                file.Close();
                var writer = new StreamWriter("hotfix_doc.csv", false);
                writer.Write(builder.ToString());
                writer.Close();
            }

            var csv = new CsvReader(new StreamReader("hotfix_doc.csv"));
            var hotfixes = csv.GetRecords<HotfixDTO>().ToList();
            var clone = new List<HotfixDTO>(hotfixes);
            HotfixDTO lastValidHotfix = null;
            foreach (HotfixDTO hotfix in clone)
            {
                if (String.IsNullOrWhiteSpace(hotfix.BRANCH) && String.IsNullOrWhiteSpace(hotfix.DEV_Initials))
                {
                    hotfixes.Remove(hotfix);
                    continue;
                }
                if (!String.IsNullOrWhiteSpace(hotfix.BRANCH))
                {
                    lastValidHotfix = hotfix;
                }
                if (String.IsNullOrWhiteSpace(hotfix.BRANCH) && !String.IsNullOrWhiteSpace(hotfix.DEV_Initials))
                {
                    hotfix.BRANCH = lastValidHotfix.BRANCH;
                }
            }
            foreach (HotfixDTO hotfix in clone)
            {
                if (!String.IsNullOrWhiteSpace(hotfix.BRANCH) && String.IsNullOrWhiteSpace(hotfix.DEV_Initials))
                {
                    hotfixes.Remove(hotfix);
                }
            }
            return hotfixes;
        }
    }
}
