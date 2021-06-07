using System.Linq;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.Imperative.Typescript
{
    public class JestTestsParser : IUnitTestsParser
    {
        public bool FunctionPassed(string functionName, string? output, string? error)
        {
            return error?.Contains($"ÔêÜ {functionName}") ?? false;
        }
    }
}