using Application.Contracts.IService;
using Domain.TestExecution.Helpers;
using Domain.TestExecution.OOP.CSharp;

namespace CodingChainApi.Infrastructure.Services.TestsParsers
{
    public class NUnitTestsParser : IUnitTestsParser, ICsharpTestsParser
    {
        public bool FunctionPassed(string functionName, string? output, string? error)
        {
            return output?.Contains($"Passed {functionName}") ?? false;
        }
    }
}