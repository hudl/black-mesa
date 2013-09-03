using MongoDB.Driver.Builders;
using MongoRepository;
using System;
using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class HistoryRepository : MongoRepository<History>
    {
        public IEnumerable<History> GetRecentHistory(DateTime dateTime)
        {
            return Collection.Find(Query<History>.Where(d => d.dateTime > dateTime))
                .SetSortOrder(SortBy.Descending("dateTime"));
        }
        public IEnumerable<History> GetAllHistory()
        {
            return Collection.FindAll();
        }
    }
}