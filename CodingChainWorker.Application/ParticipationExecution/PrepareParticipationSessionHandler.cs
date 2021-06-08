using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Factories;
using Application.Contracts.IService;
using Domain.TestExecution;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.ParticipationExecution
{
    public record PrepareParticipationSessionCommand(Guid Id, LanguageEnum Language) : INotification;

    public record PreparedParticipationResponse(Guid ParticipationId);

    public class PrepareParticipationSessionHandler : INotificationHandler<PrepareParticipationSessionCommand>
    {
        private IProcessServiceFactory _processServiceFactory;
        private ILogger<PrepareParticipationSessionHandler> _logger;
        private IDispatcher<PreparedParticipationResponse> _dispatcher;

        public PrepareParticipationSessionHandler(IProcessServiceFactory processServiceFactory, ILogger<PrepareParticipationSessionHandler> logger, IDispatcher<PreparedParticipationResponse> dispatcher)
        {
            _processServiceFactory = processServiceFactory;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        public async Task Handle(PrepareParticipationSessionCommand request, CancellationToken cancellationToken)
        {
            var processService = _processServiceFactory.GetProcessServiceByLanguage(request.Language);
            try
            {
                var participation =
                    ParticipationAggregateFactory.GetParticipationAggregateByLanguage(request.Id, request.Language);
                await processService.PrepareParticipationExecution(participation);
                _dispatcher.Dispatch(new PreparedParticipationResponse(request.Id));
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot prepare participation execution : {ParticipationId}, error: {Error} ",
                    request.Id, e.Message);
            }
        }
    }
}