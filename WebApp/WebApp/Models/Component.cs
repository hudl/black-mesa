using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace WebApp.Models
{
    [CollectionName("components")]
    [BsonIgnoreExtraElements]
    public class Component : BlackMesaEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        public Component(string name)
        {
            Name = name;
        }
    }
}
