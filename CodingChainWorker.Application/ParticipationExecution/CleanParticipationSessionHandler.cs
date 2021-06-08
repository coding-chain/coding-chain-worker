using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Contracts.Factories;
using Domain.TestExecution;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.ParticipationExecution
{
    public record CleanParticipationSessionCommand(Guid Id, LanguageEnum Language) : INotification;

    public class CleanParticipationSessionHandler : INotificationHandler<CleanParticipationSessionCommand>
    {
        private readonly IProcessServiceFactory _processServiceFactory;
        private readonly ILogger<CleanParticipationSessionHandler> _logger;

        public CleanParticipationSessionHandler(IProcessServiceFactory processServiceFactory, ILogger<CleanParticipationSessionHandler> logger)
        {
            _processServiceFactory = processServiceFactory;
            _logger = logger;
        }

        public async Task Handle(CleanParticipationSessionCommand request, CancellationToken cancellationToken)
        {
            var processService = _processServiceFactory.GetProcessServiceByLanguage(request.Language);
            try
            {
                var participation =
                    ParticipationAggregateFactory.GetParticipationAggregateByLanguage(request.Id, request.Language);
                await processService.CleanParticipationExecution(participation);
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot clean participation execution : {ParticipationId}, error: {Error} ",
                    request.Id, e.Message);
            }
        }
    }
}