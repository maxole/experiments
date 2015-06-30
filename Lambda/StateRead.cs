using System;
using Core.Attributes;
using Core.Trace;
using Lambda.GenH30.Properties;

namespace Lambda.GenH30
{
    /// <summary>
    /// 3.2.4 Чтение и проверка измеренных параметров
    ///   	- подать команду «DVC?»;  
    ///     - ожидание таймаута не более 1000 мс;
    /// - проверить принятые данные: 
    ///    - если измеренное напряжение равно 27,000В±3% и установленный ток не более 6,0А, то зафиксировать успешное измерение питающего напряжения  в порядке, предусмот-ренном для прочих устройств Шкафа системного.
    ///    - если измеренное напряжение больше 27В+3%, вывести диагностическое сообще-ние «Напряжение 27В превышено: U = .../принятое значение/, I = …/измеренный ток/»;
    ///    - если измеренное напряжение меньше 27В-3%, вывести диагностическое сообще-ние «Напряжение 27В занижено: U = .../принятое значение/, I = …/измеренный ток/»;
    ///    - если измеренное напряжение больше 27В+10%, 
    ///      - подать команду «OUT 0»;
    ///    - вывести диагностическое сообщение «Отключение питания 27В из-за превы-шения: U = .../принятое значение/, I = …/измеренный ток/»;
    /// - перейти к п. 3.2.5.
    /// </summary>
    [TypeRegistration(typeof (StateRead))]
    public class StateRead : State
    {
        private readonly ILogger _logger;
        private readonly ILambdaProtocol _protocol;
        private readonly LambdaUnit _unit;

        public static readonly string Ok = Resources.ParametersOk;
        public static readonly string Info = Resources.InfoVoltagEexceeded;
        public static readonly string Fatal = Resources.FatalVoltagEexceeded;

        public StateRead([InstanceName(LambdaName.LoggerName)]ILogger logger, ILambdaProtocol protocol, LambdaUnit unit)
        {
            _logger = logger;
            _protocol = protocol;
            _unit = unit;
        }

        public override void PowerOn(ILambdaWorker worker)
        {
            throw new LambdaFailureException(Resources.AlreadyInitialized);
        }

        public override void Read(ILambdaWorker worker)
        {
            try
            {
                _unit.ClearMeasuredParameters();
                var result = _protocol.Dvc(_unit.ExpectedIdentifier, _unit.Address, _unit.Voltage);
                _unit.MeasuredParameters(result[0], result[1], result[2], result[3], result[4], result[5]);

                Process();
            }
            catch (Exception)
            {
                worker.Goto(worker.StateFailure);
                throw;
            }
        }

        public void Process()
        {
            if (_unit.MeasureVoltage3() && _unit.MeasureCurrent())
            {
                _logger.Info(Ok);
                return;
            }

            if (_unit.MeasureVoltage3())
                _logger.Info(Info, _unit.Voltage, _unit.MeasuredVoltage, _unit.MeasuredCurrent);
            else if (_unit.MeasureVoltagMore3Less10())
                _logger.Info(Info, _unit.Voltage, _unit.MeasuredVoltage, _unit.MeasuredCurrent);
            else if (_unit.MeasureVoltagLess3())
                _logger.Info(Info, _unit.Voltage, _unit.MeasuredVoltage, _unit.MeasuredCurrent);
            else
            {
                if (_unit.MeasureVoltagMore10())
                    _protocol.OutOn(_unit.ExpectedIdentifier, _unit.Address, _unit.Voltage);
                throw new LambdaFailureException(string.Format(Fatal, _unit.Voltage, _unit.MeasuredVoltage, _unit.MeasuredCurrent));
            }
        }

        public override void ReadInit(ILambdaWorker worker)
        {
            throw new LambdaFailureException(Resources.InitParametersAlreadyRead);
        }

        public override void Failure()
        {
            throw new LambdaFailureException(string.Format(Resources.DeviceFailure, _unit.ExpectedIdentifier,
                _unit.Address));
        }
    }
}