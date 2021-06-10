using System.Collections.Generic;
using System.Linq;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution
{
    public abstract class ParticipationTestingAggregate<TFunction> : ParticipationAggregate
        where TFunction : FunctionBase
    {
        private readonly List<FunctionEntity> _functions;
        private readonly List<TestEntity> _tests;


        protected ParticipationTestingAggregate(ParticipationId id, LanguageEnum language, string? headerCode,
            IList<FunctionEntity> functions, IList<TestEntity> tests, IUnitTestsParser unitTestsParser) : base(id, language, headerCode)
        {
            _functions = functions.ToList();
            _tests = tests.ToList();
            UnitTestsParser = unitTestsParser;
        }

        public override IReadOnlyCollection<FunctionEntity> Functions => _functions.AsReadOnly();
        public override IReadOnlyCollection<TestEntity> Tests => _tests.AsReadOnly();
        protected abstract ICodeAnalyzer CodeAnalyzer { get; }
        protected abstract ICodeGenerator<TFunction> CodeGenerator { get; }
        private IUnitTestsParser UnitTestsParser { get; }

        public override string GetExecutableCode()
        {
            return CodeGenerator.GetExecutableCode();
        }

        public override void ParseResult()
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
                        test.HasPassed = false;
                    else
                        test.HasPassed = UnitTestsParser.FunctionPassed(generatedTest.Name, Output, Error);
                }
            });
        }
    }
}