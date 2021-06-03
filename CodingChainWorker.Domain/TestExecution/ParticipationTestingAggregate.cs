using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Domain.Contracts;
using Domain.TestExecution.Helpers;
using Domain.TestExecution.OOP;

namespace Domain.TestExecution
{
    public abstract class ParticipationTestingAggregate : Aggregate<ParticipationId>
    {
        private readonly List<FunctionEntity> _functions;
        private readonly List<TestEntity> _tests;
        public LanguageEnum Language { get; }
        public string HeaderCode { get; }

        public string? Output { get; private set; }
        public string? Error { get; private set; }

        public IReadOnlyCollection<FunctionEntity> Functions => _functions.AsReadOnly();
        public IReadOnlyCollection<TestEntity> Tests => _tests.AsReadOnly();

        public void AddOutput(string output)
        {
            Output ??= "";
            Output += output;
        }

        public void AddError(string error)
        {
            Error ??= "";
            Error += error;
        }

        protected abstract ICodeAnalyzer CodeAnalyzer { get; }
        protected abstract ICodeGenerator CodeGenerator { get; }
        protected abstract IUnitTestsParser UnitTestsParser { get; }

        protected ParticipationTestingAggregate(ParticipationId id, LanguageEnum language, string headerCode,
            IList<FunctionEntity> functions, IList<TestEntity> tests) : base(id)
        {
            _functions = functions.ToList();
            _tests = tests.ToList();
            Language = language;
            HeaderCode = headerCode;
        }

        public string GetExecutableCode() => CodeGenerator.GetExecutableCode();

        public void ParseResult()
        {
            _tests.ForEach(test =>
            {
                if (Output is null)
                {
                    test.HasPassed = false;
                }
                else
                {
                    var generatedTest = CodeGenerator.Tests.FirstOrDefault(t => t.Id == test.Id);
                    if (generatedTest is null)
                    {
                        test.HasPassed = false;
                    }
                    else
                    {
                        test.HasPassed = UnitTestsParser.FunctionPassed(generatedTest.Name, Output);
                    }
                }
            });
        }
    }
}