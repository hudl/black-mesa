using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.App_Start
{
    public class HttpClientConfig
    {
        private readonly AuthorizationConfig _authorizationConfiguration;
        private static HttpClientConfig _instance;

        private HttpClientConfig(AuthorizationConfig authorizationConfiguration)
        {
            _authorizationConfiguration = authorizationConfiguration;
        }

        public static void RegisterSslCertificates()
        {
            var configuration = new PrivateConfigRepository().GetPrivateConfig();
            _instance = new HttpClientConfig(configuration.Authorization);
            ServicePointManager.ServerCertificateValidationCallback = _instance.OnServerCertificateValidationCallback;
        }

        private bool OnServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            var request = sender as HttpWebRequest;

            if (request == null)
            {
                var message = String.Format(
                    "OnServerCertificateValidationCallback: expected 'sender' to be HttpWebRequest, but it was {0}", sender == null ? "null" : sender.GetType().Name);
                throw new ArgumentException(message, "sender");
            }

            // tyson.stewart 28 December 2012 - This needs to be expanded to handle multiple certificates if we use
            //   more than just one web service (via HTTPS)
            var certificateIsValid = request.RequestUri.Equals(_authorizationConfiguration.Uri)
                && String.Equals(_authorizationConfiguration.Certificate.Subject, certificate.Subject)
                && String.Equals(_authorizationConfiguration.Certificate.Serial, certificate.GetSerialNumberString());

            return certificateIsValid;
        }
    }
}