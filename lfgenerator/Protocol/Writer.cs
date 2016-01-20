using System;
using System.Linq;
using LFGenerator2.Transport;

namespace LFGenerator2.Protocol
{
    public static class WriterExt
    {
        public static Writer Use(this WriteRequest request, ITransportBoundary boundary)
        {
            return new Writer(request, boundary);
        }
    }

    public class Writer : IDisposable
    {
        private readonly WriteRequest _request;
        private readonly IBoundaryWriter _writer;
        private ReadableBoundary _readable;

        public Writer(WriteRequest request, ITransportBoundary boundary)
        {
            _request = request;
            _writer = boundary.Writer();
        }

        public ReadableBoundary Write(bool autoClose = true, int timeout = 500)
        {
            _readable = _writer.Write(_request.Parameters.ToArray(), timeout);
            if (autoClose)
                _readable.Dispose();
            return _readable;
        }

        public byte[] Read(ushort size, int timeout = 500)
        {
            if(_readable == null)
                throw new Exception("Cannot read without write");
            return _readable.Read(size, timeout);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}