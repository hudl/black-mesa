using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class History
    {
        [JsonProperty("person")]
        public string Person { get; set; }

        [JsonProperty("deployId")]
        public string DeployId { get; set; }

        [JsonProperty("dateTime")]
        public DateTime dateTime { get; set; }

        [JsonProperty("propertyChanged")]
        public string PropertyChanged { get; set; }

        [JsonProperty("oldValue")]
        public string OldValue { get; set; }

        [JsonProperty("newValue")]
        public string NewValue { get; set; }
    }
}