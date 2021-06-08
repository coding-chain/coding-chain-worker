using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Factories;
using Application.Contracts.IService;
using Application.Contracts.Processes;
using Domain.TestExecution;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.ParticipationExecution
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
        private readonly IProcessServiceFactory _processServiceFactory;
        private readonly IDispatcher<CodeProcessResponse> _dispatcher;
        private readonly ILogger<RunParticipationTestsHandler> _logger;

        public RunParticipationTestsHandler(IProcessServiceFactory processServiceFactory, IDispatcher<CodeProcessResponse> dispatcher, ILogger<RunParticipationTestsHandler> logger)
        {
            _processServiceFactory = processServiceFactory;
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public async Task Handle(RunParticipationTestsCommand request, CancellationToken cancellationToken)
        {
            var processService = _processServiceFactory.GetProcessServiceByLanguage(request.Language);
            try
            {
                var execution =
                    ParticipationAggregateFactory.GetParticipationAggregateByLanguage(request.Id, request.Language,
                        request.Tests, request.Functions, request.HeaderCode);

                await processService.WriteAndExecuteParticipation(execution);
                execution.ParseResult();
                _dispatcher.Dispatch(new CodeProcessResponse(
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
                _dispatcher.Dispatch(new CodeProcessResponse(
                    request.Id,
                    "Parsing / Execution error", null, new List<Guid>()));
            }
        }
    }
}