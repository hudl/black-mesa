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
            var components = new ComponentRepository().GetAll();
            ViewBag.Components = components;
            return View();
        }

        public ActionResult Item(string id)
        {
            return View("Index");
        }
    }
}
