namespace CodingChainApi.Infrastructure.Settings
{
    public interface ITypescriptExecutionSettings
    {
        string BaseTestFileName { get; set; }
    }

    public class TypescriptExecutionSettings : ITypescriptExecutionSettings
    {
        public string BaseTestFileName { get; set; }
    }
}