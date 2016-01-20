using LFGenerator2.Protocol;
using LFGenerator2.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LfGenerator2.Test
{
    [TestClass]
    public class get_id_test
    {
        private Logger _logger;
        private ITransportBoundary _boundary;

        [TestInitialize]
        public void Init()
        {
            _logger = new Logger();

            _boundary = new TransportBoundary(new Writer());
        }

        [TestMethod]
        public void get_id()
        {
            const byte expected = 0xc3;
            var protocol = new LfProtocol(_boundary);
            var response = protocol.GetId();
            Assert.AreEqual(expected, response);
        }

        class Writer : IBoundaryWriter
        {
            public void Dispose()
            {                
            }

            public ReadableBoundary Write(byte[] command, int timeout)
            {
                return new ReadableBoundary(this);
            }

            public byte[] Read(ushort size, int timeout)
            {
                return new byte[] {0xc3};
            }
        }
        
   }
}
