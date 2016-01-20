using System.IO.Ports;

namespace LFGenerator2.Transport
{
    public class SerialPortBoundary : ITransportBoundary, IBoundaryWriter
    {
        private readonly SerialPort _port;
        private ReadableBoundary _readable;

        public SerialPortBoundary(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);            
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

        public ReadableBoundary Write(byte[] command, int timeout)
        {
            _readable = new ReadableBoundary(this);
#if !NO_WRITE_TO_RS232
            _port.WriteTimeout = timeout;
            _port.Write(command, 0, command.Length);            
#endif
            return _readable;
        }

        public byte[] Read(ushort size, int timeout)
        {
            var answer = new byte[size];
#if !NO_WRITE_TO_RS232
            _port.ReadTimeout = timeout;
            _port.Read(answer, 0, size);
#endif
            return answer;
        }

        public IBoundaryWriter Writer()
        {
#if !NO_WRITE_TO_RS232
            Initailize();
#endif
            return this;
        }
    }
}