using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Attributes;
using WebApp.Controllers.Api;

namespace WebApp.Controllers
{
    [CookieAuthenticated]
    public class HistoryController : BaseController
    {
        //
        // GET: /History/

        public ActionResult Index()
        {
            return View();
        }

    }
}
