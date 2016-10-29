using System;

namespace CloudBus.Aws.Config
{
    public interface IQueueAndTopicNamingConvention
    {
        string GetQueueNameForCommand(Type commandType);

        string GetTopicNameForEvent(Type eventType);
    }
}
