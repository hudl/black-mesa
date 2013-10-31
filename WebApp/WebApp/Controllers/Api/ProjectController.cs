using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using MongoDB.Bson;
using WebApp.Attributes;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers.Api
{
    [CookieAuthenticated]
    [RoutePrefix("projects")]
    public class ProjectController : BaseController
    {
        [GET("/")]
        public ActionResult GetProjects()
        {
            return JsonNet(new ProjectRepository().GetAll());
        }

        [POST("/")]
        public ActionResult AddProject(string project)
        {
            new ProjectRepository().Add(new Project(project));

            return new RedirectResult("/Projects");
        }

        [DELETE("{id}")]
        public ActionResult Delete(string id)
        {
            var repo = new ProjectRepository();
            repo.Delete(new ObjectId(id));

            return JsonNet(new {success = true});
        }
    }
}
