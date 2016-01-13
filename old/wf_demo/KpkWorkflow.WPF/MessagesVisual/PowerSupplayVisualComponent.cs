using System;
using System.ComponentModel;
using System.Windows.Input;
using ClientWorkflow.WPF;
using KpkWorkflow.Model;
using KpkWorkflow.Model.Annotations;

namespace KpkWorkflow.WPF.Components
{
    [MessageTypeView(typeof(PowerSupplayComponentView))]
    public class PowerSupplayVisualComponent : IVisualPrototype<PowerSupplayType>, INotifyPropertyChanged
    {
        private readonly IEventBroker _eventBroker;
        private readonly IMessageAgent _messageAgent;
        private readonly IEventAgent _eventAgent;
        public ICommand EnableCommand { get; private set; }
        public ICommand DisableCommand { get; private set; }
        private BackgroundWorker _worker;

        public PowerSupplayVisualComponent(IEventBroker eventBroker, IMessageAgent messageAgent, IEventAgent eventAgent)
        {
            _eventBroker = eventBroker;
            _messageAgent = messageAgent;
            _eventAgent = eventAgent;

            EnableCommand = new RelayCommand(
                param => Enable(),
                param => CanEnable);

            DisableCommand = new RelayCommand(
                param => Disable(),
                param => CanDisable);

            CanEnable = true;
            CanDisable = false;
            U = 0;
        }

        public bool CanDisable { get; set; }

        public double U { get; set; }

        public bool CanEnable { get; set; }

        private void Disable()
        {
            CanEnable = true;
            CanDisable = false;

            if (_worker != null && _worker.IsBusy)
                _worker.CancelAsync();
        }

        private void Enable()
        {
            CanDisable = true;
            CanEnable = false;
            _worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            _worker.DoWork += (s, e) =>
            {
                while (true)
                {
                    if (_worker.CancellationPending)
                        break;
                    U = new Random((int)DateTime.Now.Ticks).NextDouble() * 10.0;
                    OnPropertyChanged("U");
                    if (U > 8.7)
                    {
                        _eventBroker.Publish(new PowerSupplayErrorEvent());

                        var prototype = _messageAgent.GetPrototype(new MessageTypeError());

                        var messagePrototype = prototype as IMessagePrototype;
                        if (messagePrototype != null)
                            messagePrototype.Initailize("some error key");

                        var o = _eventAgent.GetEvent(prototype);
                        _eventBroker.Publish(o);
                        
                        break;
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            };
            _worker.RunWorkerAsync();
        }

        public object Clone()
        {
            return new PowerSupplayVisualComponent(_eventBroker, _messageAgent, _eventAgent);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}