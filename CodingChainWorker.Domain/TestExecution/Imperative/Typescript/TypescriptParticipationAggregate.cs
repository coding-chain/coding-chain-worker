using System.Collections.Generic;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.Imperative.Typescript
{
    public class TypescriptParticipationAggregate : ParticipationTestingAggregate<ImperativeFunction>
    {
        public TypescriptParticipationAggregate(ParticipationId id, LanguageEnum language, string? headerCode,
            IList<FunctionEntity> functions, IList<TestEntity> tests, IUnitTestsParser unitTestsParser) : base(id, language, headerCode, functions, tests, unitTestsParser)
        {
            CodeGenerator = new TypescriptCodeGenerator(tests, functions, CodeAnalyzer, HeaderCode);
        }


        protected sealed override ICodeAnalyzer CodeAnalyzer { get; } = new TypescriptCodeAnalyzer();
        protected override ICodeGenerator<ImperativeFunction> CodeGenerator { get; }
  
    }
}