using Sparc.Hardware.Modbus.Registers;

namespace Sparc.Kpk12.Certification.Domain
{
    public interface IReadValue<T>
    {
        T Actual { get; set; }
        IRegister Register { get; set; }
    }

    public class ReadOnlyValue<T> : ValueNotifyPropertyChanged, IReadValue<T>
    {
        public T Actual { get; set; }
        public IRegister Register { get; set; }
    }
}