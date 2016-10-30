using System;
using System.Collections.Generic;
using CloudBus.Azure.Environment;

namespace CloudBus.Azure.Config
{
    public interface IAzureBusConfig
    {
        Dictionary<Type, string> CommandTypeAndQueueName { get; }
        Dictionary<Type, string> EventTypeAndTopicName { get; }
        IQueueAndTopicNamingConvention QueueAndTopicNamingConvention { get; }
        IAzureClientFactory ClientFactory { get; }
    }
}