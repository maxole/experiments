using LFGenerator2.Transport;

namespace LFGenerator2.Protocol
{
    public class LfProtocol : ILfProtocol
    {
        private readonly ITransportBoundary _boundary;

        public LfProtocol(ITransportBoundary boundary)
        {
            _boundary = boundary;
        }

        public byte GetId()
        {
            var request = new WriteRequest(Command.GetDeviceId);
            var response = WriteAndRead(request);
            return response[0];     
        }

        public byte[] GetSoftwareVersion()
        {
            var request = new WriteRequest(Command.GetSoftwareVersion);
            var response = WriteAndRead(request, 8);
            return response;         
        }

        public byte SetChannel2(ushort frequency, ushort rms)
        {
            var request = new WriteRequest(Command.SetChannel2).With(frequency).With(rms);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetConstantVoltage(ushort voltage)
        {
            var request = new WriteRequest(Command.SetConstantVoltage).With(voltage);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetChannel2Noise(ushort amplitude)
        {
            var request = new WriteRequest(Command.SetChannel2Noise).With(amplitude);
            var response = WriteAndRead(request);
            return response[0];
        }
        
        public byte SetChannel2Summator(ushort frequency1, ushort amplitude1, ushort frequency2, ushort amplitude2)
        {
            var request = new WriteRequest(Command.SetChannel2Hybrid)
                .With(frequency1).With(amplitude1)
                .With(frequency2).With(amplitude2);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetChannel1(ushort frequency, ushort rms)
        {
            var request = new WriteRequest(Command.SetChannel1).With(frequency).With(rms);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte ResetAllChannels()
        {
            var request = new WriteRequest(Command.ResetAllChannels);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetChannel1K(float k)
        {
            var request = new WriteRequest(Command.SetChannel1K).With(k);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetChannel1B(ushort b1)
        {
            var request = new WriteRequest(Command.SetChannel1B).With(b1);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetChannel2K(float k)
        {
            var request = new WriteRequest(Command.SetChannel2K).With(k);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetChannel2B(ushort b1)
        {
            var request = new WriteRequest(Command.SetChannel2B).With(b1);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetConstantK(float k)
        {
            var request = new WriteRequest(Command.SetConstantK).With(k);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte SetConstantB(ushort b)
        {
            var request = new WriteRequest(Command.SetConstantB).With(b);
            var response = WriteAndRead(request);
            return response[0];
        }

        public byte[] GetCalibration()
        {
            var request = new WriteRequest(Command.GetCalibration);
            var response = WriteAndRead(request);
            return response;
        }

        private byte[] WriteAndRead(WriteRequest request, ushort readBufferSize = 1)
        {            
            using (var readable = request.Use(_boundary).Write())
                return readable.Read(readBufferSize);
        }
    }
}