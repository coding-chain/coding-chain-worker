using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP.CSharp
{
    public class NUnitTestsParser : IUnitTestsParser
    {
        public bool FunctionPassed(string functionName, string? output, string? error)
        {
            return output?.Contains($"Passed {functionName}") ?? false;
        }
    }
}