namespace CodingChainApi.Infrastructure.Settings
{
    public interface IElasticSearchSettings
    {
        string Url { get; set; }
        string CodeProcessResponseLogIndex { get; set; }
    }

    public class ElasticSearchSettings : IElasticSearchSettings
    {
        public string Url { get; set; }
        public string CodeProcessResponseLogIndex { get; set; }
    }
}