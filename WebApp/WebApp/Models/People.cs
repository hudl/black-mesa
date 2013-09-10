using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models
{
    public class People
    {
        [BsonElement("quails")]
        public IEnumerable<string> Quails { get; set; }

        [BsonElement("designers")]
        public IEnumerable<string> Designers { get; set; }

        [BsonElement("developers")]
        public IEnumerable<string> Developers { get; set; }

        [BsonElement("codeReviewers")]
        public IEnumerable<string> CodeReviewers { get; set; }

        [BsonElement("projectManagers")]
        public IEnumerable<string> ProjectManagers { get; set; }
    }
}