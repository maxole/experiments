using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using EfawateerGateway;
using EfawateerGateway.Proxy.Domain;

namespace EfawateerWcf
{
    /*
     навалено все в кучу для проверки работоспособности сервисов
     */
    public class Class1
    {
        public void Foo3()
        {
            var _overriders = new XmlParameterOverriders(new Dictionary<string, XmlSerializer>
            {
                {
                    typeof (BillPaymentRequest).Name,
                    new XmlSerializer(typeof (BillPaymentRequest), new XmlRootAttribute("MFEP"))
                },
                {typeof (RequestResult).Name, new XmlSerializer(typeof (RequestResult), new XmlRootAttribute("MFEP"))}
            });

            var s = new Serializer(_overriders);

            var pf = new X509Certificate2(@"d:\private\Ykhanov\ARKAIM-TEST with private.pfx", "12345678");
            var key = (RSACryptoServiceProvider) pf.PrivateKey;
            key.KeySize = 2048;

            var binding = new WSHttpBinding(SecurityMode.None, true);
            var endpoint = new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc");
            var t = new Token.TokenServiceClient(binding, endpoint);
            var element = t.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");

            var r = s.Deserialize<RequestResult>(element);

            var p = new PrepaidValidation.PrepaidValidationClient(new WSHttpBinding(SecurityMode.None, true),
                new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6018/PrepaidValidation.svc"));
            var validate = p.Validate(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, new XElement("MFEP"));
        }

        public void Foo()
        {

            var _overriders = new XmlParameterOverriders(new Dictionary<string, XmlSerializer>
            {
                {typeof (BillPaymentRequest).Name, new XmlSerializer(typeof (BillPaymentRequest), new XmlRootAttribute("MFEP"))},
                {typeof (RequestResult).Name, new XmlSerializer(typeof (RequestResult), new XmlRootAttribute("MFEP"))}
            });

            var s = new Serializer(_overriders);

            var pf = new X509Certificate2(@"d:\private\Ykhanov\ARKAIM-TEST with private.pfx", "12345678");
            var key = (RSACryptoServiceProvider) pf.PrivateKey;
            key.KeySize = 2048;

            var binding = new WSHttpBinding(SecurityMode.None, true);
            var endpoint = new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc");
            var t = new Token.TokenServiceClient(binding, endpoint);
            var element = t.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");

            var r = s.Deserialize<RequestResult>(element);


            var text = File.ReadAllText("Bill.xml");
            var pay = XDocument.Parse(text).Element("MFEP");

            var p = CryptoConfig.CreateFromName("SHA1");
            var data = key.SignData(Encoding.Unicode.GetBytes(pay.Element("MsgBody").ToString()), p);
            var buffer = Convert.ToBase64String(data);

            pay.Element("MsgFooter").Element("Security").Element("Signature").Value = buffer;

            //var p = new PrepaidPayment.PrepaidPaymentClient("WSHttpBinding_IPrepaidPayment");
            //var xElement = p.Pay(Guid.NewGuid().ToString(), token, pay);

            var bill = new BillPayment.PaymentClient("WSHttpBinding_IPayment");
            var x = bill.PayBill(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, pay);

            var result = s.Deserialize<RequestResult>(x);
        }

        public void Foo2()
        {
            var p = new BillPaymentRequest
            {
                MsgHeader =
                {
                    TmStp = DateTime.Now.ToString("s"),
                    TrsInf = {RcvCode = 4, ReqTyp = "42", SdrCode = 14}
                },
            };
            p.MsgFooter.Security.Signature = "signature";

            var serializer = new XmlSerializer(typeof(BillPaymentRequest), new XmlRootAttribute("MFEP"));
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);

            var w = XmlWriter.Create(buffer, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            });

            var ns = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName(string.Empty, string.Empty)
            });

            serializer.Serialize(w, p, ns);
        }


    }
}
