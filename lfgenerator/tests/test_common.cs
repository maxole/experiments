using System;
using System.Collections.Generic;
using System.Linq;
using LFGenerator2.Trace;
using LFGenerator2.Transport;

namespace LfGenerator2.Test
{
    class Logger : ILogger
    {
        private readonly List<string> _message;

        public string Name { get; private set; }
        public void Log(LoggerEntryLevel level, string message, Exception exception = null)
        {
            _message.Add(message);
        }

        public Logger()
        {
            _message = new List<string>();
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, _message);
        }
    }

    class TransportBoundary : ITransportBoundary
    {
        private readonly IBoundaryWriter _writer;

        public TransportBoundary(IBoundaryWriter writer)
        {
            _writer = writer;
        }

        public IBoundaryWriter Writer()
        {
            return _writer;
        }
    }
}