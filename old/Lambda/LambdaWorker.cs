using System;
using Lambda.GenH30.Properties;

namespace Lambda.GenH30
{
    public interface ILambdaWorker
    {
        State StatePowerOn { get; }
        State StateRead { get; }
        State StateReadInit { get; }
        State StateFailure { get; }

        /// <summary>
        /// ��������� �������
        /// </summary>
        void PowerOn();

        /// <summary>
        /// ������ ������������� ����������
        /// </summary>
        void ReadInit();

        /// <summary>
        /// ������ ����������
        /// </summary>
        void Read();

        /// <summary>
        /// ������� � ����� ������ ������������
        /// </summary>
        void Failure();

        void Goto(State state);
    }

    /// <summary>
    /// 
    /// </summary>    
    public class LambdaWorker : ILambdaWorker
    {
        private readonly ILogger _logger;

        /// <summary>
        /// ������� ���������
        /// </summary>
        private State _currentState;

        /// <summary>
        /// ��������� �������
        /// </summary>
        private readonly State _powerOn;

        /// <summary>
        /// ������ ����������
        /// </summary>
        private readonly State _read;

        /// <summary>
        /// ��������� ������ ����������
        /// </summary>
        private readonly State _readInit;

        /// <summary>
        /// ����� ����������
        /// </summary>
        private readonly State _stateFailure;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="powerOn"></param>
        /// <param name="read"></param>
        /// <param name="readInit"></param>
        /// <param name="stateFailure"></param>
        public LambdaWorker(ILogger logger,
                            StatePowerOn powerOn,
                            StateRead read,
                            StateReadInit readInit,
                            StateFailure stateFailure)
        {
            _logger = logger;
            _powerOn = powerOn;
            _read = read;
            _readInit = readInit;
            _stateFailure = stateFailure;
            _currentState = _powerOn;
        }

        public State StatePowerOn
        {
            get { return _powerOn; }
        }

        public State StateRead
        {
            get { return _read; }
        }

        public State StateReadInit
        {
            get { return _readInit; }
        }

        public State StateFailure
        {
            get { return _stateFailure; }
        }

        /// <summary>
        /// ��������� �������
        /// </summary>
        public void PowerOn()
        {
            Process(() => _currentState.PowerOn(this));
        }

        /// <summary>
        /// ������ ������������� ����������
        /// </summary>
        public void ReadInit()
        {
            Process(() => _currentState.ReadInit(this));
        }

        /// <summary>
        /// ������ ����������
        /// </summary>
        public void Read()
        {
            Process(() => _currentState.Read(this));
        }

        public void Failure()
        {
            Process(() => _currentState.Failure());
        }

        public void Goto(State state)
        {
            _currentState = state;
        }

        private void Process(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                var message = exception.Message;
                if (exception.InnerException != null)
                    message += exception.InnerException.Message;
                _logger.Error(message);
                throw;
            }
        }
    }
}