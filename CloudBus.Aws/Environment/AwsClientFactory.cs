using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using CloudBus.Aws.Config;

namespace CloudBus.Aws.Environment
{
    public class AwsClientFactory : IAwsClientFactory
    {
        private readonly AWSCredentials credentials;
        private readonly AmazonSimpleNotificationServiceConfig snsConfig;
        private readonly AmazonSQSConfig sqsConfig;
        private readonly RegionEndpoint endpoint;

        /// <summary>
        /// Creates a new client factory using the specified endpoint and credentials specified in app.config
        /// </summary>
        /// <param name="endpoint"></param>
        public AwsClientFactory(RegionEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        /// <summary>
        /// Creates a new client factory using the specified endpoint and the AwsCredentials 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="credentials"></param>
        public AwsClientFactory(RegionEndpoint endpoint, AWSCredentials credentials)
        {
            this.credentials = credentials;
        }

        /// <summary>
        /// Creates a new client factory using the AwsCredentials, and the specified SQSCredentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="sqsConfig"></param>
        public AwsClientFactory(AWSCredentials credentials, AmazonSQSConfig sqsConfig)
        {
            this.credentials = credentials;
            this.sqsConfig = sqsConfig;
        }


        /// <summary>
        /// Creates a new client factory using the AwsCredentials, and the specified SQSCredentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="snsConfig"></param>
        public AwsClientFactory(AWSCredentials credentials, AmazonSimpleNotificationServiceConfig snsConfig)
        {
            this.credentials = credentials;
            this.snsConfig = snsConfig;
        }

        public IAmazonSQS CreateSqsClient()
        {
            if (credentials == null)
            {
                return new AmazonSQSClient(endpoint);
            }
            if (sqsConfig == null)
            {
                return new AmazonSQSClient(credentials, endpoint);
            }
            return new AmazonSQSClient(credentials, sqsConfig);
        }

        public IAmazonSimpleNotificationService CreateSnsClient()
        {
            if (credentials == null)
            {
                return new AmazonSimpleNotificationServiceClient(endpoint);
            }
            if (snsConfig == null)
            {
                return new AmazonSimpleNotificationServiceClient(credentials, endpoint);
            }
            return new AmazonSimpleNotificationServiceClient(credentials, snsConfig);
        }
    }
}
