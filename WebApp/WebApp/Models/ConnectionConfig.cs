using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ConnectionConfig
    {
        public string Host { get; set; }
        public string Path { get; set; }
        public int Port { get; set; }
        public string GorupName { get; set; }

        public Certificate Certificate { get; set; }

        [BsonIgnore]
        public Uri Uri
        {
            get
            {
                var uriBuilder = new UriBuilder(Uri.UriSchemeHttps, Host, Port, Path);
                return uriBuilder.Uri;
            }
        }
    }
}