using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using Domain.Contracts;
using Domain.TestExecution.OOP;

namespace Domain.TestExecution
{
    public abstract class ParticipationAggregate : Aggregate<ParticipationId>

    {
        public LanguageEnum Language { get; }
        public string? HeaderCode { get; }

        public string? Output { get; private set; }
        public string? Error { get; private set; }

        public abstract IReadOnlyCollection<FunctionEntity> Functions { get; }
        public abstract IReadOnlyCollection<TestEntity> Tests { get; }
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


        protected ParticipationAggregate(ParticipationId id, LanguageEnum language, string? headerCode) : base(id)
        {
            Language = language;
            HeaderCode = headerCode;
        }

        public abstract string GetExecutableCode();

        public abstract void ParseResult();
       
    }
}