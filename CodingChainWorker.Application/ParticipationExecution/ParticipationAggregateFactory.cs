using System;
using System.Collections.Generic;
using System.Linq;
using Domain.TestExecution;
using Domain.TestExecution.Imperative.Typescript;
using Domain.TestExecution.OOP.CSharp;

namespace Application.ParticipationExecution
{
    public static class ParticipationAggregateFactory
    {
        public static ParticipationAggregate GetParticipationAggregateByLanguage(Guid id, 
                LanguageEnum languageEnum, IList<RunParticipationTestsCommand.Test>? tests = null, IList<RunParticipationTestsCommand.Function>? functions = null, string? headerCode = null)
        {
            tests ??= new List<RunParticipationTestsCommand.Test>();
            functions ??= new List<RunParticipationTestsCommand.Function>();
            return languageEnum switch
            {
                LanguageEnum.CSharp => GetCSharpAggregate(id, tests, functions, headerCode),
                _ => GetTypescriptAggregate(id, tests, functions, headerCode)
            };
        }

        private static IList<FunctionEntity> CommandFunctionsToEntity(
            IList<RunParticipationTestsCommand.Function> functions) =>
            functions.Select(f => new FunctionEntity(f.Code, f.Order, new FunctionId(f.Id))).ToList();

        private static IList<TestEntity> CommandTestsToEntity(
            IList<RunParticipationTestsCommand.Test> tests) => tests
            .Select(t => new TestEntity(new TestId(t.Id), t.OutputValidator, t.InputGenerator)).ToList();

        private static CSharpParticipationAggregate GetCSharpAggregate(
            Guid id, IList<RunParticipationTestsCommand.Test> tests, IList<RunParticipationTestsCommand.Function> functions, string? headerCode = null)
        {
            return new(
                new ParticipationId(id),
                LanguageEnum.CSharp,
                headerCode,
                CommandFunctionsToEntity(functions),
                CommandTestsToEntity(tests));
        }

        private static TypescriptParticipationAggregate GetTypescriptAggregate(
            Guid id, IList<RunParticipationTestsCommand.Test> tests, IList<RunParticipationTestsCommand.Function> functions, string? headerCode = null)
        {
            return new(
                new ParticipationId(id),
                LanguageEnum.Typescript,
                headerCode,
                CommandFunctionsToEntity(functions),
                CommandTestsToEntity(tests));
        }
    }
}