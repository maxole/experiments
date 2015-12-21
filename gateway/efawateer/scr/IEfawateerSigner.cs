using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Gateways
{
    public interface IEfawateerSigner
    {
        void CheckCerificate();
        string SignData(string toString);
        bool VerifyData(string toString);
    }

    public class EfawateerSigner : IEfawateerSigner
    {
        private readonly string _certificate;
        private readonly string _password;

        public EfawateerSigner(string certificate, string password)
        {
            _certificate = certificate;
            _password = password;
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
                var signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
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

        public string SignData(string toString)
        {
            var certificate = GetCertificateFromStore(_certificate);
            var key = (RSACryptoServiceProvider) certificate.PrivateKey;
            var data = key.SignData(Encoding.Unicode.GetBytes(toString), CryptoConfig.MapNameToOID("SHA256"));
            return Convert.ToBase64String(data);
        }

        public bool VerifyData(string toString)
        {
            return true;
        }
    }
}