using MongoDB.Bson;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Repositories;

namespace WebApp.Attributes
{
    public class CookieAuthenticatedAttribute : AuthorizeAttribute
    {
        public const string CookieName = "blackmesa-auth";
        private static readonly ILog Log = LogManager.GetLogger(typeof(CookieAuthenticatedAttribute).Name);

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authCookie = httpContext.Request.Cookies[CookieName];
            var authorized = authCookie != null && !String.IsNullOrWhiteSpace(authCookie.Value) && IsValid(authCookie.Value);
            return authorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Redirect;
            filterContext.HttpContext.Response.Redirect("/Login", true);
        }

        private bool IsValid(string authCookieValue)
        {
            var objectId = new ObjectId(authCookieValue);
            return new SessionRepository().GetById(objectId) != null;
        }
    }
}