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
                    Items = repository.GetPage(0, 100).ToArray()
                };
        }
    }
}