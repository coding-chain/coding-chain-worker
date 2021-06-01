using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Application.Read.Execution;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Write
{
    public record RunParticipationTestsCommand(Guid ParticipationId, string Language, string HeaderCode,
        IList<RunParticipationTestsCommand.Test> Tests,
        IList<RunParticipationTestsCommand.Function> Functions) : INotification
    {
        public record Test(string OutputValidator, string InputGenerator);

        public record Function(string Code, int Order);
    }

    public class RunParticipationTestsHandler : INotificationHandler<RunParticipationTestsCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public RunParticipationTestsHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Handle(RunParticipationTestsCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var processService = scope.ServiceProvider
                .GetRequiredService<IProcessService<CSharpParticipationTestingAggregate>>();
            var executionResponseService = scope.ServiceProvider.GetRequiredService<IExecutionResponseService>();
            var execution = new CSharpParticipationTestingAggregate(
                new ParticipationId(request.ParticipationId),
                request.Language,
                request.HeaderCode,
                request.Functions.Select(f => new Function(f.Code, f.Order)).ToList(),
                request.Tests.Select(t => new Test(t.OutputValidator, t.InputGenerator)).ToList());
            var result = await processService.WriteAndExecuteParticipation(execution);
            executionResponseService.Dispatch(result);
        }
    }
}