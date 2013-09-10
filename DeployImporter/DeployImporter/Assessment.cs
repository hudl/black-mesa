using MongoDB.Bson.Serialization.Attributes;

namespace DeployImporter
{
    public class Assessment
    {
        [BsonElement("culpability")]
        public decimal? Culpability { get; set; }

        [BsonElement("hudlWideImpact")]
        public decimal? HudlWideImpact { get; set; }

        [BsonElement("affectedUserImpact")]
        public decimal? AffectedUserImpact { get; set; }

        [BsonElement("initials")]
        public string Initials { get; set; }
    }
}