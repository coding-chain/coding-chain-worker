using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Domain.Contracts;

namespace Domain.TestExecution
{
    public abstract class ParticipationTestingAggregate : Aggregate<ParticipationId>
    {
        private class FunctionComparer : IComparer<Function>
        {
            public int Compare(Function? x, Function? y) => x?.Order ?? 0 - y?.Order ?? 0;
        }

        public string Language { get; protected set; }
        public string HeaderCode { get; protected set; }
        public abstract IReadOnlyCollection<Function> Functions { get; }
        public abstract IReadOnlyCollection<Test> Tests { get; }


        public abstract string GetExecutableCode();

        protected ParticipationTestingAggregate(ParticipationId id, string language, string headerCode) : base(id)
        {
            Language = language;
            HeaderCode = headerCode;
        }
    }
}