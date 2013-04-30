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
    }
}