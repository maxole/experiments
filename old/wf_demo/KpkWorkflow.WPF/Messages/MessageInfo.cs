using KpkWorkflow.Model;

namespace KpkWorkflow.WPF.Components
{
    [MessageTypeView(typeof(MessageInfoView))]
    public class MessageInfo : IMessagePrototype<MessageTypeInfo>, IMessagePrototype<MessageTypeWarn>
    {
        private readonly IMessageProvider _provider;

        public string Message { get; private set; }

        public MessageInfo(IMessageProvider provider)
        {
            _provider = provider;
        }

        public void Initailize(object value)
        {
            Message = _provider.GetMessageItem((string)value).GetMessageFor();
        }

        public object Clone()
        {
            return new MessageInfo(_provider);
        }
    }
}