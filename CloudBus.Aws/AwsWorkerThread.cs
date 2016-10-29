using System;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using Castle.Core.Internal;
using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class AwsWorkerThread
    {
        public int Id { get; private set; }

        private readonly IConfiguration config;

        private readonly IAwsBusConfiguration awsConfig;

        private readonly string queueUrl;

        private IAmazonSQS sqs;

        public AwsWorkerThread(int id, IConfiguration config, IAwsBusConfiguration awsConfig, string queueUrl)
        {
            Id = id;
            this.config = config;
            this.awsConfig = awsConfig;
            this.queueUrl = queueUrl;
            this.sqs = this.awsConfig.ClientFactory.CreateSqsClient();
        }

        public void Work()
        {
            while (true)
            {
                ReceiveMessageResponse response = sqs.ReceiveMessage(new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl
                });
                if (response.Messages.Count == 0)
                {
                    continue;
                }
                foreach (var message in response.Messages)
                {
                    HandleSqsMessage(message);
                }
            }
        }

        private void HandleSqsMessage(Message message)
        {
            // Deserialize to envelope
            MessageEnvelope envelope = (MessageEnvelope) config.MessageSerializer.Deserialize(typeof(MessageEnvelope), message.Body);

            Type bodyType = Type.GetType(envelope.BodyType);
            object body = config.MessageSerializer.Deserialize(bodyType, envelope.Body);

            // Get handlers
            var handlers = config.HandlerResolver.ResolveHandlersForMessage(bodyType);

            // Invoke
            handlers.ForEach(x => x(body));
        }
    }
}
