using KpkWorkflow.Model;

namespace KpkWorkflow.WPF.Components
{
    public interface IMessagePrototype<T> : IMessagePrototype where T : MessageType
    {
        
    }

    public interface IVisualPrototype<T> : IVisualPrototype where T : MessageType
    {
        
    }
}