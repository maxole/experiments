using System.ComponentModel;
using KpkWorkflow.Model;
using KpkWorkflow.Model.Annotations;

namespace KpkWorkflow.WPF
{
    public interface IUserNotificationView : IListener<MessageNofiticationEvent>
    {
    }

    public class UserNotificationView : INotifyPropertyChanged, IUserNotificationView
    {
        public IPrototype Component { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Handle(MessageNofiticationEvent e)
        {
            Component = e.Message;
            OnPropertyChanged("Component");
        }
    }
}