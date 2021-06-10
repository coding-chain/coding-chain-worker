using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.Imperative.Typescript
{
    public class JestTestsParser : IUnitTestsParser
    {
        public bool FunctionPassed(string functionName, string? output, string? error)
        {
            return error?.Contains($"{((char)0x2713).ToString()} {functionName}") ?? false;
        }
    }
}