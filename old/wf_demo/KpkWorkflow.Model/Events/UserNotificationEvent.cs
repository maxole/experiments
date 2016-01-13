namespace KpkWorkflow.Model
{
    public class UserNotificationEvent : IEvent
    {
        public string MessageKey { get; private set; }

        public UserNotificationEvent(string messageKey)
        {
            MessageKey = messageKey;
        }
    }
}