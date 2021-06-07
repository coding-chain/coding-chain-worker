using Application.ParticipationExecution;
using Application.PlagiarismAnalyze;
using CodingChainApi.Infrastructure.Settings;
using CodingChainApi.Services.Code;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Services.Messaging
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
    
    public class PlagiarismExecutionListenerService : NotifierListenerService<PlagiarismAnalyzeNotification>
    {
        public PlagiarismExecutionListenerService(IRabbitMqSettings settings,
            ILogger<PlagiarismExecutionListenerService> logger, IPublisher mediator) : base(settings,
            logger, mediator)
        {
            Exchange = settings.PlagiarismExchange;
            RoutingKey = settings.PlagiarismAnalyzeExecutionRoutingKey;
        }
    }
    public class PrepareParticipationExecutionListenerService : NotifierListenerService<PrepareParticipationSessionCommand>
    {
        public PrepareParticipationExecutionListenerService(IRabbitMqSettings settings,
            ILogger<PrepareParticipationExecutionListenerService> logger, IPublisher mediator) : base(settings,
            logger, mediator)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.PrepareExecutionRoutingKey;
        }
    }
    public class CleanParticipationExecutionListenerService : NotifierListenerService<CleanParticipationSessionCommand>
    {
        public CleanParticipationExecutionListenerService(IRabbitMqSettings settings,
            ILogger<CleanParticipationExecutionListenerService> logger, IPublisher mediator) : base(settings,
            logger, mediator)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.CleanExecutionRoutingKey;
        }
    }
}