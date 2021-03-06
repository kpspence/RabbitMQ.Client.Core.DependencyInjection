using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.BatchMessageHandlers;
using RabbitMQ.Client.Core.DependencyInjection.Models;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace Examples.BatchMessageHandler
{
    public class CustomBatchMessageHandler : BaseBatchMessageHandler
    {
        readonly ILogger<CustomBatchMessageHandler> _logger;

        public CustomBatchMessageHandler(
            IRabbitMqConnectionFactory rabbitMqConnectionFactory,
            IEnumerable<BatchConsumerConnectionOptions> batchConsumerConnectionOptions,
            ILogger<CustomBatchMessageHandler> logger)
            : base(rabbitMqConnectionFactory, batchConsumerConnectionOptions, logger)
        {
            _logger = logger;
        }

        public override ushort PrefetchCount { get; set; } = 3;

        // You have to be aware that BaseBatchMessageHandler does not declare the specified queue. So if it does not exists an exception will be thrown.
        public override string QueueName { get; set; } = "queue.name";

        public override Task HandleMessages(IEnumerable<ReadOnlyMemory<byte>> messages, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling a batch of messages.");
            foreach (var message in messages)
            {
                var stringifiedMessage = Encoding.UTF8.GetString(message.ToArray());
                _logger.LogInformation(stringifiedMessage);
            }
            return Task.CompletedTask;
        }
    }
}