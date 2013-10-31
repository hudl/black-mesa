using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using ServiceStack.Text;
using WebApp.Attributes;
using WebApp.Models;
using WebApp.Repositories;
using log4net;

namespace WebApp.Controllers.Api
{
    [CookieAuthenticated]
    [RoutePrefix("deploys")]
    public class DeployController : BaseController
    {
        private const int MaxReturnSize = 100;
        private static readonly ILog log = LogManager.GetLogger(typeof (DeployController));

        [GET("/")]
        public ActionResult Get()
        {
            var repository = new DeployRepository();
            return JsonNet(new Deploys
            {
                Items = repository.GetSince(DateTime.UtcNow.AddMonths(-2), MaxReturnSize).Where(x => x.DateDeleted == null).ToArray()
            });
        }

        [GET("all")]
        public ActionResult GetAll()
        {
            var repository = new DeployRepository();
            return JsonNet(new Deploys
            {
                Items = repository.GetSince(DateTime.UtcNow.AddYears(-1000), Int32.MaxValue).ToArray()
            });
        }

        [POST("/")]
        public ActionResult Post(NewDeploy deploy)
        {
            ConventNewDeployToDeploy(deploy);
            var repository = new DeployRepository();
            repository.Update(deploy);
            return JsonNet(new { success = true });
        }

        [POST("new")]
        public ActionResult NewDeploy(NewDeploy deploy)
        {
            ConventNewDeployToDeploy(deploy);
            deploy.DeployTime = DateTime.Now;
            var repository = new DeployRepository();
            repository.Add(deploy);
            return JsonNet(new { success = true });
        }

        [DELETE("{id}")]
        public ActionResult Delete(string id)
        {
            
            var repository = new DeployRepository();
            var first = repository.FirstOrDefault(x => x.Id == id);
            if (first != null)
            {
                log.InfoFormat("Deleting {0}\n{1}", first.Id, first.ToJson());
                first.DateDeleted = DateTime.UtcNow;
                repository.Update(first);
            }

            return JsonNet(new { success = true });
        }



        private void ConventNewDeployToDeploy(NewDeploy deploy)
        {
            deploy.People = new People
            {
                CodeReviewers = deploy.CodeReview != null ? deploy.CodeReview.Split(',') : new string[0],
                Designers = deploy.Design != null ? deploy.Design.Split(',') : new string[0],
                Developers = deploy.Dev != null ? deploy.Dev.Split(',') : new string[0],
                ProjectManagers = deploy.ProjectManager != null ? deploy.ProjectManager.Split(',') : new string[0],
                Quails = deploy.Qa != null ? deploy.Qa.Split(',') : new string[0],
            };
            if (deploy.Type != null && deploy.Type.Equals("Hotfix", StringComparison.OrdinalIgnoreCase))
            {
                deploy.Hotfixes = new List<Hotfix>(1);
                deploy.Hotfixes.Add(new Hotfix()
                {
                    BranchThatBrokeIt = deploy.BadBranch,
                    ProdTicket = deploy.ProdTicket,
                    Ticket = deploy.Ticket,
                    Assessments = new Assessments()
                    {
                        Quails = new Assessment()
                        {
                            AffectedUserImpact = deploy.QaUserImpact,
                            Culpability = deploy.QaTeamCulp,
                            HudlWideImpact = deploy.QaHudlImpact,
                            Initials = deploy.QaInitials
                        },
                        Developers = new Assessment()
                        {
                            AffectedUserImpact = deploy.DevUserImpact,
                            Culpability = deploy.DevTeamCulp,
                            HudlWideImpact = deploy.DevHudlImpact,
                            Initials = deploy.DevInitials
                        }
                    },
                    Notes = deploy.HotfixNotes,
                    Special = deploy.Special,
                    HotfixComponent = deploy.HotfixComponent,
                    TheProblem = deploy.TheProblem,
                    TheFix = deploy.TheFix,
                    HowMissed = deploy.HowMissed,
                });
            }
        }
    }
}