using System.Text.RegularExpressions;

namespace Domain.TestExecution
{
    public record Test
    {
        public Test(string outputValidator, string inputGenerator)
        {
            OutputValidator = outputValidator;
            InputGenerator = inputGenerator;
            CleanedInputGenerator = Regex.Replace(inputGenerator, @"\s+", " ");
            CleanedOutputValidator = Regex.Replace(outputValidator, @"\s+", " ");

        }

        public string OutputValidator { get;  }
        public string InputGenerator { get;  }

        public string CleanedInputGenerator { get; init; }
        public string CleanedOutputValidator { get; init; }

    }
}