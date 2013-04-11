using System;
using System.Linq;
using System.Web.Http;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers.Api
{
    public class DeploysController : ApiController
    {
        private const int MaxReturnSize = 100;
        public Deploys Get(int returnSize = MaxReturnSize)
        {
            var repository = new DeployRepository();
            return new Deploys
                {
                    Items = repository.GetSince(DateTime.UtcNow.AddMonths(-12), returnSize).ToArray()
                };
        }

        public void Post(Deploy deploy)
        {
            var repository = new DeployRepository();
            repository.Update(deploy);
        }
    }
}