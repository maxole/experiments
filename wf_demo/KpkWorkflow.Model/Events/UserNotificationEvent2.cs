using System;
using Sparc.Kpk12.Domain;

namespace KpkWorkflow.Model
{
    public class UserNotificationEvent2 : IEvent
    {
        public Type EventType { get; private set; }
        public NotificationEventArgs Args { get; private set; }        

        public UserNotificationEvent2(Type eventType, string messageKey)
        {
            EventType = eventType;
            Args = new NotificationEventArgs
            {
                MessageKey = messageKey
            };
        }
    }
}