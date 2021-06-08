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
        private readonly IServiceProvider _serviceProvider;


        public CleanParticipationSessionHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Handle(CleanParticipationSessionCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var processService = scope.ServiceProvider
                .GetRequiredService<IProcessServiceFactory>().GetProcessServiceByLanguage(request.Language);
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CleanParticipationSessionHandler>>();
            try
            {
                var participation =
                    ParticipationAggregateFactory.GetParticipationAggregateByLanguage(request.Id, request.Language);
                await processService.CleanParticipationExecution(participation);
            }
            catch (Exception e)
            {
                logger.LogError("Cannot clean participation execution : {ParticipationId}, error: {Error} ",
                    request.Id, e.Message);
            }
        }
    }
}