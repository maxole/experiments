using System.ComponentModel;
using System.Windows.Input;
using ClientWorkflow.WPF;
using KpkWorkflow.Model;
using Sparc.Kpk12.Domain;

namespace KpkWorkflow.WPF
{
    public class EnableViewModel : INotifyPropertyChanged,
                                   IListener<WorkflowRuning>,
                                   IListener<WorkflowCompleted>,
                                   IListener<WorkflowAborted>
    {
        private readonly IWorkflowRunner _runner;
        private readonly IEventBroker _eventBroker;
        private readonly RelayCommand _startCommand;
        private readonly RelayCommand _abortCommand;

        public IComponentViewCollection ComponentView { get; set; }
        public IUserNotificationView UserNotification { get; set; }
        public IManagerButtonsView ManagerButtonsView { get; set; }

        public EnableViewModel(IWorkflowRunner runner,
                               IComponentViewCollection componentViewCollection,
                               IUserNotificationView userNotificationView,
                               IEventBroker eventBroker)
        {
            _runner = runner;
            _eventBroker = eventBroker;

            _startCommand = new RelayCommand(
                param => Start(),
                param => CanStart);

            _abortCommand = new RelayCommand(
                param => Abort(),
                param => CanAbort);

            ComponentView = componentViewCollection;
            UserNotification = userNotificationView;

            CanStart = true;
        }

        public ICommand StartCommand
        {
            get { return _startCommand; }
        }

        public ICommand AbortCommand
        {
            get { return _abortCommand; }
        }

        private bool CanAbort { get; set; }
        private bool CanStart { get; set; }

        private void Abort()
        {
            _runner.Abort();
        }

        private void Start()
        {
            _runner.Run(new EnableWorkflow
            {
                DisplayName = "enable workflow activity"
            });
            ManagerButtonsView = new ManagerButtonsAgent(_runner.CreateSequence(), _eventBroker);
            OnPropertyChanged("ManagerButtonsView");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Handle(WorkflowRuning e)
        {
            CanStart = false;
            CanAbort = true;
        }

        public void Handle(WorkflowCompleted e)
        {
            CanStart = true;
            CanAbort = false;
            // todo :\ doubles
            if (ManagerButtonsView != null)
                ManagerButtonsView.Dispose();
        }

        public void Handle(WorkflowAborted e)
        {
            CanStart = true;
            CanAbort = false;
            // todo :\ doubles
            if (ManagerButtonsView != null)
                ManagerButtonsView.Dispose();
        }
    }
}