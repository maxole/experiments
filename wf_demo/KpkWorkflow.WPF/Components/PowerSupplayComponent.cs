using System;
using System.ComponentModel;
using System.Windows.Input;
using ClientWorkflow.WPF;
using KpkWorkflow.Model;
using KpkWorkflow.Model.Annotations;

namespace KpkWorkflow.WPF
{
    public class PowerSupplayComponent : INotifyPropertyChanged
    {
        private readonly IEventBroker _eventBroker;
        public ICommand EnableCommand { get; private set; }
        public ICommand DisableCommand { get; private set; }
        private BackgroundWorker _worker;

        public PowerSupplayComponent(IEventBroker eventBroker)
        {
            _eventBroker = eventBroker;

            EnableCommand=new RelayCommand(
                param=> Enable(),
                param=> CanEnable);

            DisableCommand=new RelayCommand(
                param=> Disable(),
                param=> CanDisable);

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
                    if (U > 9.7)
                    {
                        _eventBroker.Publish(new PowerSupplayErrorEvent());
                        //_eventBroker.Publish(new UserNotificationEvent2(typeof(IErrorMessageComponent), "error key 1"));
                        break;
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            };
            _worker.RunWorkerAsync();
        }

        public void Dispose()
        {
            if (_worker != null && _worker.IsBusy)
                _worker.CancelAsync();
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}