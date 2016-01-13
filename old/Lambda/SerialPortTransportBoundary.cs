using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Lambda.GenH30
{    
    public class SerialPortTransportBoundary : ITransportBoundary
    {
        private readonly TransportConfig _configuration;
        
        public SerialPortTransportBoundary(IConfigurationManager configuration)
        {
            _configuration = configuration.GetCustomConfig<TransportConfig>();
        }

        public IBoundaryWriter Writer()
        {
            return new SerialPortBoundaryWriter(_configuration);
        }
    }

    public class SerialPortBoundaryWriter : IBoundaryWriter
    {
        private readonly SerialPort _port;
        private readonly StringBuilder _answer;
        private readonly ManualResetEvent _dataReceived;
        private const char EndChar = '\r';

        public SerialPortBoundaryWriter(TransportConfig configuration)
        {
            _port = new SerialPort(configuration.PortName, configuration.BaudRate)
            {
                Parity = Parity.None,
                StopBits = StopBits.One
            };

            _answer = new StringBuilder();
            _dataReceived = new ManualResetEvent(false);
            Initailize();
        }    

        private void Initailize()
        {            
            _port.Open();
            _port.DataReceived += DataReceived;
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _answer.Append(_port.ReadExisting());
            if (_answer.ToString().IndexOf(EndChar) == -1)
            {
                Thread.Sleep(10);
                _answer.Append(_port.ReadExisting());
            }
            if (_answer.ToString().IndexOf(EndChar) != -1)
                _dataReceived.Set();            
        }

        public void Dispose()
        {
            if (_port.IsOpen)
                _port.Close();
            _port.DataReceived -= DataReceived;
        }

        public WriteResponse Write(string command, int timeout)
        {
            var response = new WriteResponse();

            _port.DiscardInBuffer();
            _port.Write(command + EndChar);

            response.Expired = !_dataReceived.WaitOne(timeout, false);
            response.Response = _answer.ToString().Replace(EndChar.ToString(CultureInfo.InvariantCulture), string.Empty);

            return response;
        }
    }
}