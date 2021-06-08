using System.Collections.Generic;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP.CSharp
{
    public class CSharpParticipationAggregate : ParticipationTestingAggregate<OoFunction>
    {
        public CSharpParticipationAggregate(ParticipationId id, LanguageEnum language, string? headerCode,
            IList<FunctionEntity> functions, IList<TestEntity> tests) : base(id, language, headerCode, functions, tests)
        {
            CodeGenerator = new CsharpCodeGenerator(tests, functions, CodeAnalyzer, HeaderCode);
        }


        protected sealed override ICodeAnalyzer CodeAnalyzer { get; } = new CsharpCodeAnalyzer();
        protected override ICodeGenerator<OoFunction> CodeGenerator { get; }
        protected override IUnitTestsParser UnitTestsParser { get; } = new NUnitTestsParser();
    }
}