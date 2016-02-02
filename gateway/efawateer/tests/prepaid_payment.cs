using System;
using EfawateerGateway.Proxy;
using EfawateerGateway.Proxy.Domain;
using Gateways;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class prepaid_payment : test_base
    {
        private readonly AuthenticateProxy _authenticate;
        private readonly IEfawateerSigner _signer;

        public prepaid_payment()
        {
            _authenticate = new AuthenticateProxy(Serializer);
            _authenticate.Configuration(UriContext.Authenticate);

            _signer = new EfawateerSigner("ARKAIM-TEST", "12345678", Serializer);
        }

        [TestMethod]
        public void prepaid()
        {
            _authenticate.Authenticate(CustomerProvider.CustomerCode, CustomerProvider.Password);

            var proxy = new PrepaidPaymentProxy(Serializer, _signer);
            proxy.Configuration(UriContext.PrepaidPayment);

            var time = DateTime.Now.ToString("s");

            var data = new PrepaidPaymentRequest
            {
                MsgHeader = new MsgHeader2
                {
                    TmStp = time,
                    TrsInf = new MsgHeader2.TrsInfImpl {RcvCode = 1, SdrCode = 158, ReqTyp = "PREPADPMTRQ"},
                    Guid = Guid.NewGuid().ToString()
                },
                MsgFooter = new MsgFooter {Security = new MsgFooter.SecurityImlp {Signature = ""}},
                MsgBody = new MsgBody2
                {
                    TrxInf = new TrxInf
                    {
                        AcctInfo = new AcctInfo
                        {
                            //BillNo = "1694055",
                            BillingNo = "071231204",
                            BillerCode = 39
                        },
                        BankTrxID = Guid.NewGuid().ToString(),
                        ValidationCode = "1009",
                        PmtStatus = PmtStatus.PmtNew,
                        DueAmt = "07.50",
                        PaidAmt = "07.50",
                        ProcessDate = time,
                        AccessChannel = AccessChannel.ATM,
                        PaymentMethod = PaymentMethod.CCARD,
                        ServiceTypeDetails = new ServiceTypeDetails
                        {
                            ServiceType = ServiceType.Electricity
                        }
                    }
                }
            };

            var result = proxy.Pay(data);
        }
    }
}