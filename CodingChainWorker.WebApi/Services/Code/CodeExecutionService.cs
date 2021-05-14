using System;
using System.Threading.Tasks;
using Application.Write;
using CodingChainApi.Infrastructure.MessageBroker;
using CodingChainApi.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingChainApi.Services.Code
{
    public class CodeExecutionService : RabbitMqBaseListener<CodeExecutionService>
    {
        private readonly IPublisher _mediator;

        public CodeExecutionService(IRabbitMqSettings settings, ILogger<CodeExecutionService> logger,
            IPublisher mediator) : base(
            settings, logger)
        {
            RouteKey = settings.RoutingKey;
            QueueName = settings.ExecutionCodeRoute;
            _mediator = mediator;
        }

        public override bool Process(string message)
        {

            try
            {
                _logger.LogDebug("message: {Message}",message);
                var json = JObject.Parse(message);
                var runParticipation = JsonConvert.DeserializeObject<RunParticipationTestsCommand>(json.ToString());
                if (runParticipation is null)
                {
                    _logger.LogError("Cannot parse rabbitmq message");
                    return false;
                }
                _mediator.Publish(runParticipation);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Process fail,error:{ExceptionMessage},stackTrace:{ex.StackTrace},message:{Message}", ex.Message, message);
                _logger.LogError(-1, ex, "Process fail");
                return false;
            }
        }
    }
}