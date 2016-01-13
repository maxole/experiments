namespace KpkWorkflow.Model
{
    public interface IMessageAgent
    {
        void RegisterPrototype(IPrototype message);
        IPrototype GetPrototype(MessageType type);
    }
}
