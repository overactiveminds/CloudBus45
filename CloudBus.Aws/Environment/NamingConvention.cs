using System;
using CloudBus.Aws.Config;

namespace CloudBus.Aws.Environment
{ 
    public class NamingConvention : INamingConvention
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
