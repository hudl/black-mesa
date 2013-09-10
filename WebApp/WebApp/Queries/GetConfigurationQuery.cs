using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Queries
{
    public class GetConfigurationQuery
    {
        public Configuration Get()
        {
            var repository = new ConfigurationRepository();
            return repository.GetConfiguration();
        }
    }
}