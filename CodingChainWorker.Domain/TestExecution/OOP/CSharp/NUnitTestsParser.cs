using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP.CSharp
{
    public class NUnitTestsParser: IUnitTestsParser
    {

        public bool FunctionPassed(string functionName, string testsResult)
        {
            return testsResult.Contains($"Passed {functionName}");
        }
    }
}