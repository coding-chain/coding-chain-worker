using System;
using System.Text.RegularExpressions;
using Domain.Contracts;

namespace Domain.TestExecution
{
    public record TestId(Guid Value) : IEntityId
    {
        public override string ToString() => Value.ToString();
    }

    public class TestEntity : Entity<TestId>
    {
        public TestEntity(TestId id, string outputValidator, string inputGenerator) : base(id)
        {
            OutputValidator = outputValidator;
            InputGenerator = inputGenerator;
        }

        public string OutputValidator { get; }
        public string InputGenerator { get; }
        
        public bool HasPassed { get; set; } = false;



    }
}