using Hardware.AwGenerators.Sparc.Transport;

namespace Hardware.AwGenerators.Sparc.Protocol
{
    public class LfRequestFormatter : ILfRequestFormatter
    {
        public WriteRequest GetId()
        {
            return new WriteRequest(Command.GetDeviceId);
        }

        public WriteRequest GetSoftwareVersion()
        {
            return new WriteRequest(Command.GetSoftwareVersion);   
        }

        public WriteRequest SetChannel2(ushort frequency, ushort rms)
        {
            return new WriteRequest(Command.SetChannel2).With(frequency).With(rms);
        }

        public WriteRequest SetConstantVoltage(ushort voltage)
        {
            return new WriteRequest(Command.SetConstantVoltage).With(voltage);
        }

        public WriteRequest SetChannel2Noise(ushort amplitude)
        {
            return new WriteRequest(Command.SetChannel2Noise).With(amplitude);
        }

        public WriteRequest SetChannel2Summator(ushort frequency1, ushort amplitude1, ushort frequency2, ushort amplitude2)
        {
            return new WriteRequest(Command.SetChannel2Hybrid)
                .With(frequency1).With(amplitude1)
                .With(frequency2).With(amplitude2);
        }

        public WriteRequest SetChannel1(ushort frequency, ushort rms)
        {
            return new WriteRequest(Command.SetChannel1).With(frequency).With(rms);
        }

        public WriteRequest ResetAllChannels()
        {
            return new WriteRequest(Command.ResetAllChannels);
        }

        public WriteRequest SetChannel1K(float k)
        {
            return new WriteRequest(Command.SetChannel1K).With(k);
        }

        public WriteRequest SetChannel1B(ushort b1)
        {
            return new WriteRequest(Command.SetChannel1B).With(b1);
        }

        public WriteRequest SetChannel2K(float k)
        {
            return new WriteRequest(Command.SetChannel2K).With(k);
        }

        public WriteRequest SetChannel2B(ushort b1)
        {
            return new WriteRequest(Command.SetChannel2B).With(b1);
        }

        public WriteRequest SetConstantK(float k)
        {
            return new WriteRequest(Command.SetConstantK).With(k);
        }

        public WriteRequest SetConstantB(ushort b)
        {
            return new WriteRequest(Command.SetConstantB).With(b);
        }

        public WriteRequest GetCalibration()
        {
            return new WriteRequest(Command.GetCalibration);
        }
    }
}