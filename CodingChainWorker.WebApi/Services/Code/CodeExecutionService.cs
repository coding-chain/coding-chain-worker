using System;
using System.Threading.Tasks;
using Application.Write;
using CodingChainApi.Infrastructure.MessageBroker.RabbitMQ;
using CodingChainApi.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingChainApi.Services.Code
{
    public class CodeExecutionService : RabbitMqBaseListener<CodeExecutionService>
    {
        protected readonly IPublisher Mediator;

        public CodeExecutionService(IRabbitMqSettings settings, ILogger<CodeExecutionService> logger,
            IPublisher mediator) : base(
            settings, logger)
        {
            RouteKey = settings.RoutingKey;
            QueueName = settings.ExecutionCodeRoute;
            Mediator = mediator;
        }

        public override bool Process(string message)
        {
            if (message == null)
            {
                // When false is returned, the message is rejected directly, indicating that it cannot be processed
                return false;
            }

            try
            {
                _logger.LogDebug($"message: ${message}");
                var json = JObject.Parse(message);
                var runParticipation = JsonConvert.DeserializeObject<RunParticipationTestsCommand>(json.ToString());
                var res = Mediator.Publish(runParticipation);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Process fail,error:{ex.Message},stackTrace:{ex.StackTrace},message:{message}");
                _logger.LogError(-1, ex, "Process fail");
                return false;
            }
        }
    }
}