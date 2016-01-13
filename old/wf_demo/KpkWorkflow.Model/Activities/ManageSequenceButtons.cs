using System;
using System.Activities;

namespace KpkWorkflow.Model
{
    public class ManageSequenceButtons : CodeActivity
    {
        public InArgument<ManagerButtonsEnum> Button { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.RequireExtension<IEventBroker>();

            var button = new RuntimeArgument("Button", typeof(ManagerButtonsEnum), ArgumentDirection.In);
            metadata.AddArgument(button);
            metadata.Bind(this.Button, button);
        }

        protected override void Execute(CodeActivityContext context)
        {
            var broker = context.GetExtension<IEventBroker>();
            try
            {
                var buttons = Button.Get(context);
                broker.Publish(new ManageSequenceButtonsEvent(buttons));
            }
            catch (Exception exception)
            {
                broker.Publish(new ExceptionEvent(exception));
            }
        }
    }
}