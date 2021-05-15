namespace CodingChainApi.Infrastructure.Settings
{
    public class RabbitMqSettings :  IRabbitMqSettings
    {
        public string RabbitHost { get; set; }
        public string RabbitUserName { get; set; }
        public string RabbitPassword { get; set; }
        public int RabbitPort { get; set; }
        public string ExecutionCodeRoute { get; set; }
        public string ExecutedCodeRoute { get; set; }
        public string RabbitMqWorker { get; set; }
        public string RoutingKey { get; set; }
    }
}