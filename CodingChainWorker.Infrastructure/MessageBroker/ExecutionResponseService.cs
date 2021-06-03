using Application.Contracts.IService;
using Application.Contracts.Processes;
using CodingChainApi.Infrastructure.Settings;

namespace CodingChainApi.Infrastructure.MessageBroker
{
    public class ExecutionResponseService : IExecutionResponseService
    {
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        private readonly string _processedCodeRouteKey;


        public ExecutionResponseService(IRabbitMqSettings settings, IRabbitMqPublisher rabbitMqPublisher)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
            _processedCodeRouteKey = settings.ExecutedCodeRoute;
        }


        public void Dispatch(CodeProcessResponse processResponse)
        {
            _rabbitMqPublisher.PushMessage(_processedCodeRouteKey, processResponse);
        }
    }
}