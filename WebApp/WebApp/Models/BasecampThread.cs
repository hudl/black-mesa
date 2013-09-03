using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class BasecampThread
    {
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("threadId")]
        public string ThreadId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}