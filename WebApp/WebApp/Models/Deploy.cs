using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Newtonsoft.Json;

namespace WebApp.Models
{
    [CollectionName("prod")]
    public class Deploy : DeployDumpsterEntity
    {
        private static readonly TimeZoneInfo CentralTime = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        [BsonElement("action")]
        public string Action { get; set; }

        [BsonElement("branch")]
        public string Branch { get; set; }

        [BsonElement("component")]
        public string Component { get; set; }

        [BsonElement("deployTime")]
        public DateTime DeployTime { get; set; }

        [BsonElement("jiraLabel")]
        public string JiraLabel { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }

        [BsonElement("people")]
        public People People { get; set; }

        [BsonElement("project")]
        public string Project { get; set; }

        [BsonElement("pullRequestId")]
        public int PullRequestId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonIgnore]
        public string Day
        {
            get
            {
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DeployTime.ToUniversalTime(), CentralTime);
                return localTime.ToString("ddd");
            }
        }
    }
}