using System;
using System.ServiceModel;
using EfawateerGateway.Proxy.Domain;
using EfawateerGateway.Proxy.Service;

namespace EfawateerGateway.Proxy
{
    public class AuthenticateProxy
    {
        private readonly ISerializer _serializer;
        private TokenServiceClient _service;

        public AuthenticateProxy(ISerializer serializer)
        {
            _serializer = serializer;
        }

        /*
         * "http://cbj-pres-test.e-fawateer.com.jo:6001/Token.svc"
         */
        public void Configuration(string uri)
        {
            _service = new TokenServiceClient(
                new WSHttpBinding(SecurityMode.None, true),
                new EndpointAddress(uri));
        }

        public RequestResult Authenticate(int customerCode, string password)
        {
            var element = _service.Authenticate(Guid.NewGuid().ToString(), customerCode, password);
            var result = _serializer.Deserialize<RequestResult>(element);
            if(result.MsgHeader.Result.Severity == Severity.Info)
                AuthenticateTokenProvider.Current = new SuccessToken(result.MsgBody.TokenConf.ExpiryDate, result.MsgBody.TokenConf.TokenKey);
            if (result.MsgHeader.Result.Severity == Severity.Error)
                AuthenticateTokenProvider.Current = new ErrorToken();
            return result;
        }
    }
}
