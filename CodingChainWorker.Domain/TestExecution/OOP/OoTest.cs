using System.IO;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP
{
    public record OoTest : ITest

    {
        public OoTest(OoFunction ooInFunc, OoFunction ooOutFunc, TestId id, string name)
        {
            OoInFunc = ooInFunc;
            OoOutFunc = ooOutFunc;
            Id = id;
            Name = name;
        }

        public readonly OoFunction OoInFunc;
        public readonly OoFunction OoOutFunc;

        public FunctionBase InFunc => OoInFunc;
        public FunctionBase OutFunc => OoOutFunc;
        public TestId Id { get; }
        public string Name { get;  }
    }
}