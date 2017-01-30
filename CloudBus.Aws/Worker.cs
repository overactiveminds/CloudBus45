using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class Worker : IWorker
    {
        private readonly IAwsBusConfiguration awsBusConfig;
        private readonly IAwsWorkerConfiguration workerConfiguration;
        private readonly string eventSubscriptionQueueUrl;
        private readonly IConfiguration configuration;

        public Worker(IConfiguration configuration, IAwsBusConfiguration awsBusConfig, IAwsWorkerConfiguration workerConfiguration, string eventSubscriptionQueueUrl)
        {
            this.configuration = configuration;
            this.awsBusConfig = awsBusConfig;
            this.workerConfiguration = workerConfiguration;
            this.eventSubscriptionQueueUrl = eventSubscriptionQueueUrl;
        }

        public Task Start(CancellationToken token)
        {
            int id = 1;
            List<AwsWorker> workers = awsBusConfig.QueueUrlsByType.Select(commandType => new AwsWorker(id++, configuration, awsBusConfig, commandType.Value, new CommandMessageAdapter())).ToList();
            workers.Add(new AwsWorker(id, configuration, awsBusConfig, eventSubscriptionQueueUrl, new SnsEventMessageAdapter()));
            return Task.WhenAll(workers.Select(x => x.Work()).ToArray());
        }
    }
}
