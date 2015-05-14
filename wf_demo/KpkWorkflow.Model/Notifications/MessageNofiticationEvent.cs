namespace KpkWorkflow.Model
{
    public class MessageNofiticationEvent : IEvent<IMessagePrototype>
    {
        public IMessagePrototype Message { get; set; }

        public MessageNofiticationEvent(IMessagePrototype message)
        {
            Message = message;
        }
    }
}