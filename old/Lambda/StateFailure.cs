using Lambda.GenH30.Properties;

namespace Lambda.GenH30
{
    /// <summary>
    /// состояние отказ оборудования
    /// </summary>    
    public class StateFailure : State
    {
        private readonly ILogger _logger;
        private readonly LambdaUnit _unit;

        public StateFailure(ILogger logger, LambdaUnit unit)
        {
            _logger = logger;
            _unit = unit;
        }

        public override void PowerOn(ILambdaWorker worker)
        {
            Process();
        }

        public override void Read(ILambdaWorker worker)
        {
            Process();
        }

        public override void ReadInit(ILambdaWorker worker)
        {
            Process();
        }

        public override void Failure()
        {
            Process();
        }

        private void Process()
        {
            _unit.Available(false);
            _unit.ClearMeasuredParameters();
            _logger.Fatal(Resources.DeviceFailure, _unit.ExpectedIdentifier, _unit.Address);            
        }
    }
}