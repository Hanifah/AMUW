using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMUW.Helpers
{
    public class CertificateAuthenticationHelper
    {
        public static SubscriptionCloudCredentials GetCredential(
            string subscriptionId,
            string base64EncodedCert)
        {
            return new CertificateCloudCredentials(subscriptionId, new System.Security.Cryptography.X509Certificates.X509Certificate2(Convert.FromBase64String(base64EncodedCert)));
        }
    }
}