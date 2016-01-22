using System;
using System.Collections.Generic;
using System.Linq;
using Hardware.AwGenerators.Sparc.Protocol;

namespace Hardware.AwGenerators.Sparc.Transport
{
    public class WriteRequest : EventArgs
    {
        private readonly List<byte> _parameters;
        private readonly Command _command;

        public WriteRequest(Command command)
        {
            _parameters = new List<byte>();
            _command = command;
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

        public Command Command
        {
            get { return _command; }
        }
    }

    public static class WriteRequestParameterToWrite
    {
        public static byte[] ToWrite(this WriteRequest request)
        {
            return request.With((byte) request.Command).Parameters.ToArray();
        }
    }
}