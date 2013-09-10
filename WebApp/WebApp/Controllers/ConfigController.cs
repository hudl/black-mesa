using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Queries;

namespace WebApp.Controllers
{
    public class ConfigController : Controller
    {
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
