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
            CleanedInputGenerator = Regex.Replace(inputGenerator, @"\s+", " ");
            CleanedOutputValidator = Regex.Replace(outputValidator, @"\s+", " ");
        }

        public string OutputValidator { get; }
        public string InputGenerator { get; }

        public string CleanedInputGenerator { get; init; }
        public string CleanedOutputValidator { get; init; }
        
        public bool HasPassed { get; set; } = false;


        public override string ToString()
        {
            return
                $"{nameof(OutputValidator)}: {OutputValidator}, {nameof(InputGenerator)}: {InputGenerator}, {nameof(CleanedInputGenerator)}: {CleanedInputGenerator}, {nameof(CleanedOutputValidator)}: {CleanedOutputValidator}";
        }
    }
}