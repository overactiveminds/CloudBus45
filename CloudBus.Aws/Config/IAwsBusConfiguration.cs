using System;
using System.Collections.Generic;

namespace CloudBus.Aws.Config
{
    public interface IAwsBusConfiguration
    {
        Dictionary<Type, string> QueueUrlsByType { get; }

        Dictionary<Type, string> TopicArnsByType { get; }
            
        IQueueAndTopicNamingConvention QueueAndTopicNamingConvention { get; }

        IAwsClientFactory ClientFactory { get; }
    }
}