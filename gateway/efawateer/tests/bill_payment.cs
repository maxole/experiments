using System;
using EfawateerGateway.Proxy;
using EfawateerGateway.Proxy.Domain;
using Gateways;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class bill_payment : test_base
    {
        private readonly AuthenticateProxy _authenticate;
        private readonly IEfawateerSigner _signer;

        public bill_payment()
        {
            _authenticate = new AuthenticateProxy(Serializer);
            _authenticate.Configuration(UriContext.Authenticate);

            _signer = new EfawateerSigner("ARKAIM-TEST", "12345678", Serializer);
        }

        [TestMethod]
        public void pay_bill_fail()
        {
            _authenticate.Authenticate(CustomerProvider.CustomerCode, CustomerProvider.Password);

            var proxy = new BillPaymentProxy(Serializer, _signer);
            proxy.Configuration(UriContext.BillPayment);

            var data = new BillPaymentRequest
            {
                MsgHeader = new MsgHeader
                {
                    TmStp = new DateTime(2016, 11, 22, 12, 11, 13).ToString("s"),
                    TrsInf = new MsgHeader.TrsInfImpl {RcvCode = 1, ReqTyp = "BILPMTRQ", SdrCode = 2}
                },
                MsgFooter = new MsgFooter {Security = new MsgFooter.SecurityImlp {Signature = "signature"}}
            };

            var result = proxy.PayBill(data);

            Assert.AreEqual(result.MsgHeader.Result.Severity, Severity.Error);
        }

        [TestMethod]
        public void pay_bill_sucess()
        {
            _authenticate.Authenticate(CustomerProvider.CustomerCode, CustomerProvider.Password);

            var proxy = new BillPaymentProxy(Serializer, _signer);
            proxy.Configuration(UriContext.BillPayment);

            var data = CreateBill();

            var result = proxy.PayBill(data);

            Assert.AreEqual(result.MsgHeader.Result.Severity, Severity.Info, result.MsgHeader.Result.ErrorDesc);
        }

        private static BillPaymentRequest CreateBill()
        {
            var time = DateTime.Now.ToString("s");
            return new BillPaymentRequest
            {
                MsgHeader = new MsgHeader
                {
                    TmStp = time,
                    TrsInf = new MsgHeader.TrsInfImpl { RcvCode = 1, SdrCode = 158, ReqTyp = "BILPMTRQ" }
                },
                MsgFooter = new MsgFooter {Security = new MsgFooter.SecurityImlp {Signature = ""}},
                MsgBody = new MsgBody
                {
                    Transactions = new Transactions
                    {
                        TrxInf = new TrxInf
                        {
                            AcctInfo = new AcctInfo
                            {
                                BillNo = "071231204",
                                BillingNo = "071231204",
                                BillerCode = 39
                            },
                            BankTrxID = "e8fafebd-e232-44b4-a3e5-330bb9b7c3f1",//Guid.NewGuid().ToString(),
                            //ValidationCode = "1009",
                            PmtStatus = PmtStatus.PmtNew,
                            DueAmt = "07.50",
                            PaidAmt = "07.50",
                            ProcessDate = time,
                            AccessChannel = AccessChannel.ATM,
                            PaymentMethod = PaymentMethod.CASH,
                            ServiceTypeDetails = new ServiceTypeDetails
                            {
                                ServiceType = ServiceType.Electricity,
                                //PrepaidCat = "JD_5"
                            }
                        }
                    }
                },
            };
        }
    }
}