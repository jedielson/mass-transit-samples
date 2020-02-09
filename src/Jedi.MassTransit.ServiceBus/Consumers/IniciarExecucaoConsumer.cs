﻿using Jedi.MassTransit.ServiceBus.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Jedi.MassTransit.ServiceBus.Consumers
{
    public class IniciarExecucaoConsumer : IConsumer<IniciarExecucao>
    {
        private readonly ILogger<IniciarExecucaoConsumer> _logger;

        public IniciarExecucaoConsumer(ILogger<IniciarExecucaoConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IniciarExecucao> context)
        {
            _logger.LogInformation($"Receiving Message: {context.Message.Nome} at {context.Message.Data}");

            return Task.CompletedTask;
        }
    }
}
