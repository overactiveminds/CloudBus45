using System;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using CloudBus.Aws.Config;

namespace CloudBus.Aws.Environment
{
    public class AwsEnvironmentBuilder
    {
        private readonly IAmazonSimpleNotificationService sns;
        private readonly IAmazonSQS sqs;

        public AwsEnvironmentBuilder(IAwsClientFactory clientFactory)
        {
            sns = clientFactory.CreateSnsClient();
            sqs = clientFactory.CreateSqsClient();
        }

        /// <summary>
        /// Creates a queue if it doesn't alreay exists and returns the queue url
        /// </summary>
        /// <param name="name">The name of the queue to create</param>
        /// <returns>The queue url</returns>
        public string CreateQueueIfDoesntExist(string name)
        {
            try
            {
                return sqs.GetQueueUrl(name).QueueUrl;
            }
            catch
            {
                var createResponse = sqs.CreateQueue(name);
                return createResponse.QueueUrl;
            }
        }


        /// <summary>
        /// Creates a topic if doesn't already exists and return the topic arn
        /// </summary>
        /// <param name="name">The name of the topic</param>
        /// <returns>The topic arn</returns>
        public string CreateTopicIfDoesntExist(string name)
        {
            try
            {
                return sns.FindTopic(name).TopicArn;
            }
            catch
            {
                var creationResult = sns.CreateTopic(name);
                return creationResult.TopicArn;
            }
        }

        public void SubscribeQueueToTopic(string queueName, string topicArn)
        {
            throw new NotImplementedException();
        }
    }
}
