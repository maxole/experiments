using System.Collections.Generic;
using System.Linq;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF.Components
{
    public class MessageAgent : IMessageAgent
    {
        private readonly List<IPrototype> _messages;

        public MessageAgent()
        {
            _messages = new List<IPrototype>();
        }

        public void RegisterPrototype(IPrototype message)
        {
            _messages.Add(message);
        }

        public IPrototype GetPrototype(MessageType type)
        {
            foreach (var message in _messages)
            {
                var messageType = message.GetType();
                var founded = messageType.GetInterfaces()
                                         .SingleOrDefault(i => i.GetGenericArguments()
                                                                .Contains(type.GetType()));
                if (founded != null)
                    return (IPrototype)message.Clone();
            }
            throw new KeyNotFoundException();
        }
    }
}
