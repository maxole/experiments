using System.IO.Ports;
using System.Linq;

namespace Hardware.AwGenerators.Sparc.Transport
{
    public class SerialPortBoundary : ITransportBoundary, IBoundaryWriter
    {
        private readonly SerialPort _port;
        private ReadableBoundary _readable;

        public SerialPortBoundary(SerialPort port)
        {
            _port = port;            
        }

        private void Initailize()
        {
            _port.Open();
        }

        public void Dispose()
        {
            if (_port.IsOpen)
                _port.Close();
        }

        private ReadableBoundary Write(byte[] buffer, int timeout)
        {
            _readable = new ReadableBoundary(this);
            _port.WriteTimeout = timeout;
            _port.Write(buffer, 0, buffer.Length);            
            return _readable;
        }

        public ReadableBoundary Write<T>(T request, int timeout) where T : WriteRequest
        {
            return Write(request.ToWrite(), timeout);
        }

        public byte[] Read(ushort size, int timeout)
        {
            var answer = new byte[size];
            _port.ReadTimeout = timeout;
            _port.Read(answer, 0, size);
            return answer;
        }

        public IBoundaryWriter Writer()
        {
            Initailize();
            return this;
        }
    }
}