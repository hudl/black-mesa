using MongoRepository;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class ConfigurationRepository : MongoRepository<Configuration>
    {
        public Configuration GetConfiguration()
        {
            return Collection.FindOne();
        }
    }
}