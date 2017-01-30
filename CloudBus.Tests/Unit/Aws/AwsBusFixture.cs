using System;
using System.Collections.Generic;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudBus.Aws;
using CloudBus.Aws.Config;
using CloudBus.Core;
using Moq;
using NUnit.Framework;

namespace CloudBus.Tests.Unit.Aws
{
    public class AwsBusFixture
    {

        [Test]
        public void Send()
        {
            // Arrange
            const string expectedQueueUrl = "http://some.url";
            const string expectedMessageBody = "{}";
            var commandToSend = new Command();
            SendMessageRequest requestSent = null;

            Mock<IConfiguration> config = new Mock<IConfiguration>();
            config.Setup(x => x.MessageSerializer).Returns(() =>
            {
                var mockMessageSerializer = new Mock<IMessageSerializer>();
                mockMessageSerializer.Setup(x => x.Serialize(It.IsAny<MessageEnvelope>()))
                    .Returns(expectedMessageBody);
                return mockMessageSerializer.Object;
            });
            Mock<IAwsBusConfiguration> awsConfig = new Mock<IAwsBusConfiguration>();

            awsConfig.Setup(x => x.QueueUrlsByType)
                .Returns(new Dictionary<Type, string>
                {
                    { typeof(Command), expectedQueueUrl  }
                });

            awsConfig.Setup(x => x.ClientFactory.CreateSqsClient())
                .Returns(() =>
                {
                    var mockSqsClient = new Mock<IAmazonSQS>();
                    mockSqsClient.Setup(x => x.SendMessage(It.IsAny<SendMessageRequest>())).Callback((SendMessageRequest req) =>
                    {
                        requestSent = req;
                    });
                    return mockSqsClient.Object;
                });

            AwsBus sut = new AwsBus(config.Object, awsConfig.Object);

            // Act
            sut.Send(commandToSend);

            // Assert
            Assert.AreEqual(expectedQueueUrl, requestSent.QueueUrl);
            Assert.AreEqual(expectedMessageBody, requestSent.MessageBody);
        }

        [Test]
        public void Publish()
        {
            // Arrange
            const string expectedTopicArn = "http://some.url";
            const string expectedMessageBody = "{}";
            var eventToSend = new Event();
            PublishRequest requestSent = null;

            Mock<IConfiguration> config = new Mock<IConfiguration>();
            config.Setup(x => x.MessageSerializer).Returns(() =>
            {
                var mockMessageSerializer = new Mock<IMessageSerializer>();
                mockMessageSerializer.Setup(x => x.Serialize(It.IsAny<MessageEnvelope>()))
                    .Returns(expectedMessageBody);
                return mockMessageSerializer.Object;
            });
            Mock<IAwsBusConfiguration> awsConfig = new Mock<IAwsBusConfiguration>();

            awsConfig.Setup(x => x.TopicArnsByType)
                .Returns(new Dictionary<Type, string>
                {
                    { typeof(Event), expectedTopicArn  }
                });

            awsConfig.Setup(x => x.ClientFactory.CreateSnsClient())
                .Returns(() =>
                {
                    var mockSnsClient = new Mock<IAmazonSimpleNotificationService>();
                    mockSnsClient.Setup(x => x.Publish(It.IsAny<PublishRequest>())).Callback((PublishRequest req) =>
                    {
                        requestSent = req;
                    });
                    return mockSnsClient.Object;
                });

            AwsBus sut = new AwsBus(config.Object, awsConfig.Object);

            // Act
            sut.Publish(eventToSend);

            // Assert
            Assert.AreEqual(expectedTopicArn, requestSent.TopicArn);
            Assert.AreEqual(expectedMessageBody, requestSent.Message);
        }
    }

    public class Command
    {
        
    }

    public class Event
    {
        
    }
}
