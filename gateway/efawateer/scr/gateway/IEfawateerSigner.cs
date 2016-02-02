using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Gateways
{
    public interface IEfawateerSigner
    {
        void CheckCerificate();
        string SignData(string body);
        bool VerifyData(string toString);
    }

    public class EfawateerSigner : IEfawateerSigner
    {
        private readonly string _certificateName;        

        public EfawateerSigner(string certificateName)
        {
            _certificateName = certificateName;
        }

        public void CheckCerificate()
        {
            var cert = GetCertificateFromStore(_certificateName);
            if (!cert.HasPrivateKey)
                throw new Exception("Отсутствует закрытый ключ в сертификате клиента");
        }

        private static X509Certificate2 GetCertificateFromStore(string certName)
        {            
            var store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);                
                var certCollection = store.Certificates;                
                var currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                var signingCert = currentCerts.Find(X509FindType.FindBySubjectName, certName, false);
                if (signingCert.Count == 0)
                    throw new Exception("Сертификат не найден");
                return signingCert[0];
            }
            finally
            {
                store.Close();                
            }
        }

        public string SignData(string body)
        {
            var certificate = GetCertificateFromStore(_certificateName);

            var key = new RSACryptoServiceProvider();
            key.FromXmlString(certificate.PrivateKey.ToXmlString(true));
                     
            var data = key.SignData(Encoding.Unicode.GetBytes(body), CryptoConfig.MapNameToOID("sha256"));
            return Convert.ToBase64String(data);
        }

        public bool VerifyData(string toString)
        {
            // todo verification
            return true;
        }
    }
}