using System.Drawing;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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

        [GET("export/alltime")]
        public void ExportAllTime()
        {
            var repo = new DeployRepository();
            var deploys = repo.Collection.FindAll().Where(x => !x.DateDeleted.HasValue).ToList();
            WriteTsv(deploys, "AllTimeDeploys.tsv");
        }

        [GET("export/lastmonth")]
        public void ExportLastMonth()
        {
            var now = DateTime.Now;
            var isJanuary = now.Month - 1 == 0;
            var month = isJanuary ? 12 : now.Month - 1;
            var year = isJanuary ? now.Year - 1 : now.Year;
            var repo = new DeployRepository();
            var deploys = repo.Collection.FindAll().Where(x => !x.DateDeleted.HasValue && x.DeployTime.HasValue && x.DeployTime.Value.Month == month && x.DeployTime.Value.Year == year).ToList();
            WriteTsv(deploys, String.Format("Deploys{0}{1}.tsv", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month), year));
        }

        [GET("export/currentmonth")]
        public void ExportCurrentMonth()
        {
            var now = DateTime.Now;
            var repo = new DeployRepository();
            var deploys = repo.Collection.FindAll().Where(x => !x.DateDeleted.HasValue && x.DeployTime.HasValue && x.DeployTime.Value.Month == now.Month && x.DeployTime.Value.Year == now.Year).ToList();
            WriteTsv(deploys, String.Format("CurrentDeploys{0}{1}.tsv", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(now.Month), now.Year));
        }

        public void WriteTsv(List<Deploy> deploys, String filename)
        {
            StringBuilder strBuilder = new StringBuilder();
            StringWriter str = new StringWriter(strBuilder);
            using (var csv = new CsvWriter(str))
            {
                csv.Configuration.Delimiter = "\t";
                csv.Configuration.IsCaseSensitive = false;
                csv.WriteRecords<DeployForExport>(DeployForExport.convertDeploysForExport(deploys));
            }
            Response.AddHeader("Content-disposition", String.Format("attachment; filename={0}", filename));
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
