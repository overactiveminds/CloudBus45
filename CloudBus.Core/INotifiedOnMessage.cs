namespace CloudBus.Core
{
    public interface INotifiedOnMessage
    {
        void OnMessage(MessageEnvelope envelope);
    }
}
