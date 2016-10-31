using System;

namespace CloudBus.Azure.Config
{
    public class DefaultQueueAndTopicNamingConvention : IQueueAndTopicNamingConvention
    {
        public string GetQueueNameForCommand(Type commandType)
        {
            return commandType.Name.ToLower();
        }

        public string GetTopicNameForEvent(Type eventType)
        {
            return eventType.Name.ToLower();
        }
    }
}