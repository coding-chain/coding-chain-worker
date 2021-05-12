using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Application.Read.Code.Execution;
using Domain.TestExecution;
using Domain.TestExecution.OOP.CSharp;
using MediatR;
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
        private readonly IProcessService<CSharpParticipationTestingAggregate> _processService;
        private readonly IExecutionResponseService _executionResponseService;

        public RunParticipationTestsHandler(IProcessService<CSharpParticipationTestingAggregate> processService,
            IExecutionResponseService executionResponseService)
        {
            _processService = processService;
            _executionResponseService = executionResponseService;
        }

        public Task Handle(RunParticipationTestsCommand request, CancellationToken cancellationToken)
        {
            var execution = new CSharpParticipationTestingAggregate(
                new ParticipationId(request.ParticipationId),
                request.Language,
                request.HeaderCode,
                request.Functions.Select(f => new Function(f.Code, f.Order)).ToList(),
                request.Tests.Select(t => new Test(t.OutputValidator, t.InputGenerator)).ToList());
            var handler = _processService.WriteAndExecuteParticipation(execution);
            var result = new CodeProcessResponse(request.ParticipationId, handler.Error, handler.Output);
            /** handler.ProcessEnded += (o, e) =>
             {
                 result.Errors = e.Error;
                 result.Output = e.Output;
                 _executionResponseService.Dispatch(result);
             };**/
            _executionResponseService.Dispatch(result);

            return Task.FromResult(JsonConvert.SerializeObject(result));
        }
    }
}