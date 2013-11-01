using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using MongoRepository;

namespace WebApp.Repositories
{
    public class ProjectRepository : MongoRepository<Project>
    {
        public List<Project> GetAll()
        {
            return Collection.FindAll().ToList();
        }
    }
}