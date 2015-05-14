using System;
using System.Collections.Generic;
using System.Linq;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF
{
    public class EventAgent : IEventAgent
    {
        private readonly List<Type> _events;
        private readonly List<Type> _prototypes;

        public EventAgent()
        {
            _events = new List<Type>();
            _prototypes = new List<Type>();
        }

        public void Register(Type type)
        {
            var assigable = typeof (IEvent).IsAssignableFrom(type);
            if(!assigable)
                throw new NotSupportedException("not supported type");

            foreach (var i in type.GetInterfaces()
                                  .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IEvent<>))
                                  .SelectMany(i => i.GetGenericArguments()))
            {
                if (_prototypes.Contains(i))
                    throw new DuplicateWaitObjectException("dublicate interface");
                _prototypes.Add(i);
            }

            _events.Add(type);
        }

        public IEvent GetEvent(IPrototype prototype)
        {
            var founded = _prototypes.FirstOrDefault(p => p.IsInstanceOfType(prototype));
            
            var genericType = typeof(IEvent<>).MakeGenericType(founded);

            foreach (var type in _events)
            {
                var any = genericType.IsAssignableFrom(type);
                if (!any)
                    continue;
                return (IEvent) Activator.CreateInstance(type, new object[] {prototype});
            }

            throw new KeyNotFoundException("EventAgent");
        }
    }
}