using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace CloudBus.Aws.Config
{
    public interface IAwsClientFactory
    {
        IAmazonSQS CreateSqsClient();

        IAmazonSimpleNotificationService CreateSnsClient();
    }
}
