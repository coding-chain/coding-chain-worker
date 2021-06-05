using System;
using System.Threading.Tasks;
using Application.Write;
using CodingChainApi.Infrastructure.MessageBroker;
using CodingChainApi.Infrastructure.Messaging;
using CodingChainApi.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingChainApi.Services.Code
{
    public class ParticipationPendingExecutionListenerService : RabbitMqBaseListener
    {
        private readonly IPublisher _mediator;

        public ParticipationPendingExecutionListenerService(IRabbitMqSettings settings, ILogger<ParticipationPendingExecutionListenerService> logger,
            IPublisher mediator) : base(
            settings, logger)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.PendingExecutionRoutingKey;
            _mediator = mediator;
        }

        public override bool Process(string message)
        {

            try
            {
                var json = JObject.Parse(message);
                var runParticipation = JsonConvert.DeserializeObject<RunParticipationTestsCommand>(json.ToString());
                if (runParticipation is null)
                {
                    Logger.LogError("Cannot parse rabbitmq message");
                    return false;
                }
                _mediator.Publish(runParticipation);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Process fail,error:{ExceptionMessage},stackTrace:{ex.StackTrace},message:{Message}", ex.Message, message);
                Logger.LogError(-1, ex, "Process fail");
                return false;
            }
        }
    }
}