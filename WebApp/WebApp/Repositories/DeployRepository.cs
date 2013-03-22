using System.Collections.Generic;
using System.Linq;
using MongoRepository;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class DeployRepository : MongoRepository<Deploy>
    {
        public IEnumerable<Deploy> GetPage(int skip, int take)
        {
            return Collection.FindAll().Skip(skip).Take(take);
        }
    }
}