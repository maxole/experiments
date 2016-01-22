using System;

namespace Hardware.AwGenerators.Sparc.Transport
{
    public interface ITransportBoundary
    {
        IBoundaryWriter Writer();
    }

    public interface IBoundaryWriter : IDisposable
    {
        ReadableBoundary Write<T>(T request, int timeout) where T : WriteRequest;
        byte[] Read(ushort size, int timeout);
    }
}
