namespace CodingChainApi.Infrastructure.Settings
{
    public interface ICSharpExecutionSettings
    {
        string TemplatePath { get; set; }
        string BaseTestFileName { get; set; }
    }

    public class CSharpExecutionSettings : ICSharpExecutionSettings
    {
        public string TemplatePath {get;set;}
        public string BaseTestFileName {get;set;}
    }
}