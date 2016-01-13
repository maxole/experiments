using System;

namespace KpkWorkflow.Model
{
    public interface IPrototype : ICloneable
    {
        
    }
    public interface IMessagePrototype : IPrototype
    {
        void Initailize(object value);        
    }

    public interface IVisualPrototype : IPrototype
    {
        
    }
}
