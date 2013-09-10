using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class BasecampConfig : AccountConfig
    {
        public string PostUrl { get; set; }
        public string GetAccessesUrl { get; set; }
    }
}