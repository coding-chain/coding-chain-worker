using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Application.Contracts.Processes;
using Domain.Plagiarism;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Write
{
    public record RunParticipationTestsCommand(Guid Id, LanguageEnum Language, string HeaderCode,
        IList<RunParticipationTestsCommand.Test> Tests,
        IList<RunParticipationTestsCommand.Function> Functions) : INotification
    {
        public record Test(Guid Id, string OutputValidator, string InputGenerator);

        public record Function(Guid Id, string Code, int Order);
    }

    public class RunParticipationTestsHandler : INotificationHandler<RunParticipationTestsCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPlagiarismSettings _plagiarismSettings;

        public RunParticipationTestsHandler(IServiceProvider serviceProvider, IPlagiarismSettings plagiarismSettings)
        {
            _serviceProvider = serviceProvider;
            _plagiarismSettings = plagiarismSettings;
        }

        public async Task Handle(RunParticipationTestsCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var processService = scope.ServiceProvider
                .GetRequiredService<IProcessService<CSharpParticipationTestingAggregate>>();
            var executionResponseService = scope.ServiceProvider.GetRequiredService<IParticipationDoneService>();
            try
            {
                var execution = new CSharpParticipationTestingAggregate(
                    new ParticipationId(request.Id),
                    request.Language,
                    request.HeaderCode,
                    request.Functions.Select(f => new FunctionEntity(f.Code, f.Order, new FunctionId(f.Id))).ToList(),
                    request.Tests.Select(t => new TestEntity(new TestId(t.Id), t.OutputValidator, t.InputGenerator))
                        .ToList());

                await processService.WriteAndExecuteParticipation(execution);
                execution.ParseResult();
                executionResponseService.Dispatch(new CodeProcessResponse(
                    execution.Id.Value,
                    execution.Error,
                    execution.Output,
                    execution.Tests
                        .Where(t => t.HasPassed)
                        .Select(t => t.Id.Value)
                        .ToList()
                ));
            }
            catch (Exception e)
            {
                executionResponseService.Dispatch(new CodeProcessResponse(
                    request.Id,
                    "Parsing / Execution error", null, new List<Guid>()));
            }
        }
    }
}