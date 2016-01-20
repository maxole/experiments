using System;
using System.Collections.Generic;
using LFGenerator2.Protocol;

namespace LFGenerator2.Transport
{
    public class WriteRequest : EventArgs
    {        
        private readonly List<byte> _parameters;

        public WriteRequest(Command command)
        {
            _parameters = new List<byte>();
            With((byte)command);
        }

        public WriteRequest With(ushort parameter)
        {
            _parameters.AddRange(BitConverter.GetBytes(parameter));
            return this;
        }

        public WriteRequest With(float parameter)
        {
            _parameters.AddRange(BitConverter.GetBytes(parameter));
            return this;
        }

        public IEnumerable<byte> Parameters
        {
            get { return _parameters; }
        }
    }
}