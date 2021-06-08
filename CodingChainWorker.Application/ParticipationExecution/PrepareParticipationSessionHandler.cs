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
        private readonly IServiceProvider _serviceProvider;


        public PrepareParticipationSessionHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Handle(PrepareParticipationSessionCommand request, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var processService = scope.ServiceProvider
                .GetRequiredService<IProcessServiceFactory>().GetProcessServiceByLanguage(request.Language);
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<PrepareParticipationSessionHandler>>();
            var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher<PreparedParticipationResponse>>();
            try
            {
                var participation =
                    ParticipationAggregateFactory.GetParticipationAggregateByLanguage(request.Id, request.Language);
                await processService.PrepareParticipationExecution(participation);
                dispatcher.Dispatch(new PreparedParticipationResponse(request.Id));
            }
            catch (Exception e)
            {
                logger.LogError("Cannot prepare participation execution : {ParticipationId}, error: {Error} ",
                    request.Id, e.Message);
            }
        }
    }
}