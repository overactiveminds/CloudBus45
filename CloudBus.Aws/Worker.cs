﻿using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using CloudBus.Aws.Config;
using CloudBus.Core;

namespace CloudBus.Aws
{
    public class Worker : IWorker
    {
        private IAwsBusConfiguration awsBusConfig;
        private readonly IAwsWorkerConfiguration workerConfiguration;
        private readonly string localEventQueueName;
        private IConfiguration configuration;

        public Worker(IConfiguration configuration, IAwsBusConfiguration awsBusConfig, IAwsWorkerConfiguration workerConfiguration, string localEventQueueName)
        {
            this.configuration = configuration;
            this.awsBusConfig = awsBusConfig;
            this.workerConfiguration = workerConfiguration;
            this.localEventQueueName = localEventQueueName;
        }

        public void Start()
        {
            int id = 1;
            List<AwsWorkerThread> workers = awsBusConfig.QueueUrlsByType.Select(commandType => new AwsWorkerThread(id++, configuration, awsBusConfig, commandType.Value)).ToList();
            
            // Create a queue for this worker and subscribe to each event type
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
