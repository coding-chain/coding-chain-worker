using System.Threading.Tasks;
using Application.Contracts.Processes;
using Application.ParticipationExecution;
using Application.PlagiarismAnalyze;
using CodingChainApi.Infrastructure.Logs;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Nest;

namespace CodingChainApi.Infrastructure.Messaging
{
    public class ParticipationDoneResponseService : BaseDispatcherService<CodeProcessResponse>
    {
        private readonly IElasticClient _client;

        public ParticipationDoneResponseService(IRabbitMqSettings settings,
            ILogger<ParticipationDoneResponseService> logger, IElasticClient client) : base(
            settings, logger)
        {
            _client = client;
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.DoneExecutionRoutingKey;
        }

        public override async Task Dispatch(CodeProcessResponse message)
        {
            await base.Dispatch(message);
            await _client.IndexDocumentAsync(new CodeProcessResponseLog(message.ParticipationId, message.Errors,
                message.Output, message.TestsPassedIds));
        }
    }

    public class PlagiarismDoneResponseService : BaseDispatcherService<PlagiarismAnalyzeResponse>
    {
        public PlagiarismDoneResponseService(IRabbitMqSettings settings, ILogger<PlagiarismDoneResponseService> logger)
            : base(
                settings, logger)
        {
            Exchange = settings.PlagiarismExchange;
            RoutingKey = settings.PlagiarismAnalyzeDoneRoutingKey;
        }
    }

    public class PreparedParticipationResponseService : BaseDispatcherService<PreparedParticipationResponse>
    {
        public PreparedParticipationResponseService(IRabbitMqSettings settings,
            ILogger<PreparedParticipationResponseService> logger)
            : base(
                settings, logger)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.PreparedExecutionRoutingKey;
        }
    }
}