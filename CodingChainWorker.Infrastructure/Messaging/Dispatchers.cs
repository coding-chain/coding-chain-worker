using System;
using System.Threading.Tasks;
using Application.Contracts.Processes;
using Application.ParticipationExecution;
using Application.PlagiarismAnalyze;
using CodingChainApi.Infrastructure.Logs;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;

namespace CodingChainApi.Infrastructure.Messaging
{
    public class ParticipationDoneResponseService : BaseDispatcherService<CodeProcessResponse>
    {
        private readonly IServiceProvider _serviceProvider;

        public ParticipationDoneResponseService(IRabbitMqSettings settings,
            ILogger<ParticipationDoneResponseService> logger, IServiceProvider serviceProvider) : base(
            settings, logger)
        {
            _serviceProvider = serviceProvider;
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.DoneExecutionRoutingKey;
        }

        public override async Task Dispatch(CodeProcessResponse message)
        {
            await base.Dispatch(message);
            try
            {
                var client = _serviceProvider.GetRequiredService<IElasticClient>();
                await client.IndexDocumentAsync(new CodeProcessResponseLog(message.ParticipationId, message.Errors,
                    message.Output, message.TestsPassedIds));
            }
            catch (Exception e)
            {
                Logger.LogError("Cannot send message in ElasticSearch");
            }
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