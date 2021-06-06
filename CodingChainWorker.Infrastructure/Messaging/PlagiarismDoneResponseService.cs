﻿using Application.Contracts.IService;
using Application.PlagiarismAnalyze;
using CodingChainApi.Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace CodingChainApi.Infrastructure.Messaging
{
    public class PlagiarismDoneResponseService : RabbitMqBasePublisher, IPlagiarismDoneService
    {
        public PlagiarismDoneResponseService(IRabbitMqSettings settings, ILogger<PlagiarismDoneResponseService> logger)
            : base(
                settings, logger)
        {
            Exchange = settings.ParticipationExchange;
            RoutingKey = settings.DoneExecutionRoutingKey;
        }

        public void Dispatch(PlagiarismAnalyzeResponse plagiarismResponse)
        {
            PushMessage(plagiarismResponse);
        }
    }
}