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
using EfawateerWcf.AccountInquiry;
using EfawateerWcf.AccountUpload;
using EfawateerWcf.AddCustomerBilling;
using EfawateerWcf.BillersList;
using EfawateerWcf.BillInquiry;
using EfawateerWcf.PaymentInquiry;
using EfawateerWcf.PrepaidPayment;
using EfawateerWcf.PrepaidValidation;
using Gateways;

namespace EfawateerWcf
{
    /*
     навалено все в кучу для проверки работоспособности сервисов
     */
    public class Class1
    {

        public void PrepaidValidation()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("PrepaidValidation.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            var time = DateTime.Now.ToString("s");
            var guid = Guid.NewGuid().ToString();
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgHeader").Element("GUID").Value = guid;
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new PrepaidValidationClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6018/PrepaidValidation.svc"));
            var upload = client.Validate(guid, r.MsgBody.TokenConf.TokenKey, request);
        }

        public void PrepaidPayment()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("PrepaidPayment.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            var time = DateTime.Now.ToString("s");
            var guid = Guid.NewGuid().ToString();
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgHeader").Element("GUID").Value = guid;
            request.Element("MsgBody").Element("TrxInf").Element("ProcessDate").Value = time;
            request.Element("MsgBody").Element("TrxInf").Element("BankTrxID").Value = guid;
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new PrepaidPaymentClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6017/PrepaidPayment.svc"));
            var upload = client.Pay(guid, r.MsgBody.TokenConf.TokenKey, request);
        }

        public void PaymentInquiry()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("PaymentInquiry.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            var time = DateTime.Now.ToString("s");
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("ProcessDate").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("PmtGuid").Value = Guid.NewGuid().ToString();
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("ParTrxID").Value = Guid.NewGuid().ToString();
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new PaymentInquiryClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6019/PaymentInquiry.svc"));
            var upload = client.Inquire(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, request);
        }

        public void BillInquiry()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("BillInquiry.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            var time = DateTime.Now.ToString("s");
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new BillInquiryClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6004/BillInquiry.svc"));
            var upload = client.Inquire(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, request);             
        }

        public void BillPayment()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("BillPayment.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            var time = DateTime.Now.ToString("s");
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("ProcessDate").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("BankTrxID").Value = Guid.NewGuid().ToString();
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new BillPayment.PaymentClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6003/BillPayment.svc"));
            var upload = client.PayBill(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, request);             
        }

        public void AddCustomerBilling()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("AddCustomerBilling.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            request.Element("MsgHeader").Element("TmStp").Value = DateTime.Now.ToString("s");
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new AddCustomerBillingClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6006/AddCustomerBilling.svc"));
            var upload = client.AddCustomerBilling(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, request);  
        }

        public void AccountInquiry()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("AccountInquiry.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            request.Element("MsgHeader").Element("TmStp").Value = DateTime.Now.ToString("s");
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new AccountInquiryClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6008/AccountInquiry.svc"));
            var upload = client.AccountInquiry(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, request);  
        }

        public void AccountUpload()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var request = XElement.Parse(File.ReadAllText("AccountUpload.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST", "12345678", s);

            request.Element("MsgHeader").Element("TmStp").Value = DateTime.Now.ToString("s");
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new AccountUploadClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6009/AccountUpload.svc"));
            var upload = client.AccountUpload(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, request);  
        }

        public void BillerList()
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

            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new Token.TokenServiceClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var r = s.Deserialize<RequestResult>(element);

            var client = new BillersListClient(binding, new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6000/BillersList.svc"));
            var list = client.GetBillersList(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey);            
        }

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



        public void Foo34()
        {

            var _overriders = new XmlParameterOverriders(new Dictionary<string, XmlSerializer>
            {
                {typeof (BillPaymentRequest).Name, new XmlSerializer(typeof (BillPaymentRequest), new XmlRootAttribute("MFEP"))},
                {typeof (RequestResult).Name, new XmlSerializer(typeof (RequestResult), new XmlRootAttribute("MFEP"))}
            });

            var s = new Serializer(_overriders);

            var pf = new X509Certificate2(@"d:\private\Ykhanov\ARKAIM-TEST with private.pfx", "12345678");
            var key = (RSACryptoServiceProvider)pf.PrivateKey;
            key.KeySize = 2048;

            var binding = new WSHttpBinding(SecurityMode.None, true);
            var endpoint = new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc");
            var t = new Token.TokenServiceClient(binding, endpoint);
            var element = t.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");

            var r = s.Deserialize<RequestResult>(element);


            var text = File.ReadAllText("Bill.xml");
            var account = XDocument.Parse(text).Element("MFEP");

            var p = CryptoConfig.CreateFromName("SHA1");
            var data = key.SignData(Encoding.Unicode.GetBytes(account.Element("MsgBody").ToString()), p);
            var buffer = Convert.ToBase64String(data);

            account.Element("MsgFooter").Element("Security").Element("Signature").Value = buffer;

            //var p = new PrepaidPayment.PrepaidPaymentClient("WSHttpBinding_IPrepaidPayment");
            //var xElement = p.Pay(Guid.NewGuid().ToString(), token, pay);

            binding = new WSHttpBinding(SecurityMode.None, true);
            endpoint = new EndpointAddress("http://cbj-pres-test.e-fawateer.com.jo:6008/AccountInquiry.svc");
            var client = new AccountInquiryClient(binding, endpoint);
            var x = client.AccountInquiry(Guid.NewGuid().ToString(), r.MsgBody.TokenConf.TokenKey, account);

            var result = s.Deserialize<RequestResult>(x);

            var t1 = new PrepaidPaymentClient();
        }
    }
}
