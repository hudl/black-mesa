using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    public class ProjectsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Projects = new ProjectRepository().GetAll();
            return View();
        }

    }
}
