﻿using System.Threading.Tasks;
using Application.Contracts.IService;
using CodingChainApi.Infrastructure.Common.Exceptions;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Messaging
{
    public abstract class BaseDispatcherService<TMessage> : RabbitMqBasePublisher, IDispatcher<TMessage>
    {
        protected BaseDispatcherService(IRabbitMqSettings settings, ILogger<BaseDispatcherService<TMessage>> logger) :
            base(settings, logger)
        {
        }

        public virtual Task Dispatch(TMessage message)
        {
            PushMessage(message ??
                        throw new InfrastructureException("Cannot send null message with rabbitmq"));
            return Task.CompletedTask;
        }
    }
}