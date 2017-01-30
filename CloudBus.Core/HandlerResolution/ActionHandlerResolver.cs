using System;
using System.Collections.Generic;

namespace CloudBus.Core.HandlerResolution
{
    public class ActionHandlerResolver : IHandlerResolver
    {
        readonly Dictionary<Type, Action<object>> allHandlers = new Dictionary<Type, Action<object>>();

        public ActionHandlerResolver WithCommandHandler<TCommand>(Action<TCommand> handler)
        {
            Type commandType = typeof (TCommand);
            if (allHandlers.ContainsKey(commandType))
            {
                throw new ArgumentException($"Command handler for type {commandType} has already been registered");
            }
            allHandlers.Add(typeof(TCommand), x => handler((TCommand)x));
            return this;
        }

        public ActionHandlerResolver WithEventHandler<TEvent>(Action<TEvent> handler)
        {
            Type eventType = typeof(TEvent);
            if (allHandlers.ContainsKey(eventType))
            {
                throw new ArgumentException($"Event handler for type {eventType} has already been registered");
            }
            allHandlers.Add(typeof(TEvent), x => handler((TEvent)x));
            return this;
        }

        public Action<object> ResolveHandlerForMessage(Type messageType)
        {
            Action<object> handler;
            return allHandlers.TryGetValue(messageType, out handler) ? handler : null;
        }
    }
}
