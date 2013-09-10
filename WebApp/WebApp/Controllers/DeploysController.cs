using System.Web.Mvc;
using WebApp.Attributes;
using WebApp.Controllers.Api;

namespace WebApp.Controllers
{
    [CookieAuthenticated]
    public class DeploysController : BaseController
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
