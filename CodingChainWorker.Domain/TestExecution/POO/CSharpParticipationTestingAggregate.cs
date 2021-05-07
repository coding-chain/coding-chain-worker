using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.TestExecution.POO
{
    public  class CSharpParticipationTestingAggregate : ParticipationTestingAggregate
    {
        private IPooCodeBuilder _codeBuilder;

        public CSharpParticipationTestingAggregate(ParticipationId id, string language, string headerCode,
            IList<Function> functions, IList<Test> tests) : base(id, language, headerCode)
        {
            _codeBuilder = new CsharpCodeBuilder(tests, functions, new CsharpCodeAnalyzer(), HeaderCode);
        }
        public override string GetExecutableCode() => _codeBuilder.GetExecutableCode();
    }
}