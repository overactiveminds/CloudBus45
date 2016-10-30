using System;

namespace CloudBus.Azure.Config
{
    public interface IQueueAndTopicNamingConvention
    {
        string GetQueueNameForCommand(Type commandType);

        string GetTopicNameForEvent(Type eventType);
    }
}
