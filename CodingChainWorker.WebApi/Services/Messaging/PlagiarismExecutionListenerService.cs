using Application.PlagiarismAnalyze;
using CodingChainApi.Infrastructure.Settings;
using CodingChainApi.Services.Code;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Services.Messaging
{
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
}