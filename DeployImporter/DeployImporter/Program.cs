using System.Globalization;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DeployImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Do you have the latest deploy_doc.csv and hotfix_doc.csv in the folder than this exe is in? Press enter when you do");
            Console.Read();
            var deploys = doDeployDoc();
            var hotfixes = doHotfixDoc();
            var finalDeploys = new List<Deploy>();
            foreach (var deploy in deploys)
            {
                finalDeploys.Add(new Deploy
                {
                    Action = deploy.ACTION.Trim(),
                    Branch = deploy.BRANCH.Trim(),
                    Component = deploy.COMPONENT.Trim(),
                    DeployTime = deploy.DATE_TIME.Trim().TryParseDateTime(),
                    LineNumber = deploy.ID,
                    Notes = deploy.NOTES,
                    Project = deploy.PROJECT.Trim(),
                    PullRequestId = deploy.PR.Trim().TryParseInt(),
                    Type = deploy.TYPE.Trim(),
                    People = new People
                        {
                            Designers = new List<string> { deploy.DES.Trim() },
                            Developers = new List<string> { deploy.DEV.Trim() },
                            CodeReviewers = new List<string> { deploy.DEVCR.Trim() },
                            ProjectManagers = new List<string> { deploy.PM.Trim() },
                            Quails = new List<string> { deploy.QA.Trim() },
                        },
                });
            }
            var i = 0;
            foreach (var hotfix in hotfixes)
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
                    deploy.Hotfixes.Add(new Hotfix
                        {
                            BranchThatBrokeIt = hotfix.Breaker_Branch.Trim(),
                            Notes = hotfix.Notes,
                            ProdTicket = hotfix.PROD_Ticket_Num.Trim().TryParseInt(),
                            Special = hotfix.SPECIAL,
                            Assessments = new Assessments
                                {
                                    Developers = new Assessment
                                        {
                                            Culpability = hotfix.DEV_Team_Culpability.Trim().TryParseDecimal(),
                                            HudlWideImpact = hotfix.DEV_Hudl_Wide_Impact.Trim().TryParseDecimal(),
                                            AffectedUserImpact = hotfix.DEV_Affected_User_Impact.Trim().TryParseDecimal(),
                                            Initials = hotfix.DEV_Initials.Trim(),
                                        },
                                    Quails = new Assessment
                                        {
                                            Culpability = hotfix.QA_Team_Culpability.Trim().TryParseDecimal(),
                                            HudlWideImpact = hotfix.QA_Hudl_Wide_Impact.Trim().TryParseDecimal(),
                                            AffectedUserImpact = hotfix.QA_Affected_User_Impact.Trim().TryParseDecimal(),
                                            Initials = hotfix.QA_Initials.Trim(),
                                        }
                                }
                        });
                }
            }
            Console.WriteLine(i + " unmatched hotfixes");
            var repo = new DeployRepository();
            i = 0;
            foreach (Deploy d in finalDeploys)
            {
                Console.WriteLine("Updating Entry " + ++i +" of " + finalDeploys.Count);
                repo.Upsert(d);
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
