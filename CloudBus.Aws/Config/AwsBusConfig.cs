using System;
using System.Collections.Generic;

namespace CloudBus.Aws.Config
{
    public class AwsBusConfig : IAwsBusConfiguration
    {
        public Dictionary<Type, string> QueueUrlsByType { get; private set; }

        public Dictionary<Type, string> TopicArnsByType { get; private set; }

        public IQueueAndTopicNamingConvention QueueAndTopicNamingConvention { get; private set; }

        public IAwsClientFactory ClientFactory { get; private set; }

        public AwsBusConfig(Dictionary<Type, string> queueUrlsByType, Dictionary<Type, string> topicArnsByType, IQueueAndTopicNamingConvention queueAndTopicNamingConvention, IAwsClientFactory clientFactory)
        {
            QueueUrlsByType = queueUrlsByType;
            TopicArnsByType = topicArnsByType;
            QueueAndTopicNamingConvention = queueAndTopicNamingConvention;
            ClientFactory = clientFactory;
        }
    }
}