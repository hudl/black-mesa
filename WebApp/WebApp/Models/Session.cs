using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [CollectionName("sessions")]
    public class Session : BlackMesaEntity
    {
        public string Username { get; set; }

        [BsonIgnoreIfDefault]
        public string DisplayName { get; set; }

        [BsonIgnoreIfDefault]
        public ICollection<string> Groups { get; set; }
    }
}