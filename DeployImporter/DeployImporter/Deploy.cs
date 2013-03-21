using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployImporter
{
    [CollectionName("deploys")]
    class Deploy : IEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public string Id { get; set; }
        public string LineNumber { get; set; }
        public string Day { get; set; }
        public string DateTime { get; set; }
        public string Action { get; set; }
        public string Component { get; set; }
        public string Type { get; set; }
        public string Project { get; set; }
        public string Branch { get; set; }
        public string PullRequest { get; set; }
        public string PullRequestLink{ get; set; }
        public string Jira { get; set; }
        public string QA { get; set; }
        public string Des { get; set; }
        public string Dev { get; set; }
        public string DevCodeReview { get; set; }
        public string ProjectManager { get; set; }
        public string Notes { get; set; }

        public bool IsHotfix { get; set; }

        public string Special { get; set; }
        public string DevTeamCulpability { get; set; }
        public string DevHudlWideImpact { get; set; }
        public string DevAffectedUserImpact { get; set; }
        public string DevInitials { get; set; }
        public string QaTeamCulpability { get; set; }
        public string QaHudlWideImpact { get; set; }
        public string QaAffectedUserImpact { get; set; }
        public string QaInitials { get; set; }
        public string BranchThatBrokeIt { get; set; }
        public string ProdTicketNum { get; set; }
        public string ProdTicket { get; set; }
        public string HfNotes { get; set; }
    }
}
