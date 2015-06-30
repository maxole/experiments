using System;
using Core.Attributes;

namespace Lambda.GenH30
{
    /// <summary>
    /// 3.2.1 После включения питания сети подать команду «ADR 06».
    /// 	- ожидание таймаута 100…200мс;
    /// - подать команду OUT 0;
    /// - ожидание таймаута 100…500мс;
    /// - при отсутствии ответа выдать диагностическое сообщение:  «Источник питания 27В Lambda GENH30-25 по адресу 06 не обнаружен»;
    /// - в случае ответа «OK» переход на п.3.2.2.
    /// 3.2.2 Определить тип устройства:
    ///  	- подать команду «IDN ?»;
    ///     - ожидание таймаута не более 1000 мс;
    ///     - в случае ответа: «LAMBDA,GENH30-25» зафиксировать успешное определение ИП по адресу 06 в порядке, предусмотренном для прочих устройств Шкафа системного и перей-ти на п. 3.2.3.
    ///     - в случае иного ответа вывести диагностическое сообщение: «Источник питания LAMBDA,GENH30-25 по адресу 06 не обнаружен. Ответ: …... /вывести принятый ответ/».
    /// </summary>
    [TypeRegistration(typeof (StatePowerOn))]
    public class StatePowerOn : State
    {
        private readonly ILambdaProtocol _protocol;
        private readonly LambdaUnit _unit;

        public StatePowerOn(ILambdaProtocol protocol, LambdaUnit unit)
        {
            _protocol = protocol;
            _unit = unit;
        }

        public override void PowerOn(ILambdaWorker worker)
        {
            try
            {
                _unit.Available(false);
                _protocol.Adr(_unit.ExpectedIdentifier, _unit.Address, _unit.Address);
                _unit.Available(true);

                _protocol.OutOn(_unit.ExpectedIdentifier, _unit.Address, _unit.Voltage);

                _protocol.Idn(_unit.ExpectedIdentifier, _unit.Address);

                worker.Goto(worker.StateReadInit);
            }
            catch (Exception)
            {
                worker.Goto(worker.StateFailure);
                throw;
            }
        }

        public override void Read(ILambdaWorker worker)
        {
            throw new LambdaFailureException(Properties.Resources.NotInitialized);
        }

        public override void ReadInit(ILambdaWorker worker)
        {
            throw new LambdaFailureException(Properties.Resources.NotInitialized);
        }

        public override void Failure()
        {
            throw new LambdaFailureException(Properties.Resources.NotInitialized);
        }
    }
}