using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Domain.Contracts;

namespace Domain.TestExecution
{
    public abstract class ParticipationTestingAggregate : Aggregate<ParticipationId>
    {
        public string Language { get;  }
        public string HeaderCode { get; }

        public abstract string GetExecutableCode();

        protected ParticipationTestingAggregate(ParticipationId id, string language, string headerCode) : base(id)
        {
            Language = language;
            HeaderCode = headerCode;
        }
    }
}