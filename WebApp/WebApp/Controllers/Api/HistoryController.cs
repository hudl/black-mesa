using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers.Api
{
    [RoutePrefix("history")]
    public class HistoryController : BaseController
    {
        [POST("/")]
        public ActionResult ChangedMade(History history)
        {
            var historyRepo = new HistoryRepository();
            historyRepo.Collection.Insert(history);
            return JsonNet(new { success = true });
        }

        [GET("/")]
        public ActionResult GetChanges()
        {
            var historyRepo = new HistoryRepository();
            return JsonNet(new Histories()
            {
                Items = historyRepo.GetRecentHistory(DateTime.UtcNow.AddMonths(-1))
            });
        }

        [GET("/all")]
        public ActionResult GetChanges()
        {
            var historyRepo = new HistoryRepository();
            return JsonNet(new Histories()
            {
                Items = historyRepo.GetAllHistory()
            });
        }
    }
}
