using System.Collections.Generic;

namespace Domain.TestExecution.OOP.CSharp
{
    public  class CSharpParticipationTestingAggregate : ParticipationTestingAggregate
    {
        private IPooCodeGenerator _codeGenerator;

        public CSharpParticipationTestingAggregate(ParticipationId id, string language, string headerCode,
            IList<Function> functions, IList<Test> tests) : base(id, language, headerCode)
        {
            _codeGenerator = new CsharpCodeGenerator(tests, functions, new CsharpCodeAnalyzer(), HeaderCode);
        }
        public override string GetExecutableCode() => _codeGenerator.GetExecutableCode();
    }
}