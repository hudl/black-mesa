using MongoRepository;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

namespace DeployImporter
{
    class DeployRepository : MongoRepository<Deploy>
    {
        public DeployRepository()
        {
            Collection.EnsureIndex(Deploy.Fields.LineNumber);
        }

        public void Upsert(Deploy deploy)
        {
            deploy.Id = null;
            Collection.Update(Query.EQ(Deploy.Fields.LineNumber, deploy.LineNumber), MongoDB.Driver.Builders.Update.Replace(deploy), UpdateFlags.Upsert);
        }
    }
}
