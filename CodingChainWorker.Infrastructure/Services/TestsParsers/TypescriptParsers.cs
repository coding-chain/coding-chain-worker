using Application.Contracts.IService;
using Domain.TestExecution.Imperative.Typescript;

namespace CodingChainApi.Infrastructure.Services.TestsParsers
{
    public class WindowsJestTestsParser : ITypescriptTestsParser
    {
        public bool FunctionPassed(string functionName, string? output, string? error)
        {
            return error?.Contains($"√ {functionName}") ?? false;
        }
    }

    public class UnixJestTestsParser : ITypescriptTestsParser
    {
        public bool FunctionPassed(string functionName, string? output, string? error)
        {
            return error?.Contains($"✓ {functionName}") ?? false;
        }
    }
}