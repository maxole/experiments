using System;

namespace KpkWorkflow.Model
{
    public class VisibilityComponentEvent : IEvent
    {
        public Type ComponentType { get; private set; }
        public bool Visible { get; set; }

        public VisibilityComponentEvent(Type type, bool visible)
        {
            ComponentType = type;
            Visible = visible;
        }
    }
}