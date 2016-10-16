using System;
using System.Collections.Generic;

namespace CloudBus.Aws.Config
{
    public class AwsBusConfig : IAwsBusConfiguration
    {
        public Dictionary<Type, string> QueueUrlsByType { get; private set; }

        public Dictionary<Type, string> TopicArnsByType { get; private set; }

        public INamingConvention NamingConvention { get; private set; }

        public IAwsClientFactory ClientFactory { get; private set; }

        public AwsBusConfig(Dictionary<Type, string> queueUrlsByType, Dictionary<Type, string> topicArnsByType, INamingConvention namingConvention, IAwsClientFactory clientFactory)
        {
            QueueUrlsByType = queueUrlsByType;
            TopicArnsByType = topicArnsByType;
            NamingConvention = namingConvention;
            ClientFactory = clientFactory;
        }
    }
}