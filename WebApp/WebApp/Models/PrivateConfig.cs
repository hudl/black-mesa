﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PrivateConfig : IEntity
    {
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }

        public GithubConfig GithubConfig { get; set; }
        public BasecampConfig BasecampConfig { get; set; }
        public JiraConfig JiraConfig { get; set; }
        public string UserAgent { get; set; }
        public ConnectionConfig Authorization { get; set; }
        public ConnectionConfig ActiveDirectory { get; set; }
    }
}