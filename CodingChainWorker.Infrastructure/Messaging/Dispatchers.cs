using Application.Contracts.Processes;
using Application.ParticipationExecution;
using Application.PlagiarismAnalyze;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Messaging
{
    public class ParticipationDoneResponseService : BaseDispatcherService<CodeProcessResponse>
    {
        public ParticipationDoneResponseService(IRabbitMqSettings settings,
            ILogger<ParticipationDoneResponseService> logger) : base(
            settings, logger)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.DoneExecutionRoutingKey;
        }
    }

    public class PlagiarismDoneResponseService : BaseDispatcherService<PlagiarismAnalyzeResponse>
    {
        public PlagiarismDoneResponseService(IRabbitMqSettings settings, ILogger<PlagiarismDoneResponseService> logger)
            : base(
                settings, logger)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.DoneExecutionRoutingKey;
        }
    }
    public class PreparedParticipationResponseService : BaseDispatcherService<PreparedParticipationResponse>
    {
        public PreparedParticipationResponseService(IRabbitMqSettings settings, ILogger<PreparedParticipationResponseService> logger)
            : base(
                settings, logger)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.PreparedExecutionRoutingKey;
        }
    
    }
}