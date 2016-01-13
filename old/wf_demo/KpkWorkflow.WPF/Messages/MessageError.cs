using System.Windows.Input;
using ClientWorkflow.WPF;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF.Components
{
    [MessageTypeView(typeof(MessageErrorView))]
    public class MessageError : IMessagePrototype<MessageTypeError>
    {
        private readonly IMessageProvider _provider;
        private readonly IEventBroker _eventBroker;
        public string Message { get; private set; }
        public ICommand AbortCommand { get; private set; }

        public MessageError(IMessageProvider provider, IEventBroker eventBroker)
        {
            _provider = provider;
            _eventBroker = eventBroker;

            AbortCommand = new RelayCommand(
                param => Abort(),
                p => true);
        }

        private void Abort()
        {
            _eventBroker.Publish(new WorkflowAbortEvent());
        }

        public object Clone()
        {
            return new MessageError(_provider, _eventBroker);
        }

        public void Initailize(object value)
        {
            Message = _provider.GetMessageItem((string)value).GetMessageFor();
        }
    }
}