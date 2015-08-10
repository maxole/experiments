using Sparc.Hardware.Modbus.Registers;
using Sparc.Kpk12.Hardware.MeasuringBlock;

namespace Sparc.Kpk12.Certification.Domain
{
    public static class Extension
    {
        public static RegisterWorker Use(this IKpk12MeasuringUnit unit, IRegisterWriter writer)
        {
            return new RegisterWorker(writer, unit);
        }

        public static RegisterValueFormatter For(this IValueFormatter formatter, IRegister register)
        {
            return new RegisterValueFormatter(formatter, register);
        }
    }
}