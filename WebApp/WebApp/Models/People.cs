using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models
{
    public class People
    {
        [BsonElement("qa")]
        public IEnumerable<string> QualityAnalysts { get; set; }

        [BsonElement("design")]
        public IEnumerable<string> Designers { get; set; }

        [BsonElement("dev")]
        public IEnumerable<string> Developers { get; set; }

        [BsonElement("codeReview")]
        public IEnumerable<string> CodeReviewers { get; set; }
    }
}