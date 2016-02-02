using System;
using System.ServiceModel;
using System.Xml.Linq;
using EfawateerGateway.Proxy.Domain;
using EfawateerGateway.Proxy.Service;
using Gateways;

namespace EfawateerGateway.Proxy
{
    public class PrepaidPaymentProxy
    {
        private readonly ISerializer _serializer;
        private readonly IEfawateerSigner _signer;
        private PrepaidPaymentClient _service;

        public PrepaidPaymentProxy(ISerializer serializer, IEfawateerSigner signer)
        {
            _serializer = serializer;
            _signer = signer;
        }

        public void Configuration(string uri)
        {
            _service = new PrepaidPaymentClient(
                new WSHttpBinding(SecurityMode.None, true),
                new EndpointAddress(uri));
        }

        public RequestResult Pay(PrepaidPaymentRequest payment)
        {
            var e = XElement.Parse(_serializer.Serialize(payment.MsgBody)).ToString();

            payment.MsgFooter.Security.Signature = _signer.SignData(e);
            var request = _serializer.Serialize(payment);
            var element = _service.Pay(payment.MsgHeader.Guid, AuthenticateTokenProvider.Current.TokenKey, XElement.Parse(request));
            return _serializer.Deserialize<RequestResult>(element);
        }
    }
}