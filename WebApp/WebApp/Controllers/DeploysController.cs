using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DeploysController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Item(string id)
        {
            return View("Index");
        }
    }
}
