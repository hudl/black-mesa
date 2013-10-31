using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace WebApp.Models
{
    [CollectionName("projects")]
    [BsonIgnoreExtraElements]
    public class Project : BlackMesaEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        public Project(string name)
        {
            Name = name;
        }
    }
}