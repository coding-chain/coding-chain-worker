using System.Collections.Generic;
using Domain.TestExecution.OOP;

namespace Domain.TestExecution.Helpers
{
    public interface ICodeGenerator<TFunction> where TFunction : FunctionBase

    {
        string GetExecutableCode();

        public string GetTestNameByOrder(int order);
        public IReadOnlyCollection<Test<TFunction>> Tests { get; }
        public IReadOnlyCollection<FunctionBase> Functions { get; }

        public string TestPrefix { get; }
    }
}