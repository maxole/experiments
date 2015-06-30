using System;
using Lambda.GenH30.Properties;

namespace Lambda.GenH30
{        
    public class LambdaProtocol : ILambdaProtocol
    {
        private readonly ILogger _logger;
        private readonly ITransportBoundary _boundary;
        private readonly TransportConfig _config;

        private const string ResponseOk = "OK";

        public LambdaProtocol(ILogger logger, IConfigurationManager configuration, ITransportBoundary boundary)
        {
            _logger = logger;
            _boundary = boundary;
            _config = configuration.GetCustomConfig<TransportConfig>();
        }

        public WriteResponse Adr(byte address)
        {
            return Write("ADR " + address, _config.Adr, _config.RepeatCount, _config.RepeatTimeout);            
        }

        public WriteResponse Idn()
        {
            return Write("IDN?", _config.Idn, _config.RepeatCount, _config.RepeatTimeout);
        }

        public WriteResponse OutOn()
        {
            return Write("OUT 0", _config.Out, _config.RepeatCount, _config.RepeatTimeout);            
        }

        public WriteResponse OutOff()
        {
            return Write("OUT 1", _config.Out, _config.RepeatCount, _config.RepeatTimeout);
        }

        public WriteResponse Dvc()
        {
            return Write("DVC?", _config.Dvc, _config.RepeatCount, _config.RepeatTimeout);
        }

        public WriteResponse Pv(float value)
        {
            return Write("Pv " + Math.Round(value, 0), _config.Pv, _config.RepeatCount, _config.RepeatTimeout);
        }

        public WriteResponse Pc(float value)
        {
            return Write("Pc " + Math.Round(value, 0), _config.Pc, _config.RepeatCount, _config.RepeatTimeout);
        }        

        private WriteResponse Write(string request, int timeout, int repeatCount, int repeatTimeout)
        {
            using (var writer = _boundary.Writer())
            {                
                var response = writer.Write(request, timeout, repeatCount, repeatTimeout);                
                var l = _logger.Debug();
                if (response.Error != null)
                    l = _logger.Error();
                l.Log("request: '{0}', response '{1}'", request, response.Response);                
                if (!response.Response.Equals(ResponseOk))
                    response.AddError(new LambdaFailureException(Resources.DoesNotContainStandartAnswer));
                return response;
            }
        }
    }
}