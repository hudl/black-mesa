using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployImporter
{
    class HotfixDTO
    {
        public string ID { get; set; }
        public string BRANCH { get; set; }
        public string TICKET { get; set; }
        public string COMPONENT { get; set; }
        public string SPECIAL { get; set; }
        public string DEV_Team_Culpability { get; set; }
        public string DEV_Hudl_Wide_Impact { get; set; }
        public string DEV_Affected_User_Impact { get; set; }
        public string DEV_Initials { get; set; }
        public string QA_Team_Culpability { get; set; }
        public string QA_Hudl_Wide_Impact { get; set; }
        public string QA_Affected_User_Impact { get; set; }
        public string QA_Initials { get; set; }
        public string Breaker_Branch { get; set; }
        public string PROD_Ticket_Num { get; set; }
        public string PROD_Ticket { get; set; }
        public string Notes { get; set; }
    }
}
