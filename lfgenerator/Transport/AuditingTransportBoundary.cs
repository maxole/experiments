using System;
using System.Collections.Generic;
using System.Linq;
using LFGenerator2.Trace;

namespace LFGenerator2.Transport
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

        public ReadableBoundary Write(byte[] command, int timeout)
        {
            try
            {
                Initialize();
                _writer.Write(command, timeout);
                _logger.Log(LoggerEntryLevel.Info, string.Format("request: '{0}'", command.Format()));
                return new ReadableBoundary(this);
            }
            catch (Exception exception)
            {
                _logger.Log(LoggerEntryLevel.Error, exception.Message);
                throw;
            }
        }

        public byte[] Read(ushort size, int timeout)
        {
            try
            {
                Initialize();
                var response = _writer.Read(size, timeout);
                _logger.Log(LoggerEntryLevel.Info, string.Format("response: '{0}'", response.Format()));
                return response;
            }
            catch (Exception exception)
            {
                _logger.Log(LoggerEntryLevel.Error, exception.Message);
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
    }

    public static class AuditingWriterExt
    {
        public static AuditingWriter Auditing(this ITransportBoundary boundary, ILogger logger)
        {
            return new AuditingWriter(boundary, logger);
        }
    }

    public static class AuditingFormatParameterExt
    {
        public static string Format(this IEnumerable<byte> collection)
        {
            return string.Join(string.Empty, collection.Select(p => "0x" + Convert.ToString(p, 16)));
        }
    }
}