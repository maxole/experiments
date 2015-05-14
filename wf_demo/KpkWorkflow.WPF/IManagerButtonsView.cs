using System;
using System.ComponentModel;
using System.Windows.Input;
using ClientWorkflow.WPF;
using KpkWorkflow.Model;
using KpkWorkflow.Model.Annotations;
using Sparc.Kpk12.Domain;

namespace KpkWorkflow.WPF
{
    public interface IManagerButtonsView : IDisposable
    {
        void SetButtons(ManagerButtonsEnum buttons);
        ManagerButtonsEnum Buttons { get; set; }
        bool CanNext { get; set; }
        bool CanSkip { get; set; }
        bool CanCancel { get; set; }
        bool CanFinish { get; set; }
        void Next();
        void Skip();
        void Cancel();
        void Finish();
    }

    public class ManagerButtonsAgent : IManagerButtonsView, IListener<ManageSequenceButtonsEvent>, INotifyPropertyChanged
    {
        private readonly ISequenceActivity _sequence;
        private readonly IEventBroker _eventBroker;
        private ManagerButtonsEnum _buttons;
        private readonly ICommand _cancelCommand;
        private readonly ICommand _nextCommand;
        private readonly ICommand _skipCommand;
        private readonly ICommand _finishCommand;

        public ManagerButtonsAgent(ISequenceActivity sequence, IEventBroker eventBroker)
        {
            _sequence = sequence;
            _eventBroker = eventBroker;

            _eventBroker.Register(this);

            _cancelCommand = new RelayCommand(p => Cancel(), p => CanCancel);
            _nextCommand = new RelayCommand(p => Next(), p => CanNext);
            _skipCommand = new RelayCommand(p => Skip(), p => CanSkip);
        }

        public void SetButtons(ManagerButtonsEnum buttons)
        {
            DisableButtons();
            _buttons = buttons;
            switch (_buttons)
            {
                case ManagerButtonsEnum.Cancel:
                    CanCancel = true;
                    break;
                case ManagerButtonsEnum.Next:
                    CanNext = true;
                    break;
                case ManagerButtonsEnum.NextCancel:
                    CanCancel = CanNext = true;
                    break;
                case ManagerButtonsEnum.NextSkip:
                    CanSkip = CanNext = true;
                    break;
                case ManagerButtonsEnum.Skip:
                    CanSkip = true;
                    break;
                case ManagerButtonsEnum.Finish:
                    CanFinish = true;
                    break;
            }
            OnPropertyChanged("Buttons");
            OnPropertyChanged("CanCancel");
            OnPropertyChanged("CanNext");
            OnPropertyChanged("CanSkip");
        }

        private void DisableButtons()
        {
            CanCancel = CanNext = CanSkip = false;
        }

        public ManagerButtonsEnum Buttons
        {
            get { return _buttons; }
            set { _buttons = value; }
        }

        public bool CanNext { get; set; }
        public bool CanSkip { get; set; }
        public bool CanCancel { get; set; }
        public bool CanFinish { get; set; }

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        public ICommand NextCommand
        {
            get { return _nextCommand; }
        }

        public ICommand SkipCommand
        {
            get { return _skipCommand; }
        }

        public ICommand FinishCommand
        {
            get { return _finishCommand; }
        }

        public void Next()
        {
            _sequence.Next();
        }

        public void Skip()
        {
            _sequence.Skip();
        }

        public void Cancel()
        {
            _sequence.Cancel();
        }

        public void Finish()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Handle(ManageSequenceButtonsEvent e)
        {
            SetButtons(e.Buttons);
        }

        public void Dispose()
        {
            _eventBroker.Unregister(this);
        }
    }
}