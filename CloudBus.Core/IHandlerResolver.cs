using System;
using System.Collections.Generic;

namespace CloudBus.Core
{
    public interface IHandlerResolver
    {
        void BeginLifetimeScope();

        IEnumerable<Action<TMessage>> ResolveHandlersForMessage<TMessage>();

        void EndLifetimeScope();
    }
}
