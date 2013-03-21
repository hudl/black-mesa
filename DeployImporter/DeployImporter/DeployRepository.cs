using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoRepository;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace DeployImporter
{
    class DeployRepository : MongoRepository.MongoRepository<Deploy>
    {

        public void UpdateSomething(Deploy deploy)
        {
            //if (!BsonClassMap.IsClassMapRegistered(typeof(Deploy)))
            //{
            //    BsonClassMap.RegisterClassMap<Deploy>(m => {
            //        m.AutoMap();
            //        m.GetMemberMap(x => x.Id).SetIgnoreIfNull(true);
            //    });
            //}
            deploy.Id = null;
            this.Collection.Update(Query.EQ("LineNumber", deploy.LineNumber), MongoDB.Driver.Builders.Update.Replace(deploy), UpdateFlags.Upsert);
        }
    }
}
