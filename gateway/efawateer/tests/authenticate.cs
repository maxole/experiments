using System.Collections.Generic;
using System.Xml.Serialization;
using EfawateerGateway;
using EfawateerGateway.Proxy;
using EfawateerGateway.Proxy.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    public abstract class test_base
    {
        private readonly ISerializer _serializer;

        public test_base()
        {
            var overriders = new XmlParameterOverriders(new Dictionary<string, XmlSerializer>
            {
                {typeof (BillPaymentRequest).Name, new XmlSerializer(typeof (BillPaymentRequest), new XmlRootAttribute("MFEP"))},
                {typeof (RequestResult).Name, new XmlSerializer(typeof (RequestResult), new XmlRootAttribute("MFEP"))},
                {typeof (MsgBody).Name, new XmlSerializer(typeof (MsgBody))},
                {typeof(PrepaidPaymentRequest).Name, new XmlSerializer(typeof(PrepaidPaymentRequest), new XmlRootAttribute("MFEP"))},
                {typeof (MsgBody2).Name, new XmlSerializer(typeof (MsgBody2))},
            });

            _serializer = new Serializer(overriders);
        }

        protected ISerializer Serializer
        {
            get { return _serializer; }
        }
    }

    [TestClass]
    public class authenticate : test_base
    {        
        [TestMethod]
        public void authenticate_success()
        {
            var proxy = new AuthenticateProxy(Serializer);
            proxy.Configuration(UriContext.Authenticate);
            var result = proxy.Authenticate(CustomerProvider.CustomerCode, CustomerProvider.Password);
            Assert.AreEqual(Severity.Info, result.MsgHeader.Result.Severity);
        }

        [TestMethod]
        public void authenticate_fail()
        {
            var proxy = new AuthenticateProxy(Serializer);
            proxy.Configuration(UriContext.Authenticate);
            var result = proxy.Authenticate(CustomerProvider.CustomerCode, "Test@1235");
            Assert.AreEqual(Severity.Error, result.MsgHeader.Result.Severity);
        }

        [TestMethod]
        public void authenticate_token()
        {
            var proxy = new AuthenticateProxy(Serializer);
            proxy.Configuration(UriContext.Authenticate);
            var result = proxy.Authenticate(CustomerProvider.CustomerCode, CustomerProvider.Password);

            Assert.IsNotNull(AuthenticateTokenProvider.Current);
        }
    }
}