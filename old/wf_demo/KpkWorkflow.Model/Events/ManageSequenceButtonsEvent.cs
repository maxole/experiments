namespace KpkWorkflow.Model
{
    public class ManageSequenceButtonsEvent : IEvent
    {
        public ManagerButtonsEnum Buttons { get; private set; }

        public ManageSequenceButtonsEvent(ManagerButtonsEnum buttons)
        {
            Buttons = buttons;
        }
    }
}