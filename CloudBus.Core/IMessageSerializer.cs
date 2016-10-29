using System;

namespace CloudBus.Core
{
    public interface IMessageSerializer
    {
        string Serialize<TMessage>(TMessage message);

        object Deserialize(Type type, string data);
    }
}
