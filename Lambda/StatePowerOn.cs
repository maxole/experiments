using System;
using Core.Attributes;

namespace Lambda.GenH30
{
    /// <summary>
    /// 3.2.1 ����� ��������� ������� ���� ������ ������� �ADR 06�.
    /// 	- �������� �������� 100�200��;
    /// - ������ ������� OUT 0;
    /// - �������� �������� 100�500��;
    /// - ��� ���������� ������ ������ ��������������� ���������:  ��������� ������� 27� Lambda GENH30-25 �� ������ 06 �� ���������;
    /// - � ������ ������ �OK� ������� �� �.3.2.2.
    /// 3.2.2 ���������� ��� ����������:
    ///  	- ������ ������� �IDN ?�;
    ///     - �������� �������� �� ����� 1000 ��;
    ///     - � ������ ������: �LAMBDA,GENH30-25� ������������� �������� ����������� �� �� ������ 06 � �������, ��������������� ��� ������ ��������� ����� ���������� � �����-�� �� �. 3.2.3.
    ///     - � ������ ����� ������ ������� ��������������� ���������: ��������� ������� LAMBDA,GENH30-25 �� ������ 06 �� ���������. �����: �... /������� �������� �����/�.
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