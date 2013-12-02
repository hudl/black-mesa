using System.Web.Mvc;
using MongoDB.Driver;
using WebApp.Attributes;
using WebApp.Controllers.Api;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [CookieAuthenticated]
    public class DeploysController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Components = new ComponentRepository().GetAll();;
            ViewBag.Projects = new ProjectRepository().GetAll();
            return View();
        }

        public ActionResult Item(string id)
        {
            ViewBag.Components = new ComponentRepository().GetAll();
            ViewBag.Projects = new ProjectRepository().GetAll();
            return View("Index");
        }
    }
}
