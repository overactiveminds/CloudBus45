namespace CloudBus.Core
{
    public interface IMessageSerializer
    {
        string Serialize<TMessage>(TMessage message);

        TMessage Deserialize<TMessage>(string data);
    }
}
