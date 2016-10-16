using System;

namespace CloudBus.Aws.Config
{
    public interface INamingConvention
    {
        string GetQueueNameForCommand(Type commandType);

        string GetTopicNameForEvent(Type eventType);
    }
}
