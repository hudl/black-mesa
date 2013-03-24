using MongoDB.Bson.Serialization.Attributes;

namespace DeployImporter
{
    public class Hotfix
    {
        public Hotfix()
        {
            Assessments = new Assessments();
        }

        [BsonElement("special")]
        public string Special { get; set; }

        [BsonElement("assessments")]
        public Assessments Assessments { get; set; }

        [BsonElement("breakingBranch")]
        public string BranchThatBrokeIt { get; set; }

        [BsonElement("prodTicket")]
        public int? ProdTicket { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }
    }
}