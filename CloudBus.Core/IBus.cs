namespace CloudBus.Core
{
    public interface IBus
    {
        void Send<TCommand>(TCommand command);

        void Publish<TEvent>(TEvent @event);
    }
}
