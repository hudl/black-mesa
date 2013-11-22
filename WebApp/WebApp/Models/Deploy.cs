using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System.Collections.Generic;

namespace WebApp.Models
{
    [CollectionName("deploys")]
    [BsonIgnoreExtraElements]
    public class Deploy : BlackMesaEntity
    {
        private static readonly TimeZoneInfo CentralTime = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        [BsonElement("action")]
        public string Action { get; set; }

        [BsonElement("branch")]
        public string Branch { get; set; }

        [BsonElement("component")]
        public string Component { get; set; }

        [BsonElement("dateDeleted")]
        public DateTime? DateDeleted { get; set; }

        [BsonElement("deployTime")]
        public DateTime? DeployTime { get; set; }

        [BsonElement("jiraLabel")]
        public string JiraLabel { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }

        [BsonElement("people")]
        public People People { get; set; }

        [BsonElement("project")]
        public string Project { get; set; }

        [BsonElement("pullRequestId")]
        public int? PullRequestId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("hotfixes")]
        public List<Hotfix> Hotfixes { get; set; }

        [BsonElement("repository")]
        public String Repository { get; set; }

        [BsonIgnore]
        public string Day
        {
            get
            {
                if (!DeployTime.HasValue)
                {
                    return String.Empty;
                }

                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DeployTime.Value.ToUniversalTime(), CentralTime);
                return localTime.ToString("ddd");
            }
        }
    }
}