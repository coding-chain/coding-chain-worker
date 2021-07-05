using System;
using System.Collections.Generic;
using Application.ParticipationExecution;
using Domain.TestExecution;

namespace Application.Contracts.Factories
{
    public interface IParticipationAggregateFactory
    {
        ParticipationAggregate GetParticipationAggregateByLanguage(Guid id,
            LanguageEnum languageEnum, IList<RunParticipationTestsCommand.Test>? tests = null,
            IList<RunParticipationTestsCommand.Function>? functions = null, string? headerCode = null);
    }

}