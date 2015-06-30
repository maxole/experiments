using System;
using System.Threading;
using Core.Attributes;

namespace Lambda.GenH30
{
    /// <summary>
    /// 3.2.3 Чтение установочных параметров:
    ///   	- подать команду «DVC?»;
    /// - ожидание таймаута не более 1000 мс;
    /// - Проверить принятые данные: 
    /// - Если установленное напряжение равно 27,000В и установленный ток равен 6,0А, то подать команду «OUT 1» и перейти на п.3.2.4.
    /// - если параметры не равны приведенным, передать команды: «PV 27» и «PC 6» и ожидать ответа «OK»? после чего перейти к п. 3.2.4.
    /// </summary>
    [TypeRegistration(typeof (StateReadInit))]
    public class StateReadInit : State
    {
        private readonly ILambdaProtocol _protocol;
        private readonly LambdaUnit _unit;

        public StateReadInit(ILambdaProtocol protocol, LambdaUnit unit)
        {
            _protocol = protocol;
            _unit = unit;
        }

        public override void PowerOn(ILambdaWorker worker)
        {
            throw new LambdaFailureException(Properties.Resources.AlreadyInitialized);
        }

        public override void Read(ILambdaWorker worker)
        {
            throw new LambdaFailureException(Properties.Resources.InitParametersNotReaded);
        }

        public override void ReadInit(ILambdaWorker worker)
        {
            try
            {

                var tryCount = 2;
                do
                {
                    _unit.ClearMeasuredParameters();
                    var result = _protocol.Dvc(_unit.ExpectedIdentifier, _unit.Address, _unit.Voltage);
                    _unit.MeasuredParameters(result[0], result[1], result[2], result[3], result[4], result[5]);

                    if (!Process())
                        break;
                    tryCount--;
                    Thread.Sleep(100);
                } while (tryCount >= 0);

                if (tryCount < 0)
                    throw new LambdaFailureException(
                        string.Format(Properties.Resources.UnableToEstablishVaotageOrCurrent,
                            _unit.Voltage, _unit.Current));

                worker.Goto(worker.StateRead);
            }
            catch (Exception)
            {
                worker.Goto(worker.StateFailure);
                throw;
            }
        }

        public override void Failure()
        {
            throw new LambdaFailureException(string.Format(Properties.Resources.DeviceFailure, _unit.ExpectedIdentifier,
                _unit.Address));
        }

        public bool Process()
        {
            if (_unit.IsSettedVoltage() &&
                _unit.IsSettedCurrent())
                _protocol.OutOff(_unit.ExpectedIdentifier, _unit.Address, _unit.Voltage);
            else
            {                
                if (!_unit.IsSettedVoltage())
                    _protocol.Pv(_unit.ExpectedIdentifier, _unit.Address, _unit.Voltage);
                if (!_unit.IsSettedCurrent())
                    _protocol.Pc(_unit.ExpectedIdentifier, _unit.Address, _unit.Voltage, _unit.Current);
                return true;
            }
            return false;
        }
    }
}