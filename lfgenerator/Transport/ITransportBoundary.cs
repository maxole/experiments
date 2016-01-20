using System;

namespace LFGenerator2.Transport
{
    public interface ITransportBoundary
    {
        IBoundaryWriter Writer();
    }

    public interface IBoundaryWriter : IDisposable
    {
        ReadableBoundary Write(byte[] command, int timeout);
        byte[] Read(ushort size, int timeout);
    }
}
