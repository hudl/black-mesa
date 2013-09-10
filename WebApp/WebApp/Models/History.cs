using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [CollectionName("history")]
    [BsonIgnoreExtraElements]
    public class History : BlackMesaEntity
    {
        [BsonElement("person")]
        public string Person { get; set; }

        [BsonElement("deployId")]
        public string DeployId { get; set; }

        [BsonElement("dateTime")]
        public DateTime dateTime { get; set; }

        [BsonElement("propertyChanged")]
        public string PropertyChanged { get; set; }

        [BsonElement("oldValue")]
        public string OldValue { get; set; }

        [BsonElement("newValue")]
        public string NewValue { get; set; }
    }
}