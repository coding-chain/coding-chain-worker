using System.Collections.Generic;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP.CSharp
{
    public class CSharpParticipationAggregate : ParticipationTestingAggregate<OoFunction>
    {
        public CSharpParticipationAggregate(ParticipationId id, LanguageEnum language, string? headerCode,
            IList<FunctionEntity> functions, IList<TestEntity> tests, IUnitTestsParser unitTestsParser) : base(id,
            language, headerCode, functions, tests, unitTestsParser)
        {
            CodeGenerator = new CsharpCodeGenerator(tests, functions, CodeAnalyzer, HeaderCode);
        }


        protected sealed override ICodeAnalyzer CodeAnalyzer { get; } = new CsharpCodeAnalyzer();
        protected override ICodeGenerator<OoFunction> CodeGenerator { get; }
    }
}