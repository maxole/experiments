using LFGenerator2.Configuration;
using LFGenerator2.Protocol;
using LFGenerator2.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LfGenerator2.Test
{
    [TestClass]
    public class UnitTest1
    {
        private LFGenerator2.Trace.Logger _logger;
        private ITransportBoundary _boundary;
        private TransportConfiguration _configuration;

        [TestInitialize]
        public void Init()
        {
            _logger = new LFGenerator2.Trace.Logger("a");
            _configuration = new TransportConfiguration
            {
                BaudRate = 1,
                PortName = "COM0"
            };

            _boundary = new SerialPortBoundary(_configuration.PortName, _configuration.BaudRate, _configuration.Parity, _configuration.DataBits, _configuration.StopBits);
        }

        [TestMethod]
        public void get_id_auditing()
        {
            const string expected = "request: '0xfd', response '0xfd'";
            var protocol = new LfProtocol(_boundary.Auditing(_logger));
            var response = protocol.GetId();
            Assert.AreEqual(expected, _logger.ToString());
        }
    }
}
