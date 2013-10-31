using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using MongoDB.Bson;
using WebApp.Attributes;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers.Api
{
    [CookieAuthenticated]
    [RoutePrefix("components")]
    public class ComponentController : BaseController
    {
        //
        // GET: /Component/

        [GET("/")]
        public ActionResult GetComponents()
        {
            return JsonNet(new ComponentRepository().GetAll());
        }

        [POST("/")]
        public ActionResult AddComponent(string component)
        {
            new ComponentRepository().Add(new Component(component));

            return new RedirectResult("/Components");
        }

        [DELETE("{id}")]
        public ActionResult Delete(string id)
        {
            var repo = new ComponentRepository();
            repo.Delete(new ObjectId(id));
            var dude = repo.FirstOrDefault(x => x.Id == id);

            return JsonNet(new {success = true});
        }

    }
}
