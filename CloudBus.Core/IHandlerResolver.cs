using System;
using System.Collections.Generic;

namespace CloudBus.Core
{
    public interface IHandlerResolver
    {
        IEnumerable<Action<object>> ResolveHandlersForMessage(Type messageType);
    }
}
