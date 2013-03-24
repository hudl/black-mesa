using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class Configuration : IEntity
    {
        [JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }

        [JsonProperty("jiraSearchByLabelBaseUrl")]
        public string JiraSearchByLabelBaseUrl { get; set; }

        [JsonProperty("gitHubPullRequestBaseUrl")]
        public string GitHubPullRequestBaseUrl { get; set; }
    }
}