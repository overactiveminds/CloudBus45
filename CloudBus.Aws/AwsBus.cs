using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class AwsBus : IBus
    {
        private readonly IConfiguration config;
        private readonly IAwsBusConfiguration awsBusConfiguration;
        private readonly IAmazonSQS sqs;
        private readonly IAmazonSimpleNotificationService sns;

        public AwsBus(IConfiguration config, IAwsBusConfiguration awsBusConfiguration)
        {
            this.config = config;
            this.awsBusConfiguration = awsBusConfiguration;
            sqs = this.awsBusConfiguration.ClientFactory.CreateSqsClient();
            sns = this.awsBusConfiguration.ClientFactory.CreateSnsClient();
        }

        public void Send<TCommand>(TCommand command)
        {
            string messageBody = config.MessageSerializer.Serialize(command);
            MessageEnvelope envelope = new MessageEnvelope
            {
                BodyType = typeof (TCommand).ToString(),
                Body = messageBody
            };
            SendMessageRequest request = new SendMessageRequest(awsBusConfiguration.QueueUrlsByType[typeof(TCommand)], config.MessageSerializer.Serialize(envelope));
            sqs.SendMessage(request);
        }

        public void Publish<TEvent>(TEvent @event)
        {
            string messageBody = config.MessageSerializer.Serialize(@event);
            MessageEnvelope envelope = new MessageEnvelope
            {
                BodyType = typeof(TEvent).ToString(),
                Body = messageBody
            };
            PublishRequest publishRequest = new PublishRequest(awsBusConfiguration.TopicArnsByType[typeof(TEvent)], config.MessageSerializer.Serialize(envelope));
            sns.Publish(publishRequest);
        }
    }
}
