using System.Collections.Generic;
using Domain.TestExecution.Helpers;
using Domain.TestExecution.OOP.CSharp;

namespace Domain.TestExecution.Imperative.Typescript
{
    public class TypescriptParticipationAggregate : ParticipationTestingAggregate<ImperativeFunction>
    {
        public TypescriptParticipationAggregate(ParticipationId id, LanguageEnum language, string? headerCode,
            IList<FunctionEntity> functions, IList<TestEntity> tests) : base(id, language, headerCode, functions, tests)
        {
            CodeGenerator = new TypescriptCodeGenerator(tests, functions, CodeAnalyzer, HeaderCode);
        }


        protected sealed override ICodeAnalyzer CodeAnalyzer { get; } = new TypescriptCodeAnalyzer();
        protected override ICodeGenerator<ImperativeFunction> CodeGenerator { get; }
        protected override IUnitTestsParser UnitTestsParser { get; } = new JestTestsParser();
    }
}