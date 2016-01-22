using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Hardware.AwGenerators.Sparc.Transport
{
    public class AuditingWriter : ITransportBoundary, IBoundaryWriter
    {
        private IBoundaryWriter _writer;
        private readonly ITransportBoundary _boundary;
        private readonly ILogger _logger;

        public AuditingWriter(ITransportBoundary boundary, ILogger logger)
        {            
            _boundary = boundary;
            _logger = logger;
        }

        private void Initialize()
        {
            if (_writer == null)
                _writer = _boundary.Writer();
        }

        public ReadableBoundary Write<T>(T request, int timeout) where T : WriteRequest
        {
            try
            {
                Initialize();
                _writer.Write(request, timeout);
                _logger.Log(LogLevel.Info, AuditRequest(request));
                return new ReadableBoundary(this);
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception.Message);
                throw;
            }
        }

        public byte[] Read(ushort size, int timeout)
        {
            try
            {
                Initialize();
                var response = _writer.Read(size, timeout);
                _logger.Log(LogLevel.Info, AuditResponse(response));
                return response;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception.Message);
                throw;
            }
        }

        public void Dispose()
        {            
            _writer.Dispose();
        }

        public IBoundaryWriter Writer()
        {
            return new AuditingWriter(_boundary, _logger);
        }

        private static string AuditRequest<T>(T request) where T : WriteRequest
        {
            return string.Format("> '{0}' '{1}'", request.Command, AuditCollection(request.Parameters));
        }

        private static string AuditResponse(IEnumerable<byte> buffer)
        {
            return string.Format("< '{0}'", AuditCollection(buffer));
        }

        private static string AuditCollection(IEnumerable<byte> collection)
        {
            return string.Join(string.Empty, collection.Select(p => "0x" + Convert.ToString(p, 16)));
        }
    }

    public static class MakeWriterAudited
    {
        public static AuditingWriter Auditing(this ITransportBoundary boundary, ILogger logger)
        {
            return new AuditingWriter(boundary, logger);
        }
    }
}