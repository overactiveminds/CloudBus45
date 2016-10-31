using System;
using System.Collections.Generic;

namespace CloudBus.Aws.Config
{
    public class AwsBusConfig : IAwsBusConfiguration
    {
        public Dictionary<Type, string> QueueUrlsByType { get; }

        public Dictionary<Type, string> TopicArnsByType { get; }

        public IQueueAndTopicNamingConvention QueueAndTopicNamingConvention { get; }

        public IAwsClientFactory ClientFactory { get; }

        public AwsBusConfig(Dictionary<Type, string> queueUrlsByType, Dictionary<Type, string> topicArnsByType, IQueueAndTopicNamingConvention queueAndTopicNamingConvention, IAwsClientFactory clientFactory)
        {
            QueueUrlsByType = queueUrlsByType;
            TopicArnsByType = topicArnsByType;
            QueueAndTopicNamingConvention = queueAndTopicNamingConvention;
            ClientFactory = clientFactory;
        }
    }
}