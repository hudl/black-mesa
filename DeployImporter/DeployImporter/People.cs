using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace DeployImporter
{
    public class People
    {
        public People()
        {
            Quails = new List<string>();
            Developers = new List<string>();
            Designers = new List<string>();
            CodeReviewers = new List<string>();
            ProjectManagers = new List<string>();
        }

        [BsonElement("quails")]
        public List<string> Quails { get; set; }

        [BsonElement("developers")]
        public List<string> Developers { get; set; } 

        [BsonElement("designers")]
        public List<string> Designers { get; set; }

        [BsonElement("codeReviewers")]
        public List<string> CodeReviewers { get; set; }

        [BsonElement("projectManagers")]
        public List<string> ProjectManagers { get; set; }
    }
}