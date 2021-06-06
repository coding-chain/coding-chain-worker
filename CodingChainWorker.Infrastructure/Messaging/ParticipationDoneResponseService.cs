using Application.Contracts.IService;
using Application.Contracts.Processes;
using CodingChainApi.Infrastructure.MessageBroker;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Messaging
{
    public class ParticipationDoneResponseService : RabbitMqBasePublisher, IParticipationDoneService
    {
        public ParticipationDoneResponseService(IRabbitMqSettings settings,
            ILogger<ParticipationDoneResponseService> logger) : base(
            settings, logger)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.DoneExecutionRoutingKey;
        }

        public void Dispatch(CodeProcessResponse processResponse)
        {
            PushMessage(processResponse);
        }
    }
}