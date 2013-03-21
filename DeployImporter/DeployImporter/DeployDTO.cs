using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployImporter
{
    class DeployDTO
    {
        public string ID { get; set; }
        public string Day { get; set; }
        public string DATE_TIME { get; set; }
        public string ACTION { get; set; }
        public string COMPONENT { get; set; }
        public string TYPE { get; set; }
        public string PROJECT { get; set; }
        public string BRANCH { get; set; }
        public string PR { get; set; }
        public string PRL { get; set; }
        public string JIRA { get; set; }
        public string QA { get; set; }
        public string DES { get; set; }
        public string DEV { get; set; }
        public string DEVCR { get; set; }
        public string PM { get; set; }
        public string NOTES { get; set; }
    }
}
