using System;
using System.ServiceModel;
using System.Xml.Linq;
using EfawateerGateway.Proxy.Domain;
using EfawateerGateway.Proxy.Service;
using Gateways;

namespace EfawateerGateway.Proxy
{
    public class BillPaymentProxy
    {
        private readonly ISerializer _serializer;
        private readonly IEfawateerSigner _signer;
        private BillPaymentClient _service;

        public BillPaymentProxy(ISerializer serializer, IEfawateerSigner signer)
        {
            _serializer = serializer;
            _signer = signer;
        }

        /*
        * http://cbj-pres-test.e-fawateer.com.jo:6003/BillPayment.svc/BasicHttp"
        */
        public void Configuration(string uri)
        {
            _service = new BillPaymentClient(
                new WSHttpBinding(SecurityMode.None, true),
                new EndpointAddress(uri));
        }

        public RequestResult PayBill(BillPaymentRequest payment)
        {
            payment.MsgFooter.Security.Signature = _signer.SignData(payment.MsgBody);
            var request = _serializer.Serialize(payment);
            var element = _service.PayBill(Guid.NewGuid().ToString(), AuthenticateTokenProvider.Current.TokenKey, XElement.Parse(request));
            return _serializer.Deserialize<RequestResult>(element);
        }
    }
}