using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using EfawateerGateway;
using EfawateerGateway.Proxy.Domain;

namespace Gateways
{
    public interface IEfawateerSigner
    {
        void CheckCerificate();
        string SignData(MsgBody body);
        string SignData(string body);
        bool VerifyData(string toString);
    }

    public class EfawateerSigner : IEfawateerSigner
    {
        private readonly string _certificate;
        private readonly string _password;
        private readonly ISerializer _serializer;

        public EfawateerSigner(string certificate, string password, ISerializer serializer)
        {
            _certificate = certificate;
            _password = password;
            _serializer = serializer;
        }

        public void CheckCerificate()
        {
            var cert = GetCertificateFromStore(_certificate);
            if (!cert.HasPrivateKey)
                throw new Exception("Отсутствует закрытый ключ в сертификате клиента");
        }

        private X509Certificate2 GetCertificateFromStore(string certName)
        {

            // Get the certificate store for the current user.
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                // Place all certificates in an X509Certificate2Collection object.
                X509Certificate2Collection certCollection = store.Certificates;
                // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
                // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
                var currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                var signingCert = currentCerts.Find(X509FindType.FindBySubjectName, certName, false);
                if (signingCert.Count == 0)
                    throw new Exception("Сертификат не найден");
                // Return the first certificate in the collection, has the right name and is current.
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }

        public string SignData(MsgBody body)
        {
            var e = XElement.Parse(_serializer.Serialize(body)).ToString();
            return SignData(e);
        }

        public string SignData(string body)
        {
            var certificate = GetCertificateFromStore(_certificate);

            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(certificate.PrivateKey.ToXmlString(true));
         
            // todo algorithm ?
            var data = key.SignData(Encoding.Unicode.GetBytes(body), CryptoConfig.MapNameToOID("sha256"));
            return Convert.ToBase64String(data);
        }

        public bool VerifyData(string toString)
        {
            return true;
        }
    }
}