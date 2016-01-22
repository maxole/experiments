using System;
using EfawateerGateway;
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

            var data = new BillPaymentRequest
            {
                MsgHeader = new MsgHeader
                {
                    TmStp = new DateTime(2016, 11, 22, 12, 11, 13).ToString("s"),
                    TrsInf = new MsgHeader.TrsInfImpl { RcvCode = 1, SdrCode = 2, ReqTyp = "BILPMTRQ" }
                },
                MsgFooter = new MsgFooter {Security = new MsgFooter.SecurityImlp {Signature = "signature"}},
                MsgBody = new MsgBody
                {
                    Transactions = new Transactions
                    {
                        TrxInf = new TrxInf
                        {
                            AcctInfo = new AcctInfo
                            {
                                BillNo = "07140",
                                BillingNo = "07140",
                                BillerCode = 158
                            },
                            BankTrxID = "123456789012345",
                            PmtStatus = PmtStatus.PmtNew,
                            DueAmt = "0.1",
                            PaidAmt = "0.6",
                            ProcessDate = new DateTime(2016, 11, 22, 12, 11, 13).ToString("s"),
                            AccessChannel = AccessChannel.ATM,
                            PaymentMethod = PaymentMethod.CASH,
                            ServiceTypeDetails = new ServiceTypeDetails {ServiceType = ServiceType.Prepaid}
                        }
                    }
                },
            };

            var result = proxy.PayBill(data);

            Assert.AreEqual(result.MsgHeader.Result.Severity, Severity.Info, result.MsgHeader.Result.ErrorDesc);
        }
    }
}