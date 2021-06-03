using System.IO;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP
{
    public record OoTest : ITest

    {
        public OoTest(Function ooInFunc, Function ooOutFunc, TestId id, string name)
        {
            OoInFunc = ooInFunc;
            OoOutFunc = ooOutFunc;
            Id = id;
            Name = name;
        }

        public readonly Function OoInFunc;
        public readonly Function OoOutFunc;

        public FunctionBase InFunc => OoInFunc;
        public FunctionBase OutFunc => OoOutFunc;
        public TestId Id { get; }
        public string Name { get;  }
    }
}