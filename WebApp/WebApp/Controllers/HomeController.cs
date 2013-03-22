using System.Web.Mvc;
using Newtonsoft.Json;
using WebApp.Queries;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoute(new {controller = "Deploys", action = "Index"});
        }

        public ContentResult Config()
        {
            var getConfigurationQuery = new GetConfigurationQuery();
            var configuration = getConfigurationQuery.Get();
            return new ContentResult
                {
                    Content = @"(function(blackmesa){blackmesa.config=" + JsonConvert.SerializeObject(configuration) + @";})(window.BlackMesa);",
                    ContentType = "text/javascript"
                };
        }
    }
}
