using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Attributes;

namespace WebApp.Controllers.Api
{
    [CookieAuthenticated]
    [RoutePrefix("people")]
    public class PeopleController : BaseController
    {
        [GET("/")]
        public ActionResult GithubUsernames()
        {
            //TODO: Redis cache for 10 minutes so we don't keep pinging the AD server.
            return JsonNet(GetPageSource(PrivateConfig.AdServer, null), true);
        }
    }
}
