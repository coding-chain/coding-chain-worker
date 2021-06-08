using System.Collections.Generic;

namespace Domain.TestExecution.Helpers
{
    public interface ICodeGenerator<TFunction> where TFunction : FunctionBase

    {
        public IReadOnlyCollection<Test<TFunction>> Tests { get; }
        public IReadOnlyCollection<FunctionBase> Functions { get; }

        public string TestPrefix { get; }
        string GetExecutableCode();

        public string GetTestNameByOrder(int order);
    }
}