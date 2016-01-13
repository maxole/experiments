using System.ComponentModel;
using System.Windows.Input;
using ClientWorkflow.WPF;
using KpkWorkflow.Model;
using KpkWorkflow.Model.Annotations;

namespace KpkWorkflow.WPF
{
    public class PowerSupplayViewModel : INotifyPropertyChanged, IComponentItemView<PowerSupplayComponent>
    {
        private readonly IEventBroker _eventBroker;
        public bool Visibility { get; set; }
        public PowerSupplayComponent Component { get; private set; }

        public ICommand EnableCommand { get; set; }

        public PowerSupplayViewModel(PowerSupplayComponent component, IEventBroker eventBroker)
        {
            _eventBroker = eventBroker;
            Component = component;
            Visibility = false;

            EnableCommand = new RelayCommand(
                param=>Enable(),
                param=>CanEnable);

            CanEnable = true;
        }

        public bool CanEnable { get; set; }

        private void Enable()
        {
            
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