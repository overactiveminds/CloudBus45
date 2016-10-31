namespace CloudBus.Core
{
    public abstract class Bus : IBus
    {
        private readonly IConfiguration _busConfig;

        protected Bus(IConfiguration busConfig)
        {
            _busConfig = busConfig;
        }

        public abstract void SendCommand<TCommand>(TCommand command);

        public abstract void PublishEvent<TEvent>(TEvent @event);

        public void Send<TCommand>(TCommand command)
        {
            SendCommand(command);
            _busConfig.AfterCommandActions.ForEach(x => x(command));
        }

        public void Publish<TEvent>(TEvent @event)
        {
            PublishEvent(@event);
            _busConfig.AfterEventActions.ForEach(x => x(@event));
        }
    }
}
