using System;

namespace KpkWorkflow.Model
{
    public interface IEventAgent
    {
        void Register(Type type);
        IEvent GetEvent(IPrototype prototype);
    }
}