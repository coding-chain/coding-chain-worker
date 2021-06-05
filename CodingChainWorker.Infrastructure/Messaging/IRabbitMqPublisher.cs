namespace CodingChainApi.Infrastructure.MessageBroker
{
    public interface IRabbitMqPublisher
    {
        void PushMessage( object message);
    }
}