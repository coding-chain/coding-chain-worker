using System.Collections.Generic;
using Domain.TestExecution.OOP;

namespace Domain.TestExecution.Helpers
{
    public interface ICodeGenerator
    {
        string GetExecutableCode();

        public string GetTestNameByOrder(int order);
        public IReadOnlyCollection<ITest> Tests { get; }
        public IReadOnlyCollection<FunctionBase> Functions { get; }

        public string TestPrefix { get; }
    }
}