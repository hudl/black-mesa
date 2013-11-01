using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Builders;
using WebApp.Models;
using MongoRepository;

namespace WebApp.Repositories
{
    public class ComponentRepository : MongoRepository<Component>
    {
        public List<Component> GetAll()
        {
            return Collection.FindAll().ToList();
        }
    }
}