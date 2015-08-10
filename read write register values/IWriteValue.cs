using System.ComponentModel;
using Sparc.Hardware.Modbus.Registers;

namespace Sparc.Kpk12.Certification.Domain
{
    public interface IWriteValue<T> : IReadValue<T>
    {
        T Expected { get; set; }
    }

    public class ReadWriteValue<T> : ValueNotifyPropertyChanged, IWriteValue<T>, IDataErrorInfo
    {
        public ReadWriteValue()
        {
            
        }

        public ReadWriteValue(ReadWriteValue<T> value)
        {
            Expected = value.Expected;
            Actual = value.Actual;
            Register = value.Register;
        }

        private T _actual;

        public T Expected { get; set; }

        public T Actual
        {
            get { return _actual; }
            set
            {
                _actual = value;
                OnPropertyChanged("Expected");
            }
        }

        public IRegister Register { get; set; }

        private bool TheSame
        {
            get { return Expected.Equals(Actual); }
        }

        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;
                if (!TheSame)
                    error = string.Format("Заданное значение '{0}' не равно установленному '{1}'", Expected, Actual);
                return error;
            }
        }

        public string Error
        {
            get { return "error property"; }
        }
    }
}