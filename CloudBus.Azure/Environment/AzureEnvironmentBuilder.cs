using Microsoft.ServiceBus;

namespace CloudBus.Azure.Environment
{
    public class AzureEnvironmentBuilder
    {
        private readonly NamespaceManager namespaceManager;

        public AzureEnvironmentBuilder(IAzureClientFactory clientFactory)
        {
            namespaceManager = clientFactory.CreateNamespaceManager();
        }

        public void CreateQueueIfDoesntExist(string queueName)
        {
            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(queueName);
            }
        }

        public void CreateTopicIfDoesntExist(string topicName)
        {
            if (!namespaceManager.TopicExists(topicName))
            {
                namespaceManager.CreateTopic(topicName);
            }
        }

        public void CreateSubscriptionIfDoesntExist(string subscriptionName, string topic)
        {
            if (!namespaceManager.SubscriptionExists(topic, subscriptionName))
            {
                namespaceManager.CreateSubscription(topic, subscriptionName);
            }
        }
    }
}
