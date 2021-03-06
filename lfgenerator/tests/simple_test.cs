using System.CodeDom;
using LFGenerator2.Protocol;
using LFGenerator2.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LfGenerator2.Test
{
    [TestClass]
    public class simple_test
    {
        private Logger _logger;
        private ITransportBoundary _boundary;

        [TestInitialize]
        public void Init()
        {
            _logger = new Logger();

            _boundary = new TransportBoundary(new Writer()).Auditing(_logger);
        }

        [TestMethod]
        public void set_channel_2()
        {
            new LfProtocol().SetChannel2(1098, 56).Use(_boundary).Write();

            const Command cmd = Command.SetChannel2;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(1098)
                    .With(56)
                    .Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_constant_voltage()
        {
            new LfProtocol().SetConstantVoltage(456).Use(_boundary).Write();

            const Command cmd = Command.SetConstantVoltage;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(456)
                    .Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_channel_2_noise_voltage()
        {
            new LfProtocol().SetChannel2Noise(456).Use(_boundary).Write();

            const Command cmd = Command.SetChannel2Noise;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(456)
                    .Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_channel_2_1()
        {
            new LfProtocol().SetChannel2Summator(1098, 56, 9875, 1235).Use(_boundary).Write();

            const Command cmd = Command.SetChannel2Hybrid;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(1098)
                    .With(56)
                    .With(9875)
                    .With(1235)
                    .Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_channel_1()
        {
            new LfProtocol().SetChannel1(1098, 56).Use(_boundary).Write();

            const Command cmd = Command.SetChannel1;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(1098).With(56)
                    .Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void reset_all_channels()
        {
            new LfProtocol().ResetAllChannels().Use(_boundary).Write();

            const Command cmd = Command.ResetAllChannels;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_channel_1_k()
        {
            const float v = 78.0f;
            new LfProtocol().SetChannel1K(v).Use(_boundary).Write();

            const Command cmd = Command.SetChannel1K;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(v).Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_channel_1_b()
        {
            const ushort v = 79;
            new LfProtocol().SetChannel1B(v).Use(_boundary).Write();

            const Command cmd = Command.SetChannel1B;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(v).Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_channel_2_k()
        {
            const float v = 178.0f;
            new LfProtocol().SetChannel2K(v).Use(_boundary).Write();

            const Command cmd = Command.SetChannel2K;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(v).Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_channel_2_b()
        {
            const ushort v = 79;
            new LfProtocol().SetChannel2B(v).Use(_boundary).Write();

            const Command cmd = Command.SetChannel2B;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(v).Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_constant_k()
        {
            const float v = 178.0f;
            new LfProtocol().SetConstantK(v).Use(_boundary).Write();

            const Command cmd = Command.SetConstantK;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(v).Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void set_constant_b()
        {
            const ushort v = 178;
            new LfProtocol().SetConstantB(v).Use(_boundary).Write();

            const Command cmd = Command.SetConstantB;
            var expected = string.Format("request: '{0}'",
                new WriteRequest(cmd).With(v).Parameters.Format(),
                new[] { (byte)cmd }.Format());

            var actual = _logger.ToString();

            Assert.AreEqual(expected, actual);
        }

        class Writer : IBoundaryWriter
        {
            private byte _command;

            public void Dispose()
            {
            }

            public ReadableBoundary Write(byte[] command, int timeout)
            {
                _command = command[0];
                return new ReadableBoundary(this);
            }

            public byte[] Read(ushort size, int timeout)
            {
                return new byte[] { _command };
            }
        }
    }
}