using Hardware.AwGenerators.Sparc.Transport;

namespace Hardware.AwGenerators.Sparc.Protocol
{
    public static class WriterExt
    {
        public static ReadableBoundary Write<T>(this ITransportBoundary boundary, T request, bool autoClose = true, int timeout = 500)
            where T : WriteRequest
        {
            var writer = boundary.Writer();
            var response = writer.Write(request, timeout);
            if(autoClose)
                response.Dispose();
            return response;
        }
    }
}