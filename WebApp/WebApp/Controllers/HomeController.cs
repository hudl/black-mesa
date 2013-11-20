using System.Linq.Expressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebApp.Queries;
using WebApp.Repositories;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;
using AttributeRouting.Web.Mvc;
using WebApp.Controllers.Api;
using WebApp.Attributes;
using MongoDB.Bson;


namespace WebApp.Controllers
{
    [CookieAuthenticated]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToRoute(new {controller = "Deploys", action = "Index"});
        }

        [GET("export")]
        public void DataExport()
        {
            var repo = new DeployRepository();
            var deploys = repo.Collection.FindAll().Where(x => !x.DateDeleted.HasValue).ToList();
            StringBuilder strBuilder = new StringBuilder();
            StringWriter str = new StringWriter(strBuilder);
            using (var csv = new CsvWriter(str))
            {
                csv.Configuration.Delimiter = "\t";
                csv.Configuration.IsCaseSensitive = false;
                csv.WriteRecords<DeployForExport>(DeployForExport.convertDeploysForExport(deploys));
            }
            Response.AddHeader("Content-disposition", "attachment; filename=deployData.tsv");
            Response.ContentType = "application/octet-stream";
            Response.Write(str.ToString());
            Response.End();
        }

        public ContentResult Config()
        {

            if (HttpContext.Request.Cookies[CookieAuthenticatedAttribute.CookieName] == null)
            {
                return null;
            }

            var authCookie = HttpContext.Request.Cookies[CookieAuthenticatedAttribute.CookieName].Value;
            var displayName = new SessionRepository().GetById(new ObjectId(authCookie)).DisplayName;
            return new ContentResult
            {
                Content = @"(function(blackmesa){blackmesa.username='" + displayName + @"';})(window.BlackMesa);",
                ContentType = "text/javascript"
            };
        }
    }
}
