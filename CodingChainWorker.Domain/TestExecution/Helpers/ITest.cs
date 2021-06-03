using Domain.TestExecution.OOP;

namespace Domain.TestExecution.Helpers
{
    public interface ITest
    {
        FunctionBase InFunc { get; }
        FunctionBase OutFunc { get; }

        TestId Id { get; }

        string Name { get; }
    }
}