using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Attributes;
using WebApp.Controllers.Api;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [CookieAuthenticated]
    public class ComponentsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Components = new ComponentRepository().GetAll();
            return View();
        }

    }
}
