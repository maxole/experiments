using System;

namespace KpkWorkflow.WPF.Components
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageTypeViewAttribute : Attribute
    {        
        public Type View { get; private set; }

        public MessageTypeViewAttribute(Type view)
        {
            View = view;
        }
    }
}