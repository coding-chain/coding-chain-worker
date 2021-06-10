using System;
using System.Collections.Generic;
using System.Linq;
using Application.Contracts.Factories;
using Application.Contracts.IService;
using Application.ParticipationExecution;
using Domain.TestExecution;
using Domain.TestExecution.Imperative.Typescript;
using Domain.TestExecution.OOP.CSharp;

namespace CodingChainApi.Infrastructure.Factories
{
    public class ParticipationAggregateFactory : IParticipationAggregateFactory
    {
        private readonly ITypescriptTestsParser _typescriptTestsParser;
        private readonly ICsharpTestsParser _csharpTestsParser;

        public ParticipationAggregateFactory(ITypescriptTestsParser typescriptTestsParser,
            ICsharpTestsParser csharpTestsParser)
        {
            _typescriptTestsParser = typescriptTestsParser;
            _csharpTestsParser = csharpTestsParser;
        }

        public ParticipationAggregate GetParticipationAggregateByLanguage(Guid id,
            LanguageEnum languageEnum, IList<RunParticipationTestsCommand.Test>? tests = null,
            IList<RunParticipationTestsCommand.Function>? functions = null, string? headerCode = null)
        {
            tests ??= new List<RunParticipationTestsCommand.Test>();
            functions ??= new List<RunParticipationTestsCommand.Function>();
            return languageEnum switch
            {
                LanguageEnum.CSharp => GetCSharpAggregate(id, tests, functions, headerCode),
                _ => GetTypescriptAggregate(id, tests, functions, headerCode)
            };
        }

        private IList<FunctionEntity> CommandFunctionsToEntity(
            IList<RunParticipationTestsCommand.Function> functions)
        {
            return functions.Select(f => new FunctionEntity(f.Code, f.Order, new FunctionId(f.Id))).ToList();
        }

        private IList<TestEntity> CommandTestsToEntity(
            IList<RunParticipationTestsCommand.Test> tests)
        {
            return tests
                .Select(t => new TestEntity(new TestId(t.Id), t.OutputValidator, t.InputGenerator)).ToList();
        }

        private CSharpParticipationAggregate GetCSharpAggregate(
            Guid id, IList<RunParticipationTestsCommand.Test> tests,
            IList<RunParticipationTestsCommand.Function> functions, string? headerCode = null)
        {
            return new(
                new ParticipationId(id),
                LanguageEnum.CSharp,
                headerCode,
                CommandFunctionsToEntity(functions),
                CommandTestsToEntity(tests), _csharpTestsParser);
        }

        private TypescriptParticipationAggregate GetTypescriptAggregate(
            Guid id, IList<RunParticipationTestsCommand.Test> tests,
            IList<RunParticipationTestsCommand.Function> functions, string? headerCode = null)
        {
            return new(
                new ParticipationId(id),
                LanguageEnum.Typescript,
                headerCode,
                CommandFunctionsToEntity(functions),
                CommandTestsToEntity(tests), _typescriptTestsParser);
        }
    }
}