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
            history.dateTime = DateTime.Now;
            historyRepo.Collection.Insert(history);
            return JsonNet(new { success = true });
        }

        [GET("/")]
        public ActionResult GetRecentHistory()
        {
            var historyRepo = new HistoryRepository();
            return JsonNet(new Histories()
            {
                Items = historyRepo.GetRecentHistory(DateTime.UtcNow.AddDays(-7))
            });
        }

        [GET("all")]
        public ActionResult GetAllHistory()
        {
            var historyRepo = new HistoryRepository();
            return JsonNet(new Histories()
            {
                Items = historyRepo.GetAllHistory()
            });
        }
    }
}
