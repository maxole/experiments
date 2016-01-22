using System;
using EfawateerGateway;
using EfawateerGateway.Proxy.Domain;
using Gateways;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class signer : test_base
    {
        [TestMethod]
        public void check_cerificate()
        {
            var s = new EfawateerSigner("ARKAIM-TEST", "12345678", Serializer);
            s.CheckCerificate();
        }

        [TestMethod]
        public void sign_data()
        {
            var s = new EfawateerSigner("ARKAIM-TEST", "12345678", Serializer);
            var body = new MsgBody
            {
                Transactions = new Transactions
                {
                    TrxInf = new TrxInf
                    {
                       PaymentMethod = PaymentMethod.CASH
                    }
                }
            };
            var data = s.SignData(body);
        }

        [TestMethod]
        public void verify_data()
        {
            var s = new EfawateerSigner("ARKAIM-TEST", "12345678", Serializer);
            var data = s.VerifyData("hello");
        }
    }
}
