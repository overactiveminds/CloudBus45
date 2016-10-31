using System;
using System.Collections.Generic;
using CloudBus.Azure.Environment;

namespace CloudBus.Azure.Config
{
    public class AzureBusConfig : IAzureBusConfig
    {
        public Dictionary<Type, string> CommandTypeAndQueueName { get;  }

        public Dictionary<Type, string> EventTypeAndTopicName { get;  }

        public IQueueAndTopicNamingConvention QueueAndTopicNamingConvention { get;  }

        public IAzureClientFactory ClientFactory { get;  }

        public AzureBusConfig(Dictionary<Type, string> commandTypeAndQueueName, Dictionary<Type, string> eventTypeAndTopicName, IQueueAndTopicNamingConvention queueAndTopicNamingConvention, IAzureClientFactory clientFactory)
        {
            CommandTypeAndQueueName = commandTypeAndQueueName;
            EventTypeAndTopicName = eventTypeAndTopicName;
            QueueAndTopicNamingConvention = queueAndTopicNamingConvention;
            ClientFactory = clientFactory;
        }
    }
}