using System;
using System.Globalization;
using Core.Attributes;
using Sparc.Hardware.Modbus.Registers;

namespace Sparc.Kpk12.Certification.Domain
{
    public interface IValueFormatter
    {
        string Format(ushort value);
        string Format(float value);
        string Format(int value);
        string Format(DateTime dateTime);
        string Format(uint value);
        string Format<T>(T actual);
    }
    /// <summary>
    /// форматирование значения для логирования
    /// </summary>
    public class ValueFormatter : IValueFormatter
    {
        public string Format(ushort value)
        {
            return Format((int) value);
        }

        public string Format(float value)
        {
            return Format("'{0}'", value.ToString(CultureInfo.InvariantCulture));
        }

        public string Format(int value)
        {
            var bin = Convert.ToString(value, 2).PadLeft(14, '0');
            var hex = value.ToString("X");
            return Format("'{0}' '0x{1}' '{2}'", value, hex, bin);
        }

        public string Format(DateTime dateTime)
        {
            return Format("'{0}'", dateTime.ToString("G"));
        }

        public string Format(uint value)
        {
            return Format((int) value);
        }

        private string Format(string value, params Object[] args)
        {
            return string.Format(value, args);
        }

        public string Format<T>(T actual)
        {
            var tmp = (object)actual;
            if (actual is ushort)
                return Format((ushort)tmp);
            if (actual is float)
                return Format((float)tmp);
            if (actual is int)
                return Format((int)tmp);
            if (actual is DateTime)
                return Format((DateTime)tmp);
            if (actual is uint)
                return Format((uint)tmp);
            throw new NotImplementedException("Конвертация невозможна, неизвестный формат данных");
        }
    }
    /// <summary>
    /// форматирование значение для логирования с учетом регистра
    /// </summary>
    public class RegisterValueFormatter
    {
        private readonly IValueFormatter _formatter;
        private string _value;
        private readonly IRegister _register;
        private ushort _moduleId;

        public RegisterValueFormatter(IValueFormatter formatter, IRegister register)
        {
            _formatter = formatter;
            _register = register;
        }

        public RegisterValueFormatter With<T>(IWriteValue<T> value)
        {
            _value = _formatter.Format(value.Expected);
            return this;
        }

        public RegisterValueFormatter With<T>(IReadValue<T> value)
        {
            _value = _formatter.Format(value.Actual);
            return this;
        }

        public RegisterValueFormatter ModuleId(ushort id)
        {
            _moduleId = id;
            return this;
        }

        public string Request()
        {
            return _moduleId == 0 ? string.Format("{0} -> {1}", _register.Address, _value) : 
                string.Format("{0} {1} -> {2}", _moduleId, _register.Address, _value);
        }

        public string Response()
        {
            return _moduleId == 0 ? string.Format("{0} <- {1}", _register.Address, _value) :
                string.Format("{0} {1} <- {2}", _moduleId, _register.Address, _value);
        }
    }
}