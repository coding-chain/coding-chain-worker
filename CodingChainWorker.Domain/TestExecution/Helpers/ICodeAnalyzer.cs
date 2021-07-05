namespace Domain.TestExecution.Helpers
{
    public interface ICodeAnalyzer
    {
        public string? FindFunctionName(string code);
    }
}