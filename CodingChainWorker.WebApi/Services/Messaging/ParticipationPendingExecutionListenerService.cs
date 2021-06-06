using System.Threading.Tasks;
using Application.Write;
using CodingChainApi.Infrastructure.MessageBroker;
using CodingChainApi.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Services.Code
{
    public class ParticipationPendingExecutionListenerService : NotifierListenerService<RunParticipationTestsCommand>
    {
        public ParticipationPendingExecutionListenerService(IRabbitMqSettings settings,
            ILogger<ParticipationPendingExecutionListenerService> logger, IPublisher mediator) : base(settings,
            logger, mediator)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.PendingExecutionRoutingKey;
        }
    }
}