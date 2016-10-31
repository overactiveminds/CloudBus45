using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace CloudBus.Azure.Environment
{
    public interface IAzureClientFactory
    {
        NamespaceManager CreateNamespaceManager();

        QueueClient CreateQueueClient(string queueName);

        TopicClient CreateTopicClient(string topicName);

        SubscriptionClient CreateSubscriptionClient(string subscriptionName, string topicName);
    }

    public class AzureClientFactory : IAzureClientFactory
    {
        private readonly string connectionString;

        public AzureClientFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public NamespaceManager CreateNamespaceManager()
        {
            return NamespaceManager.CreateFromConnectionString(connectionString);
        }

        public QueueClient CreateQueueClient(string queueName)
        {
            return QueueClient.CreateFromConnectionString(connectionString, queueName);
        }

        public TopicClient CreateTopicClient(string topicName)
        {
            return TopicClient.CreateFromConnectionString(connectionString, topicName);
        }

        public SubscriptionClient CreateSubscriptionClient(string subscriptionName, string topicName)
        {
            return SubscriptionClient.CreateFromConnectionString(connectionString, topicName, subscriptionName);
        }
    }
}
