using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoRepository;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace DeployExporter
{
    class DeployRepository : MongoRepository.MongoRepository<Deploy>
    {
    }
}
