using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.IService;
using Domain.TestExecution;
using MediatR;

namespace Application.Write
{
    public record RunParticipationTestsCommand(Guid ParticipationId, string Language, string HeaderCode,
        IList<RunParticipationTestsCommand.Test> Tests, IList<RunParticipationTestsCommand.Function> Functions) : IRequest<string>
    {
        public record Test(string OutputValidator, string InputGenerator);

        public record Function(string Code, int Order);
    }

    public class RunParticipationTestsHandler : IRequestHandler<RunParticipationTestsCommand, string>
    {
        private readonly IProcessService<CSharpParticipationTestingAggregate> _processService;

        public RunParticipationTestsHandler(IProcessService<CSharpParticipationTestingAggregate> processService)
        {
            _processService = processService;
        }

        public Task<string> Handle(RunParticipationTestsCommand request, CancellationToken cancellationToken)
        {
            var execution = new CSharpParticipationTestingAggregate(
                new ParticipationId(request.ParticipationId),
                request.Language,
                request.HeaderCode,
                request.Functions.Select(f => new Function(f.Code, f.Order)).ToList(),
                request.Tests.Select(t => new Test(t.OutputValidator, t.InputGenerator)).ToList());
            _processService.ExecuteParticipationCode(execution);
            return Task.FromResult(execution.Id.ToString());
        }
    }
}