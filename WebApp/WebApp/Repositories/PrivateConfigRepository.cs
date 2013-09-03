using MongoRepository;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class PrivateConfigRepository : MongoRepository<PrivateConfig>
    {
        public PrivateConfig GetPrivateConfig()
        {
            return Collection.FindOne();
        }
    }
}