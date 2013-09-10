using MongoDB.Driver.Builders;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class SessionRepository : MongoRepository<Session>
    {
    }
}