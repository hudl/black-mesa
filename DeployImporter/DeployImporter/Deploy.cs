using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;

namespace DeployImporter
{
    [CollectionName("deploys")]
    class Deploy : IEntity
    {
        public Deploy()
        {
            Hotfixes = new List<Hotfix>();
            People = new People();
        }

        public static class Fields
        {
            public const string LineNumber = "lineNumber";
            public const string DeployTime = "deployTime";
            public const string Action = "action";
            public const string Component = "component";
            public const string Type = "type";
            public const string Project = "project";
            public const string Branch = "branch";
            public const string PullRequestId = "pullRequestId";
            public const string People = "people";
            public const string Notes = "notes";
            public const string Hotfixes = "hotfixes";
        }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string Id { get; set; }

        [BsonElement(Fields.LineNumber)]
        public string LineNumber { get; set; }

        [BsonElement(Fields.DeployTime)]
        public DateTime? DeployTime { get; set; }

        [BsonElement(Fields.Action)]
        public string Action { get; set; }

        [BsonElement(Fields.Component)]
        public string Component { get; set; }

        [BsonElement(Fields.Type)]
        public string Type { get; set; }

        [BsonElement(Fields.Project)]
        public string Project { get; set; }

        [BsonElement(Fields.Branch)]
        public string Branch { get; set; }

        [BsonElement(Fields.PullRequestId)]
        public int? PullRequestId { get; set; }

        [BsonElement(Fields.People)]
        public People People { get; set; }

        [BsonElement(Fields.Notes)]
        public string Notes { get; set; }

        [BsonElement(Fields.Hotfixes)]
        public List<Hotfix> Hotfixes { get; set; }
    }
}
