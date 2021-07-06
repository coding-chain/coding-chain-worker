using System;
using System.Text;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CodingChainApi.Infrastructure.Messaging
{
    public abstract class RabbitMqBasePublisher
    {
        private readonly IModel? _channel;

        protected readonly ILogger<RabbitMqBasePublisher> Logger;
        protected string? Exchange;

        protected string? RoutingKey;

        public RabbitMqBasePublisher(IRabbitMqSettings settings, ILogger<RabbitMqBasePublisher> logger)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = settings.Host
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMQClient init fail");
            }

            Logger = logger;
        }

        protected void PushMessage(object message)
        {
            Logger.LogInformation("PushMessage in {Exchange}, routing key:{RoutingKey}", Exchange, RoutingKey);
            _channel.ExchangeDeclare(Exchange, ExchangeType.Topic);

            string msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            _channel.BasicPublish(Exchange,
                RoutingKey,
                null,
                body);
        }
    }
}