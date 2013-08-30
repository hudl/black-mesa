using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models
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

        [BsonElement("hotfixComponent")]
        public string HotfixComponent { get; set; }

        [BsonElement("notes")]
        public string Notes { get; set; }

        [BsonElement("ticket")]
        public string Ticket { get; set; }
    }
}