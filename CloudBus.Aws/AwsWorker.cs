using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class AwsWorker
    {
        public int Id { get; private set; }

        private readonly IConfiguration config;

        private readonly IAwsBusConfiguration awsConfig;

        private readonly string queueUrl;

        private readonly IMessageAdapter messageAdapter;

        private readonly IAmazonSQS sqs;

        public AwsWorker(int id, IConfiguration config, IAwsBusConfiguration awsConfig, string queueUrl, IMessageAdapter messageAdapter)
        {
            Id = id;
            this.config = config;
            this.awsConfig = awsConfig;
            this.queueUrl = queueUrl;
            this.messageAdapter = messageAdapter;
            this.sqs = this.awsConfig.ClientFactory.CreateSqsClient();
        }

        public Task Work()
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    ReceiveMessageResponse response = sqs.ReceiveMessage(new ReceiveMessageRequest
                    {
                        QueueUrl = queueUrl,
                        WaitTimeSeconds = 5
                    });
                    if (response.Messages.Count == 0)
                    {
                        continue;
                    }
                    foreach (var message in response.Messages)
                    {
                        HandleSqsMessage(message);
                    }
                    return;
                }
            });
        }

        private void HandleSqsMessage(Message message)
        {
            // Deserialize to envelope
            MessageEnvelope envelope = (MessageEnvelope) config.MessageSerializer.Deserialize(typeof(MessageEnvelope), messageAdapter.GetMessageBody(message));
            
            Type bodyType = Type.GetType(envelope.BodyType);
            object body = config.MessageSerializer.Deserialize(bodyType, envelope.Body);

            // Get handler
            var handler = config.HandlerResolver.ResolveHandlerForMessage(bodyType);

            // Invoke
            try
            {
                handler(body);

                // Handle complete, remove from queue
                sqs.DeleteMessage(queueUrl, message.ReceiptHandle);
            }
            catch (Exception ex)
            {
                // TODO: NLog
                Console.WriteLine($"Error {ex.Message} handling message {message.MessageId}");
            }
        }
    }
}
