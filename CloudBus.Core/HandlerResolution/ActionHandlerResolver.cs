using System;
using System.Collections.Generic;

namespace CloudBus.Core.HandlerResolution
{
    public class ActionHandlerResolver : IHandlerResolver
    {
        readonly Dictionary<Type, List<Action<object>>> allHandlers = new Dictionary<Type, List<Action<object>>>();

        public ActionHandlerResolver WithCommandHandler<TCommand>(Action<TCommand> handler)
        {
            Type commandType = typeof (TCommand);
            if (allHandlers.ContainsKey(commandType))
            {
                throw new ArgumentException($"Command handler for type {commandType} has already been registered");
            }
            List<Action<object>> handlers = new List<Action<object>>
            {
                x => handler((TCommand)x)
            };
            allHandlers.Add(typeof(TCommand), handlers);
            return this;
        }

        public ActionHandlerResolver WithEventHandler<TEvent>(Action<TEvent> handler)
        {
            List<Action<object>> handlers;
            if (!allHandlers.TryGetValue(typeof (TEvent), out handlers))
            {
                handlers = new List<Action<object>>();
            }
            handlers.Add(x => handler((TEvent) x));
            return this;
        }

        public IEnumerable<Action<object>> ResolveHandlersForMessage(Type messageType)
        {
            List<Action<object>> handlers;
            return allHandlers.TryGetValue(messageType, out handlers) ? handlers : new List<Action<object>>();
        }
    }
}
