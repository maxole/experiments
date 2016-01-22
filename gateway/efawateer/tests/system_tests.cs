using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using EfawateerGateway;
using EfawateerGateway.Proxy.Domain;
using EfawateerWcf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class system_tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var c = new Class1();
            c.Foo3();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var c = new Class1();
            c.Foo2();
        }
        
        private readonly Serializer _serializer;

        public system_tests()
        {
            var overriders = new XmlParameterOverriders(new Dictionary<string, XmlSerializer>
            {
                {typeof (BillPaymentRequest).Name, new XmlSerializer(typeof (BillPaymentRequest), new XmlRootAttribute("MFEP"))},
                {typeof (RequestResult).Name, new XmlSerializer(typeof (RequestResult), new XmlRootAttribute("MFEP"))}
            });

            _serializer = new Serializer(overriders);
        }

        [TestMethod]
        public void Test1()
        {
            var data = new BillPaymentRequest
            {
                MsgHeader =
                {
                    TmStp = new DateTime(2016, 11, 22, 12, 11, 13).ToString("s"),
                    TrsInf = { RcvCode = 4, ReqTyp = "42", SdrCode = 14 }
                },
                MsgFooter = { Security = { Signature = "signature" } }
            };

            var element = _serializer.Serialize(data);
            var actual = element.ToString();

            var expected = File.ReadAllText("prepare_result_success.xml");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test2()
        {
            var d = new RequestResult();

            var data = new RequestResult
            {
                MsgHeader =
                {
                    TmStp = new DateTime(2016, 11, 22, 12, 11, 13).ToString("s"),
                    TrsInf = new RequestResult.MsgHeaderImlp.TrsInfImpl{RcvCode = 4, ResTyp = "42"},
                    Result = { ErrorCode = 2, ErrorDesc = "InvalidSignature", Severity = Severity.Error }
                }
            };

            var element = _serializer.Serialize(data);
            var actual = element.ToString();
        }

        [TestMethod]
        public void Test3()
        {
            var error = File.ReadAllText("error.xml");
            var element = XElement.Parse(error);
            //var s = new Serializer(_overriders);
            //var result = s.Deserialize<RequestResult>(element);
        }
    }
}
