﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class Worker : IWorker
    {
        private IAwsBusConfiguration awsBusConfig;
        private readonly IAwsWorkerConfiguration workerConfiguration;
        private readonly string eventSubscriptionQueueUrl;
        private IConfiguration configuration;

        public Worker(IConfiguration configuration, IAwsBusConfiguration awsBusConfig, IAwsWorkerConfiguration workerConfiguration, string eventSubscriptionQueueUrl)
        {
            this.configuration = configuration;
            this.awsBusConfig = awsBusConfig;
            this.workerConfiguration = workerConfiguration;
            this.eventSubscriptionQueueUrl = eventSubscriptionQueueUrl;
        }

        public void Start()
        {
            // TODO: Add cancelllation token
            int id = 1;
            List<AwsWorkerThread> workers = awsBusConfig.QueueUrlsByType.Select(commandType => new AwsWorkerThread(id++, configuration, awsBusConfig, commandType.Value, new CommandMessageAdapter())).ToList();
            workers.Add(new AwsWorkerThread(id, configuration, awsBusConfig, eventSubscriptionQueueUrl, new SnsEventMessageAdapter()));
            foreach (var awsWorkerThread in workers)
            {
                Thread thread = new Thread(awsWorkerThread.Work);
                thread.Start();
            }
        }

        public void Stop()
        {
            // TODO: Cancel workers
            throw new NotImplementedException();
        }
    }
}
