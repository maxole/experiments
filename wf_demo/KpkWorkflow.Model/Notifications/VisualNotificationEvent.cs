namespace KpkWorkflow.Model
{
    public class VisualNotificationEvent : IEvent<IVisualPrototype>
    {
        public IVisualPrototype Message { get; set; }

        public VisualNotificationEvent(IVisualPrototype message)
        {
            Message = message;
        }
    }
}