using System;
using System.Linq;
using System.Web.Http;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers.Api
{
    public class DeploysController : ApiController
    {
        public Deploys Get()
        {
            var repository = new DeployRepository();
            return new Deploys
                {
                    Items = repository.GetSince(DateTime.UtcNow.AddMonths(-2)).ToArray()
                };
        }

        public void Post(Deploy deploy)
        {
            var repository = new DeployRepository();
            repository.Update(deploy);
        }
    }
}