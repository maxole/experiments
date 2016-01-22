using System;

namespace Hardware.AwGenerators.Sparc.Transport
{
    public class ReadableBoundary : IDisposable
    {
        private readonly IBoundaryWriter _boundary;

        public ReadableBoundary(IBoundaryWriter boundary)
        {
            _boundary = boundary;
        }

        public byte[] Read(ushort size, int timeout = 500)
        {
            return _boundary.Read(size, timeout);
        }

        public void Dispose()
        {
            _boundary.Dispose();
        }
    }
}