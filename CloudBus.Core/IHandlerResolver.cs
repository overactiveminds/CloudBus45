using System;

namespace CloudBus.Core
{
    public interface IHandlerResolver
    {
        Action<object> ResolveHandlerForMessage(Type messageType);
    }
}
