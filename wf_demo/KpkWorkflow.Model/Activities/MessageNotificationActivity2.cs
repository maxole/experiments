using System;
using System.Activities;
using System.Activities.Tracking;

namespace KpkWorkflow.Model
{
    public class MessageNotificationActivity2 : NativeActivity<ActivityState>
    {
        public InArgument<string> Key { get; set; }
        [RequiredArgument]
        public InArgument<MessageType> MessageType { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.RequireExtension<IMessageAgent>();
            metadata.RequireExtension<IEventBroker>();

            var messageType = new RuntimeArgument("MessageType", typeof(MessageType), ArgumentDirection.In);
            metadata.AddArgument(messageType);
            metadata.Bind(this.MessageType, messageType);

            var key = new RuntimeArgument("Key", typeof(string), ArgumentDirection.In);
            metadata.AddArgument(key);
            metadata.Bind(this.Key, key);
        }

        protected override void Execute(NativeActivityContext context)
        {
            var broker = context.GetExtension<IEventBroker>();
            try
            {

                var messageType = MessageType.Get(context);
                var key = Key.Get<string>(context);

                var agent = context.GetExtension<IMessageAgent>();
                var message = agent.GetPrototype(messageType);

                //todo some strange init
                var init = message as IMessagePrototype;
                if (init != null)
                    init.Initailize(key);

                var eventAgent = context.GetExtension<IEventAgent>();
                
                var e = eventAgent.GetEvent(message);
                broker.Publish(e);                
                
                const string name = "Waiting";
                context.CreateBookmark(name, OnResumeBookmark);
            }
            catch (Exception exception)
            {
                broker.Publish(new ExceptionEvent(exception));
            }
        }

        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        public void OnResumeBookmark(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            if (!(obj is ActivityState))
                throw new NotSupportedException("OnResumeBookmark ActivityState");
            Result.Set(context, (ActivityState) obj);
        }

        protected override void Cancel(NativeActivityContext context)
        {
            context.Track(new CustomTrackingRecord("cancel invoked !"));
            base.Cancel(context);
        }
    }
}