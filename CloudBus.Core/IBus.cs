using System;

namespace CloudBus.Core
{

    public delegate void MessageDelegate(object message);

    public interface IBus
    {
        void Send<TCommand>(TCommand command);

        void Publish<TEvent>(TEvent @event);
    }
}
