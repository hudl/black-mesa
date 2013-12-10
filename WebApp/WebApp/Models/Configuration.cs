using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApp.Models
{
    [BsonIgnoreExtraElements]
    public class Configuration : IEntity
    {
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }

        [JsonProperty("jiraSearchByLabelBaseUrl")]
        public string JiraSearchByLabelBaseUrl { get; set; }

        [JsonProperty("githubBaseUrl")]
        public string GithubBaseUrl { get; set; }

        [JsonProperty("gitHubPullRequestBaseUrl")]
        public string GitHubPullRequestBaseUrl { get; set; }

        [JsonProperty("hotfixThread")]
        public string HotfixThread { get; set; }

        [JsonProperty("loginServer")]
        public string LoginServer { get; set; }

        [JsonProperty("basecampThreads")]
        public ICollection<BasecampThread> BasecampThreads { get; set; }
    }
}