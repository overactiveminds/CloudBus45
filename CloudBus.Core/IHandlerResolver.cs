using System;
using System.Collections.Generic;

namespace CloudBus.Core
{
    public interface IHandlerResolver
    {
        void BeginLifetimeScope();

        IEnumerable<Action<object>> ResolveHandlersForMessage(Type messageType);
        
        void EndLifetimeScope();
    }
}
