using System;
using System.Net;
using TestGrill.Infrastructure.GrillServiceOData;

namespace TestGrill.Infrastructure
{
    public class ODataClient : IODataClient
    {
        public GrillMenuContext Service { get; set; }

        public ODataClient(string oDataUri)
        {
            var serviceUri = new Uri(oDataUri);
            var serviceCreds = new NetworkCredential("jobs@isolutions.ch", "cleancode");
            var cache = new CredentialCache { { serviceUri, "Basic", serviceCreds } };
            Service = new GrillMenuContext(serviceUri)
            {
                Credentials = cache
            }; 
        }
    }
}