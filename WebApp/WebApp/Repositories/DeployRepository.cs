using System;
using System.Collections.Generic;
using MongoDB.Driver.Builders;
using MongoRepository;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class DeployRepository : MongoRepository<Deploy>
    {
        public IEnumerable<Deploy> GetSince(DateTime deployTime, int returnSize)
        {
            return Collection
                .Find(Query<Deploy>.Where(d => d.DeployTime > deployTime))
                .SetLimit(returnSize);
        }
    }
}