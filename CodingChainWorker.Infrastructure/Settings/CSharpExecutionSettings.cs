namespace CodingChainApi.Infrastructure.Settings
{
    public interface ICSharpExecutionSettings
    {
        string BaseTestFileName { get; set; }
    }

    public class CSharpExecutionSettings : ICSharpExecutionSettings
    {
        public string BaseTestFileName { get; set; }
    }


}