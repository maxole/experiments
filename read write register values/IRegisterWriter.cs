using System;
using Core.Attributes;
using Sparc.Hardware.Modbus.Registers;
using Sparc.Kpk12.Hardware.MeasuringBlock;

namespace Sparc.Kpk12.Certification.Domain
{
    public interface IRegisterWriter
    {
        void Write<T>(IKpk12MeasuringUnit unit, IWriteValue<T> value, string source);
        void Read<T>(IKpk12MeasuringUnit unit, IReadValue<T> value, string source, bool silent = false);
    }

    public class RegisterWriter : IRegisterWriter
    {        
        private readonly TracerAgent _logger;
        private readonly IValueFormatter _formatter;

        public RegisterWriter(TracerAgent logger, IValueFormatter formatter)
        {
            _logger = logger;
            _formatter = formatter;
        }

        public void Write<T>(IKpk12MeasuringUnit unit, IWriteValue<T> value, string source)
        {
            ThrowIfRegisterNull(value.Register);

            var register = value.Register;

            try
            {                

                ((Register<T>) register).Value = value.Expected;
                unit.Write(register);

                var message = _formatter.For(register)
                                        .With(value)
                                        .ModuleId(unit.UnitIdRegister.Value)
                                        .Request();
                _logger.Info(message, source);
            }
            catch (Exception exception)
            {
                _logger.Error(string.Format("Ошибка записи регистра '{0}' модуль '{1}'", register.Address, unit.UnitIdRegister.Value), source, exception);
                throw;
            }
        }

        public void Read<T>(IKpk12MeasuringUnit unit, IReadValue<T> value, string source,
                            bool silent = false)
        {
            ThrowIfRegisterNull(value.Register);

            var register = value.Register;
            try
            {                

                unit.Read(register);

                var converted = (Register<T>) register;
                value.Actual = converted.Value;                

                if (silent)
                    return;
                var message = _formatter.For(register)
                                        .With(value)
                                        .ModuleId(unit.UnitIdRegister.Value)
                                        .Response();
                _logger.Info(message, source);
            }
            catch (Exception exception)
            {
                _logger.Error(string.Format("Ошибка чтения регистра '{0}' модуль '{1}'", register.Address, unit.UnitIdRegister.Value), source, exception);
                throw;
            }
        }

        private void ThrowIfRegisterNull(IRegister register)
        {
            if (register == null)
                throw new NullReferenceException("Не задано значение регистра");
        }
    }

    public interface IRegisterWorker
    {        
        RegisterWorker For(IRegister register);
        void WriteTo<T>(ReadWriteValue<T> value);
        void ReadTo<T>(ReadOnlyValue<T> value, bool silent = false);
        void Initialize<T>(ReadWriteValue<T> value);
    }
    
    public class RegisterWorker : IRegisterWorker
    {
        private readonly IRegisterWriter _writer;
        private readonly IKpk12MeasuringUnit _unit;
        private IRegister _register;        
        private string _source;

        public RegisterWorker(IRegisterWriter writer, IKpk12MeasuringUnit unit)
        {
            _writer = writer;
            _unit = unit;
        }

        public RegisterWorker Source(string source)
        {
            _source = source;
            return this;
        }

        public RegisterWorker For(IRegister register)
        {
            _register = register;
            return this;
        }

        public void WriteTo<T>(ReadWriteValue<T> value)
        {
            value.Register = _register;
            _writer.Write(_unit, value, _source);
        }

        public void ReadTo<T>(ReadWriteValue<T> value, bool silent = false)
        {
            ThrowIfValueNull(value);
            value.Register = _register;
            _writer.Read(_unit, value, _source, silent);
        }

        public void ReadTo<T>(ReadOnlyValue<T> value, bool silent = false)
        {
            ThrowIfValueNull(value);
            value.Register = _register;
            _writer.Read(_unit, value, _source, silent);
        }

        public void Initialize<T>(ReadWriteValue<T> value)
        {
            ThrowIfValueNull(value);
            ReadTo(value);
            value.Expected = value.Actual;
        }

        private void ThrowIfValueNull<T>(IReadValue<T> value)
        {
            if (value == null)
                throw new Exception("Не задано значение параметра");
        }
    }
}