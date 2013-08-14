using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class NewDeploy : Deploy
    {
        public string Qa { get; set; }
        public string Dev { get; set; }
        public string Design { get; set; }
        public string CodeReview { get; set; }
        public string ProjectManager { get; set; }
        public int? ProdTicket { get; set; }
        public string Ticket { get; set; }
        public int? QaTeamCulp { get; set; }
        public int? QaHudlImpact { get; set; }
        public int? QaUserImpact { get; set; }
        public string QaInitials { get; set; }
        public int? DevTeamCulp { get; set; }
        public int? DevHudlImpact { get; set; }
        public int? DevUserImpact { get; set; }
        public string DevInitials { get; set; }
        public string BadBranch { get; set; }
        public string HotfixNotes { get; set; }
        public string Special { get; set; }
    }
}