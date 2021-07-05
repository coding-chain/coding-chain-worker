namespace Domain.TestExecution.Helpers
{
    public interface IUnitTestsParser
    {
        public bool FunctionPassed(string functionName, string? output, string? error);
    }
}