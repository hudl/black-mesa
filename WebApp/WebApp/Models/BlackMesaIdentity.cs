using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebApp.Models
{
    public class BlackMesaIdentity : IIdentity
    {
        public BlackMesaIdentity(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }

        public string Name { get; set; }
        public string Username { get; set; }
        public string AuthenticationType { get { return String.Empty; } }
        public bool IsAuthenticated { get; private set; }
        public bool IsReadOnly { get; set; }
    }
}