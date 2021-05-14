namespace CodingChainApi.Infrastructure.Settings
{
    public interface IRabbitMqSettings
    {
        string RabbitHost { get; set; }
        string RabbitUserName { get; set; }
        string RabbitPassword { get; set; }
        int RabbitPort { get; set; }
        string ExecutionCodeRoute { get; set; }
        string ExecutedCodeRoute { get; set; }
        string RabbitMqWorker { get; set; }
        string RoutingKey { get; set; }
    }
}